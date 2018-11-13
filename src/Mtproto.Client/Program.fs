module Program

open Bytes
open Serialization
open System

let withLengthField (bytes : byte[]) =
    let lengthField = bytes.Length |> Encode.fromInt32L
    Array.append lengthField bytes

let pack = UnencryptedMessage.serialize >> withLengthField

let sendMessage message connection = async {
    let package = pack message
    printfn "Sending message: %A\n" message
    do! connection |> TcpClient.send package
}

let recieveResPq connection = async {
    let! response = connection |> TcpClient.tryRead 140
    match response with
    | Ok(package) ->
        let message = UnencryptedMessage.deserialize package
        printfn "Recieve message: %A\n" message
        return Ok(message)
    | Error(msg) ->
        printfn "Recieving of message failed: %s\n" msg
        return Error(msg)
}

let workflow = async {
    use! connection = TcpClient.connect "localhost" 3000
    do! connection |> sendMessage (Random.getReqPq())
    let! resPq = recieveResPq connection
    match resPq with
    | Ok(_) ->
        do! connection |> sendMessage (Random.getReqDHParams())
        return Ok()
    | Error(msg) -> return Error(msg)
}

[<EntryPoint>]
let main _ =
    let result = workflow |> Async.RunSynchronously

    Console.ReadLine() |> ignore
    match result with
    | Ok(_) -> 0
    | Error(_) -> -1