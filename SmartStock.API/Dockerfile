FROM mcr.microsoft.com/dotnet/sdk:8.0 AS restore
WORKDIR /src
COPY SmartStock.API.csproj .
RUN dotnet restore

FROM restore AS publish
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# .NET 8+ containers listen on port 8080 by default
ENV ASPNETCORE_URLS=http://+:8080
ENV ASPNETCORE_ENVIRONMENT=Production

COPY --from=publish /app/publish .

USER app

EXPOSE 8080

ENTRYPOINT ["dotnet", "SmartStock.API.dll"]
