using DisciplineTradingJournalAPI.DataEntity;
using System;
using System.IO;
using System.IO.Compression;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DisciplineTradingJournalAPI.DBModel
{
    public class CrudeLiveDataProcess
    {
        public async Task<ApiResponse> GetCrudeOilLiveData()
        {
            ApiResponse mCXResponse = new ApiResponse();
            string url = "https://www.mcxindia.com/backpage.aspx/GetOptionChain";

            using (HttpClientHandler handler = new HttpClientHandler
            {
                AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
            })
            using (HttpClient client = new HttpClient(handler))
            {
                // Set headers
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("gzip"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("deflate"));
                client.DefaultRequestHeaders.AcceptEncoding.Add(new StringWithQualityHeaderValue("br")); // Brotli
                client.DefaultRequestHeaders.Add("Accept-Language", "en-GB,en;q=0.9,en-US;q=0.8,ta;q=0.7");
                client.DefaultRequestHeaders.Add("Connection", "keep-alive");
                client.DefaultRequestHeaders.Add("Host", "www.mcxindia.com");
                client.DefaultRequestHeaders.Add("Origin", "https://www.mcxindia.com");
                client.DefaultRequestHeaders.Add("Referer", "https://www.mcxindia.com/market-data/option-chain");
                client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "empty");
                client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "cors");
                client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
                client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/133.0.0.0 Safari/537.36");
                client.DefaultRequestHeaders.Add("X-Requested-With", "XMLHttpRequest");

                // JSON Request Payload
                //string jsonPayload = "{\"Commodity\":\"CRUDEOIL\",\"Expiry\":\"16APR2025\"}";
                string jsonPayload = "{\"Commodity\":\"GOLD\",\"Expiry\":\"27MAY2025\"}";
                var requestBody = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                try
                {
                    HttpResponseMessage response = await client.PostAsync(url, requestBody);
                    if (response.IsSuccessStatusCode)
                    {
                        // Get content encoding type
                        var encoding = string.Join(",", response.Content.Headers.ContentEncoding);
                        using (var responseStream = await response.Content.ReadAsStreamAsync())
                        {
                            string responseBody;

                            if (response.Content.Headers.ContentEncoding.Contains("br"))
                            {
                                responseBody = DecompressBrotli(responseStream);
                            }
                            else if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                            {
                                responseBody = DecompressGzip(responseStream);
                            }
                            else if (response.Content.Headers.ContentEncoding.Contains("deflate"))
                            {
                                responseBody = DecompressDeflate(responseStream);
                            }
                            else
                            {
                                using (var reader = new StreamReader(responseStream))
                                {
                                    responseBody = await reader.ReadToEndAsync();
                                }
                            }

                            var result = JsonSerializer.Deserialize<ApiResponse>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                            if (result?.d?.Data != null)
                            {
                                mCXResponse = result;
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine($"Error: {response.StatusCode}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception: " + ex.Message);
                }
            }
            return mCXResponse;
        }

        private static string DecompressBrotli(Stream stream)
        {
            using (var decompressedStream = new BrotliStream(stream, CompressionMode.Decompress))
            using (var reader = new StreamReader(decompressedStream))
            {
                return reader.ReadToEnd();
            }
        }

        private static string DecompressGzip(Stream stream)
        {
            using (var decompressedStream = new GZipStream(stream, CompressionMode.Decompress))
            using (var reader = new StreamReader(decompressedStream))
            {
                return reader.ReadToEnd();
            }
        }

        private static string DecompressDeflate(Stream stream)
        {
            using (var decompressedStream = new DeflateStream(stream, CompressionMode.Decompress))
            using (var reader = new StreamReader(decompressedStream))
            {
                return reader.ReadToEnd();
            }
        }
    }

}
