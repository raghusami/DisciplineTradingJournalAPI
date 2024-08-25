using Asp.Versioning;
using DisciplineTradingJournalAPI;
using DisciplineTradingJournalAPI.DBModel;
using DisciplineTradingJournalAPI.Filters;
using DisciplineTradingJournalAPI.Helper;
using DisciplineTradingJournalAPI.Swagger;
using JWTAuthenticationManager;
using LISServiceAPI.Validator;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.IO;
using System.IO.Compression;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Configuration setup
builder.Configuration.SetBasePath(Directory.GetCurrentDirectory())
                     .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                     .AddEnvironmentVariables();
IConfiguration configuration = builder.Configuration;

// Logging setup
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();

// DbContext setup
builder.Services.AddDbContext<TradingJournalDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DBConfiguration")));

// IOptionsSnapshot Implementation
builder.Services.AddOptions()
                .Configure<AppConfiguration>(configuration)
                .AddTransient<IConfiguration>(item => configuration);

// API response and request JSON handler
builder.Services.AddControllersWithViews(option =>
{
    option.Filters.Add(typeof(CoreAPIExceptionFilter));
})
.AddNewtonsoftJson(option =>
{
    option.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Include;
    option.SerializerSettings.DefaultValueHandling = Newtonsoft.Json.DefaultValueHandling.Include;
});

// API versioning and Swagger setup
builder.Services.AddApiVersioning(options =>
{
    options.ReportApiVersions = true;
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ApiVersionSelector = new CurrentImplementationApiVersionSelector(options);
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ApiVersionReader = new MediaTypeApiVersionReader();
})
.AddMvc()
.AddApiExplorer();

builder.Services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
builder.Services.AddSwaggerGen(options => options.OperationFilter<SwaggerDefaultValues>());

// Response compression
builder.Services.AddResponseCompression(option =>
{
    option.Providers.Add<BrotliCompressionProvider>();
    option.Providers.Add<GzipCompressionProvider>();
});

builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

builder.Services.Configure<GzipCompressionProviderOptions>(options =>
{
    options.Level = CompressionLevel.SmallestSize;
});

// Post request body size configuration
builder.Services.Configure<FormOptions>(x =>
{
    x.MultipartBoundaryLengthLimit = int.MaxValue;
    x.ValueLengthLimit = int.MaxValue;
    x.MultipartBodyLengthLimit = long.MaxValue;
    x.MemoryBufferThreshold = 1024000000;
});

// Request rate limiter implementation
builder.Services.AddRateLimiter(_ => _
                .AddConcurrencyLimiter(policyName: "ConcurrencyRateLimiter", options =>
                {
                    options.PermitLimit = Convert.ToInt32(configuration["RateLimiter:ConcurrencyPermitLimit"]);
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = Convert.ToInt32(configuration["RateLimiter:ConcurrencyQueueLimit"]);
                }).RejectionStatusCode = StatusCodes.Status429TooManyRequests);

builder.Services.AddRateLimiter(_ => _
                .AddFixedWindowLimiter(policyName: "FixedWindowRateLimiter", options =>
                {
                    options.PermitLimit = Convert.ToInt32(configuration["RateLimiter:FixedWindowRateLimiter"]);
                    options.Window = TimeSpan.FromSeconds(Convert.ToInt32(configuration["RateLimiter:FixedWindowSize"]));
                    options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                    options.QueueLimit = Convert.ToInt32(configuration["RateLimiter:FixedWindowQueueLimit"]);
                }).RejectionStatusCode = StatusCodes.Status429TooManyRequests);

builder.Services.RegisterModelDependencies();
builder.Services.RegisterValidation();
builder.Services.AddSingleton<JWTAuthenticationHandler>();
builder.Services.JWTConfigValidator();

// CORS setup
builder.Services.AddCors(options =>
{
    options.AddPolicy("CORSPolicy", builder => builder.AllowAnyMethod().AllowAnyHeader().AllowCredentials().SetIsOriginAllowed((hosts) => true));
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("CORSPolicy");
app.UseResponseCompression();
app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto,
});
app.UseAuthentication();
app.UseAuthorization();
app.UseRateLimiter();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});
app.UseSwagger();
app.UseSwaggerUI(option =>
{
    foreach (var description in app.DescribeApiVersions())
    {
        option.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
    }
    option.RoutePrefix = string.Empty;
});
app.Run();

