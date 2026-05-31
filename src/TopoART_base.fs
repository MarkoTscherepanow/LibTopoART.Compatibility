namespace LibTopoART.Compatibility

open LibTopoART
open System.IO.Compression
open System.Numerics

//**********************************************************************************************************************

/// <summary>Class <c>TopoART_base</c> is the base class of all wrappers contained in LibTopoART.Compatibility.
/// </summary>
/// <typeparam name="TInt">The integer type used for parameters.</typeparam>
/// <typeparam name="TFloat">The floating-point type used for parameters.</typeparam>
[<AbstractClass>]
type TopoART_base<'TInt, 'TFloat
#if NET7_0_OR_GREATER
    when 'TInt :> IBinaryInteger<'TInt>
    and 'TFloat :> IFloatingPoint<'TFloat>> =
#else
    when 'TInt : struct
    and 'TFloat : struct> =
#endif

#if DEBUG
    val Net : Internal.NetID * ITopoART
#else
    val internal Net : Internal.NetID * ITopoART
#endif

    /// <summary>This constructor initialises an instance of <c>TopoART_base</c>.</summary>
    /// <param name="net">A pair containing a network ID and a corresponding network.</param>
    internal new(net : Internal.NetID * ITopoART) = { Net = net }

//----------------------------------------------------------------------------------------------------------------------

    /// <value>Property <c>Alpha</c> represents the choice parameter alpha.</value>
    abstract member Alpha: 'TFloat with get, set

    /// <value>Property <c>Beta_sbm</c> represents the learning rate of the second best-matching nodes.</value>
    abstract member Beta_sbm: 'TFloat with get, set

    /// <value>Property <c>ClusterNum</c> represents the number of TopoART clusters found by each module.</value>
    abstract member ClusterNum: 'TInt[]

    /// <value>Property <c>D_len</c> returns the length of the output vector (dependent variables).</value>
    abstract member D_len: 'TInt

    /// <value>Property <c>InputLen</c> returns the length of the input vector.</value>
    abstract member InputLen: 'TInt

    /// <value>Property <c>I_len</c> returns the length of the input vector (independent variables).</value>
    abstract member I_len: 'TInt

    /// <value>Property <c>Key1Len</c> returns the length of the first key vector.</value>
    abstract member Key1Len: 'TInt

    /// <value>Property <c>Key2Len</c> returns the length of the second key vector.</value>
    abstract member Key2Len: 'TInt

    /// <value>Property <c>LearningSteps</c> represents the total number of performed learning steps.</value>
    abstract member LearningSteps: 'TInt

    /// <value>Property <c>ModuleNum</c> represents the number of TopoART modules used.
    /// (The original TopoART uses two modules.)</value>
    abstract member ModuleNum: 'TInt

    /// <value>Property <c>NodeNum</c> represents the number of TopoART nodes used by each module.</value>
    abstract member NodeNum: 'TInt[]

    /// <value>Property <c>Nu</c> represents the default value used for the maximum cardinality of the
    /// neighbourhood set N during prediction. If the parameter <c>nu</c> is not explicitly provided for prediction,
    /// this property will be applied. (This parameter does not modify the network. It may be arbitrarily changed
    /// for each prediction step.)</value>
    abstract member Nu: 'TInt with get, set

    /// <value>Property <c>Phi</c> represents the parameter phi required for the removal of nodes and edges as
    /// well as for the propagation of input to subsequent TopoART modules.</value>
    abstract member Phi: 'TInt with get, set

    /// <value>Property <c>Phis</c> constitutes an extension of property <c>Phi</c> that enables individual values
    /// of phi for each module. Thereby, the removal of nodes and edges as well as the propagation of input to
    /// subsequent TopoART modules can be controlled in a task-dependent manner.</value>
    abstract member Phis: 'TInt[] with get, set

    /// <value>Property <c>R</c> represents the radial extend parameter R of Hypersphere TopoART.</value>
    abstract member R: 'TFloat

    /// <value>Property <c>Rho_a</c> represents the vigilance parameter of the first TopoART module (TA a).</value>
    abstract member Rho_a: 'TFloat

    /// <value>Property <c>T_max</c> represents the maximum considered time frame.</value>
    abstract member T_max: 'TInt

    /// <value>Property <c>Tau</c> represents the parameter tau required for the removal of nodes and edges.</value>
    abstract member Tau: 'TInt with get, set

//----------------------------------------------------------------------------------------------------------------------

    /// <summary>This method computes the cluster IDs for all neurons.</summary>
    member x.ComputeClusterIDs() =
        (snd x.Net).ComputeClusterIDs()

//----------------------------------------------------------------------------------------------------------------------

    /// <summary>This method stops the recall process of an Episodic TopoART network or a TopoART-AM network and frees
    /// temporary resources.</summary>
    /// <exception cref="InvalidCastException">Throws when the wrapped network is neither an Episodic TopoART network
    /// nor a TopoART-AM network.</exception>
    member x.EndRecall() =
        if fst x.Net = Internal.NetID.Fast_TopoART_AM then
            ((snd x.Net) :?> ITopoART_AM).EndRecall()
        else
            ((snd x.Net) :?> IEpisodic_TopoART).EndRecall()

//----------------------------------------------------------------------------------------------------------------------

    /// <summary>This method saves the entire network as a text file.</summary>
    /// <param name="path">A <c>string</c> representing the path of the file to save.</param>
    member x.SaveText(path : string) =
        (snd x.Net).SaveText(path)

    /// <summary>This method saves the entire network as a binary file.</summary>
    /// <param name="path">A <c>string</c> representing the path of the file to save.</param>
    member x.Save(path : string) =
        (snd x.Net).Save(path, CompressionLevel.NoCompression)

    /// <summary>This method saves the entire network as a binary file.</summary>
    /// <param name="path">A <c>string</c> representing the path of the file to save.</param>
    /// <param name="compression">Compression level of the save file (Compression is not supported by LibTopoART v0.93
    /// and below.)</param>
    member x.Save(path : string, compression : CompressionLevel) =
        (snd x.Net).Save(path, compression)

//----------------------------------------------------------------------------------------------------------------------

    /// <summary>This method resets the adaptation state to <c>AdaptationState.NO_ADAPTATION</c>.</summary>
    /// <exception cref="InvalidNumberException">Throws when the number of edges of an F2 node is greater than
    /// <c>int.MaxValue</c>.</exception>
    member x.ResetAdaptationState() =
        (snd x.Net).ResetAdaptationState()

    /// <summary>This method returns the current adaptation state using a default value of 0.001 for the threshold
    /// <c>epsilon</c>.</summary>
    /// <returns>An enumeration describing the adaptation state.</returns>
    /// <exception cref="InvalidStateException">Throws when the network is in an invalid state.</exception>
    /// <exception cref="InvalidNumberException">Throws when the number of edges of an
    /// F2 node is greater than <c>int.MaxValue</c>.</exception>
    member x.GetAdaptationState() =
        (snd x.Net).GetAdaptationState(0.001m)

    /// <summary>This method returns the current adaptation state.</summary>
    /// <param name="epsilon">The threshold for weight adaptations to be considered.</param>
    /// <returns>An enumeration describing the adaptation state.</returns>
    abstract member GetAdaptationState: epsilon : 'TFloat -> AdaptationState
