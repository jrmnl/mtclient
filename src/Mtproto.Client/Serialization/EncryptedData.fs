module EncryptedData

open Bytes
open Messages

[<Literal>]
let private ClassId = 0x83c95aec

//let serialize (data : EncryptedData) =
//    [ data.Pq
//      data.P
//      data.Q
//      data.Nonce
//      data.ServerNonce
//      data.NewNonce
//
//      
//      
//      
//      
//      
//      ]
//    |> Array.concat