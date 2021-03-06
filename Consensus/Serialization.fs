﻿module Consensus.Serialization

open Consensus.Types

open MsgPack
open MsgPack.Serialization
open System.Runtime.Serialization



type OutputLockSerializer(ownerContext) =
    inherit MessagePackSerializer<OutputLock>(ownerContext: SerializationContext)

    override __.PackToCore(packer: Packer, oLock: OutputLock) =
        match oLock with
        | HighVLock (lockCore = lCore) when lCore.version = 0u ->
            // possible code smell; this is really validation logic
            raise <| new SerializationException("Cannot serialize malformed HighVLock")
        | _ ->
            let bodyLength = if List.isEmpty <| lockData oLock then 2 else 3
            packer.PackArrayHeader(bodyLength)
                  .Pack(lockVersion oLock)
                  .Pack(typeCode oLock) |> ignore
            if bodyLength = 3 then packer.PackArray(lockData oLock) |> ignore
        
    override __.UnpackFromCore(unpacker: Unpacker) : OutputLock =
        let mutable tC = 0
        if not <| unpacker.IsArrayHeader then
            raise <| new SerializationException("OutputLock is not an array")
        else
            use subtreeReader = unpacker.ReadSubtree()
            if subtreeReader.ItemsCount <> 2L && subtreeReader.ItemsCount <> 3L then
                raise <| new SerializationException("Wrong number of items in OutputLock")
            subtreeReader.Read() |> ignore
            let v = subtreeReader.Unpack<uint32>()
            subtreeReader.Read() |> ignore
            let tC = subtreeReader.Unpack<int32>()
            let lD =
                if subtreeReader.ItemsCount = 3L then
                    subtreeReader.Read() |> ignore
                    subtreeReader.Unpack<byte[] list>(ownerContext) else
                    List.empty
            let lCore = {version=v;lockData=lD} : LockCore
            match v, tC with
                | _, 0 ->
                    CoinbaseLock lCore
                | _, 1 ->
                    FeeLock lCore
                | _, 2 ->
                    ContractSacrificeLock lCore
                | v, tC when (v > 0u) -> 
                    HighVLock (lockCore=lCore,typeCode=tC)
                | _, 3 when lD.Length <> 1 ->
                    raise <| new SerializationException("PKLock has too many items")
                | _, 3 ->
                    let hd = lD.Head
                    if hd.Length <> PubKeyHashBytes then
                        raise <| new SerializationException("PKLock hash is wrong length")
                    else
                        PKLock hd
                | _, 4 when lD.Length <> 2 ->
                    raise <| new SerializationException("ContractLock has wrong number of items")
                | _, 4 when lD.[0].Length <> ContractHashBytes ->
                    raise <| new SerializationException("ContractLock hash is wrong length")
                | _, 4 ->
                    ContractLock (lD.[0], lD.[1])
                | _ ->
                    raise <| new SerializationException("Bad typeCode for version 0 OutputLock")

type SpendSerializer(ownerContext) =
    inherit MessagePackSerializer<Spend>(ownerContext: SerializationContext)

    override __.PackToCore(packer: Packer, {asset=asset; amount=amount}: Spend) =
        packer.PackArrayHeader(2).PackBinary(asset).Pack(amount)
        |> ignore
           
    override __.UnpackFromCore(unpacker: Unpacker) : Spend =
        if not <| unpacker.IsArrayHeader then
            raise <| new SerializationException("Spend is not an array")
        else
            let mutable asset:Hash = Hash.Empty()
            let mutable amount:uint64 = 0uL
            use subtreeReader = unpacker.ReadSubtree()
            if subtreeReader.ItemsCount <> 2L then
                raise <| new SerializationException(sprintf "Wrong number of items in Spend: %i" subtreeReader.ItemsCount)
            elif not <| subtreeReader.ReadBinary(&asset) then
                raise <| new SerializationException("Bad asset for Spend")
            elif not <| subtreeReader.ReadUInt64(&amount) then
                raise <| new SerializationException("Bad amount for Spend")
            // TODO: Review this check to determine if it should occur here.
            elif asset.Length <> ContractHashBytes then
                raise <| new SerializationException("Asset of Spend has the wrong length.")
            else 
                {asset=asset; amount=amount} : Spend

type OutputSerializer(ownerContext) =
    inherit MessagePackSerializer<Output>(ownerContext: SerializationContext)

    override __.PackToCore(packer: Packer, {lock=lock; spend=spend}: Output) =
        packer.PackArrayHeader(2)
              .Pack<OutputLock>(lock,ownerContext)
              .Pack<Spend>(spend,ownerContext)
        |> ignore
    
    override __.UnpackFromCore(unpacker: Unpacker) : Output =
        if not <| unpacker.IsArrayHeader then
            raise <| new SerializationException("Output is not an array")
        else
            use subtreeReader = unpacker.ReadSubtree()
            if subtreeReader.ItemsCount <> 2L then
                raise <| new SerializationException("Wrong number of items in Output")
            else
                subtreeReader.Read() |> ignore
                let lock = subtreeReader.Unpack<OutputLock>(ownerContext)
                subtreeReader.Read() |> ignore
                let spend = subtreeReader.Unpack<Spend>(ownerContext)
                {lock=lock; spend=spend} : Output
        

type OutpointSerializer(ownerContext) =
    inherit MessagePackSerializer<Outpoint>(ownerContext: SerializationContext)

    override __.PackToCore(packer: Packer, {txHash=txHash; index=index}: Outpoint) =
        packer.PackArrayHeader(2).PackBinary(txHash).Pack(index)
        |> ignore
    
    override __.UnpackFromCore(unpacker: Unpacker) : Outpoint =
        if not <| unpacker.IsArrayHeader then
            raise <| new SerializationException("Outpoint is not an array")
        else
            use subtreeReader = unpacker.ReadSubtree()
            if subtreeReader.ItemsCount <> 2L then
                raise <| new SerializationException("Wrong number of items in Outpoint")
            else
                subtreeReader.Read() |> ignore
                let txHash = subtreeReader.Unpack<Hash>(ownerContext)
                subtreeReader.Read() |> ignore
                let index = subtreeReader.Unpack<uint32>(ownerContext)
                {txHash=txHash; index=index} : Outpoint

type ContractSerializer(ownerContext) =
    inherit MessagePackSerializer<Contract>(ownerContext: SerializationContext)

    override __.PackToCore(packer: Packer, {code=code;bounds=bounds;hint=hint}: Contract) =
        packer.PackArray([code; bounds; hint])
        |> ignore
    
    override __.UnpackFromCore(unpacker: Unpacker) : Contract =
       if not <| unpacker.IsArrayHeader then
           raise <| new SerializationException("Spend is not an array")
       else
           use subtreeReader = unpacker.ReadSubtree()
           if subtreeReader.ItemsCount <> 3L then
               raise <| new SerializationException("Wrong number of items in Contract")
           else
               subtreeReader.Read() |> ignore 
               let code = subtreeReader.Unpack<byte[]>()
               subtreeReader.Read() |> ignore
               let bounds = subtreeReader.Unpack<byte[]>()
               subtreeReader.Read() |> ignore
               let hint = subtreeReader.Unpack<byte[]>()
               {code=code; bounds=bounds; hint=hint} : Contract

type ExtendedContractSerializer(ownerContext) =
    inherit MessagePackSerializer<ExtendedContract>(ownerContext: SerializationContext)

    override __.PackToCore(packer: Packer, eContract: ExtendedContract) =
        packer.PackArrayHeader(2)
         .Pack((contractVersion eContract),ownerContext) |> ignore
        match eContract with
            | HighVContract(data=data) -> packer.PackBinary(data) |> ignore
            | Contract ct ->
                use contractStream = new System.IO.MemoryStream()
                use contractPacker = Packer.Create(contractStream)
                contractPacker.Pack<Contract>(ct,ownerContext) |> ignore
                packer.PackBinary(contractStream.ToArray()) |> ignore
              
    override __.UnpackFromCore(unpacker: Unpacker) : ExtendedContract =
        if not <| unpacker.IsArrayHeader then
            raise <| new SerializationException("ExtendedContract is not an array")
        use subtreeReader = unpacker.ReadSubtree()
        if subtreeReader.ItemsCount <> 2L then
            raise <| new SerializationException("Wrong number of items in ExtendedContract")
        subtreeReader.Read() |> ignore
        let v = subtreeReader.Unpack<uint32>()
        subtreeReader.Read() |> ignore
        let contractData = subtreeReader.Unpack<byte[]>()
        match v with
            | 0u ->
                let contractStream = new System.IO.MemoryStream(contractData) 
                let contractUnpacker = Unpacker.Create(contractStream)
                contractUnpacker.Read() |> ignore
                let ct = contractUnpacker.Unpack<Contract>(ownerContext)
                if contractUnpacker.Read() then
                    raise <| new SerializationException("Too many items in extended contract data")
                Contract ct
            | _ ->
                HighVContract(version=v,data=contractData)


type TransactionSerializer(ownerContext) =
    inherit MessagePackSerializer<Transaction>(ownerContext: SerializationContext)
    member this.typeCode:byte = 1uy

    override __.PackToCore(packer: Packer, tx: Transaction) =
        use body = new System.IO.MemoryStream()
        use bodyPacker = Packer.Create(body)
        let bodyLength = match tx.contract with
                         | None -> 4
                         | Some _ -> 5
        bodyPacker.PackArrayHeader(bodyLength)
         .Pack(tx.version)
         .PackArray(tx.inputs,ownerContext)
         .PackArray(tx.witnesses,ownerContext)
         .PackArray(tx.outputs,ownerContext) |> ignore
        if tx.contract.IsSome then
            bodyPacker.Pack<ExtendedContract>(tx.contract.Value, ownerContext) |> ignore
        packer.PackExtendedTypeValue(__.typeCode,body.ToArray())
        |> ignore

    
    override __.UnpackFromCore(unpacker: Unpacker) : Transaction =
        let extObj = unpacker.LastReadData.AsMessagePackExtendedTypeObject()
        if extObj.TypeCode <> __.typeCode then
            raise <| new SerializationException("Not a transaction")
        use body = new System.IO.MemoryStream(extObj.GetBody())
        use bodyUnpacker = Unpacker.Create(body)
        bodyUnpacker.Read() |> ignore
        if not <| bodyUnpacker.IsArrayHeader then
            raise <| new SerializationException("tx body not an array")
        use subtreeReader = bodyUnpacker.ReadSubtree()
        if subtreeReader.ItemsCount <> 4L && 
           subtreeReader.ItemsCount <> 5L then
               raise <| new SerializationException "wrong number of items in tx body"
        subtreeReader.Read() |> ignore
        let version = subtreeReader.Unpack<uint32>()
        subtreeReader.Read() |> ignore
        let inputs = subtreeReader.Unpack<Outpoint list>(ownerContext)
        subtreeReader.Read() |> ignore
        let witnesses = subtreeReader.Unpack<Witness list>(ownerContext)
        subtreeReader.Read() |> ignore
        let outputs = subtreeReader.Unpack<Output list>(ownerContext)
        let contract =
            if subtreeReader.ItemsCount = 5L then
                subtreeReader.Read() |> ignore
                Some <| subtreeReader.Unpack<ExtendedContract>(ownerContext) else
                None
        {version=version; inputs=inputs; witnesses=witnesses; outputs=outputs; contract=contract}


type BlockHeaderSerializer(ownerContext) =
    inherit MessagePackSerializer<BlockHeader>(ownerContext: SerializationContext)
    member this.typeCode:byte = 3uy

    override __.PackToCore(packer: Packer, bh: BlockHeader) =
        use body = new System.IO.MemoryStream()
        use bodyPacker = Packer.Create(body)
        bodyPacker.PackArrayHeader(7)
         .Pack<uint32>(bh.version)
         .Pack<Hash>(bh.parent)
         .Pack<uint32>(bh.blockNumber)
         .Pack<Hash list>(merkleData bh)
         .Pack<int64>(bh.timestamp)
         .Pack<uint32>(bh.pdiff)
         .Pack<byte[]>(bh.nonce)
         |> ignore
        packer.PackExtendedTypeValue(__.typeCode,body.ToArray())
        |> ignore
    
    override __.UnpackFromCore(unpacker: Unpacker) : BlockHeader =
        let extObj = unpacker.LastReadData.AsMessagePackExtendedTypeObject()
        if extObj.TypeCode <> __.typeCode then
            raise <| new SerializationException("Not a block header")
        use body = new System.IO.MemoryStream(extObj.GetBody())
        use bodyUnpacker = Unpacker.Create(body)
        bodyUnpacker.Read() |> ignore
        if not <| bodyUnpacker.IsArrayHeader then
            raise <| new SerializationException("Block header is not an array")
        use subtreeReader = bodyUnpacker.ReadSubtree()
        if subtreeReader.ItemsCount <> 7L then
            raise <| SerializationException("Wrong number of items in block header")
        subtreeReader.Read() |> ignore
        let version = subtreeReader.Unpack<uint32>()
        subtreeReader.Read() |> ignore
        let parent = subtreeReader.Unpack<Hash>(ownerContext)
//        if not <| subtreeReader.IsArrayHeader then
//            raise <| SerializationException("merkle roots not in array")
        subtreeReader.Read() |> ignore
        let blockNumber = subtreeReader.Unpack<uint32>(ownerContext)
        subtreeReader.Read() |> ignore
        let mData = subtreeReader.Unpack<Hash list>(ownerContext)
        if mData.Length < 3 then
            raise <| SerializationException("too few merkle roots in array")
        let txMR = mData.Item(0)
        let wMR = mData.Item(1)
        let cMR = mData.Item(2)
        let exData = List.skip 3 mData
        subtreeReader.Read() |> ignore
        let timestamp = bodyUnpacker.Unpack<int64>()
        subtreeReader.Read() |> ignore
        let pdiff = bodyUnpacker.Unpack<uint32>()
        subtreeReader.Read() |> ignore
        let nonce = bodyUnpacker.Unpack<Nonce>(ownerContext)
        {
            version=version;
            parent=parent;
            blockNumber=blockNumber;
            txMerkleRoot=txMR;
            witnessMerkleRoot=wMR;
            contractMerkleRoot=cMR;
            extraData=exData;
            timestamp=timestamp;
            pdiff=pdiff;
            nonce=nonce
        }

type BlockSerializer(ownerContext) =
    inherit MessagePackSerializer<Block>(ownerContext: SerializationContext)
    member this.typeCode:byte = 2uy

    override __.PackToCore(packer: Packer, blk: Block) =
        use body = new System.IO.MemoryStream()
        use bodyPacker = Packer.Create(body,ownerContext.CompatibilityOptions.PackerCompatibilityOptions)
        bodyPacker.PackArrayHeader(2)
         .Pack(blk.header,ownerContext)
         .PackArray(blk.transactions,ownerContext)
         |> ignore
        packer.PackExtendedTypeValue(__.typeCode,body.ToArray())
        |> ignore

    override __.UnpackFromCore(unpacker: Unpacker) : Block =
        let  extObj = unpacker.LastReadData.AsMessagePackExtendedTypeObject()
        if extObj.TypeCode <> __.typeCode then
            raise <| new SerializationException("Not a block")        
        use body = new System.IO.MemoryStream(extObj.GetBody())
        use bodyUnpacker = Unpacker.Create(body)
        bodyUnpacker.Read() |> ignore
        if not <| bodyUnpacker.IsArrayHeader then
            raise <| new SerializationException("Block is not an array")
        use subtreeReader = bodyUnpacker.ReadSubtree()
        if subtreeReader.ItemsCount <> 2L then
            raise <| new SerializationException("wrong number of items in block")
        subtreeReader.Read() |> ignore
        let header = subtreeReader.Unpack<BlockHeader>(ownerContext)
        subtreeReader.Read() |> ignore
        let transactions = subtreeReader.Unpack<Transaction list>(ownerContext)
        {header=header;transactions=transactions}

let context = new SerializationContext()

//TODO setup reading the typecode from the serializers

context.CompatibilityOptions.PackerCompatibilityOptions <- PackerCompatibilityOptions.None

context.Serializers.RegisterOverride<OutputLock>(new OutputLockSerializer(context))
context.Serializers.RegisterOverride<Spend>(new SpendSerializer(context))
context.Serializers.RegisterOverride<Output>(new OutputSerializer(context))
context.Serializers.RegisterOverride<Outpoint>(new OutpointSerializer(context))
context.Serializers.RegisterOverride<Contract>(new ContractSerializer(context))
context.Serializers.RegisterOverride<ExtendedContract>(new ExtendedContractSerializer(context))
context.Serializers.RegisterOverride<Transaction>(new TransactionSerializer(context))
context.Serializers.RegisterOverride<BlockHeader>(new BlockHeaderSerializer(context))
context.Serializers.RegisterOverride<Block>(new BlockSerializer(context))

context.ExtTypeCodeMapping.Add("Transaction", 0x01uy) |> ignore
context.ExtTypeCodeMapping.Add("Block", 0x02uy) |> ignore
context.ExtTypeCodeMapping.Add("BlockHeader", 0x03uy) |> ignore
