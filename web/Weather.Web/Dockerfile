FROM  mcr.microsoft.com/dotnet/aspnet:5.0-bullseye-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-bullseye-slim AS build
WORKDIR /src
COPY ["web/Weather.Web/Weather.Web.csproj", "web/Weather.Web/"]

RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash -
RUN apt-get install -y nodejs

RUN npm install -g npm@8.1.0

RUN dotnet restore "web/Weather.Web/Weather.Web.csproj"
COPY . .
WORKDIR "/src/web/Weather.Web"
RUN dotnet build "Weather.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Weather.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Weather.Web.dll"]