FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE ${PORT:-80}

FROM microsoft/dotnet:latest AS build
WORKDIR /src
COPY LBHTenancyAPI.sln ./
COPY LBHTenancyAPI/LBHTenancyAPI.csproj LBHTenancyAPI/

ARG VERSION_SUFFIX

COPY . .
RUN dotnet restore LBHTenancyAPI.sln -nowarn:msb3202,nu1503

RUN dotnet build LBH.Data.Domain/LBH.Data.Domain.csproj -c Release --version-suffix $VERSION_SUFFIX
RUN dotnet build LBH.Data.Repository/LBH.Data.Repository.csproj -c Release --version-suffix $VERSION_SUFFIX

RUN dotnet build LBHTenancyAPI/LBHTenancyAPI.csproj -c Release -o /app --version-suffix $VERSION_SUFFIX
#RUN dotnet build LBHTenancyAPI/LBHTenancyAPI.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish LBHTenancyAPI/LBHTenancyAPI.csproj -c Release -o /app

FROM base AS final
WORKDIR /app

ENV CORECLR_ENABLE_PROFILING=1 \
  CORECLR_PROFILER={36032161-FFC0-4B61-B559-F6C5D41BAE5A} \
  CORECLR_NEWRELIC_HOME=./newrelic \
  CORECLR_PROFILER_PATH=./newrelic/libNewRelicProfiler.so \
  NEW_RELIC_LICENSE_KEY="${NEW_RELIC_LICENSE_KEY}" \
  NEW_RELIC_APP_NAME="${NEW_RELIC_APP_NAME}"

COPY --from=publish /app .
COPY newrelic ./newrelic

EXPOSE ${PORT:-80}
CMD ASPNETCORE_URLS=http://+:${PORT:-80} dotnet LBHTenancyAPI.dll
