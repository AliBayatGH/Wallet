#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM docker.hasti.co/dotnet/hit-dotnet-asp-run AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM docker.hasti.co/dotnet/hit-dotnet-asp-build AS build
WORKDIR /src
COPY ["WalletService/Server.csproj", "WalletService/"]
COPY ["Data/Data.csproj", "Data/"]
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Dtat/Dtat.csproj", "Dtat/"]
COPY ["Dtat.Wallet.Abstractions/Dtat.Wallet.Abstractions.csproj", "Dtat.Wallet.Abstractions/"]
COPY ["Dtos/Dtos.csproj", "Dtos/"]
COPY ["Resources/Resources.csproj", "Resources/"]
RUN dotnet restore "WalletService/Server.csproj"
COPY . .
WORKDIR "/src/WalletService"
RUN dotnet build "Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Server.dll"]