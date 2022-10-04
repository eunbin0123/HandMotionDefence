using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using System.IO;
using UnityEditor;

public class cshInceptionv3ImageClassifier
{
    private TFGraph graph;
    private TFSession session;

    private static string[] labels = new string[] {
            "Axe",
            "Bow",
            "Gun",
            "Sword",
            "Thumb",
            "Stop",
            "Ok",
    };

    public void LoadModel(string your_name_graph)
    {
        if (graph == null)
        {
            Debug.Log("Loading tensor graph " + your_name_graph);
            TextAsset graphModel = Resources.Load<TextAsset>(your_name_graph);

            if (graphModel == null)
            {
                Debug.LogError("Failed to load tensor graph " + your_name_graph);
            }

            graph = new TFGraph();
            //graph.Import(graphModel.bytes);
            graph.Import(new TFBuffer(graphModel.bytes));
            session = new TFSession(graph);
        }
    }


    //public int PredictClass(Texture2D image)
    public int PredictClass()
    {
        var runner = session.GetRunner();
        string pathRsc = "Assets/Resources/images/screen_512x512.jpg";
        AssetDatabase.ImportAsset(pathRsc, ImportAssetOptions.ImportRecursive);
        // 필요하다면 image width와 height를 가져와 CreateTensorFromImageFile로 넘겨서 일반화

        TFTensor input = ImageUtil.CreateTensorFromImageFile(pathRsc);
        runner
            .AddInput(graph["ExpandDims"][0], input);
        runner.Fetch(graph["final_result"][0]);
        float[,] recurrent_tensor = runner.Run()[0].GetValue() as float[,];

        float maxVal = float.MinValue;
        int bestIdx = -1;
        for (int i = 0; i < 7; ++i)
        {
            float val = recurrent_tensor[0, i];
            Debug.Log(i + ": " + val);
            if (val > maxVal)
            {
                maxVal = val;
                bestIdx = i;
            }
        }

        return bestIdx;
    }

    //public string PredictLabel(Texture2D image)
    public string PredictLabel()
    {
        //int classId = PredictClass(image);
        int classId = PredictClass();
        if (classId >= 0 && classId < labels.Length)
        {
            return labels[classId];
        }
        return "unknown";
    }

    // 이미지를 TFTensor 클래스로 변환하기 위한 유틸
    public static class ImageUtil
    {
        // Convert the image in filename to a Tensor suitable as input to the Inception model.
        public static TFTensor CreateTensorFromImageFile(string file, TFDataType destinationDataType = TFDataType.Float)
        {
            byte[] contents;
            if (Application.platform == RuntimePlatform.Android)
            {
                WWW reader = new WWW(file);
                while (!reader.isDone) { }

                contents = reader.bytes;
            }
            else
            {
                contents = File.ReadAllBytes(file);
            }

            // var contents = File.ReadAllBytes (file);

            // DecodeJpeg uses a scalar String-valued tensor as input.
            var tensor = TFTensor.CreateString(contents);

            TFGraph graph;
            TFOutput input, output;

            // Construct a graph to normalize the image
            ConstructGraphToNormalizeImage(out graph, out input, out output, destinationDataType);

            // Execute that graph to normalize this one image
            using (var session = new TFSession(graph))
            {
                var normalized = session.Run(
                         inputs: new[] { input },
                         inputValues: new[] { tensor },
                         outputs: new[] { output });

                return normalized[0];
            }
        }

        // The inception model takes as input the image described by a Tensor in a very
        // specific normalized format (a particular image size, shape of the input tensor,
        // normalized pixel values etc.).
        //
        // This function constructs a graph of TensorFlow operations which takes as
        // input a JPEG-encoded string and returns a tensor suitable as input to the
        // inception model.
        private static void ConstructGraphToNormalizeImage(out TFGraph graph, out TFOutput input, out TFOutput output, TFDataType destinationDataType = TFDataType.Float)
        {
            // Some constants specific to the pre-trained model at:
            // https://storage.googleapis.com/download.tensorflow.org/models/inception5h.zip
            //
            // - The model was trained after with images scaled to 256x267 pixels.
            // - The colors, represented as R, G, B in 1-byte each were converted to
            //   float using (value - Mean)/Scale.

            // image의 사이즈가 바뀌면 수정
            const int W = 256;
            const int H = 256;
            const float Mean = 0.0f;
            const float Scale = 1;

            graph = new TFGraph();
            input = graph.Placeholder(TFDataType.String);

            output = graph.Cast(graph.Div(
                x: graph.Sub(
                    x: graph.ResizeBilinear(
                        images: graph.ExpandDims(
                            input: graph.Cast(
                                graph.DecodeJpeg(contents: input, channels: 3), DstT: TFDataType.Float),
                            dim: graph.Const(0, "make_batch")),
                        size: graph.Const(new int[] { W, H }, "size")),
                    y: graph.Const(Mean, "mean")),
                y: graph.Const(Scale, "scale")), destinationDataType);
        }
    }
}
