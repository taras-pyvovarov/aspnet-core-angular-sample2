#r "./packages/fake/tools/FakeLib.dll" 
open System.IO
open Fake
open Fake.NpmHelper

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

let currentDir = FileSystemHelper.currentDirectory
let solutionRootDir = Directory.GetParent(currentDir).FullName;
let entryProjectRootDir = solutionRootDir @@ @"src\aspnet-core-angular-sample2";
let publishDir = solutionRootDir @@ @"publishfiles"


let dotnetVersion = DotNetCli.getVersion ()

let dotnetPublish = (fun () ->
    DotNetCli.Restore (fun p -> 
    { p with 
        NoCache = true;
        Project = entryProjectRootDir @@ @"aspnet-core-angular-sample2.csproj";
    })

    DotNetCli.Publish (fun p -> 
    { p with 
        Configuration = "Release";
        Project = entryProjectRootDir @@ @"aspnet-core-angular-sample2.csproj";
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
    //Running vendor webpack flow.
    let command = entryProjectRootDir @@ @"node_modules\.bin\webpack.cmd"
    let args = @"--config webpack.config.vendor.js"
    let workingDir = entryProjectRootDir
    //printfn "Command: %s %s" command args
    let result = Shell.Exec(command, args, workingDir)

    //Running app webpack flow.
    let command = entryProjectRootDir @@ @"node_modules\.bin\webpack.cmd"
    let args = @"--config webpack.config.js"
    let workingDir = entryProjectRootDir
    //printfn "Command: %s %s" command args
    let result = Shell.Exec(command, args, workingDir)
)

let copyNodeModules = (fun () ->
    FileHelper.CopyDir (publishDir @@ @"node_modules") (entryProjectRootDir @@ @"node_modules") allFiles
)

let zipBuildFiles = (fun () ->
    !! (publishDir @@ "**" @@ "*.*") 
        |> Zip publishDir (solutionRootDir @@ @"publisharchive\appBuild.zip")
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