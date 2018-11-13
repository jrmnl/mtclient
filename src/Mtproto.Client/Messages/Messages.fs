namespace Messages

open Types

type VectorLong = int64[]

type ReqPq = { Nonce: int128 }

type ResPq =
    { Nonce: int128
      ServerNonce: int128
      Pq : bigint
      Fingerprints : VectorLong }
      
type ReqDHParams =
    { Nonce: int128
      ServerNonce: int128
      P: int64
      Q: int64
      Fingerprint: int64
      EncryptedData: byte[] }

type Message =
    | ReqPq of ReqPq
    | ResPq of ResPq
    | ReqDHParams of ReqDHParams

type UnencryptedMessage =
    { AuthKeyId : int64 
      MessageId : int64
      Message : Message }