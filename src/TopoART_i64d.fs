namespace LibTopoART.Compatibility

open LibTopoART

//**********************************************************************************************************************

/// <summary>Class <c>TopoART_i64d</c> is a wrapper for the TopoART implementations contained in LibTopoART. It forwards
/// the major methods and properties using the data types <c>int64</c> and <c>double</c>.</summary>
type TopoART_i64d =
    inherit TopoART_base<int64, double>

//----------------------------------------------------------------------------------------------------------------------

    /// <summary>This constructor initialises a TopoART(-C) network or a Hypersphere TopoART(-C) network (with R =
    /// sqrt(<paramref name="inputLen" />) / 2). All input is required to lie in the interval [0, 1].</summary>
    /// <param name="inputLen">The length of input vectors to be learnt.</param>
    /// <param name="moduleNum">The number of TopoART modules.</param>
    /// <param name="rho_a">The vigilance parameter of the first TopoART module.</param>
    /// <param name="netType">The type of the network to be created. (supported types: <c>TopoART</c>,
    /// <c>Fast_TopoART</c>, <c>Hypersphere_TopoART</c>, <c>TopoART_C</c>, <c>Fast_TopoART_C</c>, and
    /// <c>Hypersphere_TopoART_C</c>)</param>
    /// <exception cref="InvalidTypeException">Throws when the selected network type is not supported.</exception>
    new(inputLen : int64, moduleNum : int64, rho_a : double, netType : Network) = {
        inherit TopoART_base<int64, double>(Types.toNetID netType,
                    match netType with
                        | Network.TopoART -> new TopoART(inputLen, moduleNum, decimal rho_a) :> ITopoART
                        | Network.Fast_TopoART -> new Fast_TopoART(inputLen, moduleNum, decimal rho_a) :> ITopoART
                        | Network.Hypersphere_TopoART -> new Hypersphere_TopoART(inputLen, moduleNum, decimal rho_a) :> ITopoART
                        | Network.TopoART_C -> new TopoART_C(inputLen, moduleNum, decimal rho_a) :> ITopoART
                        | Network.Fast_TopoART_C -> new Fast_TopoART_C(inputLen, moduleNum, decimal rho_a) :> ITopoART
                        | Network.Hypersphere_TopoART_C -> new Hypersphere_TopoART_C(inputLen, moduleNum, decimal rho_a) :> ITopoART
                        | _ -> raise (InvalidTypeException(Common.InvalidTypeException_UnsupportedNetworkType)))
    }

    /// <summary>This constructor initialises a Hypersphere TopoART(-C) network. The input range is controlled by the
    /// radial extend parameter <paramref name="R" />.</summary>
    /// <param name="inputLen">The length of input vectors to be learnt.</param>
    /// <param name="moduleNum">The number of TopoART modules.</param>
    /// <param name="rho_a">The vigilance parameter of the first module.</param>
    /// <param name="R">The radial extend parameter.</param>
    /// <param name="classifier">The type of network to be created (true -> <c>Hypersphere_TopoART_C</c>, false ->
    /// <c>Hypersphere_TopoART</c>)</param>
    new(inputLen : int64, moduleNum : int64, rho_a : double, R : double, classifier : bool) = {
        inherit TopoART_base<int64, double>(
              if classifier then
                  (Internal.NetID.Hypersphere_TopoART_C, new Hypersphere_TopoART_C(inputLen, moduleNum, decimal rho_a, decimal R) :> ITopoART)
              else
                  (Internal.NetID.Hypersphere_TopoART, new Hypersphere_TopoART(inputLen, moduleNum, decimal rho_a, decimal R) :> ITopoART)
        )
    }

    /// <summary>This constructor initialises an Episodic TopoART network based on class <c>Fast_Episodic_TopoART</c>.
    /// All input is required to lie in the interval [0, 1].</summary>
    /// <param name="inputLen">The length of input vectors to be learnt.</param>
    /// <param name="moduleNum">The number of TopoART modules.</param>
    /// <param name="rho_a">The vigilance parameter of the first module.</param>
    /// <param name="t_max">The parameter limiting the considered time frame.</param>
    new(inputLen : int64, moduleNum : int64, rho_a : double, t_max : int64) = {
        inherit TopoART_base<int64, double>(Internal.NetID.Fast_Episodic_TopoART, new Fast_Episodic_TopoART(inputLen, moduleNum, decimal rho_a, t_max) :> ITopoART)
    }

    /// <summary>This constructor initialises a TopoART-R network. All input is required to lie in the interval
    /// [0, 1].</summary>
    /// <param name="len1">The length of the input vector (TopoART-R) or the first key vector (TopoART-AM) to be learnt.
    /// </param>
    /// <param name="len2">The length of the output vector (TopoART-R) or the second key vector (TopoART-AM) to be
    /// learnt.</param>
    /// <param name="moduleNum">The number of modules.</param>
    /// <param name="rho_a">The vigilance parameter of the first module.</param>
    /// <param name="netType">The type of the network to be created. (supported types:<c>TopoART_R</c>,
    /// <c>Fast_TopoART_R</c>, and <c>Fast_TopoART_AM</c>)</param>
    /// <exception cref="InvalidTypeException">Throws when the selected network type is not supported.</exception>
    new(len1 : int64, len2 : int64, moduleNum : int64, rho_a : double, netType : Network) = {
         inherit TopoART_base<int64, double>(Types.toNetID netType,
                    match netType with
                        | Network.Fast_TopoART_R -> new Fast_TopoART_R(len1, len2, moduleNum, decimal rho_a) :> ITopoART
                        | Network.TopoART_R -> new TopoART_R(len1, len2, moduleNum, decimal rho_a) :> ITopoART
                        | Network.Fast_TopoART_AM -> new Fast_TopoART_AM(len1, len2, moduleNum, decimal rho_a) :> ITopoART
                        | _ -> raise (InvalidTypeException(Common.InvalidTypeException_UnsupportedNetworkType))
                )
    }

    /// <summary>This constructor loads a saved network.</summary>
    /// <param name="path">The path of a binary network file.</param>
    /// <exception cref="InvalidFileException">Throws when the given file cannot be loaded.</exception>
    new(path : string) = {
        inherit TopoART_base<int64, double>(Internal.load(path))
    }

//----------------------------------------------------------------------------------------------------------------------

    /// <value>Property <c>Alpha</c> represents the choice parameter alpha.</value>
    override x.Alpha
        with get() = double (snd x.Net).Alpha 
        and set(value : double) = (snd x.Net).Alpha <- decimal value

    /// <value>Property <c>Beta_sbm</c> represents the learning rate of the second best-matching nodes.</value>
    override x.Beta_sbm
        with get() = double (snd x.Net).Beta_sbm 
        and set(value : double) = (snd x.Net).Beta_sbm <- decimal value

    /// <value>Property <c>ClusterNum</c> represents the number of TopoART clusters found by each module.</value>
    override x.ClusterNum = (snd x.Net).ClusterNum

    /// <value>Property <c>D_len</c> returns the length of the output vector (dependent variables).</value>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-R network.</exception>
    override x.D_len = ((snd x.Net) :?> ITopoART_R_base).D_len

    /// <value>Property <c>InputLen</c> returns the length of the input vector.</value>
    override x.InputLen = (snd x.Net).InputLen

    /// <value>Property <c>I_len</c> returns the length of the input vector (independent variables).</value>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-R network.</exception>
    override x.I_len = ((snd x.Net) :?> ITopoART_R_base).I_len

    /// <value>Property <c>Key1Len</c> returns the length of the first key vector.</value>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    override x.Key1Len = ((snd x.Net) :?> ITopoART_AM_base).Key1Len

    /// <value>Property <c>Key2Len</c> returns the length of the second key vector.</value>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    override x.Key2Len = ((snd x.Net) :?> ITopoART_AM_base).Key2Len

    /// <value>Property <c>LearningSteps</c> represents the total number of performed learning steps.</value>
    override x.LearningSteps = (snd x.Net).LearningSteps

    /// <value>Property <c>ModuleNum</c> represents the number of TopoART modules used. 
    /// (The original TopoART uses two modules.)</value>
    override x.ModuleNum = (snd x.Net).ModuleNum

    /// <value>Property <c>NodeNum</c> represents the number of TopoART nodes used by each module.</value>
    override x.NodeNum = (snd x.Net).NodeNum

    /// <value>Property <c>Nu</c> represents the default value used for the maximum cardinality of the
    /// neighbourhood set N during prediction. If the parameter <c>nu</c> is not explicitly provided for prediction,
    /// this property will be applied. (This parameter does not modify the network. It may be arbitrarily changed
    /// for each prediction step.)</value>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-R network.</exception>
    override x.Nu
        with get() = if ((fst x.Net = Internal.NetID.TopoART_R) || (fst x.Net = Internal.NetID.Fast_TopoART_R))
                        then ((snd x.Net) :?> ITopoART_R_base).Nu
                        else ((snd x.Net) :?> ITopoART_C_base).Nu
        and set value = if ((fst x.Net = Internal.NetID.TopoART_R) || (fst x.Net = Internal.NetID.Fast_TopoART_R))
                            then ((snd x.Net) :?> ITopoART_R_base).Nu <- value
                            else ((snd x.Net) :?> ITopoART_C_base).Nu <- value

    /// <value>Property <c>Phi</c> represents the parameter phi required for the removal of nodes and edges as
    /// well as for the propagation of input to subsequent TopoART modules.</value>
    override x.Phi
        with get() = (snd x.Net).Phi 
        and set value = (snd x.Net).Phi <- value

    /// <value>Property <c>Phis</c> constitutes an extension of property <c>Phi</c> that enables individual values
    /// of phi for each module. By this, the removal of nodes and edges as well as for the propagation of input to
    /// subsequent TopoART modules can be controlled in a task dependent manner.</value>
    override x.Phis
        with get() = (snd x.Net).Phis 
        and set value = (snd x.Net).Phis <- value

    /// <value>Property <c>R</c> represents the radial extend parameter R of Hypersphere TopoART.</value>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a Hypersphere TopoART network.
    /// </exception>
    override x.R = double ((snd x.Net) :?> IHypersphere_TopoART).R

    /// <value>Property <c>Rho_a</c> represents the vigilance parameter of the first TopoART module (TA a).</value>
    override x.Rho_a = double (snd x.Net).Rho_a

    /// <value>Property <c>T_max</c> represents the maximum considered time frame.</value>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not an Episodic TopoART network.
    /// </exception>
    override x.T_max = ((snd x.Net) :?> IEpisodic_TopoART).T_max

    /// <value>Property <c>Tau</c> represents the parameter tau required for the removal of nodes and edges.</value>
    override x.Tau
        with get() = (snd x.Net).Tau 
        and set value = (snd x.Net).Tau <- value

//----------------------------------------------------------------------------------------------------------------------

    /// <summary>This method finds the closest category for a given test input.</summary>
    /// <param name="input">The input vector.</param>
    /// <returns>An array of type <c>F2_output_i64d</c>. Each entry contains the ID of the best-matching node and the
    /// corresponding cluster ID for one TopoART module.</returns>
    member x.GetBMOutput(input : double[]) =
        let output = (snd x.Net).GetBMOutput(Internal.toArray_Decimal input)
        Types.toArray_F2_output_i64d(output)

    /// <summary>This method finds the closest category for a given pair of keys.</summary>
    /// <param name="key1">The first key vector.</param>
    /// <param name="key2">The second key vector corresponding to <paramref name="key1"/>.</param>
    /// <returns>An array of type <c>F2_output_i64d</c>. Each entry contains the ID of the best-matching node and the
    /// corresponding cluster ID for one TopoART-AM module.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.GetBMOutput(key1 : double[], key2 : double[]) =
       let output = ((snd x.Net) :?> ITopoART_AM).GetBMOutput(Internal.toArray_Decimal key1, Internal.toArray_Decimal key2)
       Types.toArray_F2_output_i64d(output)

    /// <summary>This method finds the closest category for a given test input.</summary>
    /// <param name="input">The input vector.</param>
    /// <param name="mask">A mask vector excluding individual dimensions of the input vector from the computation.
    /// (Setting an element of the mask vector to <c>true</c>, excludes the corresponding elements of the input vector.)
    /// </param>
    /// <returns>An array of type <c>F2_output_i64d</c>. Each entry contains the ID of the best-matching node and the
    /// corresponding cluster ID for one TopoART module.</returns>
    member x.GetBMOutput(input : double[], mask : bool[]) =
       let output = (snd x.Net).GetBMOutput(Internal.toArray_Decimal input, mask)
       Types.toArray_F2_output_i64d(output)

    /// <summary>This method performs a single training step.</summary>
    /// <param name="input">The input vector to be learnt.</param>
    member x.Learn(input : double[]) =
        (snd x.Net).Learn(Internal.toArray_Decimal input)

    /// <summary>This method performs multiple training steps. Each sample is learnt in an individual step.</summary>
    /// <param name="inputs">The input vectors to be learnt. The first dimension indicates the input vectors.</param>
    member x.Learn(inputs : double[,]) =
        for i in 0..inputs.GetLength(0)-1 do
            (snd x.Net).Learn(Internal.toArray_Decimal inputs[i, *])

    /// <summary>This method performs a single training step of a TopoART-C network.</summary>
    /// <param name="input">The input vector to be learnt.</param>
    /// <param name="classID">The class ID corresponding to <paramref name="input"/>. (must be equal to or larger than
    /// 0)</param>
    /// <exception cref="InvalidClassIDException">Throws when <paramref name="classID"/> is less than 0.</exception>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-C network.</exception>
    member x.Learn(input : double[], classID : int64) =
        ((snd x.Net) :?> ITopoART_C).Learn(Internal.toArray_Decimal input, classID)

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
    member x.Learn(inputs : double[,], classIDs : int64[]) =
        for i in 0..inputs.GetLength(0)-1 do
            ((snd x.Net) :?> ITopoART_C).Learn(Internal.toArray_Decimal inputs[i, *], classIDs[i])

    /// <summary>This method performs a single training step of a TopoART-AM network or a TopoART-R network.</summary>
    /// <param name="vec1">The input vector (TopoART-R) or the first key vector (TopoART-AM) to be learnt.</param>
    /// <param name="vec2">The output vector (TopoART-R) or the second key vector (TopoART-AM) corresponding to
    /// <paramref name="vec1"/>.</param>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is neither a TopoART-AM network nor a
    /// TopoART-R network.</exception>
    member x.Learn(vec1 : double[], vec2 : double[]) =
        if fst x.Net = Internal.NetID.Fast_TopoART_AM
            then ((snd x.Net) :?> ITopoART_AM).Learn(Internal.toArray_Decimal vec1, Internal.toArray_Decimal vec2)
            else ((snd x.Net) :?> ITopoART_R).Learn(Internal.toArray_Decimal vec1, Internal.toArray_Decimal vec2)

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
    member x.Learn(vecs1 : double[,], vecs2 : double[,]) =
        for i in 0..vecs1.GetLength(0)-1 do
            if fst x.Net = Internal.NetID.Fast_TopoART_AM
                then ((snd x.Net) :?> ITopoART_AM).Learn(Internal.toArray_Decimal vecs1[i, *], Internal.toArray_Decimal vecs2[i, *])
                else ((snd x.Net) :?> ITopoART_R).Learn(Internal.toArray_Decimal vecs1[i, *], Internal.toArray_Decimal vecs2[i, *])

    /// <summary>This method predicts the class ID using the default value of nu.</summary>
    /// <param name="input">The input vector the class ID of which is to be predicted.</param>
    /// <returns>The predicted class ID.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-C network.</exception>
    member x.Classify(input : double[]) =
        ((snd x.Net) :?> ITopoART_C).Predict(Internal.toArray_Decimal input)

    /// <summary>This method predicts the class ID.</summary>
    /// <param name="input">The input vector the class ID of which is to be predicted.</param>
    /// <param name="nu">The maximum cardinality of the set of enclosing categories E and the neighbourhood set N. (This
    /// parameter does not modify the network. It may be arbitrarily changed in each prediction step.)</param>
    /// <returns>The predicted class ID.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-C network.</exception>
    member x.Classify(input : double[], nu : int64) =
        ((snd x.Net) :?> ITopoART_C).Predict(Internal.toArray_Decimal input, nu)

    /// <summary>This method predicts the class ID using the default value of nu. Unknown elements of the input vector
    /// can be signified by setting the corresponding value of <paramref name="mask"/> to <c>true</c>.</summary>
    /// <param name="input">The input vector the class ID of which is to be predicted.</param>
    /// <param name="mask">The mask vector corresponding to <paramref name="input"/>.</param>
    /// <returns> An object of type <c>TopoART_C_prediction_i64d</c> containing the predicted class ID and a
    /// corresponding confidence value.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-C network.</exception>
    member x.Classify(input : double[], mask : bool[]) =
        TopoART_C_prediction_i64d(((snd x.Net) :?> ITopoART_C).Predict(Internal.toArray_Decimal input, mask))

    /// <summary>This method predicts the class ID. Unknown elements of the input vector can be signified by setting the
    /// corresponding value of <paramref name="mask"/> to <c>true</c>.</summary>
    /// <param name="input">The input vector the class ID of which is to be predicted.</param>
    /// <param name="mask">The mask vector corresponding to <paramref name="input"/>.</param>
    /// <param name="nu">The maximum cardinality of the set of enclosing categories E and the neighbourhood set N. (This
    /// parameter does not modify the network. It may be arbitrarily changed in each prediction step.)</param>
    /// <returns> An object of type <c>TopoART_C_prediction_i64d</c> containing the predicted class ID and a
    /// corresponding confidence value.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-C network.</exception>
    member x.Classify(input : double[], mask : bool[], nu : int64) =
        TopoART_C_prediction_i64d(((snd x.Net) :?> ITopoART_C).Predict(Internal.toArray_Decimal input, mask, nu))

    /// <summary>This method predicts the dependent variables using the default value of nu.</summary>
    /// <param name="input">The input vector (independent variables).</param>
    /// <returns>The predicted values for all dependent variables.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-R network.</exception>
    member x.Predict(input : double[]) =
        Internal.toArray_Double (((snd x.Net) :?> ITopoART_R).Predict(Internal.toArray_Decimal input))

    /// <summary>This method predicts the dependent variables.</summary>
    /// <param name="input">The input vector (independent variables).</param>
    /// <param name="nu">The maximum cardinality of the neighbourhood set N. (In the original TopoART-R network, nu is
    /// fixed to 10. But task-specific adaptations might lead to an improved prediction accuracy. This parameter does
    /// not modify the network. It may be arbitrarily changed in each prediction step.)</param>
    /// <returns>The predicted values for all dependent variables.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-R network.</exception>
    member x.Predict(input : double[], nu : int64) =
        Internal.toArray_Double (((snd x.Net) :?> ITopoART_R).Predict(Internal.toArray_Decimal input, nu))

    /// <summary>This method predicts the dependent variables for a given set of independent variables using the
    /// default value of nu. Unknown values of independent variables can be signified by setting the corresponding
    /// value of <paramref name="mask"/> to <c>true</c>.</summary>
    /// <param name="input">The input vector (independent variables).</param>
    /// <param name="mask">The mask vector corresponding to <paramref name="input"/>.</param>
    /// <returns> An object of type <c>TopoART_R_prediction_i64d</c> containing the predicted values for the unknown
    /// independent variables and all dependent variables.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-R network.</exception>
    member x.Predict(input : double[], mask : bool[]) =
        TopoART_R_prediction_i64d(((snd x.Net) :?> ITopoART_R).Predict(Internal.toArray_Decimal input, mask))

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
    member x.Predict(input : double[], mask : bool[], nu : int64) =
        TopoART_R_prediction_i64d(((snd x.Net) :?> ITopoART_R).Predict(Internal.toArray_Decimal input, mask, nu))

//----------------------------------------------------------------------------------------------------------------------

    /// <summary>This method starts the recall process of an Episodic TopoART network.</summary>
    /// <param name="stimulus">The stimulus (input) which is used to trigger recall.</param>
    /// <returns>The number of F3 nodes created.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not an Episodic TopoART network.
    /// </exception>
    member x.BeginRecall(stimulus : double[]) =
        ((snd x.Net) :?> IEpisodic_TopoART).BeginRecall(Internal.toArray_Decimal stimulus)

    /// <summary>This method starts the recall process of the final module of a TopoART-AM network for the first key
    /// vector.</summary>
    /// <param name="key2">The stimulus (second key vector) which is used to trigger recall.</param>
    /// <returns>The number of F3 nodes created.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.BeginRecallKey1(key2 : double[]) =
        ((snd x.Net) :?> ITopoART_AM).BeginRecallKey1(Internal.toArray_Decimal key2, Network_base.FINAL_MODULE)

    /// <summary>This method starts the recall process of a TopoART-AM network for the first key vector.</summary>
    /// <param name="key2">The stimulus (second key vector) which is used to trigger recall.</param>
    /// <param name="module_index">Index of the TopoART-AM module to be used for recall.</param>
    /// <returns>The number of F3 nodes created.</returns>
    /// <exception cref="InvalidModuleIndexException">Throws when <paramref name="module_index"/> is invalid.
    /// </exception>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.BeginRecallKey1(key2 : double[], module_index : int64) =
        ((snd x.Net) :?> ITopoART_AM).BeginRecallKey1(Internal.toArray_Decimal key2, module_index)

    /// <summary>This method starts the recall process of the final module of a TopoART-AM network for the second key
    /// vector.</summary>
    /// <param name="key1">The stimulus (first key vector) which is used to trigger recall.</param>
    /// <returns>The number of F3 nodes created.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.BeginRecallKey2(key1 : double[]) =
        ((snd x.Net) :?> ITopoART_AM).BeginRecallKey2(Internal.toArray_Decimal key1, Network_base.FINAL_MODULE)

    /// <summary>This method starts the recall process of a TopoART-AM network for the second key vector.</summary>
    /// <param name="key1">The stimulus (first key vector) which is used to trigger recall.</param>
    /// <param name="module_index">Index of the TopoART-AM module to be used for recall.</param>
    /// <returns>The number of F3 nodes created.</returns>
    /// <exception cref="InvalidModuleIndexException">Throws when <paramref name="module_index"/> is invalid.
    /// </exception>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.BeginRecallKey2(key1 : double[], module_index : int64) =
        ((snd x.Net) :?> ITopoART_AM).BeginRecallKey2(Internal.toArray_Decimal key1, module_index)

    /// <summary>This method performs a single inter-episode recall step of an Episodic TopoART network and sets the
    /// starting point for intra-episode recall.</summary>
    /// <returns>Returns the response of the recall step.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not an Episodic TopoART network.
    /// </exception>
    member x.InterEpisodeRecallStep() =
        let (_, result : decimal[], activation : decimal) = ((snd x.Net) :?> IEpisodic_TopoART).InterEpisodeRecallStep()
        RecallResponse_i64d(result, activation)

    /// <summary>This method performs a single intra-episode recall step of an Episodic TopoART network.</summary>
    /// <returns>Returns the result of the recall step. A value of <c>null</c> indicates a failed recall step.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not an Episodic TopoART network.
    /// </exception>
    member x.IntraEpisodeRecallStep() =
        let (_, result : decimal[]) = ((snd x.Net) :?> IEpisodic_TopoART).IntraEpisodeRecallStep()
        Internal.toArray_Double result

    /// <summary>This method performs a single associative recall step of a TopoART-AM network.</summary>
    /// <returns>Returns the response of the recall step.</returns>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is not a TopoART-AM network.</exception>
    member x.RecallStep() =
        let (_, result : decimal[], activation : decimal) = ((snd x.Net) :?> ITopoART_AM).RecallStep()
        RecallResponse_i64d(result, activation)

//----------------------------------------------------------------------------------------------------------------------
 
    /// <summary>This method returns the current adaptation state.</summary>
    /// <param name="epsilon">The threshold for weight adaptations to be considered.</param>
    /// <returns>An enumeration describing the adaptation state.</returns>
    /// <exception cref="InvalidStateException">Throws when the network is in an invalid state.</exception>
    /// <exception cref="InvalidNumberException">Throws when the number of edges of an F2 node is greater than
    /// <c>int.MaxValue</c>.</exception>
    override x.GetAdaptationState(epsilon : double) =
        (snd x.Net).GetAdaptationState(decimal epsilon)

    /// <summary>This method collects information on the categories of the final module.</summary>
    /// <returns>A list containing information about the respective categories.</returns>
    member x.GetCategories() =
        Types.toResizeArray_CategoryInfo_i64d <| ((snd x.Net) :?> ICategoryAccess).GetCategories(LibTopoART_info.FINAL_MODULE)

    /// <summary>This method collects information on the categories of a specified module.</summary>
    /// <param name="module_index">The index of the module the categories of which are to be analysed.</param>
    /// <returns>A list containing information about the respective categories.</returns>
    /// <exception cref="InvalidModuleIndexException">Throws when <paramref name="module_index"/> is invalid.
    /// </exception>
    /// <exception cref="InvalidNumberException">Throws when the number of nodes of a module is greater than
    /// <c>int.MaxValue</c>.</exception>
    member x.GetCategories(module_index : int64) =
        Types.toResizeArray_CategoryInfo_i64d <| ((snd x.Net) :?> ICategoryAccess).GetCategories(module_index)