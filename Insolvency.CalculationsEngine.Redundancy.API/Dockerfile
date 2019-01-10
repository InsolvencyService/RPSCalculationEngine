FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY Insolvency.CalculationsEngine.Redundancy.API/Insolvency.CalculationsEngine.Redundancy.API.csproj Insolvency.CalculationsEngine.Redundancy.API/
COPY Insolvency.CalculationsEngine.Redundancy.BL/Insolvency.CalculationsEngine.Redundancy.BL.csproj Insolvency.CalculationsEngine.Redundancy.BL/
COPY Insolvency.CalculationsEngine.Redundancy.Common/Insolvency.CalculationsEngine.Redundancy.Common.csproj Insolvency.CalculationsEngine.Redundancy.Common/
RUN dotnet restore Insolvency.CalculationsEngine.Redundancy.API/Insolvency.CalculationsEngine.Redundancy.API.csproj
COPY . .
WORKDIR /src/Insolvency.CalculationsEngine.Redundancy.API
RUN dotnet build Insolvency.CalculationsEngine.Redundancy.API.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Insolvency.CalculationsEngine.Redundancy.API.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Insolvency.CalculationsEngine.Redundancy.API.dll"]
