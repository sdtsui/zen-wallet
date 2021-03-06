﻿module ContractExamples.QuotedContracts

open Microsoft.FSharp.Quotations
open Newtonsoft.Json.Linq
open Consensus.Types
open Consensus.Authentication
open System.Collections.Generic

type ContractFunctionInput = Contracts.ContractFunctionInput
type TransactionSkeleton = Contracts.TransactionSkeleton
type ContractFunction = Contracts.ContractFunction

let maybe = MaybeWorkflow.maybe
type InvokeMessage = byte * Outpoint list

let simplePackOutpoint : Outpoint -> byte[] = fun p ->
    match p with
    | {txHash=txHash;index=index} ->
        if index > 255u then failwith "oops!"
        else
            let res = Array.zeroCreate 33
            res.[0] <- (byte)index
            Array.blit txHash 0 res 1 32
            res

let packManyOutpoints : Outpoint list -> byte[] = fun ps ->
    ps |> List.map simplePackOutpoint |> Array.concat

let makeOutpoint (outpointb:byte[]) = {txHash=outpointb.[1..]; index = (uint32)outpointb.[0]}

let tryParseInvokeMessage (message:byte[]) =
    maybe {
        try
            let opcode, rest = message.[0], message.[1..]
            let outpointbytes = Array.chunkBySize 33 rest
            if outpointbytes |> Array.last |> Array.length <> 33 then   // >=1 outpoint
                failwith "last output has wrong length"
            let outpoints = Array.map makeOutpoint outpointbytes
            return opcode, outpoints
        with _ ->
            return! None
    }

let bytesToUInt64 : byte[] -> uint64 = fun bs ->
    if bs.Length <> 8 then failwith "wrong length byte array for uint64"
    let sysbytes =
        if System.BitConverter.IsLittleEndian then
            Array.rev bs
        else
            Array.copy bs
    System.BitConverter.ToUInt64 (sysbytes, 0)

let uint64ToBytes : uint64 -> byte[] = fun v ->
    let sysbytes = System.BitConverter.GetBytes v
    if System.BitConverter.IsLittleEndian then
        Array.rev sysbytes
    else
        sysbytes

type CallOptionParameters =
    {
        numeraire: Hash;
        controlAsset: Hash;
        controlAssetReturn: Hash;
        oracle: Hash;
        underlying: string;
        price: decimal;
        strike: decimal;
        minimumCollateralRatio: decimal;
        ownerPubKey: byte[]
    }
   
let BadTx : Expr<TransactionSkeleton> = <@ [], [], [||] @>

type DataFormat = uint64 * uint64 * uint64 // tokens issued, quantity of collateral, authenticated use counter

let tryParseData (data:byte[]) =
    maybe {
        try
            if data.Length <> 24 then
                failwith "data of wrong length"
            let tokens, collateral, counter = data.[0..7], data.[8..15], data.[16..23]
            return (bytesToUInt64 tokens, bytesToUInt64 collateral, bytesToUInt64 counter)
        with _ ->
            return! None
    }
let makeData (tokens, collateral, counter) = Array.concat <| List.map uint64ToBytes [tokens; collateral; counter]

let returnToSender (opoint:Outpoint, oput:Output) = List.singleton opoint, List.singleton oput, Array.empty<byte>

type SecureTokenParameters = {destination: Hash}
        
let secureTokenFactory : SecureTokenParameters -> Expr<ContractFunction> = fun _p ->
    <@
    let contractType = "securetoken"
    let meta = _p
    let p = dict [
        "destination", box <| _p.destination;
    ]
    fun (message, contracthash, utxos) ->
        maybe {
            let! opcode, outpoints = tryParseInvokeMessage message
            let! fundsLoc = Array.tryHead outpoints   
            let! funds = utxos fundsLoc
            let! commandSpend =
                match funds with
                | {
                    lock=ContractLock (contractHash=contractHash);
                    spend=spend
                  } when contractHash=contracthash
                    -> Some spend
                | _ -> None
            let destination = unbox<byte[]> <| p.Item "destination"
            let oput = {lock=PKLock destination; spend=commandSpend}
            // send a contract token as well
            let cput = {lock=PKLock destination; spend={asset=contracthash; amount=1000UL}}
            return ([fundsLoc;],[oput; cput;],[||])
        } |> Option.defaultValue %BadTx @>


let makeCollateralizeData (returnPubKeyHash:Hash) (counter:uint64) (keypair:Sodium.KeyPair) =
    let toSign = Array.append [|0uy|] <| uint64ToBytes counter
    let signature = sign toSign keypair.PrivateKey
    Array.concat [[|0uy|];returnPubKeyHash;signature]

let makeBuyData purchaserPubKeyHash = Array.append [|1uy|] purchaserPubKeyHash

let makeExerciseData ownerPubKeyHash oracleRawData = Array.concat[ [|2uy|]; ownerPubKeyHash; oracleRawData]

let makeCloseData returnPubKeyHash counter (keypair:Sodium.KeyPair) =
    let toSign = Array.append [|3uy|] <| uint64ToBytes counter
    let signature = sign toSign keypair.PrivateKey
    Array.concat [[|3uy|];returnPubKeyHash;signature]

//let callOptionFactory : CallOptionParameters -> Expr<ContractFunction> = fun _optParams ->
    //<@
    //fun (message,contracthash,utxos) ->

    //let contractType = "calloption"
    //let meta = _optParams

    //let optParams = dict[
    //    "numeraire", box _optParams.numeraire;
    //    "controlAsset", box _optParams.controlAsset;
    //    "controlAssetReturn", box _optParams.controlAssetReturn;
    //    "oracle", box _optParams.oracle;
    //    "underlying", box _optParams.underlying;
    //    "price", box _optParams.price;
    //    "strike", box _optParams.strike;
    //    "minimumCollateralRatio", box _optParams.minimumCollateralRatio;
    //    "ownerPubKey", box _optParams.ownerPubKey
    //]

    //let (|Collateralize|_|) (data:byte[]) =
    //    maybe {
    //        let! opcode = Array.tryHead data
    //        if opcode <> 0uy then
    //            return! None
    //        if data.Length <> 97 then
    //            return! None // TODO: return to default instead
    //        return (data.[1..32], data.[33..96])
    //    }

    //let (|Buy|_|) (data:byte[]) =
    //    maybe {
    //        let! opcode = Array.tryHead data
    //        if opcode <> 1uy then
    //            return! None
    //        if data.Length <> 33 then
    //            return! None
    //        return data.[1..32]
    //    }

    //let (|Exercise|_|) (data:byte[]) =
    //    maybe {
    //        let! opcode = Array.tryHead data
    //        if opcode <> 2uy then
    //            return! None
    //        if data.Length < 34 then
    //            return! None
    //        return (data.[1..32], data.[33..])
    //    }

    //let (|Close|_|) (data:byte[]) =
    //    maybe {
    //        let! opcode = Array.tryHead data
    //        if opcode <> 3uy then
    //            return! None
    //        if data.Length <> 97 then
    //            return! None // TODO: return to default instead
    //        return (data.[1..32], data.[33..96])
    //    }

    //let initialize :
    //        IDictionary<string,obj> ->
    //        Outpoint * Outpoint ->
    //        Output * Output ->
    //        Hash -> byte[] -> Hash -> TransactionSkeleton =
    //    fun optionParams
    //        ((x,y) as outpoints)
    //        ((b,d) as outputs)
    //        returnHash
    //        pubsig
    //        contractHash ->
    //            let optTx = maybe {
    //                let msg = [|0uy|]
    //                let ownerPubKey = unbox<byte[]> <| optionParams.Item "ownerPubKey"
    //                if not <| verify pubsig msg ownerPubKey then
    //                    return! None
    //                let numeraire = unbox<Hash> <| optionParams.Item "numeraire" 
    //                if b.spend.asset <> numeraire then
    //                    return! None
    //                let updated = (0UL, b.spend.amount, 1UL)
    //                let data = { d with lock=ContractLock (contractHash, makeData updated) }
    //                let funds = {
    //                    lock=ContractLock (contractHash, [||]);
    //                    spend={asset=numeraire;amount=b.spend.amount}
    //                }
    //                return ([x;y], [data;funds], [||])
    //            }
    //            match optTx with
    //            | Some tx -> tx
    //            | None ->
    //                let returnOutput = { lock=PKLock returnHash; spend=b.spend }
    //                returnToSender (x, returnOutput)


    //// TODO: time limits?
    //let collateralize :
    //          IDictionary<string,obj> ->
    //          Outpoint * Outpoint * Outpoint -> 
    //          Output * Output * Output -> 
    //          uint64 * uint64 * uint64 ->
    //          Hash -> byte[] -> Hash -> TransactionSkeleton =
    //    fun optionParams
    //        ((x,y,z) as outpoints)
    //        ((b,d,f) as outputs)
    //        (tokens, collateral, counter)
    //        returnHash
    //        pubsig
    //        contractHash ->
    //            let optTx = maybe {
    //                let msg = Array.append [|0uy|] <| uint64ToBytes counter
    //                let ownerPubKey = unbox<byte[]> <| optionParams.Item "ownerPubKey"
    //                if not <| verify pubsig msg ownerPubKey then
    //                    return! None
    //                let numeraire = unbox<Hash> <| optionParams.Item "numeraire"
    //                if b.spend.asset <> numeraire then
    //                    return! None
    //                let updated = (tokens, collateral + b.spend.amount, counter+1UL)
    //                let data = { d with lock=ContractLock (contractHash, makeData updated) }
    //                let funds = { f with spend={ f.spend with amount = f.spend.amount+b.spend.amount } }
    //                return ([x;y;z], [data;funds], [||])
    //            }
    //            match optTx with
    //            | Some tx -> tx
    //            | None ->
    //                let returnOutput = { lock=PKLock returnHash; spend=b.spend }
    //                returnToSender (x, returnOutput)

    //// TODO: collateral limits, use oracle, time limits
    //let buy : IDictionary<string,obj> ->
    //          Outpoint * Outpoint * Outpoint -> 
    //          Output * Output * Output -> 
    //          uint64 * uint64 * uint64 ->
    //          Hash -> Hash -> TransactionSkeleton =
    //    fun optionParams (x,y,z as outpoints) ((b,d,f) as outputs) optionData pubkeyhash contracthash ->
    //        let optTx = maybe {
    //            let numeraire = unbox<Hash> <| optionParams.Item "numeraire" 
    //            if b.spend.asset <> numeraire then
    //                return! None
    //            let! optionsPurchased =
    //                maybe { try
    //                            let price = unbox<decimal> <| optionParams.Item "price" 
    //                            let res = ((decimal)b.spend.amount / price) |> fun frac ->
    //                                if frac <= 0m then failwith "non-positive"
    //                                else frac |> floor |> (uint64)
    //                            return res
    //                        with _ -> return! None
    //                }
    //            if optionsPurchased = 0UL then
    //              return! None
    //            let tokens, collateral, counter = optionData
    //            let purchase = { lock=PKLock pubkeyhash; spend={ asset=contracthash;amount=optionsPurchased } }
    //            let data = { d with lock=ContractLock (
    //                                        contracthash,
    //                                        makeData (tokens + optionsPurchased,collateral + b.spend.amount,counter))}
    //            let funds = { f with spend={ f.spend with amount = f.spend.amount + b.spend.amount } }
    //            let outpointlist = [x;y;z]
    //            return (outpointlist,[purchase; data; funds],[||])
    //        }
    //        match optTx with
    //        | Some tx -> tx
    //        | None ->
    //            match x, b with
    //            | buypoint, buyput ->
    //                let returnOutput = { lock=PKLock pubkeyhash; spend=buyput.spend }
    //                returnToSender (buypoint, returnOutput)

    //let exercise : IDictionary<string,obj> ->
    //          Outpoint * Outpoint * Outpoint * (Outpoint option) -> 
    //          Output * Output * Output * (Output option)-> 
    //          uint64 * uint64 * uint64 ->
    //          Hash -> byte[] -> Hash -> TransactionSkeleton =
    //    fun optionParams (x,y,z,wOpt as outpoints) ((b,d,f,orOpt) as outputs) (tokens, collateral, counter) pubkeyhash auditB contracthash ->
    //        let optTx = maybe {
    //            let! w = wOpt
    //            let! orOutput = orOpt
    //            let oracle = unbox<Hash> <| optionParams.Item "oracle"
    //            let! root =
    //                match orOutput.lock with
    //                | ContractLock (orc, root) when orc = oracle -> Some root
    //                | _ -> None
    //            if b.spend.asset <> contracthash then
    //                return! None
    //            let! auditPath =
    //                try
    //                 auditB
    //                     |> System.Text.Encoding.ASCII.GetString
    //                     |> Oracle.fromPath
    //                     |> Some
    //                with
    //                    | _ -> None
    //            if root <> Merkle.rootFromAuditPath auditPath then return! None
    //            let auditJson = JObject.Parse(System.Text.Encoding.ASCII.GetString <| match auditPath with | (data,_,_) -> data)
    //            let underlying, price, timestamp =
    //                auditJson.Item("item") |> fun it -> it.Item("underlying").Value<string>(), it.Item("price").Value<decimal>(), it.Item("timestamp").Value<string>()
    //            let _underlying = unbox<string> <| optionParams.Item "underlying"
    //            if underlying <> _underlying then return! None
    //            // if not <| timestamp `near` currentTime then return! None
    //            let strike = unbox<decimal> <| optionParams.Item "strike"
    //            let intrinsic = price - strike
    //            if intrinsic <= 0m then return! None
    //            let collateralizedTokens = (decimal)collateral / intrinsic |> floor |> (uint64)
    //            if b.spend.amount > collateralizedTokens then // could create change, but it'd be a mess
    //                return! None
    //            let payoffAmt = intrinsic * decimal b.spend.amount |> floor |> (uint64)
    //            let remainingCollateral = collateral - payoffAmt
    //            let numeraire = unbox<Hash> <| optionParams.Item "numeraire"
    //            let payoff = { lock=PKLock pubkeyhash; spend={ asset=numeraire; amount=payoffAmt } }
    //            let data = { d with lock=ContractLock (
    //                                        contracthash,
    //                                        makeData (tokens-b.spend.amount, remainingCollateral, counter)) }
    //            let outputlist =
    //                if remainingCollateral <= 0UL then
    //                    [payoff;data]
    //                else
    //                    let funds = { f with spend={ f.spend with amount = remainingCollateral } }
    //                    [payoff;data;funds]
    //            return ([x;y;z], outputlist, [||])
    //        }
    //        match optTx with
    //        | Some tx -> tx
    //        | None ->
    //            let returnOutput = { lock=PKLock pubkeyhash; spend=b.spend }
    //            returnToSender (x, returnOutput)

    //// TODO: timelocks
    //let close : IDictionary<string,obj> ->
    //        Outpoint * Outpoint * Outpoint -> 
    //        Output * Output * Output -> 
    //        uint64 * uint64 * uint64 ->
    //        Hash -> byte[] -> Hash -> TransactionSkeleton =
    //    fun optionParams ((x,y,z) as outpoints) ((b,d,f) as outputs) (tokens, collateral, counter) returnHash pubsig contracthash ->
    //        let optTx = maybe {
    //            let msg = Array.append [|3uy|] <| uint64ToBytes counter
    //            let ownerPubKey = unbox<byte[]> <| optionParams.Item "ownerPubKey"
    //            if not <| verify pubsig msg ownerPubKey then
    //                return! None
    //            let numeraire = unbox<Hash> <| optionParams.Item "numeraire"
    //            if b.spend.asset <> numeraire then
    //                return! None
    //            let funds = {f with lock=PKLock returnHash}
    //            let control = {d with lock=PKLock returnHash}
    //            let returnOutput = {b with lock = PKLock returnHash}
    //            return ([x;y;z], [returnOutput;funds;control], [||])
    //        }
    //        match optTx with
    //        | Some tx -> tx
    //        | None ->
    //            let returnOutput = { lock=PKLock returnHash; spend=b.spend }
    //            returnToSender (x, returnOutput)

    //maybe {
    //    // parse message, obtaining opcode and three outpoints
    //    let! opcode, outpoints = tryParseInvokeMessage message
    //    let! commandLoc, dataLoc =
    //        if Array.length outpoints < 2 then None
    //        else
    //            Some <| (Array.get outpoints 0, Array.get outpoints 1)
    //    // try to get the outputs. Fail early if they aren't there!
    //    let! commandOutput = utxos commandLoc
    //    let! dataOutput = utxos dataLoc
    //    let! commandData, commandSpend =
    //        match commandOutput with
    //        | {
    //            lock=ContractLock (contractHash=contractHash; data=data);
    //            spend=spend
    //          } when contractHash=contracthash
    //            -> Some (data, spend)
    //        | _ -> None
    //    // get the user's command
    //    // opcodes must match
    //    let! commandOp = Array.tryHead commandData
    //    if opcode <> commandOp then
    //        return! None
    //    // the contract's data output must own the control token
    //    let controlAsset = unbox<byte[]> <| optParams.Item "controlAsset"
    //    let! optionsOwnData =
    //        match dataOutput with
    //        | {
    //            lock=ContractLock (contractHash=contractHash; data=data);
    //            spend={asset=asset}
    //          } when contractHash = contracthash && asset = controlAsset
    //            -> Some <| data
    //        | _ -> None // short-circuiting
    //    let initTxSkeleton =
    //        if opcode = 0uy && outpoints.Length = 2 then    // Special case: initial collateral
    //            if optionsOwnData <> [||] then None else
    //            match commandData with
    //            | Collateralize (returnAddress, pubsig) ->
    //                Some <| initialize
    //                            optParams
    //                            (commandLoc, dataLoc)
    //                            (commandOutput, dataOutput)
    //                            returnAddress
    //                            pubsig
    //                            contracthash
    //            | _ -> None
    //        else None

    //    // switch on commands
    //    let! txskeleton =
    //        if Option.isSome initTxSkeleton then initTxSkeleton else
    //        if outpoints.Length < 3 then None else
    //        maybe {
    //            // validate funds (to stop lying about amount of collateralization)
    //            let fundsLoc = outpoints.[2]
    //            let! fundsOutput = utxos fundsLoc
    //            let! tokens, collateral, counter = tryParseData optionsOwnData
    //            let otherPoints = outpoints.[3..]
    //            let numeraire = unbox<Hash> <| optParams.Item "numeraire"
    //            if fundsOutput.spend.asset <> numeraire || fundsOutput.spend.amount <> collateral
    //            then
    //                return! None
    //            else
                
    //            return! match commandData with
    //                    | Collateralize (returnAddress, pubsig) ->
    //                        Some <| collateralize
    //                                    optParams
    //                                    (commandLoc, dataLoc, fundsLoc)
    //                                    (commandOutput, dataOutput, fundsOutput)
    //                                    (tokens, collateral, counter)
    //                                    returnAddress
    //                                    pubsig
    //                                    contracthash
    //                    | Buy pubkeyhash ->
    //                        Some <| buy
    //                                    optParams
    //                                    (commandLoc, dataLoc, fundsLoc)
    //                                    (commandOutput, dataOutput, fundsOutput) 
    //                                    (tokens, collateral, counter)
    //                                    pubkeyhash
    //                                    contracthash
    //                    | Exercise (pubkeyhash, auditPath) ->
    //                        let oracleLoc = Array.tryHead otherPoints
    //                        let oracleOutput = Option.bind utxos oracleLoc
    //                        Some <| exercise
    //                                    optParams
    //                                    (commandLoc, dataLoc, fundsLoc, oracleLoc)
    //                                    (commandOutput, dataOutput, fundsOutput, oracleOutput) 
    //                                    (tokens, collateral, counter)
    //                                    pubkeyhash
    //                                    auditPath
    //                                    contracthash
    //                    | Close (pubkeyhash, pubsig) ->
    //                        Some <| close
    //                                    optParams
    //                                    (commandLoc, dataLoc, fundsLoc)
    //                                    (commandOutput, dataOutput, fundsOutput) 
    //                                    (tokens, collateral, counter)
    //                                    pubkeyhash
    //                                    pubsig
    //                                    contracthash
    //                    | _ ->
    //                         None
    //        }

    //    return txskeleton

    //} |> Option.defaultValue %BadTx 
    //@>

type OracleParameters =
    {
        ownerPubKey: byte[]
    }

let oracleFactory : OracleParameters -> Expr<ContractFunction> = fun oParams ->
    <@
    fun (message,contracthash,utxos) ->
        let contractType = "oracle"
        let meta = oParams
        maybe {
            if message.Length <> 129 then return! None
            let m, s = message.[0..64], message.[65..128]
            if not <| verify s m oParams.ownerPubKey then return! None
            let opoint = {txHash=m.[1..32]; index = (uint32)m.[0]}
            let! oput = utxos opoint
            let dataOutput = {
                spend={asset=contracthash; amount=1UL};
                lock=ContractLock (contracthash, m.[33..64])
            }
            return ([opoint;], [oput; dataOutput], [||])
        } |> Option.defaultValue %BadTx
    @>