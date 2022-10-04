using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshCASAMonster : MonoBehaviour
{

    private Animator m_Anim;
    private Transform m_thisTrans;

    public int m_iAttackState = 0;

    private int m_iMonster = 0; // 0:Walk, 1:Attack, 2:Hit, 3:Die

    public float m_HitTime;
    public float m_DieTime;
    public float m_AttackTime;

    public GameObject m_CASAObj;

    // Start is called before the first frame update
    void Start()
    {
        m_thisTrans = GetComponent<Transform>();
        m_Anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_iMonster == 3)
        {
            m_Anim.SetTrigger("Die");
            Destroy(gameObject, m_DieTime);
        }

    }

    public void CreateCASA()
    {
        if(m_iAttackState == 0)
            Instantiate(m_CASAObj, transform.position, Quaternion.Euler(0.0f, 180.0f, 0.0f));
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "bullet")
        {
            if (gameObject.tag == "MonGun")
            {
                m_iMonster = 3;
                CreateCASA();
                //if (m_iAttackState == 0) CreateCASA();
                m_iAttackState++;
                
            }
        }
    }

    public void SetMonster(int i)
    {
        m_iMonster = i;
    }
}
