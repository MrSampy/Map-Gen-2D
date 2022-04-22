FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Course-work/Course-work.csproj", "Course-work/"]
RUN dotnet restore "Course-work/Course-work.csproj"
COPY . .
WORKDIR "/src/Course-work"
RUN dotnet build "Course-work.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Course-work.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Course-work.dll"]
