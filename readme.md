# 🧾 Subscriptions API (.NET 8 + PostgreSQL Supabase)


---


## ⚙️ Environment Configuration

1. Clone repository:
   ```bash
   git clone https://github.com/tu-usuario/planifika-back.git
   cd SubscriptionsApi

   
2. install pakage:
   dotnet add package Microsoft.EntityFrameworkCore
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
dotnet add package DotNetEnv
dotnet add package Swashbuckle.AspNetCore

dotnet restore

2. to run :
dotnet run

docker: 
docker build -t subscriptions-api .
docker run -it --rm -p 8080:8080 subscriptions-api