module Serialization.ResPq

open Bytes
open DecodeExtensions
open Messages

[<Literal>]
let ClassId = 0x05162463

let serialize (resPq: ResPq) =
    [ Encode.fromInt32L ClassId
      Encode.fromInt128L resPq.Nonce
      Encode.fromInt128L resPq.ServerNonce
      Encode.fromBigIntL 64 resPq.Pq 
      VectorLong.serialize resPq.Fingerprints ]
    |> Array.concat

let deserialize byteSeq =
    { Nonce = byteSeq |> nextInt128L
      ServerNonce = byteSeq |> nextInt128L
      Pq = byteSeq |> nextBigIntL 64
      Fingerprints = byteSeq |> ignoreLength |> VectorLong.deserialize }
    |> ResPq