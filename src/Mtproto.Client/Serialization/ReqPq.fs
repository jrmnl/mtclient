module Serialization.ReqPq

open Bytes
open DecodeExtensions
open Messages

[<Literal>]
let ClassId = 0x60469778

let serialize (repPq : ReqPq) =
    [ Encode.fromInt32L ClassId
      Encode.fromInt128L repPq.Nonce]
    |> Array.concat

let deserialize byteSeq =
    { Nonce = byteSeq |> nextInt128L }
    |> ReqPq