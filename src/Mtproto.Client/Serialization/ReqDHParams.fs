module Serialization.ReqDHParams

open Bytes
open Enumerator
open DecodeExtensions
open Messages

[<Literal>]
let ClassId = 0xd712e4be

let serialize (dh : ReqDHParams) =
    [ Encode.fromInt32L ClassId
      Encode.fromInt128L dh.Nonce
      Encode.fromInt128L dh.ServerNonce
      Encode.fromInt64L dh.P
      Encode.fromInt64L dh.Q
      Encode.fromInt64L dh.Fingerprint
      dh.EncryptedData ]
    |> Array.concat

let deserialize byteSeq =
    { Nonce = byteSeq |> nextInt128L
      ServerNonce =  byteSeq |> nextInt128L
      P = byteSeq |> nextInt64L
      Q = byteSeq |> nextInt64L
      Fingerprint = byteSeq |> nextInt64L
      EncryptedData = byteSeq |> next 260 }
    |> ReqDHParams