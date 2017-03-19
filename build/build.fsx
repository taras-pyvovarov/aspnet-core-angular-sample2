#r "./packages/fake/tools/FakeLib.dll" 
open System.IO
open Fake
open Fake.NpmHelper

//Working with EnvironmentHelper:
//http://fsharp.github.io/FAKE/apidocs/fake-environmenthelper.html
//Working with FileSystemHelper:
//http://fsharp.github.io/FAKE/apidocs/fake-filesystemhelper.html
//Working with DotNetCli:
//http://fsharp.github.io/FAKE/apidocs/fake-dotnetcli.html
//Working with NpmHelper:
//http://fsharp.github.io/FAKE/apidocs/fake-npmhelper.html
//Working with FileHelper:
//http://fsharp.github.io/FAKE/apidocs/fake-filehelper.html
//Working with ZipHelper:
//http://fsharp.github.io/FAKE/apidocs/fake-ziphelper.html

let buildHeaderText = """
---------------------------------------------------------
********************FAKE BUILD SAMPLE********************
---------------------------------------------------------
"""
let isWindows = EnvironmentHelper.isWindows
let currentDir = FileSystemHelper.currentDirectory
let solutionRootDir = Directory.GetParent(currentDir).FullName;
let entryProjectRootDir = solutionRootDir @@ "src" @@ "aspnet-core-angular-sample2";
let entryProjectNodeModules = entryProjectRootDir @@ "node_modules";
let publishDir = solutionRootDir @@ "publishfiles"
let publishDirRelative = ".." @@ "publishfiles"

//All webpack arguments in the order as they are executed.
let webpackArgs = [| 
    "-p --config webpack.config.vendor.js";
    "-p --config webpack.config.js"
|]


let dotnetVersion = DotNetCli.getVersion ()

let dotnetPublish = (fun () ->
    DotNetCli.Restore (fun p -> 
    { p with 
        NoCache = true;
        Project = entryProjectRootDir @@ "aspnet-core-angular-sample2.csproj";
    })

    DotNetCli.Publish (fun p -> 
    { p with 
        Configuration = "Release";
        Project = entryProjectRootDir @@ "aspnet-core-angular-sample2.csproj";
        //dotnet CLI publish interprets relative path with project folder as a starting point
        //MacOS doest allow '..' navigation inside path
        //Output = @"..\..\buildArtifact";
        Output = publishDir;
    })
)

let npmInstall = (fun () ->
    Npm (fun p ->
    { p with
        Command = Install Standard
        WorkingDirectory = entryProjectRootDir
    })
)

let webpackBuild = (fun () ->
    let command = entryProjectNodeModules @@ ".bin" @@ if isWindows then "webpack.cmd" else "webpack"

    //Run webpack with specific args in project root as working dir.
    for args in webpackArgs do
        printfn "Command: %s %s" command args
        let exitCode = Shell.Exec(command, args, entryProjectRootDir)
        printfn "Exit code: %i" exitCode
)

let copyNodeModules = (fun () ->
    FileHelper.CopyDir (publishDir @@ "node_modules") entryProjectNodeModules allFiles
)

let zipBuildFiles = (fun () ->
    //In MacOS only relative path is accepted. Absolute is refered as relative.
    !! (publishDirRelative @@ "**" @@ "*.*") 
        |> Zip publishDirRelative (solutionRootDir @@ "publisharchive" @@ "appBuild.zip")
)


//Welcome text, build init messages
Target "ShowInitMessage" (fun _ ->
    printfn "%s" buildHeaderText
    printfn "Dotnet CLI version: %s" dotnetVersion
    printfn "Solution root directory: %s" solutionRootDir
    printfn "Entry project root directory: %s" entryProjectRootDir
    printfn "Current directory: %s" currentDir
)

Target "NpmInstall" (fun _ ->
    npmInstall()
)

Target "WebpackBuild" (fun _ ->
    webpackBuild()
)

Target "Build" (fun _ ->
    dotnetPublish()
)

Target "CopyNodeModules" (fun _ ->
    copyNodeModules()
)

Target "ZipBuildFiles" (fun _ ->
    zipBuildFiles()
)


"ShowInitMessage"
==> "NpmInstall"
==> "WebpackBuild"
==> "Build"
==> "CopyNodeModules"
==> "ZipBuildFiles"


RunTargetOrDefault "ZipBuildFiles"