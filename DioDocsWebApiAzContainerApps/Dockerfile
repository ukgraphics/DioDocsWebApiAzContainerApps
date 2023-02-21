#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

#フォントのインストール
RUN apt-get update && apt-get install -y wget && apt-get install -y zip unzip && apt-get install -y fontconfig
RUN wget https://moji.or.jp/wp-content/ipafont/IPAexfont/IPAexfont00301.zip
RUN unzip IPAexfont00301.zip
RUN mkdir -p /usr/share/fonts/ipa
RUN cp IPAexfont00301/*.ttf /usr/share/fonts/ipa
RUN fc-cache -fv

EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DioDocsWebApiAzContainerApps/DioDocsWebApiAzContainerApps.csproj", "DioDocsWebApiAzContainerApps/"]
RUN dotnet restore "DioDocsWebApiAzContainerApps/DioDocsWebApiAzContainerApps.csproj"
COPY . .
WORKDIR "/src/DioDocsWebApiAzContainerApps"
RUN dotnet build "DioDocsWebApiAzContainerApps.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DioDocsWebApiAzContainerApps.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DioDocsWebApiAzContainerApps.dll"]