using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class cshStopGaze : MonoBehaviour
{

    public float minimum;
    public float maximum;
    public float current;
    public Image mask;
    public Image fill;
    public Color color;

    public GameObject TimeManager;

    // Start is called before the first frame update
    void Start()
    {
        current = 0;
        TimeManager = GameObject.Find("TimeManager");

    }

    // Update is called once per frame
    void Update()
    {
        if (current < maximum)
            current += Time.deltaTime;
        GetCurrentFill();
    }

    void GetCurrentFill() 
    {
        float currentOffset = current - minimum;
        float maximumOffset = maximum - minimum;
        float fillAmount = currentOffset / maximumOffset;
        mask.fillAmount = fillAmount;

       
        fill.color = Color.red;

        if (current >= maximum)
        {
            fill.color = Color.green;
            TimeManager.GetComponent<cshTimeManager>().isFull_stop = true;
            //yield return new WaitForSeconds(10.0f);
        }
    }
}
