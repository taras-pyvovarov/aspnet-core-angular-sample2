//******Build entry point******
#r "./../packages/fake/tools/FakeLib.dll" 

#load "./generic.fsx"

open System.IO
open Fake
open BuildGeneric

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


//******Build params******
let buildType = EnvironmentHelper.getBuildParamOrDefault "buildType" "Release"

//******Initial & calculated properties******
let isDebug = if buildType = "Debug" then true else false
let currentDir = FileSystemHelper.currentDirectory
let solutionRootDir = Directory.GetParent(currentDir).FullName;
let entryProjectRootDir = solutionRootDir @@ "src" @@ "aspnet-core-angular-sample2";
let entryProjectCsproj = entryProjectRootDir @@ "aspnet-core-angular-sample2.csproj";
let publishDir = solutionRootDir @@ "publishfiles"
let publishDirRelative = ".." @@ "publishfiles"
let publishArchiveDir = solutionRootDir @@ "publisharchive"
let publishArchivePath = publishArchiveDir @@ "appBuild.zip"
//All webpack arguments in the order as they are executed.
let webpackArgs = [| 
    (if isDebug then "" else "-p ") + "--config webpack.config.vendor.js"
    (if isDebug then "" else "-p ") + "--config webpack.config.js"
|]
let dotnetVersion = DotNetCli.getVersion ()


//******Build logic******

let copyNodeModules = (fun () ->
    FileHelper.CopyDir (publishDir @@ "node_modules") (entryProjectRootDir @@ "node_modules") allFiles
)


//Welcome text, build init messages
Target "BuildInitMessage" (fun _ ->
    printfn "******Running FAKE build******"

    printfn ">Input params"
    printfn "Build type: %s" buildType

    printfn ">Environment settings"
    printfn "Dotnet CLI version: %s" dotnetVersion
    printfn "Solution root directory: %s" solutionRootDir
    printfn "Entry project root directory: %s" entryProjectRootDir
    printfn "Current directory: %s" currentDir
)

//Installs all packages needed that should be installed during build.
Target "NpmInstall" (fun _ ->
    //Installting packages for entry project
    npmInstall(entryProjectRootDir)
)

Target "WebpackBuild" (fun _ ->
    //Running webpack for entry project
    runWebpack(entryProjectRootDir, webpackArgs)
)

//Builds all dotnet projects needed.
Target "Build" (fun _ ->
    //Building entry project
    dotnetPublish(isDebug, entryProjectCsproj, publishDir)
)

Target "CopyNodeModules" (fun _ ->
    copyNodeModules()
)

//Zips project build output into zip archive.
Target "ZipBuildFiles" (fun _ ->
    zipDir(publishDirRelative, publishArchivePath)
)


"BuildInitMessage"
==> "NpmInstall"
==> "WebpackBuild"
==> "CopyNodeModules"
==> "ZipBuildFiles"

"BuildInitMessage"
==> "Build"
==> "ZipBuildFiles"

RunTargetOrDefault "ZipBuildFiles"