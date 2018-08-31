FROM microsoft/aspnetcore:2 AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/aspnetcore-build:2 AS build
WORKDIR /src
COPY LBHTenancyAPI.sln ./
COPY LBHTenancyAPITest/LBHTenancyAPITest.csproj LBHTenancyAPITest/
RUN dotnet restore -nowarn:msb3202,nu1503,CS1591
COPY . .
WORKDIR /src/LBHTenancyAPITest
RUN dotnet build -c Debug -o /app
