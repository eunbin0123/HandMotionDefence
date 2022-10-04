using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshArrow : MonoBehaviour
{
    public float m_Speed = 2000.0f;
    public Transform m_Tip = null;

    private Rigidbody m_Rigidbody = null;
    private bool m_IsStopped = true;
    private Vector3 m_LastPosition = Vector3.zero;

    private Vector3 m_initPos;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();

       
    }
    private void FixedUpdate()
    {
        if (m_IsStopped)
            return;

        // Rotate
        m_Rigidbody.MoveRotation(Quaternion.LookRotation(m_Rigidbody.velocity, transform.up));

        // Collision
        RaycastHit hit;
        if(Physics.Linecast(m_LastPosition, m_Tip.position, out hit))
        {
            if(hit.collider.gameObject.tag != "Player")
                Stop(hit.collider.gameObject);
        }

        // Store Position
        m_LastPosition = m_Tip.position;
    }

    private void Stop(GameObject hitObject)
    {
        m_IsStopped = true;

        // Parent
        transform.parent = hitObject.transform;

        m_Rigidbody.isKinematic = true;
        m_Rigidbody.useGravity = false;

        // Damage
        CheckForDamage(hitObject);
    }

    public void Fire(float pullValue)
    {
        m_IsStopped = false;

        transform.parent = null;

        m_Rigidbody.isKinematic = false;
        m_Rigidbody.useGravity = true;
        m_Rigidbody.AddForce(transform.forward * (pullValue * m_Speed));

        //GetComponentInChildren<Collider>().enabled = true;

        Destroy(gameObject, 5.0f);
    }
    // Start is called before the first frame update
    void Start()
    {
        m_LastPosition = transform.position;
        m_initPos = GetComponent<Transform>().localPosition;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void CheckForDamage(GameObject hitObject)
    {
        // hitObject 충돌된 객체
        if(hitObject.tag == "MonBow")
        {
            hitObject.GetComponent<cshMoveMonster>().SetMonster(2);
            //Destroy(hitObject);


            // CASA
            //if (hitObject.GetComponent<cshCASAMonster>())
            //{
            //    hitObject.GetComponent<cshCASAMonster>().SetMonster(3);
            //    hitObject.GetComponent<cshCASAMonster>().CreateCASA();
            //}
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Movement(float pullValue)
    {
        GetComponent<Transform>().localPosition = new Vector3(m_initPos.x, m_initPos.y, m_initPos.z - pullValue * 0.505f);
    }
}
