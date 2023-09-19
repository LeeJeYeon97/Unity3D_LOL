using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class HpBarController : MonoBehaviour
{
    private Transform parent;
    private Stat parentStat;

    public Slider hpBar;
    public Slider mpBar;
    public Text levelText;
    void Start()
    {
        parent = transform.parent;
        parentStat = parent.GetComponent<Stat>();

    }
    
    // Update is called once per frame
    void Update()
    {
        gameObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
        gameObject.GetComponent<Canvas>().worldCamera = Camera.main;
    
        parent = transform.parent;
        transform.position = parent.position + 
            new Vector3(0, (parent.GetComponent<Collider>().bounds.size.y+1),0);
        transform.rotation = Camera.main.transform.rotation;
        //transform.LookAt(Camera.main.transform);
    
        float ratio = (float)parentStat.curHp / parentStat.maxHp;
        float mpRatio = (float)parentStat.curMp / parentStat.maxMp;

        if(levelText !=null)
            levelText.text = $"{parentStat.level}";
        SetHpRatio(ratio,mpRatio);
    }
    
    public void SetHpRatio(float ratio, float mpRatio)
    {
        //transform.GetComponentInChildren<Slider>().value = ratio;
        hpBar.value = ratio;
        if(mpBar!=null)
            mpBar.value = mpRatio;
        //transform.GetComponentInChildren<Slider>().value = mpRatio;
    }
}
