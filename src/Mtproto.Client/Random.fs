module Random

open System
open Bytes
open Types
open Messages

let private generateBytes length =
    let buffer = Array.zeroCreate length
    Random().NextBytes(buffer)
    buffer

let getInt32() = Random().Next()
let getInt64() = generateBytes 8 |> Decode.toInt64L
let getInt128() = generateBytes 16 |> bigint |> Int128.create
let getBigInt = generateBytes >> bigint
    
let private getReqPqMessage() =
    { Nonce = getInt128() }
    |> ReqPq

let private getReqDHParamsMessage() =
    { Nonce = getInt128()
      ServerNonce = getInt128()
      P = getInt64()
      Q = getInt64()
      Fingerprint = getInt64()
      EncryptedData = generateBytes 260 }
    |> ReqDHParams

let private getTlMessage inner =
    { AuthKeyId = getInt64()
      MessageId = getInt64()
      Message = inner }

let getReqPq = getReqPqMessage >> getTlMessage
let getReqDHParams = getReqDHParamsMessage >> getTlMessage