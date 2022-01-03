FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

ENV ASPNETCORE_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:6.0-focal AS build
WORKDIR /src
COPY ["backend-apis", "./"]
RUN dotnet restore "CloudPharmacy.Physician.API/CloudPharmacy.Physician.API.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "CloudPharmacy.Physician.API/CloudPharmacy.Physician.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CloudPharmacy.Physician.API/CloudPharmacy.Physician.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CloudPharmacy.Physician.API.dll"]