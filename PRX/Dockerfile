#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

#Depending on the operating system of the host machines(s) that will build or run the containers, the image specified in the FROM statement may need to be changed.
#For more information, please see https://aka.ms/containercompat

## Set environment variables
#ENV ASPNETCORE_ENVIRONMENT=Development
#ENV ConnectionStrings__DefaultConnection="Server=Pirhayati\\MSSQLSERVER01;Database=PRX_V2;User Id=MMNazari;Password=123;TrustServerCertificate=True;"

FROM mcr.microsoft.com/dotnet/aspnet:8.0-nanoserver-1809 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0-nanoserver-1809 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PRX.csproj", "."]
RUN dotnet restore "./././PRX.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./PRX.csproj" -c %BUILD_CONFIGURATION% -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PRX.csproj" -c %BUILD_CONFIGURATION% -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PRX.dll"]

## Use the official ASP.NET image
#FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
#WORKDIR /app
#EXPOSE 80
#EXPOSE 443
#
## Use the official .NET SDK image to build the app
#FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
#WORKDIR /src
#
## Copy the project files
#COPY ["PRX.csproj", "./"]
#RUN dotnet restore "./PRX.csproj"
#
## Copy all the source code and build the app
#COPY . .
#WORKDIR "/src/."
#RUN dotnet build "PRX.csproj" -c Release -o /app/build
#
#FROM build AS publish
#RUN dotnet publish "PRX.csproj" -c Release -o /app/publish
#
## Final stage: run the app
#FROM base AS final
#WORKDIR /app
#COPY --from=publish /app/publish .
#ENTRYPOINT ["dotnet", "PRX.dll"]