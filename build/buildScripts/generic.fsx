//******Build module with generic functions******
module BuildGeneric

open Fake
open Fake.NpmHelper

//Working with NpmHelper:
//http://fsharp.github.io/FAKE/apidocs/fake-npmhelper.html

//Installs npm packets for working dir that supposed to store package.json
let npmInstall = (fun (workingDir) ->
    NpmHelper.Npm (fun p ->
    { p with
        Command = Install Standard
        WorkingDirectory = workingDir
    })
)

//Makes restore and publish of given csproj into publish dir. Will do debug or release build depending in flag.
let dotnetPublish = (fun (isDebug, csprojFile, publishDir) ->
    DotNetCli.Restore (fun p -> 
    { p with 
        NoCache = true;
        Project = csprojFile;
    })

    DotNetCli.Publish (fun p -> 
    { p with 
        Configuration = if isDebug then "Debug" else "Release";
        Project = csprojFile;
        //dotnet CLI publish interprets relative path with project folder as a starting point
        //MacOS doest allow '..' navigation inside path
        //Output = @"..\..\buildArtifact";
        Output = publishDir;
    })
)