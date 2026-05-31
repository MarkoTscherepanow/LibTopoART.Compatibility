namespace LibTopoART.Compatibility

open LibTopoART

//**********************************************************************************************************************

/// <summary>Class <c>TopoART_i64d_common</c> contains the shared functionality of all TopoART wrappers that forward
/// methods and properties using the data types <c>int64</c> and <c>double</c>.</summary>
[<AbstractClass>]
type TopoART_i64d_common =
    inherit TopoART_base<int64, double>

    /// <summary>This constructor initialises an instance of <c>TopoART_i64d_common</c>.</summary>
    /// <param name="net">A pair containing a network ID and a corresponding network.</param>
    internal new(net : Internal.NetID * ITopoART) = {
        inherit TopoART_base<int64, double>(net)
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
    /// of phi for each module. Thereby, the removal of nodes and edges as well as the propagation of input to
    /// subsequent TopoART modules can be controlled in a task-dependent manner.</value>
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

    /// <summary>This method returns the current adaptation state.</summary>
    /// <param name="epsilon">The threshold for weight adaptations to be considered.</param>
    /// <returns>An enumeration describing the adaptation state.</returns>
    /// <exception cref="InvalidStateException">Throws when the network is in an invalid state.</exception>
    /// <exception cref="InvalidNumberException">Throws when the number of edges of an F2 node is greater than
    /// <c>int.MaxValue</c>.</exception>
    override x.GetAdaptationState(epsilon : double) =
        (snd x.Net).GetAdaptationState(decimal epsilon)

//----------------------------------------------------------------------------------------------------------------------

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
