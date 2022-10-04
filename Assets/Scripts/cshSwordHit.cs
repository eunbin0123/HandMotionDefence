using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshSwordHit : MonoBehaviour
{
    private Animator m_Anim;

    private int m_iMonster = 0; // 0:Walk, 1:Attack, 2:Hit, 3:Die
                                // Start is called before the first frame update
    private int num;

    void Start()
    {
        m_Anim = GetComponent<Animator>();
    }

    void OnEnable() {
        //num = 0;
    }

    private void OnTriggerEnter(Collider hitObject)
    {
        if (hitObject.gameObject.tag == "MonSword")
        {
            //if (num == 0)
            //{
                hitObject.GetComponent<cshMoveMonster>().SetMonster(2);
                num++;
                Debug.Log("aaaaaaaaaaaaa" + num);
                //hitObject.GetComponent<cshMoveMonster>().isHit = true;
            //}
            //Destroy(hitObject.gameObject);
        }
        else if (hitObject.gameObject.tag == "Slime") 
        {
            //if (num==0)
            //{
                hitObject.transform.parent.gameObject.GetComponent<cshMoveMonster>().SetMonster(2);
                num++;
                Debug.Log("aaaaaaaaaaaaa" + num);
                //hitObject.transform.parent.gameObject.GetComponent<cshMoveMonster>().isHit = true;
           // }
            //Destroy(hitObject.transform.parent.gameObject);
        }

    }
}
