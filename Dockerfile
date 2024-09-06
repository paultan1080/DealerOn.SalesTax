FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY DealerOn.SalesTax.sln ./

COPY DealerOn.SalesTax.Core/DealerOn.SalesTax.Core.csproj DealerOn.SalesTax.Core/
COPY DealerOn.SalesTax.Domain/DealerOn.SalesTax.Domain.csproj DealerOn.SalesTax.Domain/
COPY DealerOn.SalesTax.Domain.Sqlite/DealerOn.SalesTax.Domain.Sqlite.csproj DealerOn.SalesTax.Domain.Sqlite/
COPY DealerOn.SalesTax.Web/DealerOn.SalesTax.Web.csproj DealerOn.SalesTax.Web/
COPY DealerOn.SalesTax.Tests/DealerOn.SalesTax.Tests.csproj DealerOn.SalesTax.Tests/
COPY DealerOn.SalesTax.Console/DealerOn.SalesTax.Console.csproj DealerOn.SalesTax.Console/

RUN dotnet restore

COPY . .

WORKDIR /src/DealerOn.SalesTax.Web
RUN dotnet publish -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app ./

ENV ASPNETCORE_URLS=http://+:80

EXPOSE 80
ENTRYPOINT ["dotnet", "DealerOn.SalesTax.Web.dll"]
