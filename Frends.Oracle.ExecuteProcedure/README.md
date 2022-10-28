# Frends.Oracle.ExecuteProcedure

[![Frends.Oracle.ExecuteProcedure Main](https://github.com/FrendsPlatform/Frends.Oracle/actions/workflows/ExecuteProcedure_build_and_test_on_main.yml/badge.svg)](https://github.com/FrendsPlatform/Frends.Oracle/actions/workflows/ExecuteProcedure_build_and_test_on_main.yml)
![MyGet](https://img.shields.io/myget/frends-tasks/v/Frends.Oracle.ExecuteProcedure?label=NuGet)
![GitHub](https://img.shields.io/github/license/FrendsPlatform/Frends.Oracle?label=License)
![Coverage](https://app-github-custom-badges.azurewebsites.net/Badge?key=FrendsPlatform/Frends.Oracle/Frends.Oracle.ExecuteProcedure|main)

Frends Task for executing stored procedures in Oracle database.

## Installing

You can install the Task via Frends UI Task View or you can find the NuGet package from the following NuGet feed.

# Building

Clone a copy of the repo

`git clone https://github.com/FrendsPlatform/Frends.Oracle.git`

Rebuild the project

`dotnet build`

### Run tests

`cd Frends.Oracle.ExecuteProcedure`

Run deploy script on git bash. This will download Oracle image and deploy the container.

`.\build\deploy_oracle_docker_container.sh`

`dotnet test`

### Create a NuGet package

`dotnet pack --configuration Release`

