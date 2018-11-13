module Serialization.DecodeExtensions

open Bytes
open Enumerator

let nextInt32L enumerator =
    enumerator |> next ByteLength.ofInt32 |> Decode.toInt32L

let nextInt64L enumerator =
    enumerator |> next ByteLength.ofInt64 |> Decode.toInt64L

let nextInt128L enumerator =
    enumerator |> next ByteLength.ofInt128 |> Decode.toInt128L

let nextBigIntL length enumerator =
    enumerator |> next length |> Decode.toBigIntL
    
let ignoreLength byteSeq = byteSeq |> skip 4

let validateInt32ClassId requiredClassId byteSeq =
    let classId = byteSeq |> nextInt32L
    if requiredClassId = classId
    then ()
    else failwith "Class id not matched required value"