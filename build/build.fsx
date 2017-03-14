#r "./packages/fake/tools/FakeLib.dll" 

open Fake


let buildHeaderText = """
***************************************
***********FAKE BUILD SAMPLE***********
***************************************
"""

//---------------------Working with dotnet CLI---------------------
//http://fsharp.github.io/FAKE/apidocs/fake-dotnetcli.html

let dotnetVersion = DotNetCli.getVersion ()

let dotnetPublish = (fun () ->
    DotNetCli.Restore (fun p -> 
    { p with 
        NoCache = true;
        Project = @"..\src\aspnet-core-angular-sample2\aspnet-core-angular-sample2.csproj";
    })
)

//Welcome text, build init messages
Target "ShowInitMessage" (fun _ ->
    printfn "%s" buildHeaderText
    printfn "dotnet CLI version: %s" dotnetVersion
)

Target "Build" (fun _ ->
    dotnetPublish()
)

"ShowInitMessage"
==> "Build"



RunTargetOrDefault "Build"