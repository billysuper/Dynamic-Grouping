# 使用官方的ASP.NET Core runtime映像作為基礎映像
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# 使用官方的.NET SDK映像來構建我們的應用程式
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
# 將csproj文件和其他需要的文件複製到映像中
COPY ["Dynamic-Grouping.csproj", "./"]
# 恢復所有的NuGet依賴
RUN dotnet restore "Dynamic-Grouping.csproj"
# 將所有源代碼文件複製到映像中
COPY . .
# 在Release模式下構建應用程式，並將輸出放到/app/build目錄下
RUN dotnet build "Dynamic-Grouping.csproj" -c Release -o /app/build

# 在另一個階段中，使用同一個.NET SDK映像來發布應用程式
FROM build AS publish
RUN dotnet publish "Dynamic-Grouping.csproj" -c Release -o /app/publish

# 在最終階段中，使用ASP.NET Core runtime映像來運行我們的應用程式
FROM base AS final
WORKDIR /app
# 從發布階段複製所有文件到當前階段
COPY --from=publish /app/publish .
# 設定應用程式的入口點
ENTRYPOINT ["dotnet", "Dynamic-Grouping.dll"]
