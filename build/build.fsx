#r "./packages/fake/tools/FakeLib.dll" 
open System
open Fake
open System.IO
open Microsoft.FSharp.Reflection
open FSharp.Data

//Welcome FAKE build text
let buildHeaderText = """
***************************************
***********FAKE BUILD SAMPLE***********
***************************************
"""

Target "ShowHeader" (fun _ ->
    printfn "%s" buildHeaderText
)

Target "Build" (fun _ ->
    printfn "%s" "Hello"
)


// Target "Build" (fun _ ->
//     let result = ExecProcessAndReturnMessages(fun p ->
//                 p.FileName <- "powershell.exe"
//                 //p.Arguments <- commandFormatString)
//                 (TimeSpan.FromMinutes 5.0)
//     //let result = ExecProcess (fun info -> 
//       //  info.FileName <- "c:/MyProc.exe" 
//         //info.WorkingDirectory <- "c:/workingDirectory" 
//         //info.Arguments <- "-v"
//     //) (TimeSpan.FromMinutes 5.0)

//     if result <> 0 then 
//         failwithf "MyProc.exe returned with a non-zero exit code"
//         //"dotnet --help"
// )

"ShowHeader"
==> "Build"



RunTargetOrDefault "Build"