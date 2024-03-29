#!/usr/bin/env pwsh
#
# Copyright (c) .NET Foundation and contributors. All rights reserved.
# Licensed under the MIT license. See LICENSE file in the project root for full license information.

# If this script fails, it is probably because docker drive sharing isn't enabled

$Time = [System.Diagnostics.Stopwatch]::StartNew()

function PrintElapsedTime {
    Log $([string]::Format("Elapsed time: {0}.{1}", $Time.Elapsed.Seconds, $Time.Elapsed.Milliseconds))
}

function Log {
    Param ([string] $s)
    Write-Output "###### $s"
}

function Check {
    Param ([string] $s)
    if ($LASTEXITCODE -ne 0) { 
        Log "Failed: $s"
        throw "Error case -- see failed step"
    }
}

$DockerOS = docker version -f "{{ .Server.Os }}"
$ImageName = "dotnetcore/pitstoppro"
$TestImageName = "dotnetcore/pitstoppro:test"
$DistImageName = "dotnetcore/pitstoppro:dist"
$Dockerfile = "Dockerfile"

$Version = "0.0.3"

PrintElapsedTime
Log "Build application image"
docker build --no-cache --pull -t $ImageName -f $Dockerfile --build-arg Version=$Version .
PrintElapsedTime
Check "docker build (application)"

Log "Build test runner image"
docker build --pull --target testrunner -t $TestImageName -f $Dockerfile --build-arg Version=$Version .
PrintElapsedTime
Check "docker build (test runner)"

Log "Build distRunner image"
docker build --pull --target distrunner -t $DistImageName -f $Dockerfile --build-arg Version=$Version .
PrintElapsedTime
Check "docker build (pack runner)"


$TestResults = "TestResults"
$TestResultsDir = Join-Path $PSScriptRoot $TestResults
$TestResultsDirExists = (Test-Path -Path $TestResultsDir)

Log "Check if $TestResults directory exists: $TestResultsDirExists"

if (!$TestResultsDirExists) {
    Log "Create $TestResults folder"
    mkdir $TestResultsDir
    gci . TestResults -ad
}
Log $TestResultsDir

$DistResults = "dist"
$DistDir = Join-Path $PSScriptRoot $DistResults
$DistDirExists = (Test-Path -Path $DistDir)
Log "Check if $DistDir directory exists: $DistDirExists"

if (!$DistDirExists) {
    Log "Create $DistDir folder"
    mkdir $DistDir
    gci . DistDir -ad
}
Log $DistDir


Log "Run test container with test runner image"

if ($DockerOS -eq "linux") {
    Log "Environment: Linux containers"
    docker run --rm -v ${TestResultsDir}:/app/test/${TestResults}	$TestImageName 
    
    docker run --rm -v ${DistDir}:/app/distMount					$DistImageName    

}
else {
    Log "Environment: Windows containers"
    docker run --rm -v ${TestResultsDir}:C:\app\test\${TestResults} $TestImageName
}

PrintElapsedTime

$testfiles = gci $TestResultsDir *.trx | Sort-Object -Property LastWriteTime | select -last 1

Log "Docker image built: $ImageName"
Log "Test log file:"
Write-Output $testfiles

$distfiles = gci $DistDir *.nupkg | Sort-Object -Property LastWriteTime | select -last 1

Log "Docker image built: $ImageName"
Log "Dist file:"
Write-Output $distfiles

