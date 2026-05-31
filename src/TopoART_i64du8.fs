namespace LibTopoART.Compatibility

open LibTopoART

//**********************************************************************************************************************

/// <summary>Class <c>TopoART_i64du8</c> is a wrapper for the TopoART implementations contained in LibTopoART. It
/// forwards the major methods and properties using the data types <c>int64</c> and <c>double</c>. Input is provided as
/// <c>uint8</c> and internally scaled from [0, 255] to [0, 1]. This makes class <c>TopoART_i64du8</c> suitable for
/// image data.</summary>
type TopoART_i64du8 =
    inherit TopoART_i64d_common

//----------------------------------------------------------------------------------------------------------------------

    /// <summary>This constructor initialises a TopoART(-C) network. Input values are internally scaled from [0, 255]
    /// to [0, 1].</summary>
    /// <param name="inputLen">The length of input vectors to be learnt.</param>
    /// <param name="moduleNum">The number of TopoART modules.</param>
    /// <param name="rho_a">The vigilance parameter of the first TopoART module.</param>
    /// <param name="netType">The type of the network to be created. (supported types: <c>Fast_TopoART</c>
    /// and <c>Fast_TopoART_C</c>)</param>
    /// <exception cref="InvalidTypeException">Throws when the selected network type is not supported.</exception>
    new(inputLen : int64, moduleNum : int64, rho_a : double, netType : Network) = {
        inherit TopoART_i64d_common(Types.toNetID netType,
            match netType with
                | Network.Fast_TopoART -> new Fast_TopoART(inputLen, moduleNum, decimal rho_a) :> ITopoART
                | Network.Fast_TopoART_C -> new Fast_TopoART_C(inputLen, moduleNum, decimal rho_a) :> ITopoART
                | _ -> raise (InvalidTypeException(Common.InvalidTypeException_UnsupportedNetworkType)))
    }

    /// <summary>This constructor initialises an Episodic TopoART network based on class <c>Fast_Episodic_TopoART</c>.
    /// Input values are internally scaled from [0, 255] to [0, 1].</summary>
    /// <param name="inputLen">The length of input vectors to be learnt.</param>
    /// <param name="moduleNum">The number of TopoART modules.</param>
    /// <param name="rho_a">The vigilance parameter of the first module.</param>
    /// <param name="t_max">The parameter limiting the considered time frame.</param>
    new(inputLen : int64, moduleNum : int64, rho_a : double, t_max : int64) = {
        inherit TopoART_i64d_common(Internal.NetID.Fast_Episodic_TopoART, new Fast_Episodic_TopoART(inputLen, moduleNum, decimal rho_a, t_max) :> ITopoART)
    }

    /// <summary>This constructor initialises a TopoART-AM network or a TopoART-R network. Input values are internally
    /// scaled from [0, 255] to [0, 1].</summary>
    /// <param name="len1">The length of the input vector (TopoART-R) or the first key vector (TopoART-AM) to be learnt.
    /// </param>
    /// <param name="len2">The length of the output vector (TopoART-R) or the second key vector (TopoART-AM) to be
    /// learnt.</param>
    /// <param name="moduleNum">The number of modules.</param>
    /// <param name="rho_a">The vigilance parameter of the first module.</param>
    /// <param name="netType">The type of the network to be created. (supported types: <c>Fast_TopoART_R</c> and
    /// <c>Fast_TopoART_AM</c>)</param>
    /// <exception cref="InvalidTypeException">Throws when the selected network type is not supported.</exception>
    new(len1 : int64, len2 : int64, moduleNum : int64, rho_a : double, netType : Network) = {
        inherit TopoART_i64d_common(Types.toNetID netType,
            match netType with
                | Network.Fast_TopoART_R -> new Fast_TopoART_R(len1, len2, moduleNum, decimal rho_a) :> ITopoART
                | Network.Fast_TopoART_AM -> new Fast_TopoART_AM(len1, len2, moduleNum, decimal rho_a) :> ITopoART
                | _ -> raise (InvalidTypeException(Common.InvalidTypeException_UnsupportedNetworkType))
       )
    }

    /// <summary>This constructor loads a saved network. (supported types: <c>Fast_TopoART</c>, <c>Fast_TopoART_C</c>,
    /// <c>Fast_Episodic_TopoART</c>, <c>Fast_TopoART_R</c>, and <c>Fast_TopoART_AM</c>)</summary>
    /// <param name="path">The path of a binary network file.</param>
    /// <exception cref="InvalidFileException">Throws when the given file cannot be loaded.</exception>
    /// <exception cref="InvalidTypeException">Throws when the loaded network type is not supported.</exception>
    new(path : string) = {
        inherit TopoART_i64d_common(
            let net = Internal.load(path)
            match fst net with
                | Internal.NetID.Fast_TopoART
                | Internal.NetID.Fast_TopoART_C
                | Internal.NetID.Fast_Episodic_TopoART
                | Internal.NetID.Fast_TopoART_R
                | Internal.NetID.Fast_TopoART_AM -> net
                | _ -> raise (InvalidTypeException(Common.InvalidTypeException_UnsupportedNetworkType)))
    }

//----------------------------------------------------------------------------------------------------------------------

    /// <summary>This method finds the closest category for a given test input.</summary>
    /// <param name="input">The input vector.</param>
    /// <returns>An array of type <c>F2_output_i64d</c>. Each entry contains the ID of the best-matching node and the
    /// corresponding cluster ID for one TopoART module.</returns>
    member x.GetBMOutput(input : uint8[]) =
        let output = ((snd x.Net) :?> IFast_TopoART).GetBMOutput(input)
        Types.toArray_F2_output_i64d(output)

    /// <summary>This method finds the closest category for a given pair of keys.</summary>
    /// <param name="key1">The first key vector.</param>
    /// <param name="key2">The second key vector corresponding to <paramref name="key1"/>.</param>
    /// <returns>An array of type <c>F2_output_i64d</c>. Each entry contains the ID of the best-matching node and the
    /// corresponding cluster ID for one TopoART-AM module.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.GetBMOutput(key1 : byte[], key2 : byte[]) =
       let output = ((snd x.Net) :?> IFast_TopoART_AM).GetBMOutput(key1, key2)
       Types.toArray_F2_output_i64d(output)

    /// <summary>This method finds the closest category for a given test input.</summary>
    /// <param name="input">The input vector.</param>
    /// <param name="mask">A mask vector excluding individual dimensions of the input vector from the computation.
    /// (Setting an element of the mask vector to <c>true</c> excludes the corresponding elements of the input vector.)
    /// </param>
    /// <returns>An array of type <c>F2_output_i64d</c>. Each entry contains the ID of the best-matching node and the
    /// corresponding cluster ID for one TopoART module.</returns>
    member x.GetBMOutput(input : uint8[], mask : bool[]) =
       let output = ((snd x.Net) :?> IFast_TopoART).GetBMOutput(input, mask)
       Types.toArray_F2_output_i64d(output)

    /// <summary>This method performs a single training step.</summary>
    /// <param name="input">The input vector to be learnt.</param>
    member x.Learn(input : uint8[]) =
        ((snd x.Net) :?> IFast_TopoART).Learn(input)

    /// <summary>This method performs multiple training steps. Each sample is learnt in an individual step.</summary>
    /// <param name="inputs">The input vectors to be learnt. The first dimension indicates the input vectors.</param>
    member x.Learn(inputs : uint8[,]) =
        for i in 0..inputs.GetLength(0)-1 do
            ((snd x.Net) :?> IFast_TopoART).Learn(inputs[i, *])

    /// <summary>This method performs a single training step of a TopoART-C network.</summary>
    /// <param name="input">The input vector to be learnt.</param>
    /// <param name="classID">The class ID corresponding to <paramref name="input"/>. (must be equal to or larger than
    /// 0)</param>
    /// <exception cref="InvalidClassIDException">Throws when <paramref name="classID"/> is less than 0.</exception>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-C network.</exception>
    member x.Learn(input : uint8[], classID : int64) =
        ((snd x.Net) :?> IFast_TopoART_C).Learn(input, classID)

    /// <summary>This method performs multiple training steps of a TopoART-C network. Each sample is learnt in an
    /// individual step.</summary>
    /// <param name="inputs">The input vectors to be learnt. The first dimension indicates the input vectors.</param>
    /// <param name="classIDs">The class IDs corresponding to <paramref name="inputs"/>. Each class ID must be equal to
    /// or larger than 0.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when the size of <paramref name="inputs"/> and
    /// the size of <paramref name="classIDs"/> do not match.</exception>
    /// <exception cref="InvalidClassIDException">Throws when an element of <paramref name="classIDs"/> is less than 0.
    /// </exception>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-C network.</exception>
    member x.Learn(inputs : uint8[,], classIDs : int64[]) =
        for i in 0..inputs.GetLength(0)-1 do
            ((snd x.Net) :?> IFast_TopoART_C).Learn(inputs[i, *], classIDs[i])

    /// <summary>This method performs a single training step of a TopoART-AM network or a TopoART-R network.</summary>
    /// <param name="vec1">The input vector (TopoART-R) or the first key vector (TopoART-AM) to be learnt.</param>
    /// <param name="vec2">The output vector (TopoART-R) or the second key vector (TopoART-AM) corresponding to
    /// <paramref name="vec1"/>.</param>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is neither a TopoART-AM network nor a
    /// TopoART-R network.</exception>
    member x.Learn(vec1 : uint8[], vec2 : uint8[]) =
        if fst x.Net = Internal.NetID.Fast_TopoART_AM
            then ((snd x.Net) :?> IFast_TopoART_AM).Learn(vec1, vec2)
            else ((snd x.Net) :?> IFast_TopoART_R).Learn(vec1, vec2)

    /// <summary>This method performs multiple training steps of a TopoART-AM network or a TopoART-R network. Each
    /// sample is learnt in an individual step.</summary>
    /// <param name="vecs1">The input vectors (TopoART-R) or the first key vectors (TopoART-AM) to be learnt. The first
    /// dimension indicates the respective vectors.</param>
    /// <param name="vecs2">The output vectors (TopoART-R) or the second key vectors (TopoART-AM) corresponding to
    /// <paramref name="vecs1"/>. The first dimension indicates the respective vectors.</param>
    /// <exception cref="IndexOutOfRangeException">Throws when the size of <paramref name="vecs1"/> and the size of
    /// <paramref name="vecs2"/> do not match.</exception>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is neither a TopoART-AM network nor a
    /// TopoART-R network.</exception>
    member x.Learn(vecs1 : uint8[,], vecs2 : uint8[,]) =
        for i in 0..vecs1.GetLength(0)-1 do
            if fst x.Net = Internal.NetID.Fast_TopoART_AM
                then ((snd x.Net) :?> IFast_TopoART_AM).Learn(vecs1[i, *], vecs2[i, *])
                else ((snd x.Net) :?> IFast_TopoART_R).Learn(vecs1[i, *], vecs2[i, *])

    /// <summary>This method predicts the class ID using the default value of nu.</summary>
    /// <param name="input">The input vector the class ID of which is to be predicted.</param>
    /// <returns>The predicted class ID.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-C network.</exception>
    member x.Classify(input : uint8[]) =
        ((snd x.Net) :?> IFast_TopoART_C).Predict(input)

    /// <summary>This method predicts the class ID.</summary>
    /// <param name="input">The input vector the class ID of which is to be predicted.</param>
    /// <param name="nu">The maximum cardinality of the set of enclosing categories E and the neighbourhood set N. (This
    /// parameter does not modify the network. It may be arbitrarily changed in each prediction step.)</param>
    /// <returns>The predicted class ID.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-C network.</exception>
    member x.Classify(input : uint8[], nu : int64) =
        ((snd x.Net) :?> IFast_TopoART_C).Predict(input, nu)

    /// <summary>This method predicts the class ID using the default value of nu. Unknown elements of the input vector
    /// can be signified by setting the corresponding value of <paramref name="mask"/> to <c>true</c>.</summary>
    /// <param name="input">The input vector the class ID of which is to be predicted.</param>
    /// <param name="mask">The mask vector corresponding to <paramref name="input"/>.</param>
    /// <returns> An object of type <c>TopoART_C_prediction_i64d</c> containing the predicted class ID and a
    /// corresponding confidence value.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-C network.</exception>
    member x.Classify(input : uint8[], mask : bool[]) =
        TopoART_C_prediction_i64d(((snd x.Net) :?> IFast_TopoART_C).Predict(input, mask))

    /// <summary>This method predicts the class ID. Unknown elements of the input vector can be signified by setting the
    /// corresponding value of <paramref name="mask"/> to <c>true</c>.</summary>
    /// <param name="input">The input vector the class ID of which is to be predicted.</param>
    /// <param name="mask">The mask vector corresponding to <paramref name="input"/>.</param>
    /// <param name="nu">The maximum cardinality of the set of enclosing categories E and the neighbourhood set N. (This
    /// parameter does not modify the network. It may be arbitrarily changed in each prediction step.)</param>
    /// <returns> An object of type <c>TopoART_C_prediction_i64d</c> containing the predicted class ID and a
    /// corresponding confidence value.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-C network.</exception>
    member x.Classify(input : uint8[], mask : bool[], nu : int64) =
        TopoART_C_prediction_i64d(((snd x.Net) :?> IFast_TopoART_C).Predict(input, mask, nu))

    /// <summary>This method predicts the dependent variables using the default value of nu.</summary>
    /// <param name="input">The input vector (independent variables).</param>
    /// <returns>The predicted values for all dependent variables.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-R network.</exception>
    member x.Predict(input : uint8[]) =
        ((snd x.Net) :?> IFast_TopoART_R).Predict(input)

    /// <summary>This method predicts the dependent variables.</summary>
    /// <param name="input">The input vector (independent variables).</param>
    /// <param name="nu">The maximum cardinality of the neighbourhood set N. (In the original TopoART-R network, nu is
    /// fixed to 10. But task-specific adaptations might lead to an improved prediction accuracy. This parameter does
    /// not modify the network. It may be arbitrarily changed in each prediction step.)</param>
    /// <returns>The predicted values for all dependent variables.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-R network.</exception>
    member x.Predict(input : uint8[], nu : int64) =
        ((snd x.Net) :?> IFast_TopoART_R).Predict(input, nu)

    /// <summary>This method predicts the dependent variables for a given set of independent variables using the
    /// default value of nu. Unknown values of independent variables can be signified by setting the corresponding
    /// value of <paramref name="mask"/> to <c>true</c>.</summary>
    /// <param name="input">The input vector (independent variables).</param>
    /// <param name="mask">The mask vector corresponding to <paramref name="input"/>.</param>
    /// <returns> An object of type <c>TopoART_R_prediction_i64d</c> containing the predicted values for the unknown
    /// independent variables and all dependent variables.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-R network.</exception>
    member x.Predict(input : uint8[], mask : bool[]) =
        TopoART_R_prediction_u8(((snd x.Net) :?> IFast_TopoART_R).Predict(input, mask))

    /// <summary>This method predicts the dependent variables for a given set of independent variables. Unknown values
    /// of independent variables can be signified by setting the corresponding value of <paramref name="mask"/> to
    /// <c>true</c>.</summary>
    /// <param name="input">The input vector (independent variables).</param>
    /// <param name="mask">The mask vector corresponding to <paramref name="input"/>.</param>
    /// <param name="nu">The maximum cardinality of the neighbourhood set N. (In the original TopoART-R network, nu is
    /// fixed to 10. But task-specific adaptations might lead to an improved prediction accuracy. This parameter does
    /// not modify the network. It may be arbitrarily changed in each prediction step.)</param>
    /// <returns> An object of type <c>TopoART_R_prediction_i64d</c> containing the predicted values for the unknown
    /// independent variables and all dependent variables.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-R network.</exception>
    member x.Predict(input : uint8[], mask : bool[], nu : int64) =
        TopoART_R_prediction_u8(((snd x.Net) :?> IFast_TopoART_R).Predict(input, mask, nu))

//----------------------------------------------------------------------------------------------------------------------

    /// <summary>This method starts the recall process of an Episodic TopoART network.</summary>
    /// <param name="stimulus">The stimulus (input) which is used to trigger recall.</param>
    /// <returns>The number of F3 nodes created.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not an Episodic TopoART network.
    /// </exception>
    member x.BeginRecall(stimulus : uint8[]) =
        ((snd x.Net) :?> IFast_Episodic_TopoART).BeginRecall(stimulus)

    /// <summary>This method starts the recall process of the final module of a TopoART-AM network for the first key
    /// vector.</summary>
    /// <param name="key2">The stimulus (second key vector) which is used to trigger recall.</param>
    /// <returns>The number of F3 nodes created.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.BeginRecallKey1(key2 : uint8[]) =
        ((snd x.Net) :?> IFast_TopoART_AM).BeginRecallKey1(key2, Network_base.FINAL_MODULE)

    /// <summary>This method starts the recall process of a TopoART-AM network for the first key vector.</summary>
    /// <param name="key2">The stimulus (second key vector) which is used to trigger recall.</param>
    /// <param name="module_index">Index of the TopoART-AM module to be used for recall.</param>
    /// <returns>The number of F3 nodes created.</returns>
    /// <exception cref="InvalidModuleIndexException">Throws when <paramref name="module_index"/> is invalid.
    /// </exception>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.BeginRecallKey1(key2 : uint8[], module_index : int64) =
        ((snd x.Net) :?> IFast_TopoART_AM).BeginRecallKey1(key2, module_index)

    /// <summary>This method starts the recall process of the final module of a TopoART-AM network for the second key
    /// vector.</summary>
    /// <param name="key1">The stimulus (first key vector) which is used to trigger recall.</param>
    /// <returns>The number of F3 nodes created.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.BeginRecallKey2(key1 : uint8[]) =
        ((snd x.Net) :?> IFast_TopoART_AM).BeginRecallKey2(key1, Network_base.FINAL_MODULE)

    /// <summary>This method starts the recall process of a TopoART-AM network for the second key vector.</summary>
    /// <param name="key1">The stimulus (first key vector) which is used to trigger recall.</param>
    /// <param name="module_index">Index of the TopoART-AM module to be used for recall.</param>
    /// <returns>The number of F3 nodes created.</returns>
    /// <exception cref="InvalidModuleIndexException">Throws when <paramref name="module_index"/> is invalid.
    /// </exception>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.BeginRecallKey2(key1 : uint8[], module_index : int64) =
        ((snd x.Net) :?> IFast_TopoART_AM).BeginRecallKey2(key1, module_index)

    /// <summary>This method performs a single inter-episode recall step of an Episodic TopoART network and sets the
    /// starting point for intra-episode recall.</summary>
    /// <returns>Returns the response of the recall step.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not an Episodic TopoART network.
    /// </exception>
    member x.InterEpisodeRecallStep() =
        let (_, result : uint8[], activation : decimal) = ((snd x.Net) :?> IFast_Episodic_TopoART).InterEpisodeRecallStep()
        RecallResponse_du8(result, activation)

    /// <summary>This method performs a single intra-episode recall step of an Episodic TopoART network.</summary>
    /// <returns>Returns the result of the recall step. A value of <c>null</c> indicates a failed recall step.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not an Episodic TopoART network.
    /// </exception>
    member x.IntraEpisodeRecallStep() =
        let (_, result : uint8[]) = ((snd x.Net) :?> IFast_Episodic_TopoART).IntraEpisodeRecallStep()
        result

    /// <summary>This method performs a single associative recall step of a TopoART-AM network.</summary>
    /// <returns>Returns the response of the recall step.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.RecallStep() =
        let (_, result : uint8[], activation : decimal) = ((snd x.Net) :?> IFast_TopoART_AM).RecallStep()
        RecallResponse_du8(result, activation)
