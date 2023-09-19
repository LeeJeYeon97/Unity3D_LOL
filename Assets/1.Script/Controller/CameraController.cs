using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    Define.CameraMode mode = Define.CameraMode.QuaterView;
    // player�������� ������ ũ��
    [SerializeField]
    Vector3 delta = new Vector3(0.0f, 7.0f, -5.0f);

    [SerializeField]
    GameObject target = null;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("PLAYER");
    }

    void LateUpdate()
    {
        if(mode == Define.CameraMode.QuaterView)
        {
            transform.position = target.transform.position + delta;
            transform.LookAt(target.transform);
        }
        
        float wheelInput = Input.GetAxis("Mouse ScrollWheel");
        if (wheelInput > 0)
        {
            // ���� �о� ������ ���� ó�� ��
            delta.y -= wheelInput;
            delta.z += wheelInput / 2;
        }
        else if (wheelInput < 0)
        {
            // ���� ��� �÷��� ���� ó�� ��
            delta.y -= wheelInput;
            delta.z += wheelInput / 2;
        }

        //Vector2 wheelInput2 = Input.mouseScrollDelta;
        //if (wheelInput2.y > 0)
        //{
        //    // ���� �о� ������ ���� ó�� ��
        //}
        //else if (wheelInput2.y < 0)
        //{
        //    // ���� ��� �÷��� ���� ó�� ��
        //}
       
    }

    public void SetQuaterView(Vector3 delta)
    {
        mode = Define.CameraMode.QuaterView;
        this.delta = delta;
    }
}

