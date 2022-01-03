#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["CloudPharmacy.Physician.WebApp/CloudPharmacy.Physician.WebApp.csproj", "CloudPharmacy.Physician.WebApp/"]
RUN dotnet restore "CloudPharmacy.Physician.WebApp/CloudPharmacy.Physician.WebApp.csproj"
COPY . .
WORKDIR "/src/CloudPharmacy.Physician.WebApp"
RUN dotnet build "CloudPharmacy.Physician.WebApp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudPharmacy.Physician.WebApp.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudPharmacy.Physician.WebApp.dll"]