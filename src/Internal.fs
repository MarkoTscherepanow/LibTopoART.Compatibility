#if DEBUG
module LibTopoART.Compatibility.Internal
#else
module internal LibTopoART.Compatibility.Internal
#endif

open LibTopoART

type NetID =
    | TopoART
    | Fast_TopoART
    | Hypersphere_TopoART
    | Fast_Episodic_TopoART
    | Fast_TopoART_AM
    | TopoART_C
    | Fast_TopoART_C
    | Hypersphere_TopoART_C
    | TopoART_R
    | Fast_TopoART_R

//----------------------------------------------------------------------------------------------------------------------

(*type System.Int64 with
    static member op_Implicit(value : int64) : int64 = value*)

let inline convert (value : ^a) : ^b = ((^a or ^b) : (static member op_Implicit : ^a -> ^b) value)

let inline toArray_Decimal(a : ^a[]) =
    if a <> null then
        let converted = Array.create a.Length 0.0m
        if a.Length > 0 then
            for i in 0..a.Length-1 do
                converted[i] <- decimal a[i]
        converted
    else
        null

let inline toArray_Double(a : ^a[]) =
    if a <> null then
        let converted = Array.create a.Length 0.0
        if a.Length > 0 then
            for i in 0..a.Length-1 do
                converted[i] <- double a[i]
        converted
    else
        null

let load path =
    let info = Common.LoadBinaryHeader(path)
    if info.IntType.HasValue then
        if info.FloatType.HasValue then
            match (info.Type, info.IntType.Value, info.FloatType.Value) with
                | NetworkType.TopoART, TypeIndex.LongIndex, TypeIndex.DecimalIndex -> (NetID.TopoART, new TopoART(path) :> ITopoART)
                | NetworkType.TopoART, TypeIndex.LongIndex, TypeIndex.IntIndex -> (NetID.Fast_TopoART, new Fast_TopoART(path) :> ITopoART)
                | NetworkType.HypersphereTopoART, TypeIndex.LongIndex, TypeIndex.DecimalIndex -> (NetID.Hypersphere_TopoART, new Hypersphere_TopoART(path) :> ITopoART)
                | NetworkType.EpisodicTopoART, TypeIndex.LongIndex, TypeIndex.IntIndex -> (NetID.Fast_Episodic_TopoART, new Fast_Episodic_TopoART(path) :> ITopoART)
                | NetworkType.TopoARTAM, TypeIndex.LongIndex, TypeIndex.IntIndex -> (NetID.Fast_TopoART_AM, new Fast_TopoART_AM(path) :> ITopoART)
                | NetworkType.TopoARTC, TypeIndex.LongIndex, TypeIndex.DecimalIndex -> (NetID.TopoART_C, new TopoART_C(path) :> ITopoART)
                | NetworkType.TopoARTC, TypeIndex.LongIndex, TypeIndex.IntIndex -> (NetID.Fast_TopoART_C, new Fast_TopoART_C(path) :> ITopoART)
                | NetworkType.HypersphereTopoARTC, TypeIndex.LongIndex, TypeIndex.DecimalIndex -> (NetID.Hypersphere_TopoART_C, new Hypersphere_TopoART_C(path) :> ITopoART)
                | NetworkType.TopoARTR, TypeIndex.LongIndex, TypeIndex.DecimalIndex -> (NetID.TopoART_R, new TopoART_R(path) :> ITopoART)
                | NetworkType.TopoARTR, TypeIndex.LongIndex, TypeIndex.IntIndex -> (NetID.Fast_TopoART_R, new Fast_TopoART_R(path) :> ITopoART)
                | _ -> raise (InvalidFileException(Common.InvalidFileException_UnsupportedNetworkType))
        else
             raise (InvalidFileException(Common.InvalidFileException_UnsupportedFloatType))
    else
        raise (InvalidFileException(Common.InvalidFileException_UnsupportedIntegerType))
