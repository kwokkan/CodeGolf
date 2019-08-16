FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CodeGolf.Web/CodeGolf.Web.csproj", "CodeGolf.Web/"]
COPY ["CodeGolf.Persistence/CodeGolf.Persistence.csproj", "CodeGolf.Persistence/"]
COPY ["CodeGolf.Domain/CodeGolf.Domain.csproj", "CodeGolf.Domain/"]
COPY ["CodeGolf.Service/CodeGolf.Service.csproj", "CodeGolf.Service/"]
COPY ["CodeGolf.Persistence.Static/CodeGolf.Persistence.Static.csproj", "CodeGolf.Persistence.Static/"]
COPY ["CodeGolf.ServiceInterfaces/CodeGolf.ServiceInterfaces.csproj", "CodeGolf.ServiceInterfaces/"]
COPY ["CodeGolf.Recapcha/CodeGolf.Recaptcha.csproj", "CodeGolf.Recapcha/"]
RUN dotnet restore "CodeGolf.Web/CodeGolf.Web.csproj"
COPY . .
WORKDIR "/src/CodeGolf.Web"
RUN dotnet build "CodeGolf.Web.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CodeGolf.Web.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CodeGolf.Web.dll"]
