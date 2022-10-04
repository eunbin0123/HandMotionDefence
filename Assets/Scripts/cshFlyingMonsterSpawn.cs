using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshFlyingMonsterSpawn : MonoBehaviour
{
    public Transform[] spawnspots;
    public GameObject[] montsers;

    public float spawnTime = 3.0f;
    public float spawnPassTime = 0.0f;

    private int before_rnd;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        int rndspot = Random.Range(0, 3);
        int rndmonster = Random.Range(0, 3);
        if (spawnPassTime >= spawnTime)
        {

            Instantiate(montsers[rndmonster], spawnspots[rndspot].position, Quaternion.Euler(0, 180.0f, 0));

            spawnPassTime = 0.0f;
        }

        else
        {
            spawnPassTime += Time.deltaTime;
        }
    }
}
