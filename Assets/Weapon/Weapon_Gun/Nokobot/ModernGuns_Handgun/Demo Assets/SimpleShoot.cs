using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleShoot : MonoBehaviour
{
    public OVRInput.Controller o_controller;
    private OVRInput.Button o_indexButton = OVRInput.Button.PrimaryIndexTrigger;
    private bool indexButtonDown; // index finger button down


    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public Transform barrelLocation;
    public Transform casingExitLocation;


    public float shotPower = 100f;

    public Animator m_parentAnim;


    void GetVrInput()
    {
        // index finger button down or up
        indexButtonDown = OVRInput.GetDown(o_indexButton, o_controller);

        if(OVRInput.Get(o_indexButton, o_controller))
        {
            if(m_parentAnim != null)
                m_parentAnim.SetBool("IsShoot", true);
        }
        else
        {
            if (m_parentAnim != null)
                m_parentAnim.SetBool("IsShoot", false);
        }
    }

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;
    }

    void Update()
    {
        /*
        if (Input.GetButtonDown("Fire1"))
        {
            GetComponent<Animator>().SetTrigger("Fire");
        }
        */

        GetVrInput();
        if (indexButtonDown)
        {
            // GetComponent<Animator>().SetTrigger("Fire");
            Shoot();
        }
    }

    void Shoot()
    {
        //  GameObject bullet;
        //  bullet = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        // bullet.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

        GameObject tempFlash;
        GameObject bulletkts;
        bulletkts = Instantiate(bulletPrefab, barrelLocation.position, barrelLocation.rotation);
        bulletkts.GetComponent<Rigidbody>().AddForce(barrelLocation.forward * shotPower);

        tempFlash = Instantiate(muzzleFlashPrefab, barrelLocation.position, barrelLocation.rotation);
        Destroy(bulletkts, 5.0f); //총알 삭제
        Destroy(tempFlash, 0.5f); //총쏘는 이펙트 삭제

        // Destroy(tempFlash, 0.5f);
        //  Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation).GetComponent<Rigidbody>().AddForce(casingExitLocation.right * 100f);

    }

    void CasingRelease()
    {
        GameObject casing;
        casing = Instantiate(casingPrefab, casingExitLocation.position, casingExitLocation.rotation) as GameObject;
        casing.GetComponent<Rigidbody>().AddExplosionForce(550f, (casingExitLocation.position - casingExitLocation.right * 0.3f - casingExitLocation.up * 0.6f), 1f); // 0.6f는 탄피날라가는속도
        casing.GetComponent<Rigidbody>().AddTorque(new Vector3(0, Random.Range(100f, 500f), Random.Range(10f, 1000f)), ForceMode.Impulse);
        Destroy(casing, 0.5f); //탄피 삭제
    }


}
