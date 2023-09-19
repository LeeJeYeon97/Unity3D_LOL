using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JinxMineController : MonoBehaviour
{
    public GameObject mine_1;
    public GameObject mine_2;

    private void Start()
    {
        
    }
    void Update()
    {

        if (transform.position.y <= 0)
        {
            transform.position = new Vector3(transform.position.x, 0, transform.position.z);
            return;
        }
        else
        {
            Vector3 dir = Vector3.down * 5.0f + transform.forward * 10.0f;
            transform.position += dir * Time.deltaTime;

            Vector3 mine1dir = -transform.right * 7.0f;
            mine_1.transform.position += mine1dir * Time.deltaTime;
            Vector3 mine2dir = transform.right * 7.0f;
            mine_2.transform.position += mine2dir * Time.deltaTime;
        }
    }
}
