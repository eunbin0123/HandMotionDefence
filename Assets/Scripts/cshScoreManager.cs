using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class cshScoreManager : MonoBehaviour
{
    private bool isFinish;
    public float time;
    public float finishTime;
    public Text scoreText;
    public Text timeText;
    public string score;
    //Spublic static string bestScore;

    char c;
    string[] str;


    void Start()
    {
        isFinish = false;
        time = 100.0f;
        finishTime = 0.0f;
        c = ':';
    }

    // Update is called once per frame
    void Update()
    {
        if (time > finishTime)
        {
            time -= Time.deltaTime;
        }
        else if (time <= finishTime)
        {
            isFinish = true;
        }

        timeText.text = "Time : " + Mathf.Ceil(time).ToString();

        if (isFinish == true)
        {
            score = scoreText.text;

            //str = score.Split(c);
            //str[1] = str[1].Trim();
            //Debug.Log(str[1]);
            SceneManager.LoadScene("scRank");
            //Debug.Log("최종점수는 " + str[1]);
        }
    }
}