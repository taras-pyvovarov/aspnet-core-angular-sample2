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