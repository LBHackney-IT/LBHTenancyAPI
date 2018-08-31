FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:latest AS build
WORKDIR /src
COPY LBHTenancyAPI.sln ./
COPY LBHTenancyAPITest/LBHTenancyAPITest.csproj LBHTenancyAPITest/
RUN dotnet restore -nowarn:msb3202,nu1503
COPY . .
WORKDIR /src/LBHTenancyAPITest
RUN dotnet build -c Debug -o /app
