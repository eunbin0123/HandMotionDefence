using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class cshMoveMonster : MonoBehaviour
{
    private Animator m_Anim;
    public Transform[] m_spots;
    private Vector3 m_MoveDir;
    private Transform m_thisTrans;
    private Vector3 m_Dest;

    private int m_iAttackState = 0;

    private int m_iMonster = 0; // 0:Walk, 1:Attack, 2:Hit, 3:Die

    public float m_HitTime;
    public float m_DieTime;
    public float m_AttackTime;
    public float walk_speed = 7.0f;

    public float move_speed = 2.0f;

    public int score = 0;
    public bool isHit = false;
    public Text ScoreText;
    private GameObject Player;
    private GameObject buffGaze;

    public GameObject DieVFX;
    GameObject fx = null;
    // Start is called before the first frame update
    private int num;
    void Start()
    {
        // Player = GameObject.Find("OVRCameraRigDeep");
        Player = GameObject.FindGameObjectWithTag("Player");
        buffGaze = GameObject.Find("BuffBar");
        //ScoreText.text = "0";
        m_thisTrans = GetComponent<Transform>();
        m_Anim = GetComponent<Animator>();
        int rnd = Random.Range(0, 3);
        m_Dest = m_spots[rnd].position;

        m_MoveDir = m_Dest- m_thisTrans.position;
        m_MoveDir.Normalize();
        num = 0;
    }

    void OnEnable()
    {
        
    }


    // Update is called once per frame
    void Update()
    {
        //score = int.Parse(ScoreText.text);

        if (m_iMonster == 0)
        {
            Vector3 look = Vector3.Slerp(m_thisTrans.forward, m_MoveDir, Time.deltaTime * walk_speed);
            m_thisTrans.rotation = Quaternion.LookRotation(look, Vector3.up);

            //m_thisTrans.LookAt(m_Dest);
            m_thisTrans.Translate(m_MoveDir * move_speed * Time.deltaTime, Space.World);
        }else if(m_iMonster == 2 )
        {
            if (DieVFX)
            {
                //Debug.Log("afda");
                if(fx == null)
                {
                    fx = Instantiate(DieVFX, transform.position, Quaternion.identity);
                    Destroy(fx, 1.0f);
                }
            }

            if (num == 0)
            {
                //Debug.Log("bbbbbbbbbbbbbbbbbbb");
                m_Anim.SetTrigger("Hit");
                //      score += 10;
                // isHit = false;
                //     Debug.Log("점수는 " + score);
                //     ScoreText.text = "Score : " + score;
                buffGaze.GetComponent<cshBuffGaze>().addGaze();
                num++;
                Destroy(gameObject, m_HitTime);
                Player.GetComponent<cshScore>().AddScore();
            }
        }
        else if(m_iMonster == 3)
        {
            if (num == 0)
            {
                m_Anim.SetTrigger("Die");
                num++;
                Destroy(gameObject, m_DieTime);
                //Player.GetComponent<cshScore>().SubScore();
            }
        }
        // 임시로 작성
        /*if (Input.GetKeyDown(KeyCode.A))
        {
            m_iMonster = 2;
        }*/
    }

    private void OnTriggerEnter(Collider other)
    {
        // 현재는 이름으로 했지만, 태그등으로 변경
        if(other.gameObject.tag == "Player")
        {
            if(gameObject.tag != "MonSword")
            {
                if (m_iAttackState == 0)
                {
                    StartCoroutine(Attack(m_AttackTime, other.gameObject.transform.position));
                }
            }
  
            // attack으로 동작을 time초 동안 실행 후, 다시 이동
           
        } else if(other.gameObject.name == "SwordZone")
        {
            if (gameObject.tag == "MonSword")
            {
                if (m_iAttackState == 0)
                {
                    StartCoroutine(Attack(m_AttackTime, other.gameObject.transform.position));
                }
            }
        }
        else if(other.gameObject.name == "DestSpot")
        {
            m_iMonster = 3;
        }


        if (other.gameObject.tag == "bullet")
        {
            if(gameObject.tag == "MonGun")
            {
                m_iMonster = 2;
               // GameObject.Find("Bullet_45mm_Full(Clone)").GetComponent<cshBullet_Monster_Destroy>().isHit = true;
            } 
        }

        // else if(화살, 총, 도끼 등과 충돌)
        // m_iMonster = 2로 변경

    }

    IEnumerator Attack(float time, Vector3 target)
    {
        m_Anim.SetTrigger("Attack");
        m_thisTrans.LookAt(target);

        m_iMonster = 1;
        m_iAttackState = 1;

        yield return new WaitForSeconds(time);

        m_iMonster = 0;
    }

    public void SetMonster(int i)
    {
        m_iMonster = i;
    }
}
