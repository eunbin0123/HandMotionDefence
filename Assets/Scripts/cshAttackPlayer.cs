using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    Animator ani;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider coll)
    {
        if (coll.gameObject.tag == "player")
        {
            ani.SetBool("Attack", true);
            

        }
        else
        {
            
            ani.SetBool("RunFWD", true);
        }
    }
    private void Awake()
    {
        ani = GetComponent<Animator>();
    }
}
