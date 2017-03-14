::Turning off line being exucuted layout:
@echo off
::Clearing console output:
::cls

ECHO Running build bootstrapper

ECHO Checking build outdated packages
.\.paket\paket.exe outdated

ECHO Installing build packages
.\.paket\paket.exe install
if errorlevel 1 (
  ::Do not continue if packages are not restored
  exit /b %errorlevel%
)

ECHO Running FAKE build
".\packages\FAKE\tools\Fake.exe" ".\build.fsx"
exit /b %errorlevel%