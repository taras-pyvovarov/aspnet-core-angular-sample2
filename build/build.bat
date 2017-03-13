::Turning off line being exucuted layout:
@echo off
::Clearing console output:
::cls

ECHO Running build batch bootstrapper file

ECHO Installing required nuget packages
..\.paket\paket.exe install
if errorlevel 1 (
  exit /b %errorlevel%
)

ECHO Running build
"..\packages\FAKE\tools\Fake.exe" .\build.fsx