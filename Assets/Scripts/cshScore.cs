using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cshScore : MonoBehaviour
{

    public static int score;
    public Text ScoreText;
    public int point;

    // Start is called before the first frame update
    void Start()
    {
        point = 10;
        score = 0;
        ScoreText.text = "Score : " + score;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void AddScore()
    {
        score += point;
        ScoreText.text = "Score : " + score;
        Debug.Log("점수는 " + score);

    }

    public void SubScore()
    {
        if(score>=10)
           score -= 10;
        ScoreText.text = "Score : " + score;
        Debug.Log("점수는 " + score);

    }
}
