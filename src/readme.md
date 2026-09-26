### LibTopoART

[LibTopoART](https://www.libtopoart.eu) provides platform-independent C# implementations of several neural networks based on the TopoART architecture. This architecture has been developed as a unified machine learning approach tackling frequent problems arising in cognitive robotics and advanced machine learning, such as online-learning, lifelong learning from data streams, as well as incremental learning and prediction from non-stationary data, noisy data, imbalanced data, and incomplete data.

TopoART combines elements of Adaptive Resonance Theory (ART) and topology-learning neural networks. From ART, it inherits fast learning, resistance to catastrophic forgetting, and an incremental architecture allowing for the insertion of new neurons on demand. By additionally associating related neurons, TopoART represents the topology of its input. Therefore, clusters of arbitrary shapes can be learnt, the spatio-temporal context of input can be used to facilitate network training, and episodes can be formed. Moreover, TopoART enables stable online clustering of stationary and non-stationary data at multiple levels of detail in parallel.

Besides the TopoART neural network itself, LibTopoART comprises implementations of Episodic TopoART (episodic clustering of data streams), Hypersphere TopoART (clustering and topology-learning), TopoART-AM (associative memory), TopoART-C and Hypersphere TopoART-C (classification), and TopoART-R (regression).

The computations of these neural networks are performed using the C# data type decimal, which is a base-10 type providing a higher precision than float and double. For most networks, accelerated variants exist which internally use fixed-point arithmetic to reduce the computation time.

### LibTopoART.Compatibility

LibTopoART.Compatibility contains wrappers for the neural networks of LibTopoART using common data types. In particular, the data type decimal is substituted by more widely supported types such as double. This renders the TopoART neural networks accessible from environments mapping .NET types to their own type systems, such as MATLAB.

The classes TopoART_i64d and TopoART_i64du8 wrap all neural networks provided by LibTopoART: TopoART, Episodic TopoART, Hypersphere TopoART, TopoART-AM, TopoART-C, Hypersphere TopoART-C, and TopoART-R. Here, int64 is used as the main integer type and double as the main floating-point type. Additionally, TopoART_i64du8 accepts uint8 data for training and inference, which accelerates the processing of image data.

LibTopoART.Compatibility is used by the packages [TopoART Layers](https://de.mathworks.com/matlabcentral/fileexchange/183840-topoart-layers) and [TopoART Neural Networks](https://de.mathworks.com/matlabcentral/fileexchange/118455-topoart-neural-networks) available from MATLAB File Exchange.