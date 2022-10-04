using UnityEngine;
using System.Collections;

public class Shockwave : MonoBehaviour {

public GameObject ShockwaveFX;
public Light ShockwaveLight;
public float lightRangeSizeStart;
public float lightRangeSizeEnd;
public float lightIntensity = 8;

private float t = 0.0f;
private float fadeEnd = 0;
private float fadeTime = 2;
private float pauseTime = 0;
private float fadeRange = 3.5f;
private bool activeShockwave = false;

    public bool isFire = false;



void Start (){

    ShockwaveFX.SetActive(false);
    fadeRange = lightRangeSizeStart;

}

 
void Update (){

    if (Input.GetButtonDown("Fire1"))
    {

        if (activeShockwave == false)
        {
			 StartCoroutine("launchShockwave");
        }

    }
        if (isFire)
        {
            if (activeShockwave == false)
            {
                StartCoroutine("launchShockwave");
            }
        }
   
}


IEnumerator FadeLight (){
   
     while (t < fadeTime) 
     {

         if (pauseTime == 0)
         {
             t += Time.deltaTime;
         }
          
         if (fadeRange < lightRangeSizeEnd)
         {
             fadeRange = fadeRange + 0.5f;
         }

         ShockwaveLight.intensity = Mathf.Lerp(lightIntensity, fadeEnd, t / fadeTime);
         ShockwaveLight.range = fadeRange;
         yield return 0;

     }                      
            
t = 0;
    
}


IEnumerator launchShockwave (){
        Destroy(gameObject, 2.0f);
        ShockwaveFX.SetActive(true);
	StartCoroutine("FadeLight");
  

    activeShockwave = true;

	yield return new  WaitForSeconds (4);  //  Wait for the shockwave to finish
   
    activeShockwave = false;
    ShockwaveFX.SetActive(false);
        
}

}