FROM dotnetcore/pitstoppro AS build-env
WORKDIR /app
EXPOSE 80
EXPOSE 443
 
# Build runtime image
FROM microsoft/dotnet:2.2-aspnetcore-runtime
COPY --from=build-env /app/dist/publish/CustomerManagementAPI.Host .

# Expose ports
 
HEALTHCHECK --interval=30s --timeout=3s --retries=1 CMD curl --silent --fail http://localhost:5100/hc || exit 1

# Start
ENTRYPOINT ["dotnet", "CustomerManagementAPI.Host.dll"]