FROM microsoft/dotnet:2.1-sdk AS builder
WORKDIR /src
COPY . /src
RUN ls -laf
RUN dotnet clean ./crypto.sln
RUN dotnet restore ./crypto.sln
#RUN dotnet test ./Crypto.Test/Crypto.Test.csproj
RUN dotnet publish ./crypto.sln -c Release -o /app

FROM builder AS publish
RUN dotnet publish -c Release -o /app

# Copy csproj and restore as distinct layers
#COPY *.csproj ./
#RUN dotnet restore

# Copy everything else and build
#COPY . ./
#RUN dotnet publish -c Release -o out

# Build runtime image
FROM microsoft/dotnet:2.1-aspnetcore-runtime
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "CryptoService.dll"]