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

let buildHeaderText = """
---------------------------------------------------------
********************FAKE BUILD SAMPLE********************
---------------------------------------------------------
"""

let currentDir = FileSystemHelper.currentDirectory
let projectRootDir = Directory.GetParent(currentDir).FullName;
let dotnetPublishDir = projectRootDir @@ "buildArtifact"


let dotnetVersion = DotNetCli.getVersion ()

let dotnetPublish = (fun () ->
    DotNetCli.Restore (fun p -> 
    { p with 
        NoCache = true;
        Project = @"..\src\aspnet-core-angular-sample2\aspnet-core-angular-sample2.csproj";
    })

    DotNetCli.Publish (fun p -> 
    { p with 
        Configuration = "Release";
        Project = @"..\src\aspnet-core-angular-sample2\aspnet-core-angular-sample2.csproj";
        //dotnet CLI publish interprets relative path with project folder as a starting point
        //MacOS doest allow '..' navigation inside path
        //Output = @"..\..\buildArtifact";
        Output = dotnetPublishDir;
    })
)

let npmInstall = (fun () ->
    Npm (fun p ->
    { p with
        Command = Install Standard
        WorkingDirectory = @"..\src\aspnet-core-angular-sample2\"
    })
)

let webpackBuild = (fun () ->
    //Running vendor webpack flow.
    let command = projectRootDir @@ @"src\aspnet-core-angular-sample2\node_modules\.bin\webpack.cmd"
    let args = @"--config webpack.config.vendor.js"
    let workingDir = @"..\src\aspnet-core-angular-sample2\"
    //printfn "Command: %s %s" command args
    let result = Shell.Exec(command, args, workingDir)

    //Running app webpack flow.
    let command = projectRootDir @@ @"src\aspnet-core-angular-sample2\node_modules\.bin\webpack.cmd"
    let args = @"--config webpack.config.js"
    let workingDir = @"..\src\aspnet-core-angular-sample2\"
    //printfn "Command: %s %s" command args
    let result = Shell.Exec(command, args, workingDir)
)

let copyNodeModules = (fun () ->
    FileHelper.CopyDir (dotnetPublishDir @@ @"node_modules") (projectRootDir @@ @"src\aspnet-core-angular-sample2\node_modules") allFiles
)


//Welcome text, build init messages
Target "ShowInitMessage" (fun _ ->
    printfn "%s" buildHeaderText
    printfn "dotnet CLI version: %s" dotnetVersion
    printfn "root directory: %s" projectRootDir
    printfn "current directory: %s" currentDir
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


"ShowInitMessage"
==> "NpmInstall"
==> "WebpackBuild"
==> "Build"
==> "CopyNodeModules"


RunTargetOrDefault "CopyNodeModules"