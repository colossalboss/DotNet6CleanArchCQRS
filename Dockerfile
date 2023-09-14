# # https://hub.docker.com/_/microsoft-dotnet
# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
# WORKDIR /source

# # copy csproj and restore as distinct layers
# COPY ../*.sln .
# COPY ./WebApi.csproj .
# COPY ../*/Application.csproj .
# RUN dotnet restore

# # copy everything else and build app
# COPY ./. .
# RUN dotnet publish -c Release -o /app --use-current-runtime --self-contained false --no-restore

# # final stage/image
# FROM mcr.microsoft.com/dotnet/aspnet:7.0
# WORKDIR /app
# COPY --from=build /app .
# ENTRYPOINT ["dotnet", "WebApi.dll"]



# Use the official .NET SDK image as the build environment
# FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# # Set the working directory
# WORKDIR /src

# # Copy the solution file and restore dependencies
# #COPY *.sln .
# COPY ["./WebApi/*.csproj", "./WebApi"]
# COPY ["./Application/*.csproj", "./Application"]
# COPY ["./Domain/*.csproj", "./Domain"]
# COPY ["./Infrastructure/*.csproj", "./Infrastructure"]

# RUN dotnet restore "./WebApi/WebApi.csproj" --disable-parallel

# # Copy the entire solution and publish projects
# COPY . .
# WORKDIR /src/CleanArchCqrs
# RUN dotnet publish -c Release -o /app/out

# # Build the runtime container
# FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
# WORKDIR /app
# COPY --from=build /app/out .

# ENTRYPOINT ["dotnet", "WebApi.dll"]


# Use the official .NET SDK image as the build environment
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

# Set the working directory
WORKDIR /src

# Copy the solution file and restore dependencies
COPY CleanArchCqrs.sln .
COPY ./WebApi/*.csproj ./WebApi/
COPY ./Domain/*.csproj ./Domain/
COPY ./Infrastructure/*.csproj ./Infrastructure/
COPY ./Application/*.csproj ./Application/
RUN dotnet restore "./CleanArchCqrs.sln" --disable-parallel

# Publish projects
COPY . .
WORKDIR /src/WebApi
RUN dotnet publish -c Release -o /app/out

# Build the runtime container
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
COPY --from=build /app/out .

ENTRYPOINT ["dotnet", "WebApi.dll"]