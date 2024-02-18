FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /NewZealandWalks
COPY . ./
RUN dotnet restore
RUN dotnet publish -c Release -o out 
# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /NewZealandWalks
COPY --from=build-env /NewZealandWalks/out .
ENTRYPOINT ["dotnet", "NewZealandWalks.dll"]
