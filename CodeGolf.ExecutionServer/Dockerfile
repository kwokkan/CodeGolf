FROM mcr.microsoft.com/dotnet/core/runtime:2.2-stretch-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["CodeGolf.ExecutionServer/CodeGolf.ExecutionServer.csproj", "CodeGolf.ExecutionServer/"]
COPY ["CodeGolf.ServiceInterfaces/CodeGolf.ServiceInterfaces.csproj", "CodeGolf.ServiceInterfaces/"]
RUN dotnet restore "CodeGolf.ExecutionServer/CodeGolf.ExecutionServer.csproj"
COPY . .
WORKDIR "/src/CodeGolf.ExecutionServer"
RUN dotnet build "CodeGolf.ExecutionServer.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "CodeGolf.ExecutionServer.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CodeGolf.ExecutionServer.dll"]
