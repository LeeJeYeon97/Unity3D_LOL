using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GoldUIController : MonoBehaviour
{
    
    private GameObject target;
    public GameObject Target
    {
        get { return target; }
        set { target = value; }
    }

    float time = 2.0f;
    public Text goldText;

    private void OnEnable()
    {
        gameObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;

        transform.rotation = Camera.main.transform.rotation;
        if (target != null)
        {
            transform.position = target.transform.position +
            new Vector3(0, target.GetComponent<Collider>().bounds.size.y, 0);

            goldText.text = $"+{target.GetComponent<Stat>().dieGold}";
        }
    }
    void Update()
    {
        if(gameObject.activeSelf)
        {
            time -= Time.deltaTime;

            if(time<=0)
            {
                gameObject.SetActive(false);
                time = 2.0f;
            }
        }
    }
}
