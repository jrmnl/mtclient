namespace Bytes

type Order =
    | LittleEndian
    | BigEndian

type Bytes = 
    | InLittleEndian of byte[]
    | InBigEndian of byte[]