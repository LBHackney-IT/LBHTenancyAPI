FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["LBHAuthenticationAPI/LBHAuthenticationAPI.csproj", "LBHAuthenticationAPI/"]
RUN dotnet restore "LBHAuthenticationAPI/LBHAuthenticationAPI.csproj"
COPY . .
WORKDIR "/src/LBHAuthenticationAPI"
RUN dotnet build "LBHAuthenticationAPI.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "LBHAuthenticationAPI.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "LBHAuthenticationAPI.dll"]