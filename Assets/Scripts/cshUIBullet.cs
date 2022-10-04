using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class cshUIBullet : MonoBehaviour
{
    
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.name == "RankButton")
        {
            //Debug.Log("RankButton");
            SceneManager.LoadScene("scRank");
        }

        else if(coll.gameObject.name == "PlayButton")
        {
            //Debug.Log("PlayButton");
            SceneManager.LoadScene("scComparison");
        }

        else if (coll.gameObject.name == "EditButton")
        {
            //Debug.Log("EditButton");
            SceneManager.LoadScene("scEdit");
        }

        else if (coll.gameObject.name == "ExitButton")
        {
            //Debug.Log("EditButton");
            Application.Quit();
        }

        else if (coll.gameObject.name == "MainMenuButton")
        {
            //Debug.Log("EditButton");
            SceneManager.LoadScene("scStart");
        }

        else if (coll.gameObject.name == "RetryButton")
        {
            //Debug.Log("EditButton");
            SceneManager.LoadScene("scComparison");
        }
    }
}
