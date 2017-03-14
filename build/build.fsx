#r "./packages/fake/tools/FakeLib.dll" 
open Fake

//Working with FileSystemHelper:
//http://fsharp.github.io/FAKE/apidocs/fake-filesystemhelper.html
//Working with DotNetCli:
//http://fsharp.github.io/FAKE/apidocs/fake-dotnetcli.html

let buildHeaderText = """
***************************************
***********FAKE BUILD SAMPLE***********
***************************************
"""

let currentDir = FileSystemHelper.currentDirectory
let dotnetPublishDir = currentDir @@ @"\..\buildArtifact"

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
        //Output = @"..\..\buildArtifact";
        Output = dotnetPublishDir;
    })
)

//Welcome text, build init messages
Target "ShowInitMessage" (fun _ ->
    printfn "%s" buildHeaderText
    printfn "dotnet CLI version: %s" dotnetVersion
    printfn "current directory: %s" currentDir
)

Target "Build" (fun _ ->
    dotnetPublish()
)

"ShowInitMessage"
==> "Build"



RunTargetOrDefault "Build"