FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY MedicalPrescription.sln ./
COPY src/MedicalPrescription.Domain/*.csproj src/MedicalPrescription.Domain/
COPY src/MedicalPrescription.Application/*.csproj src/MedicalPrescription.Application/
COPY src/MedicalPrescription.Infrastructure/*.csproj src/MedicalPrescription.Infrastructure/
COPY src/MedicalPrescription.Api/*.csproj src/MedicalPrescription.Api/
COPY tests/MedicalPrescription.UnitTests/*.csproj tests/MedicalPrescription.UnitTests/
RUN dotnet restore

COPY . .
RUN dotnet build -c Release --no-restore \
 && dotnet test -c Release --no-build --logger "trx;LogFileName=unit-tests.trx" \
 && dotnet publish src/MedicalPrescription.Api -c Release --no-build -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080 \
    DOTNET_RUNNING_IN_CONTAINER=true

COPY --from=build /app/publish .

USER $APP_UID
EXPOSE 8080

ENTRYPOINT ["dotnet", "MedicalPrescription.Api.dll"]
