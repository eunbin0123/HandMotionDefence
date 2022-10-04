using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cshLogo : MonoBehaviour
{
    float speed = 60f;
    float x, y, z = 0.01f;

    bool is_move = true;
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(0, 0, 100);
        transform.localScale = new Vector3(x, y, z);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z >0 && is_move == true)
        {
            transform.Translate(-Vector3.forward * speed * Time.deltaTime);
            x += 0.02f;
            y += 0.02f;
            z += 0.02f;
            transform.localScale = new Vector3(x, y, z);
            if (transform.position.z < 1)
                is_move = false;
        }

        if (transform.position.z <= 10 && is_move == false)
        {
            transform.Translate(Vector3.forward * 2 * speed * Time.deltaTime);
            x -= 0.05f;
            y -= 0.05f;
            z -= 0.05f;
            transform.localScale = new Vector3(x, y, z);
        }
    }
}
