using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshRankScore : MonoBehaviour
{
    public Text myScoreText;
    public Text bestScoreText;

    //private int savedScore = 0;
    //private string KeyString = "best score";

    // Start is called before the first frame update
    void Start()
    {
       myScoreText.text = "My Score : " + cshScore.score;
    }

    //private void Awake()
    //{
    //    savedScore = PlayerPrefs.GetInt(KeyString, 0);
    //}

    // Update is called once per frame
    void Update()
    {
        //myScoreText.text = "My Score : " + cshScore.score;
        //bestScoreText.text = "Best Score : " + savedScore.ToString("0");

        //if(recordScore > savedScore)
        //{
        //    PlayerPrefs.SetInt(KeyString, cshScoreManager.score);
        //}
    }
}
