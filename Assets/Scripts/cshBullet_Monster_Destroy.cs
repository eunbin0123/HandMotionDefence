using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshBullet_Monster_Destroy : MonoBehaviour
{

    //public GameObject DieVFX;
    //GameObject fx = null;
    //public bool isHit;

    // Start is called before the first frame update
    void Start()
    {
        //isHit = false;
    }

    // Update is called once per frame
    void Update()
    {

        Destroy(gameObject, 5.0f);
    }

    /*
    private void OnTriggerEnter(Collider hitObject)
    {
        if (hitObject.transform.tag == "Monster")
        {
            Destroy(hitObject.gameObject);
            Destroy(gameObject);
        }
    }
    */
}
