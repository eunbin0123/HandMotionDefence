using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class cshCaptureCamera : MonoBehaviour
{
    // 4k = 3840 x 2160   1080p = 1920 x 1080
    public int captureWidth = 1920;
    public int captureHeight = 1080;

    // optional game object to hide during screenshots (usually your scene canvas hud)
    public GameObject hideGameObject;

    // optimize for many screenshots will not destroy any objects so future screenshots will be fast
    public bool optimizeForManyScreenshots = true;

    // configure with raw, jpg, png, or ppm (simple raw format)
    public enum Format { RAW, JPG, PNG, PPM };
    public Format format = Format.PPM;

    // folder to write output (defaults to data path)
    public string folder;

    // private vars for screenshot
    private Rect rect;
    private RenderTexture renderTexture;
    private Texture2D screenShot;
    private int counter = 0; // image #

    // commands
    private bool captureScreenshot = false;
    //private bool captureVideo = false;

    // time controls
    private float capturePassTiem = 0.0f;

    public Camera CaptureCamera;

    public OVRInput.Controller o_controller;

    // 제스쳐 인식 or 행동 수행 여부를 판단
    public bool m_Predict = false; // false: 제스쳐 인식이 가능, true: 행동 수행이 가능

    public GameObject m_DefaultLHands;
    public GameObject m_DefaultRHands;
    public GameObject[] m_ItemLHands;
    public GameObject[] m_ItemRHands;

    public GameObject m_CamLight;

    public GameObject StopGaze;
    public GameObject BuffGaze;


    // create a unique filename using a one-up variable
    private string uniqueFilename(int width, int height)
    {

        // if folder not specified by now use a good default
        if (folder == null || folder.Length == 0)
        {
            folder = Application.dataPath;
            if (Application.isEditor)
            {
                // put screenshots in folder above asset path so unity doesn't index the files
                //var stringPath = folder + "/..";
                var stringPath = folder + "/..";
                folder = Path.GetFullPath(stringPath);
            }
            //folder += "/screenshots";
            folder += "/Assets/Resources/images";

            // make sure directoroy exists
            System.IO.Directory.CreateDirectory(folder);

            // count number of files of specified format in folder
            string mask = string.Format("screen_{0}x{1}*.{2}", width, height, format.ToString().ToLower());
            counter = Directory.GetFiles(folder, mask, SearchOption.TopDirectoryOnly).Length;
        }

        // use width, height, and counter for unique file name
        // 파일명 다르게 저장하는 방법
        /*
        var filename = string.Format("{0}/screen_{1}x{2}_{3}.{4}", folder, width, height, counter, format.ToString().ToLower());

        // up counter for next call
        ++counter;
        */


        //파일명 같게 저장하는 방법
        var filename = string.Format("{0}/screen_{1}x{2}.{3}", folder, width, height, format.ToString().ToLower());


        // return unique filename
        return filename;
    }

    public void CaptureScreenshot()
    {
        captureScreenshot = true;
    }

    public void CaptureHands()
    {
        // 캡쳐하는 순간 손의 layer를 hand로 바꾸고, 캡쳐가 끝나면 다시 디폴트로
        m_CamLight.SetActive(true);
        // 캡쳐하기 전에 손의 위치에 대응하여 카메라의 위치와 방향을 수정
        Vector3 pHandCenter = (m_DefaultLHands.transform.position + m_DefaultRHands.transform.position) / 2.0f;
        float HandAngle = Vector3.Angle(m_DefaultLHands.transform.up, m_DefaultRHands.transform.up);
        Transform cam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Transform>();
        Vector3 vHandDir;
        if(HandAngle > 100.0f)
        {
            float distHand_y = Mathf.Abs(m_DefaultLHands.transform.position.y - m_DefaultRHands.transform.position.y);
            float distHand_z = Mathf.Abs(m_DefaultLHands.transform.position.z - m_DefaultRHands.transform.position.z);

            if ((distHand_y+distHand_z) < 0.1f) //   OK, ThumbUp gesture
            {
                //vHandDir = cam.forward;
                vHandDir = Vector3.forward;
            }
            else // Axe, Bow, Gun gesture
            {
                vHandDir = (m_DefaultLHands.transform.up - m_DefaultRHands.transform.up).normalized;
            }
        }
        else
        {
            Vector3 handForward = (m_DefaultLHands.transform.up + m_DefaultRHands.transform.up).normalized;
            //float camToHand = Vector3.Angle(handForward, cam.forward);
            float camToHand = Vector3.Angle(handForward, Vector3.forward);
            if (camToHand > 100.0f) // Stop gesture
            {
                vHandDir = -handForward;
            }
            else // Sword gesture
            {
                vHandDir = handForward;
            }
        }
      
        CaptureCamera.transform.position = pHandCenter + 1.0f * vHandDir;
        CaptureCamera.transform.LookAt(pHandCenter);

        // hide optional game object if set
        if (hideGameObject != null) hideGameObject.SetActive(false);

        // create screenshot objects if needed
        if (renderTexture == null)
        {
            // creates off-screen render texture that can rendered into
            rect = new Rect(0, 0, captureWidth, captureHeight);
            renderTexture = new RenderTexture(captureWidth, captureHeight, 24);
            screenShot = new Texture2D(captureWidth, captureHeight, TextureFormat.RGB24, false);
        }

        // get main camera and manually render scene into rt
        //Camera camera = this.GetComponent<Camera>(); // NOTE: added because there was no reference to camera in original script; must add this script to Camera
        Camera camera = CaptureCamera.GetComponent<Camera>();
        camera.targetTexture = renderTexture;
        camera.Render();

        // read pixels will read from the currently active render texture so make our offscreen 
        // render texture active and then read the pixels
        RenderTexture.active = renderTexture;
        screenShot.ReadPixels(rect, 0, 0);

        // reset active camera texture and render texture
        camera.targetTexture = null;
        RenderTexture.active = null;


        // 이미지 저장을 통해 확인
        // get our unique filename

        string filename = uniqueFilename((int)rect.width, (int)rect.height);

        // pull in our file header/data bytes for the specified image format (has to be done from main thread)
        byte[] fileHeader = null;
        byte[] fileData = null;

        if (format == Format.RAW)
        {
            fileData = screenShot.GetRawTextureData();
        }
        else if (format == Format.PNG)
        {
            fileData = screenShot.EncodeToPNG();
        }
        else if (format == Format.JPG)
        {
            fileData = screenShot.EncodeToJPG();

        }
        else // ppm
        {
            // create a file header for ppm formatted file
            string headerStr = string.Format("P6\n{0} {1}\n255\n", rect.width, rect.height);
            fileHeader = System.Text.Encoding.ASCII.GetBytes(headerStr);
            fileData = screenShot.GetRawTextureData();
        }

        // create new thread to save the image to file (only operation that can be done in background)

        new System.Threading.Thread(() =>
        {
            // create file and write optional header with image bytes
            var f = System.IO.File.Create(filename);
            if (fileHeader != null) f.Write(fileHeader, 0, fileHeader.Length);
            f.Write(fileData, 0, fileData.Length);
            f.Close();
            Debug.Log(string.Format("Wrote screenshot {0} of size {1}", filename, fileData.Length));
        }).Start();


        // unhide optional game object if set
        if (hideGameObject != null) hideGameObject.SetActive(true);

        // cleanup if needed
        if (optimizeForManyScreenshots == false)
        {
            Destroy(renderTexture);
            renderTexture = null;
            screenShot = null;
        }
        m_CamLight.SetActive(false);
    }

    IEnumerator CaptureScene()
    {
        while (true)
        {
            if (Input.GetKeyDown("t") || OVRInput.GetDown(OVRInput.Button.One, o_controller))
            {
                if (!m_Predict)
                {
                    CaptureHands();
                    if (screenShot != null)
                        GetComponent<cshInceptionv3ImageClassifierApply>().PredictedHands();
                    // 인식된 결과를 받아오고
                    int iPredict = GetComponent<cshInceptionv3ImageClassifierApply>().m_iAction;
                    // 정상 인식이 되었다면
                    if (iPredict >= 0 && iPredict <= 3)
                    {
                        // 기본 손은 active를 false로
                        m_DefaultLHands.SetActive(false);
                        m_DefaultRHands.SetActive(false);

                        // 인식된 제스쳐에 맞는 손을 active true로 
                        m_ItemLHands[iPredict].SetActive(true);
                        m_ItemRHands[iPredict].SetActive(true);

                        m_Predict = true;
                    }
                    else
                    {
                        //Debug.Log("in");
                        // 정상 인식이 안되었다면 다시 처음으로
                        //continue;
                        //break;
                        if (iPredict == 4) //ThumbUp
                        {
                            // GUI 비교 실험이 아닐 경우 주석
                            //GameObject.Find("TimeManager").GetComponent<cshTimeManager>().isFull_buff = true;


                            //GameObject.Find("TimeManager").GetComponent<cshTimeManager>().DoFeverTime();


                            // GUI 비교 실험에서는 주석
                            if (GameObject.Find("TimeManager").GetComponent<cshTimeManager>().isFull_buff == true)
                            {
                                GameObject.Find("TimeManager").GetComponent<cshTimeManager>().DoFeverTime(); //
                                GameObject.Find("OVRCameraRigDeep").GetComponent<cshScore>().point = 20;
                                BuffGaze.GetComponent<cshBuffGaze>().current = 0;
                                BuffGaze.GetComponent<cshBuffGaze>().mask.fillAmount = 0;
                                BuffGaze.GetComponent<cshBuffGaze>().fill.color = Color.red;
                                GameObject.Find("TimeManager").GetComponent<cshTimeManager>().isFull_buff = false;
                            }
                            //GameObject.Find("OVRCameraRigDeep").GetComponent<cshScore>().point =10;


                        }
                        else if(iPredict == 5) //Stop
                        {
                            // GUI 비교 실험이 아닐 경우 주석
                            //GameObject.Find("TimeManager").GetComponent<cshTimeManager>().isFull_stop = true;

                            //GameObject.Find("TimeManager").GetComponent<cshTimeManager>().DoSlowmotion();

                            // GUI 비교 실험에서는 주석

                            if (GameObject.Find("TimeManager").GetComponent<cshTimeManager>().isFull_stop == true)
                            {
                                GameObject.Find("TimeManager").GetComponent<cshTimeManager>().DoSlowmotion();
                                StopGaze.GetComponent<cshStopGaze>().current = 0;
                                GameObject.Find("TimeManager").GetComponent<cshTimeManager>().isFull_stop = false;
                            }

                        }


                        m_Predict = false;
                        //break;
                    }

                    //m_Predict = true;
                }
                else
                {
                    // 행동이 진행되는 과저에서 다시 'A'키가 눌렸다면
                    for (int i = 0; i < 4; i++)
                    {
                        // 행동 중인 손을 찾아 active를 false로
                        if (m_ItemLHands[i].activeSelf == true)
                        {
                            m_ItemLHands[i].SetActive(false);
                            m_ItemRHands[i].SetActive(false);
                        }
                    }
                    // 기본 손의 active를 true로
                    m_DefaultLHands.SetActive(true);
                    m_DefaultRHands.SetActive(true);
                    m_Predict = false;
                }


                //UnityEditor.AssetDatabase.Refresh();
            }

            yield return null;
        }
    }

    private void Start()
    {
        StopGaze = GameObject.Find("StopBar");
        BuffGaze = GameObject.Find("BuffBar");

        CaptureHands();
        if (screenShot != null)
            GetComponent<cshInceptionv3ImageClassifierApply>().PredictedHands();


        StartCoroutine("CaptureScene");
    }

    void Update()
    {
        // 주기적인 캡쳐를 위한 임시코드
        /*
        if (captureScreenshot)
        {
            if (capturePassTiem >= 1.0f)
            {
                CaptureHands();
                capturePassTiem = 0.0f;
            }
            else
            {
                capturePassTiem += Time.deltaTime;
            }
        }*/
    }
}
