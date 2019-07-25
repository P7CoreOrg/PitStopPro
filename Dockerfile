FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /sln
COPY ./PitStopPro.sln ./nuget.config  ./

# copy csproj and restore as distinct layers
COPY ./src/GraphQLHost.Core/GraphQLHost.Core.csproj  ./src/GraphQLHost.Core/GraphQLHost.Core.csproj
COPY ./src/GraphQLPlay.Contracts/GraphQLPlay.Contracts.csproj  ./src/GraphQLPlay.Contracts/GraphQLPlay.Contracts.csproj
COPY ./src/GraphQLPlay.IdentityModelExtras/GraphQLPlay.IdentityModelExtras.csproj  ./src/GraphQLPlay.IdentityModelExtras/GraphQLPlay.IdentityModelExtras.csproj
COPY ./src/GraphQLPlay.Rollup.Shadow/GraphQLPlay.Rollup.Shadow.csproj  ./src/GraphQLPlay.Rollup.Shadow/GraphQLPlay.Rollup.Shadow.csproj
COPY ./src/MultiAuthority.AccessTokenValidation/MultiAuthority.AccessTokenValidation.csproj  ./src/MultiAuthority.AccessTokenValidation/MultiAuthority.AccessTokenValidation.csproj
COPY ./src/P7Core.GraphQLCore/P7Core.GraphQLCore.csproj  ./src/P7Core.GraphQLCore/P7Core.GraphQLCore.csproj
COPY ./src/P7Core.GraphQLCore.Controllers/P7Core.GraphQLCore.Controllers.csproj  ./src/P7Core.GraphQLCore.Controllers/P7Core.GraphQLCore.Controllers.csproj
COPY ./src/Utils/Utils.csproj  ./src/Utils/Utils.csproj
COPY ./src/AuthRequiredDemo.GraphQL/AuthRequiredDemo.GraphQL.csproj  ./src/AuthRequiredDemo.GraphQL/AuthRequiredDemo.GraphQL.csproj
COPY ./src/CustomerManagementAPI/CustomerManagementAPI.csproj  ./src/CustomerManagementAPI/CustomerManagementAPI.csproj
COPY ./src/CustomerManagementStore/CustomerManagementStore.csproj  ./src/CustomerManagementStore/CustomerManagementStore.csproj
COPY ./src/SimpleDocumentStore/SimpleDocumentStore.csproj  ./src/SimpleDocumentStore/SimpleDocumentStore.csproj
COPY ./src/CustomerManagementAPI.Host/CustomerManagementAPI.Host.csproj  ./src/CustomerManagementAPI.Host/CustomerManagementAPI.Host.csproj
COPY ./tests/TestServerFixture/TestServerFixture.csproj  ./tests/TestServerFixture/TestServerFixture.csproj
COPY ./tests/XUnitTest_CustomerManagementStore/XUnitTest_CustomerManagementStore.csproj  ./tests/XUnitTest_CustomerManagementStore/XUnitTest_CustomerManagementStore.csproj


RUN dotnet restore

COPY ./test ./test
COPY ./src ./src
RUN find -type d -name bin -prune -exec rm -rf {} \; && find -type d -name obj -prune -exec rm -rf {} \;
RUN dotnet restore

RUN dotnet build -c Release --no-restore

RUN dotnet test "./test/XUnitTest_CustomerManagementStore/XUnitTest_CustomerManagementStore.csproj" -c Release --no-build --no-restore

RUN dotnet publish "./src/CustomerManagementStore/CustomerManagementStore.csproj" -c Release -o "../../dist" --no-restore

