FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
ARG BUILD_CONFIGURATION=Release

COPY src/Framework/Shopping.List.Framework.Core/Shopping.List.Framework.Core.csproj src/Framework/Shopping.List.Framework.Core/
RUN dotnet restore src/Framework/Shopping.List.Framework.Core/Shopping.List.Framework.Core.csproj

COPY src/App/Shopping.List.App.Shared/Shopping.List.App.Shared.csproj src/App/Shopping.List.App.Shared/
RUN dotnet restore src/App/Shopping.List.App.Shared/Shopping.List.App.Shared.csproj

COPY src/App/Shopping.List.App.Blazor.Client/Shopping.List.App.Blazor.Client.csproj src/App/Shopping.List.App.Blazor.Client/
RUN dotnet restore src/App/Shopping.List.App.Blazor.Client/Shopping.List.App.Blazor.Client.csproj

COPY src/App/Shopping.List.App.Blazor/Shopping.List.App.Blazor.csproj src/App/Shopping.List.App.Blazor/
RUN dotnet restore src/App/Shopping.List.App.Blazor/Shopping.List.App.Blazor.csproj

COPY . .
RUN dotnet build src/App/Shopping.List.App.Blazor/Shopping.List.App.Blazor.csproj -c $BUILD_CONFIGURATION -o /app/build

FROM build as publish
RUN dotnet publish src/App/Shopping.List.App.Blazor/Shopping.List.App.Blazor.csproj -c $BUILD_CONFIGURATION -o /app/publish

FROM base as final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Shopping.List.App.Blazor.dll"]
