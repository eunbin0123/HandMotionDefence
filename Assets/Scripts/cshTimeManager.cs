using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshTimeManager : MonoBehaviour
{
    /*
    public float slowdownFactor = 0.05f;
    public float slowdownLength = 5f;
    */
    public GameObject spawn;
    public GameObject blurEffect;
    public GameObject blurEffectGUI;
    public Transform Player;
    public GameObject ShieldVFX;

    public Material FeverSkybox;
    public GameObject FeverLight;
    public GameObject FeverVFX;

    public Material orgSkybox;

    public bool isFull_stop;
    public bool isFull_buff;

    void Strat()
    {
        
        isFull_stop = false;
        isFull_buff = false;
    }
      
    void Update()
    {
        /*
        Time.timeScale += (1f / slowdownLength) * Time.unscaledDeltaTime;
        Time.timeScale = Mathf.Clamp(Time.timeScale, 0f, 1f);
        */
        
    }

    IEnumerator SlowMotionProcess()
    {
        spawn.SetActive(false);
        blurEffect.GetComponent<CoolMotionBlur>().enabled = true;
        // GUI 모드
        blurEffectGUI.GetComponent<CoolMotionBlur>().enabled = true;
        GameObject[] MonAxe = GameObject.FindGameObjectsWithTag("MonAxe");
        GameObject[] MonBow = GameObject.FindGameObjectsWithTag("MonBow");
        GameObject[] MonGun = GameObject.FindGameObjectsWithTag("MonGun");
        GameObject[] MonSword = GameObject.FindGameObjectsWithTag("MonSword");
        ChangeAnimationSpeed(MonAxe, 0.3f);
        ChangeMoveSpeed(MonAxe, 0.6f);
        ChangeAnimationSpeed(MonBow, 0.3f);
        ChangeMoveSpeed(MonBow, 0.6f);
        ChangeAnimationSpeed(MonGun, 0.3f);
        ChangeMoveSpeed(MonGun, 0.6f);
        ChangeAnimationSpeed(MonSword, 0.3f);
        ChangeMoveSpeed(MonSword, 0.6f);

        GameObject fx = Instantiate(ShieldVFX, Player.position, Quaternion.identity);

        Destroy(fx,10.0f);

        yield return new WaitForSeconds(10.0f);

        spawn.SetActive(true);
        blurEffect.GetComponent<CoolMotionBlur>().enabled = false;
        // GUI 모드
        blurEffectGUI.GetComponent<CoolMotionBlur>().enabled = false;
        ChangeAnimationSpeed(MonAxe, 1.0f);
        ChangeMoveSpeed(MonAxe, 2.0f);
        ChangeAnimationSpeed(MonBow, 1.0f);
        ChangeMoveSpeed(MonBow, 2.0f);
        ChangeAnimationSpeed(MonGun, 1.0f);
        ChangeMoveSpeed(MonGun, 2.0f);
        ChangeAnimationSpeed(MonSword, 1.0f);
        ChangeMoveSpeed(MonSword, 2.0f);

       // Destroy(fx);
    }

    IEnumerator FeverTimeProcess()
    {
        RenderSettings.skybox = FeverSkybox;
        FeverLight.SetActive(false);
        GameObject fx = Instantiate(FeverVFX);

        
       //GameObject.Find("OVRCameraRigDeep").GetComponent<cshScore>().point = 20;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cshScore>().point = 20;

        Destroy(fx, 10.0f);

        yield return new WaitForSeconds(10.0f);
        //GameObject.Find("OVRCameraRigDeep").GetComponent<cshScore>().point = 10;
        GameObject.FindGameObjectWithTag("Player").GetComponent<cshScore>().point = 10;

        RenderSettings.skybox = orgSkybox;
        FeverLight.SetActive(true);
        //Destroy(fx);

    }

    public void DoSlowmotion()
    {
        /*
        Time.timeScale = slowdownFactor;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
        */
        if(isFull_stop)
           StartCoroutine("SlowMotionProcess");


    }

    public void DoFeverTime()
    {
        if (isFull_buff)
            StartCoroutine("FeverTimeProcess");
    }

    void ChangeAnimationSpeed(GameObject[] obj, float speed)
    {
        for(int i=0;i<obj.Length; i++)
        {
            if(obj[i] !=null)
                obj[i].GetComponent<Animator>().speed = speed;
        }
    }

    void ChangeMoveSpeed(GameObject[] obj, float speed)
    {
        for (int i = 0; i < obj.Length; i++)
        {
            if (obj[i] != null)
                obj[i].GetComponent<cshMoveMonster>().move_speed = speed;
        }
    }
}
