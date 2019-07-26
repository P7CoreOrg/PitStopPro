FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /app
COPY ./PitStopPro.sln ./nuget.config  ./

# copy csproj and restore as distinct layers

# Copy the main source project files
COPY src/*/*.csproj ./
RUN for file in $(ls *.csproj); do mkdir -p src/${file%.*}/ && mv $file src/${file%.*}/; done

RUN dotnet restore

# copy and publish app and libraries
COPY ./src ./src

RUN find -type d -name bin -prune -exec rm -rf {} \; && find -type d -name obj -prune -exec rm -rf {} \;

RUN dotnet restore

RUN dotnet build -c Release --no-restore
RUN dotnet publish -c Release -o "../../dist" --no-restore

# test application -- see: dotnet-docker-unit-testing.md
FROM build AS testrunner
WORKDIR /app
ENTRYPOINT ["dotnet", "test", "--logger:trx"]