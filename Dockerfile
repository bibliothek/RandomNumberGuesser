FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-alpine
COPY /deploy /
WORKDIR /Server
ENTRYPOINT [ "dotnet", "Server.dll" ]