### LibTopoART

[LibTopoART](https://www.libtopoart.eu) provides implementations of several neural networks based on the TopoART architecture. This architecture has been developed as a unified machine learning approach tackling frequent problems arising in cognitive robotics and advanced machine learning, such as online-learning, lifelong learning from data streams, as well as incremental learning and prediction from non-stationary data, noisy data, imbalanced data, and incomplete data.

Besides the TopoART neural network itself, LibTopoART comprises implementations of Episodic TopoART (episodic clustering of data streams), Hypersphere TopoART (clustering and topology-learning), TopoART-AM (associative memory), TopoART-C and Hypersphere TopoART-C (classification), and TopoART-R (regression).

### LibTopoART.Compatibility

LibTopoART.Compatibility contains wrappers for the neural networks of LibTopoART using common data types. In particular, the data type decimal is substituted by more widely supported types such as double. LibTopoART.Compatibility is used by the package [TopoART Neural Networks](https://de.mathworks.com/matlabcentral/fileexchange/118455-topoart-neural-networks) available from MATLAB File Exchange.