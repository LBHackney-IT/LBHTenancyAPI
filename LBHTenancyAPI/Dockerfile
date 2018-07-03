FROM microsoft/aspnetcore:2.0 AS base
WORKDIR /app
EXPOSE ${PORT:-80}

FROM microsoft/aspnetcore-build:2.0 AS build
WORKDIR /src
COPY LBHTenancyAPI.sln ./
COPY LBHTenancyAPI/LBHTenancyAPI.csproj LBHTenancyAPI/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/LBHTenancyAPI
RUN dotnet build LBHTenancyAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish LBHTenancyAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
CMD ASPNETCORE_URLS=http://+:${PORT:-80} dotnet LBHTenancyAPI.dll
