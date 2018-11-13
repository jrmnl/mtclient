module TcpClient

open System.Net.Sockets

let connect (host : string) port = async {
    let client = new TcpClient()
    do! client.ConnectAsync(host, port) |> Async.AwaitTask
    return client
}

let send buffer (client : TcpClient) = async {
    use stream = new NetworkStream(client.Client, false)
    do! stream.WriteAsync(buffer, 0, buffer.Length) |> Async.AwaitTask
}

let tryRead length (client : TcpClient) = async {
    use stream = new NetworkStream(client.Client, false)
    let buffer = Array.zeroCreate length
    let! bytesRead = stream.ReadAsync(buffer, 0, length) |> Async.AwaitTask

    if (bytesRead = length) 
    then return Ok(buffer)
    else return Error(sprintf "Stream read %i bytes instead of %i" bytesRead length)
}