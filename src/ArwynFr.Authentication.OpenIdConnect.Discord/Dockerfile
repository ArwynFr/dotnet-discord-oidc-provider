FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS restore
WORKDIR /src
COPY ["ArwynFr.Authentication.OpenIdConnect.Discord/ArwynFr.Authentication.OpenIdConnect.Discord.csproj", "ArwynFr.Authentication.OpenIdConnect.Discord/"]
RUN dotnet restore "ArwynFr.Authentication.OpenIdConnect.Discord/ArwynFr.Authentication.OpenIdConnect.Discord.csproj"

FROM restore AS build
COPY . .
WORKDIR "/src/ArwynFr.Authentication.OpenIdConnect.Discord"
RUN dotnet build "ArwynFr.Authentication.OpenIdConnect.Discord.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ArwynFr.Authentication.OpenIdConnect.Discord.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ArwynFr.Authentication.OpenIdConnect.Discord.dll"]
