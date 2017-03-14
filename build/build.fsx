#r "./packages/fake/tools/FakeLib.dll" 

open Fake


let buildHeaderText = """
***************************************
***********FAKE BUILD SAMPLE***********
***************************************
"""

let dotnetVersion = DotNetCli.getVersion ()

//Welcome text, build init messages
Target "ShowInitMessage" (fun _ ->
    printfn "%s" buildHeaderText
    printfn "dotnet CLI version: %s" dotnetVersion
)

Target "Build" (fun _ ->
    trace "hello"
)

"ShowInitMessage"
==> "Build"



RunTargetOrDefault "ShowInitMessage"