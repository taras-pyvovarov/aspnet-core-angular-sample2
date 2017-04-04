::Turning off line being exucuted layout:
@echo off
::Clearing console output:
::cls

ECHO Running build bootstrapper

ECHO Checking build outdated packages
.\.paket\paket.exe outdated

::Paket - alrernative NuGet client to nuget cli.
::http://fsprojects.github.io/Paket/index.html
ECHO Installing build packages
.\.paket\paket.exe install
if errorlevel 1 (
  ::Do not continue if packages are not restored
  exit /b %errorlevel%
)

ECHO Running FAKE build

::Standard configuration:
".\packages\FAKE\tools\Fake.exe" ".\build.fsx"

::Debug configuration:
::".\packages\FAKE\tools\Fake.exe" ".\build.fsx" buildType=Debug

exit /b %errorlevel%