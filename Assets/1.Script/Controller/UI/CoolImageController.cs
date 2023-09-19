using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoolImageController : MonoBehaviour
{
    public float coolTime;
    private float updateTime = 0.0f;

    
    private Image image;
    private Text text;

    private void OnEnable()
    {
        image = GetComponent<Image>();
        text = GetComponentInChildren<Text>();
        image.fillAmount = 1;
        updateTime = 0.0f;
    }
    void Update()
    {
        if(image.fillAmount <= 0)
            gameObject.SetActive(false);

        if(gameObject.activeSelf)
        {
            updateTime += Time.deltaTime;
            
            text.text = $"{(int)(coolTime - updateTime)}";

            image.fillAmount = 1.0f - (Mathf.Lerp(0, 100, updateTime / coolTime) / 100);
        }
    }
}
