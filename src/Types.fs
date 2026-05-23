namespace LibTopoART.Compatibility

open LibTopoART

//----------------------------------------------------------------------------------------------------------------------

/// <summary>Enum <c>Network</c> represents the available network implementations. Its values correspond to the
/// class names of LibTopoART.</summary>
type Network =

    /// <summary>This implementation of TopoART stores real-valued data in <c>decimal</c> variables. Hence, computations
    /// are rather slow but very accurate.</summary>
    | TopoART = 0x1

    /// <summary>This implementation of TopoART maps real-valued data to <c>int32</c> variables. Hence, computations
    /// are accelerated but less accurate. As a consequence, the results may differ slightly from class <c>TopoART</c>.
    /// </summary>
    | Fast_TopoART = 0x2

    /// <summary>Implementation of Hypersphere TopoART based on class <c>TopoART</c>.</summary>
    | Hypersphere_TopoART = 0x8

    /// <summary>This implementation of TopoART-C stores real-valued data in <c>decimal</c> variables. Hence, computations
    /// are rather slow but very accurate.</summary>
    | TopoART_C = 0x11

    /// <summary>This implementation of TopoART-C maps real-valued data to <c>int32</c> variables. Hence, computations
    /// are accelerated but less accurate. As a consequence, the results may differ slightly from class <c>TopoART_C</c>.
    /// </summary>
    | Fast_TopoART_C = 0x12

    /// <summary>Implementation of Hypersphere TopoART-C based on class <c>Hypersphere_TopoART_C</c>.</summary>
    | Hypersphere_TopoART_C = 0x18
    
    /// <summary>This implementation of TopoART-R stores real-valued data in <c>decimal</c> variables. Hence, computations
    /// are rather slow but very accurate.</summary>
    | TopoART_R = 0x21
    
    /// <summary>This implementation of TopoART-R maps real-valued data to <c>int32</c> variables. Hence, computations
    /// are accelerated but less accurate. As a consequence, the results may differ slightly from class <c>TopoART_R</c>.
    /// </summary>
    | Fast_TopoART_R = 0x22
    
    /// <summary>This implementation of TopoART-AM maps real-valued data to <c>int32</c> variables. Hence, computations
    /// are accelerated but less accurate.
    /// </summary>
    | Fast_TopoART_AM = 0x42

//----------------------------------------------------------------------------------------------------------------------

/// <summary>Struct <c>CategoryInfo_i64d</c> summarises information about a node's category.</summary>
[<Struct>]
type CategoryInfo_i64d =

    /// <summary>Instance variable <c>spatial_weights</c> represents the spatial weights of the
    /// considered node.</summary>
    val spatial_weights : double[]

    /// <summary>Instance variable <c>temporal_weights</c> represents the temporal weights of
    /// the considered node if it supports temporal learning.</summary>
    val temporal_weights : double[]

    /// <summary>Instance variable <c>clusterID</c> represents the cluster ID of the considered
    /// node.</summary>
    val clusterID : int64

    /// <summary>Instance variable <c>classID</c> represents the class ID of the considered 
    /// node. If class IDs are not supported by the respective node, this value is set to
    /// <c>LibTopoART_info.UNDEFINED</c>.</summary>
    val classID : int64

    /// <summary>This constructor creates an instance of struct <c>CategoryInfo_i64d</c> from an instance of struct
    /// <c>CategoryInfo</c>.</summary>
    internal new(info : CategoryInfo) = {
        spatial_weights = Internal.toArray_Double info.spatial_weights
        temporal_weights = Internal.toArray_Double info.temporal_weights
        clusterID = info.clusterID
        classID = info.classID
    }

//----------------------------------------------------------------------------------------------------------------------

/// <summary>Class <c>F2_output_i64d</c> provides the output of a single TopoART module. It 
/// is a compressed version of the output vectors y and c.</summary>
type F2_output_i64d =

    /// <summary>Instance variable <c>bm_node_activation</c> represents the activation of the 
    /// best-matching node (prediction variant).</summary>
    val bm_node_activation : double

    /// <summary>Instance variable <c>bm_node_ID</c> represents the ID of the best-matching 
    /// node.</summary>
    val bm_node_ID : int64

    /// <summary>Instance variable <c>bm_cluster_ID</c> represents the cluster ID of the 
    /// best-matching node.</summary>
    val bm_cluster_ID : int64

    /// <summary>Instance variable <c>bm_permanent_node_activation</c> represents the activation 
    /// of the best-matching permanent node (prediction variant).</summary>
    val bm_permanent_node_activation : double

    /// <summary>Instance variable <c>bm_permanent_node_ID</c> represents the ID of the 
    /// best-matching permanent node.</summary>
    val bm_permanent_node_ID : int64

    /// <summary>Instance variable <c>bm_permanent_cluster_ID</c> represents the cluster 
    /// ID of the best-matching permanent node.</summary>
    val bm_permanent_cluster_ID : int64;

    /// <summary>This constructor initializes all fields with <c>LibTopoART_info.UNDEFINED</c>.</summary>
    internal new() = {
        bm_node_activation = double LibTopoART_info.UNDEFINED
        bm_node_ID = LibTopoART_info.UNDEFINED
        bm_cluster_ID = LibTopoART_info.UNDEFINED
        bm_permanent_node_activation = double LibTopoART_info.UNDEFINED
        bm_permanent_node_ID = LibTopoART_info.UNDEFINED
        bm_permanent_cluster_ID = LibTopoART_info.UNDEFINED
    }

    /// <summary>This constructor creates an instance of class <c>F2_output_i64d</c> from an instance of class
    /// <c>F2_output</c>.</summary>
    internal new(output : F2_output) = {
        bm_node_activation = double output.bm_node_activation
        bm_node_ID = output.bm_node_ID
        bm_cluster_ID = output.bm_cluster_ID
        bm_permanent_node_activation = double output.bm_permanent_node_activation
        bm_permanent_node_ID = output.bm_permanent_node_ID
        bm_permanent_cluster_ID = output.bm_permanent_cluster_ID
    }

//----------------------------------------------------------------------------------------------------------------------

/// <summary>Struct <c>RecallResponse_d</c> contains a recall response made by an Episodic TopoART network or a
/// TopoART-AM network using the data type <c>double</c>.</summary>
[<Struct>]
type RecallResponse_d =

    /// <summary>Instance variable <c>recallResult</c> represents the recall output. A value of <c>null</c> indicates
    /// a failed recall step.</summary>
    val recallResult : double[]

    /// <summary>Instance variable <c>F3_activation</c> represents the activation of the F3 node used for recall.
    /// A value of <c>LibTopoART_info.UNDEFINED</c> indicates a failed recall step.</summary>
    val F3_activation : double

    /// <summary>This constructor creates an instance of struct <c>RecallResponse_d</c>.</summary>
    /// <param name="recallResult">recall result</param>
    /// <param name="activation">F3 activation</param>
    internal new(recallResult : decimal[], activation : decimal) = {
        recallResult = Internal.toArray_Double recallResult
        F3_activation = double activation
    }

//----------------------------------------------------------------------------------------------------------------------

/// <summary>Struct <c>TopoART_C_prediction_i64d</c> contains a prediction made by a TopoART-C network.</summary>
[<Struct>]
type TopoART_C_prediction_i64d =

    /// <summary>Instance variable <c>classID</c> gives the predicted class ID.</summary>
    val classID : int64

    /// <summary>Instance variable <c>confidence</c> provides a confidence for the predicted class ID.</summary>
    val confidence : double

    /// <summary>This constructor creates an instance of struct <c>TopoART_C_prediction_i64d</c> from an instance of
    /// struct <c>TopoART_C_prediction</c>.</summary>
    internal new(prediction : TopoART_C_prediction) = {
        classID    =   prediction.classID
        confidence =   double prediction.confidence
    }

//----------------------------------------------------------------------------------------------------------------------

/// <summary>Struct <c>TopoART_R_prediction_d</c> contains a prediction made by a TopoART-R network using the data type
/// <c>double</c>.</summary>
[<Struct>]
type TopoART_R_prediction_d =  

    /// <summary>Instance variable <c>NO_PREDICTION</c> provides a default prediction for variables that are presented
    /// to the network; i.e., these variables are known and no prediction is computed for them.</summary>
    val NO_PREDICTION : double

    /// <summary>Instance variable <c>i_vec_prediction</c> represents predictions for unknown independent variables.
    /// </summary>
    val i_vec_prediction : double[]

    /// <summary>Instance variable <c>d_vec_prediction</c> provides the predictions for the dependent variables.
    /// </summary>
    val d_vec_prediction : double[]

    /// <summary>This constructor creates an instance of class <c>TopoART_R_prediction_d</c> from an instance of
    /// class <c>TopoART_R_prediction&lt;decimal&gt;</c>.</summary>
    internal new(prediction : TopoART_R_prediction<decimal>) = {
        NO_PREDICTION = double prediction.NO_PREDICTION
        i_vec_prediction = Internal.toArray_Double prediction.i_vec_prediction
        d_vec_prediction = Internal.toArray_Double prediction.d_vec_prediction
    }

//----------------------------------------------------------------------------------------------------------------------

module internal Types =
    let inline toResizeArray_CategoryInfo_i64d(a : ResizeArray<CategoryInfo>) =
        if a <> null then
            let converted = ResizeArray<CategoryInfo_i64d>(a.Count)
            if a.Count > 0 then
                for i in 0..a.Count-1 do
                    converted.Add(CategoryInfo_i64d(a[i]))
            converted
        else
            null

    let inline toArray_F2_output_i64d(a : F2_output[]) =
        if a <> null then
            let converted = Array.create a.Length <| F2_output_i64d()
            if a.Length > 0 then
                for i in 0..a.Length-1 do
                    converted[i] <- F2_output_i64d(a[i])
            converted
        else
            null

    let inline toNetID(net_type : Network) =
        match net_type with
            | Network.TopoART -> Internal.NetID.TopoART
            | Network.Fast_TopoART -> Internal.NetID.Fast_TopoART
            | Network.Hypersphere_TopoART -> Internal.NetID.Hypersphere_TopoART
            | Network.Fast_TopoART_AM -> Internal.NetID.Fast_TopoART_AM
            | Network.TopoART_C -> Internal.NetID.TopoART_C
            | Network.Fast_TopoART_C -> Internal.NetID.Fast_TopoART_C
            | Network.Hypersphere_TopoART_C -> Internal.NetID.Hypersphere_TopoART_C
            | Network.TopoART_R -> Internal.NetID.TopoART_R
            | Network.Fast_TopoART_R -> Internal.NetID.Fast_TopoART_R
            | _ -> raise (InvalidTypeException(Common.InvalidTypeException_UnsupportedNetworkType))
