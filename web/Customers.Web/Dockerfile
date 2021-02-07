FROM  mcr.microsoft.com/dotnet/aspnet:5.0.2-alpine3.12 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0-alpine AS build
WORKDIR /src
COPY ["web/Customers.Web/Customers.Web.csproj", "web/Customers.Web/"]

# Fetch and install Node 14. Make sure to include the --yes parameter 
# to automatically accept prompts during install, or it'll fail.
RUN apk add --update nodejs npm
RUN npm install -g npm
RUN npm install

RUN dotnet restore "web/Customers.Web/Customers.Web.csproj"
COPY . .
WORKDIR "/src/web/Customers.Web"
RUN dotnet build "Customers.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Customers.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://+:8080
ENTRYPOINT ["dotnet", "Customers.Web.dll"]