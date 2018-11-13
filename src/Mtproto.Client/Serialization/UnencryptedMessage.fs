module Serialization.UnencryptedMessage

open Bytes
open DecodeExtensions
open Enumerator
open Messages

let private withInt32Length (bytes:byte[]) =
    let lengthField = bytes.Length |> Encode.fromInt32L
    Array.append lengthField bytes
    
let serializeMessage message =
    match message with
    | ReqPq(reqPq) -> ReqPq.serialize reqPq
    | ResPq(resPq) -> ResPq.serialize resPq
    | ReqDHParams(dh) -> ReqDHParams.serialize dh

let serialize unencryptedMessage =
    [ unencryptedMessage.AuthKeyId |> Encode.fromInt64L
      unencryptedMessage.MessageId |> Encode.fromInt64L
      unencryptedMessage.Message |> serializeMessage |> withInt32Length ]
    |> Array.concat

let (|ReqPqId|ResPqId|) byteSeq =
    let id = byteSeq |> nextInt32L
    if id = ReqPq.ClassId then ReqPqId
    elif id = ResPq.ClassId then ResPqId
    else failwith "Unknown type"
    
let deserializeMessage byteSeq =
    match byteSeq with
    | ReqPqId -> ReqPq.deserialize byteSeq
    | ResPqId -> ResPq.deserialize byteSeq

let deserialize bytes =
    let enumerator = getEnumerator bytes
    { AuthKeyId = enumerator |> nextInt64L
      MessageId = enumerator |> nextInt64L
      Message = enumerator |> ignoreLength |> deserializeMessage }
