namespace Bytes

open System
open System.Buffers.Binary
open System.Numerics
open Types

module ByteLength =
    [<Literal>]
    let ofInt32 = 4
    [<Literal>]
    let ofInt64 = 8
    [<Literal>]
    let ofInt128 = 16

module Decode =

    let private checkLength requiredLength bytes =
        match bytes with
        | InBigEndian(bytes) | InLittleEndian(bytes) ->
            if bytes.Length <> requiredLength
            then 
                sprintf "Array length = %i; Required length = %i" bytes.Length requiredLength
                |> failwith
            else ()

    let toInt32 bytes =
        bytes |> checkLength ByteLength.ofInt32
        match bytes with
        | InBigEndian(bytes) -> BinaryPrimitives.ReadInt32BigEndian(ReadOnlySpan(bytes))
        | InLittleEndian(bytes) -> BinaryPrimitives.ReadInt32LittleEndian(ReadOnlySpan(bytes))

    let toInt32L bytes = toInt32 (InLittleEndian(bytes))
    let toInt32B bytes = toInt32 (InBigEndian(bytes))


    let toInt64 bytes =
        bytes |> checkLength ByteLength.ofInt64
        match bytes with
        | InBigEndian(bytes) -> BinaryPrimitives.ReadInt64BigEndian(ReadOnlySpan(bytes))
        | InLittleEndian(bytes) -> BinaryPrimitives.ReadInt64LittleEndian(ReadOnlySpan(bytes))

    let toInt64L bytes = toInt64 (InLittleEndian(bytes))
    let toInt64B bytes = toInt64 (InBigEndian(bytes))
        

    let toBigInt bytes =
        match bytes with
        | InBigEndian(bytes) -> BigInteger(ReadOnlySpan(bytes), isUnsigned = true, isBigEndian = true)
        | InLittleEndian(bytes) -> BigInteger(ReadOnlySpan(bytes), isUnsigned = true, isBigEndian = false)

    let toBigIntL bytes = toBigInt (InLittleEndian(bytes))
    let toBigIntB bytes = toBigInt (InBigEndian(bytes))
    

    let toInt128 bytes =
        bytes |> checkLength ByteLength.ofInt128
        toBigInt bytes |> Int128.create

    let toInt128L bytes = toInt128 (InLittleEndian(bytes))
    let toInt128B bytes = toInt128 (InBigEndian(bytes))

module Encode =

    let fromInt32 byteOrder int32 =
        let bytes = Array.zeroCreate ByteLength.ofInt32
        match byteOrder with
        | LittleEndian -> BinaryPrimitives.WriteInt32LittleEndian(Span(bytes), int32)
        | BigEndian -> BinaryPrimitives.WriteInt32BigEndian(Span(bytes), int32)
        bytes

    let fromInt32L = fromInt32 LittleEndian
    let fromInt32B = fromInt32 BigEndian


    let fromInt64 byteOrder int64 =
        let bytes = Array.zeroCreate ByteLength.ofInt64
        match byteOrder with
        | LittleEndian -> BinaryPrimitives.WriteInt64LittleEndian(Span(bytes), int64)
        | BigEndian -> BinaryPrimitives.WriteInt64BigEndian(Span(bytes), int64)
        bytes

    let fromInt64L = fromInt64 LittleEndian
    let fromInt64B = fromInt64 BigEndian


    let fromBigInt byteLength byteOrder (bigint : bigint)  =
        let bytes = Array.zeroCreate byteLength
        let isBigEndian =
            match byteOrder with
            | LittleEndian -> false
            | BigEndian -> true
        match bigint.TryWriteBytes(Span(bytes), ref 0, isBigEndian) with
        | true -> bytes
        | false -> failwith "Can't write bytes to array"

    let fromBigIntL byteLength = fromBigInt byteLength LittleEndian
    let fromBigIntB byteLength = fromBigInt byteLength BigEndian


    let fromInt128 byteOrder int128 =
        Int128.extractBigInt int128
        |> fromBigInt ByteLength.ofInt128 byteOrder

    let fromInt128L = fromInt128 LittleEndian
    let fromInt128B = fromInt128 BigEndian