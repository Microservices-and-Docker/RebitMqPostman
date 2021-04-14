#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:3.1 AS build
WORKDIR /src
COPY ["RebitMqPostman/RebitMqPostman.csproj", "RebitMqPostman/"]
COPY ["RebitMqPostman.BLL/RebitMqPostman.BLL.csproj", "RebitMqPostman.BLL/"]
COPY ["RebitMq.Commmon/RebitMqPostman.Common.csproj", "RebitMq.Commmon/"]
RUN dotnet restore "RebitMqPostman/RebitMqPostman.csproj"
COPY . .
WORKDIR "/src/RebitMqPostman"
RUN dotnet build "RebitMqPostman.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "RebitMqPostman.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "RebitMqPostman.dll"]