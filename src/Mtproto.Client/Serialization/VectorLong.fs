module Serialization.VectorLong

open Bytes
open DecodeExtensions

[<Literal>]
let private ClassId = 0x1cb5c415

let private flatMap mapping array =
    array 
    |> Array.map mapping
    |> Array.reduce Array.append

let deserialize byteSeq =
    byteSeq |> validateInt32ClassId ClassId
    byteSeq |> ignoreLength |> ignore 

    let value = byteSeq |> nextInt64L
    [|value|]

let serialize list =
    list |> flatMap Encode.fromInt64L
        