module Serialization.Enumerator

open System.Collections.Generic

let getEnumerator (array:'a[]) =
    let enumarable  = array :> IEnumerable<'a>
    enumarable.GetEnumerator()

let private nextElement (enumerator:IEnumerator<'a>) =
    match enumerator.MoveNext() with
    | true -> enumerator.Current
    | false -> failwith "There are no more elements"

let next length (enumerator:IEnumerator<'a>) =
    Array.init length (fun _ -> nextElement enumerator)

let skip length (enumerator:IEnumerator<'a>) =
    for _ in 1..length do
        match enumerator.MoveNext() with
        | true -> ()
        | false -> failwith "There are no more elements"
    enumerator