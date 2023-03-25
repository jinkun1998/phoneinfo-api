#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["PhoneInfo.API/PhoneInfo.API.csproj", "PhoneInfo.API/"]
COPY ["PhoneInfo.API.Application/PhoneInfo.API.Application.csproj", "PhoneInfo.API.Application/"]
COPY ["PhoneInfo.API.Core/PhoneInfo.API.Domain.csproj", "PhoneInfo.API.Core/"]
RUN dotnet restore "PhoneInfo.API/PhoneInfo.API.csproj"
COPY . .
WORKDIR "/src/PhoneInfo.API"
RUN dotnet build "PhoneInfo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "PhoneInfo.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PhoneInfo.API.dll"]
