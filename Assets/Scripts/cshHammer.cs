using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshHammer : MonoBehaviour
{
    public GameObject fire;
    public GameObject lightning;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Floor")
        {

            GameObject[] mons = GameObject.FindGameObjectsWithTag("MonAxe");
            if(mons.Length >= 1)
            {
                GameObject obj = GameObject.Find("Shockwave");
                if(obj == null)
                {
                    GameObject F = Instantiate(fire, new Vector3(0.0f, 0.2f, -6.5f), Quaternion.identity);
                    F.GetComponent<Shockwave>().isFire = true;
                }
              
            }
            //Lightning Effect
            for(int i=0; i < mons.Length; i++)
            {
                Vector3 pos = mons[i].transform.position;
                GameObject F = Instantiate(lightning, pos, Quaternion.identity);
                Destroy(F, 2.0f);
            }

            for(int i=0; i<mons.Length; i++)
            {
                mons[i].GetComponent<cshMoveMonster>().SetMonster(2);


                //CASA
                if (mons[i].gameObject.GetComponent<cshCASAMonster>())
                {
                    mons[i].gameObject.GetComponent<cshCASAMonster>().SetMonster(3);
                    
                    mons[i].gameObject.GetComponent<cshCASAMonster>().CreateCASA();
                    mons[i].gameObject.GetComponent<cshCASAMonster>().m_iAttackState++;
                }
            }
        }
    }
}
