# 📊 DisciplineTradingJournalAPI  

🚀 **Overview**  
DisciplineTradingJournalAPI is a **JWT-based API** built with **.NET 8.0**, designed to help traders track, analyze, and improve their trading performance. This API provides a structured way to log trades, analyze performance metrics, and optimize strategies through data-driven decision-making.  

## 🔑 **Key Features**
✔️ **JWT Authentication & Authorization**  
✔️ **Audit Logging for Trade Activities**  
✔️ **Dashboard & Performance Metrics**  
✔️ **Options Data & Analysis**  
✔️ **Trade Strategy & Settings Management**  
✔️ **User Profiles & Notifications**  
✔️ **Role-Based Access Control (RBAC)**  
✔️ **Swagger API Documentation**  

## 📂 **Modules & Endpoints**
- **AuditLog** - Track system and user activity  
- **Dashboard** - Summarize trading performance  
- **Notification** - Manage trade-related alerts  
- **OptionData & OptionsAnalysis** - Analyze options market trends  
- **PerformanceMetric** - Evaluate trading performance  
- **TradeNote** - Add notes and observations  
- **TradeSettings & TradeStrategy** - Define and optimize trade setups  
- **TradingCharges** - Track brokerage and transaction costs  
- **TradingUserProfile & TradingUsers** - Manage user accounts  
- **UserAlerts & UserTrades** - Set alerts and log trades  
- **UserOpenPositions & UserClosePositions** - Track active and closed trades  
- **UserPerformanceMetric** - Measure success and areas for improvement  

## 🛠️ **Tech Stack**
- **Framework:** .NET 8.0  
- **Authentication:** JWT-based authentication  
- **Database:** Microsoft SQL Server with **Entity Framework Core**  
- **Validation:** FluentValidation for request validation  
- **API Versioning:** ASP.Versioning.Mvc  
- **Resilience & Fault Handling:** Polly for retry policies  
- **Documentation:** Swagger & OpenAPI  

## 🚀 **Getting Started**
### 1️⃣ **Clone the Repository**
```bash
git clone https://github.com/raghusami/DisciplineTradingJournalAPI.git
cd DisciplineTradingJournalAPI

Clone the Repository

Configure the connection string in appsettings.json

Run the following command to apply migrations:
dotnet ef database update

Run the API
dotnet run

Access Swagger UI
Open your browser and go to:
http://localhost:5000/swagger
