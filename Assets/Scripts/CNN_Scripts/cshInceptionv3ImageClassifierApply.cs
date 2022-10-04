using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TensorFlow;
using UnityEditor;
using UnityEngine.UI;

public class cshInceptionv3ImageClassifierApply : MonoBehaviour
{
    private cshInceptionv3ImageClassifier classifier = new cshInceptionv3ImageClassifier();

    public string m_sAction = null;
    public int m_iAction = -1; // 0: Axe, 1: Bow, 2: Gun

    // Use this for initialization
    void Start()
    {
        Debug.Log(TensorFlow.TFCore.Version);
        classifier.LoadModel("tf_models/optimized_inception_graph");
        //classifier.LoadModel("tf_models/optimized_inceptionDH_graph");

        //StartCoroutine("Predicted");
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator Predicted()
    {
        while (true)
        {
            if (Input.GetKeyDown("p"))
            {
                //Texture2D img = Resources.Load<Texture2D>("images/screen_256x256");
                //Debug.Log("Predicted: " + classifier.PredictLabel(img));
                //string pathRsc = "Assets/Resources/images/screen_256x256.jpg";
                //AssetDatabase.ImportAsset(pathRsc, ImportAssetOptions.ImportRecursive);

                //Debug.Log("Predicted: " + classifier.PredictLabel(imgData));

            }
            yield return null;
        }
    }

    public void PredictedHands()
    {
        m_sAction = classifier.PredictLabel();
        switch (m_sAction)
        {
            case "Axe":
                m_iAction = 0;
                break;
            case "Bow":
                m_iAction = 1;
                break;
            case "Gun":
                m_iAction = 2;
                break;
            case "Sword":
                m_iAction = 3;
                break;
            case "Thumb":
                m_iAction = 4;
                break;
            case "Stop":
                m_iAction = 5;
                break;
            default:
                m_iAction = -1;
                break;
        }

        Debug.Log("Predicted: " + m_sAction);
    }
}
