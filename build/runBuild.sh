#Init bash shell
#!/bin/bash
#Clearing console output:
#printf '\33c\e[3J'

ECHO Running build bootstrapper

ECHO Checking build outdated packages
mono ./.paket/paket.exe outdated

#Paket - alrernative NuGet client to nuget cli.
#http://fsprojects.github.io/Paket/index.html
ECHO Installing build packages
mono ./.paket/paket.exe install

ECHO Running FAKE build

#Standard configuration:
mono ./packages/FAKE/tools/Fake.exe ./buildScripts/build.fsx

#Debug configuration:
#mono ./packages/FAKE/tools/Fake.exe ./buildScripts/build.fsx buildType=Debug