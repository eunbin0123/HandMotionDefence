using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshOculusInput : MonoBehaviour
{
    public cshBow m_Bow = null;

    public GameObject m_OppositeContoller = null;
    public OVRInput.Controller m_Controller = OVRInput.Controller.None;

    private Animator animHand;

    private bool bBowHandMotionOn = false;
    private void Awake()
    {
        animHand = m_OppositeContoller.GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }   

    // Update is called once per frame
    void Update()
    {
        if (OVRInput.GetDown(OVRInput.Button.PrimaryIndexTrigger, m_Controller))
        {
            m_Bow.Pull(m_OppositeContoller.transform);
            animHand.SetBool("IsShoot", true);
        }
        if (OVRInput.GetUp(OVRInput.Button.PrimaryIndexTrigger, m_Controller))
        { 
            m_Bow.Release();
            animHand.SetBool("IsShoot", false);
        }
    }
}
