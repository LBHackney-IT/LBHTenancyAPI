FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE ${PORT:-80}

FROM microsoft/dotnet:latest AS build
WORKDIR /src
COPY LBHTenancyAPI.sln ./
COPY LBHTenancyAPI/LBHTenancyAPI.csproj LBHTenancyAPI/


COPY . .
RUN dotnet restore LBHTenancyAPI.sln -nowarn:msb3202,nu1503

RUN dotnet build LBH.Data.Domain/LBH.Data.Domain.csproj -c Release
RUN dotnet build LBH.Data.Repository/LBH.Data.Repository.csproj -c Release

RUN dotnet build LBHTenancyAPI/LBHTenancyAPI.csproj -c Release -o /app
#RUN dotnet build LBHTenancyAPI/LBHTenancyAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish LBHTenancyAPI/LBHTenancyAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
CMD ASPNETCORE_URLS=http://+:${PORT:-80} dotnet LBHTenancyAPI.dll
