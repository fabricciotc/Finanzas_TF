FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app


FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["Finanzas_TF/Finanzas_TF.csproj", "Finanzas_TF/"]
RUN dotnet restore "Finanzas_TF/Finanzas_TF.csproj"
COPY . .
WORKDIR "/src/Finanzas_TF"
RUN dotnet build "Finanzas_TF.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Finanzas_TF.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
EXPOSE 80
ENTRYPOINT ["dotnet", "Finanzas_TF.dll"]
