namespace Types

type int128 = private Int128 of bigint

module Int128 =
    let private isValidToInt128 =
        let maxValue = bigint.Pow(bigint(2), 128) - bigint(1)
        let minValue = bigint.Pow(bigint(-2), 127)
        let betweenMinAndMax (value : bigint)=
            match maxValue.CompareTo(value), value.CompareTo(minValue) with
            | (-1, _) | (_, -1) -> false
            | _ -> true
        betweenMinAndMax
    
    let create bigint =
        match isValidToInt128 bigint with
        | true -> Int128 bigint
        | false -> raise (System.ArgumentException("It's not Int128 value"))

    let extractBigInt (Int128 e) = e