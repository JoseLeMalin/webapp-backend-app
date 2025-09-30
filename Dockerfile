# FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build
# WORKDIR /usr/src/app
# 
# # Copy csproj and restore first
# COPY *.csproj ./
# RUN dotnet restore
# 
# # Copy everything else and build
# COPY . ./
# # Build and publish a release
# RUN dotnet publish -o out --no-restore
# 
# # Build runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:b4bea3a52a0a77317fa93c5bbdb076623f81e3e2f201078d89914da71318b5d8
# WORKDIR /usr/src/app
# COPY --from=build /App/out .
# ENTRYPOINT ["dotnet", "TodoApi.dll"]

FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build
WORKDIR /App

# Copy everything
COPY ["TodoApi.csproj", "./"]
# Restore as distinct layers
RUN dotnet restore "TodoApi.csproj"

# Build 
COPY [".", "./"]
RUN dotnet build "TodoApi.csproj" -c Release -o /App/build


#Publish 
FROM build AS publish
RUN dotnet publish "TodoApi.csproj" -c Release -o /App/publish

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:b4bea3a52a0a77317fa93c5bbdb076623f81e3e2f201078d89914da71318b5d8
ENV ASPNETCORE_HTTP_PORTS=5195
EXPOSE 5195
WORKDIR /App
COPY --from=publish /App/publish .
# ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]
ENTRYPOINT ["dotnet", "TodoApi.dll"]


# FROM mcr.microsoft.com/dotnet/sdk:9.0@sha256:3fcf6f1e809c0553f9feb222369f58749af314af6f063f389cbd2f913b4ad556 AS build
# WORKDIR /App
# 
# # Copy everything
# COPY . ./
# # Restore as distinct layers
# RUN dotnet restore
# # Build and publish a release
# RUN dotnet publish -o out
# 
# # Build runtime image
# FROM mcr.microsoft.com/dotnet/aspnet:9.0@sha256:b4bea3a52a0a77317fa93c5bbdb076623f81e3e2f201078d89914da71318b5d8
# WORKDIR /App
# COPY --from=build /App/out .
# # ENTRYPOINT ["dotnet", "DotNet.Docker.dll"]
# ENTRYPOINT ["./TodoApi"]

