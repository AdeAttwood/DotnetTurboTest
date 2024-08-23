FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:8c6beed050a602970c3d275756ed3c19065e42ce6ca0809f5a6fcbf5d36fd305 AS builder

WORKDIR /App
COPY . ./

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli
ENV PATH="${PATH}:/root/.dotnet/tools"

RUN libman restore
RUN dotnet publish  ./DotnetTurbo.Web

FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:8c6beed050a602970c3d275756ed3c19065e42ce6ca0809f5a6fcbf5d36fd305

WORKDIR /App
COPY --from=builder /App/DotnetTurbo.Web/bin/Release/net8.0/publish .
COPY --from=builder /App/DotnetTurbo.Web/wwwroot ./wwwroot

ENV ASPNETCORE_ENVIRONMENT Production

ENV ASPNETCORE_URLS="http://0.0.0.0:8000"

CMD ["./DotnetTurbo.Web"]
