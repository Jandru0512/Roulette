#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Masiv.Roulette.Api/Masiv.Roulette.Api.csproj", "Masiv.Roulette.Api/"]
COPY ["Masiv.Roulette.Service/Masiv.Roulette.Service.csproj", "Masiv.Roulette.Service/"]
COPY ["Masiv.Roulette.Common/Masiv.Roulette.Common.csproj", "Masiv.Roulette.Common/"]
COPY ["Masiv.Roulette.ServiceDependencies/Masiv.Roulette.ServiceDependencies.csproj", "Masiv.Roulette.ServiceDependencies/"]
COPY ["Masiv.Roulette.Model/Masiv.Roulette.Model.csproj", "Masiv.Roulette.Model/"]
COPY ["Masiv.Roulette.Data/Masiv.Roulette.Data.csproj", "Masiv.Roulette.Data/"]
RUN dotnet restore "Masiv.Roulette.Api/Masiv.Roulette.Api.csproj"
COPY . .
WORKDIR "/src/Masiv.Roulette.Api"
RUN dotnet build "Masiv.Roulette.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Masiv.Roulette.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Masiv.Roulette.Api.dll"]