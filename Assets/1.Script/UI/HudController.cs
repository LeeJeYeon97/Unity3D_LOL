using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudController : MonoBehaviour
{
    public Slider hpBar;
    public Slider mpBar;

    public GameObject player;
    public Stat playerStatData;
    public SkillData playerSkillData;

    public Texture2D champImage;
    public Image champImage_ui;

    public Image[] skill_image;
    public Image[] cool_image;

    public Text hpBarText;
    public Text mpBarText;

    public Text levelText;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("PLAYER");
        
        foreach(Image cool in cool_image)
            cool.gameObject.SetActive(false);
    }
    private void Update()
    {
        if (playerStatData == null)
        {
            playerStatData = player.GetComponent<Stat>();
            string key = playerStatData.key;
            champImage = Resources.Load($"Textures/Player/{key}/{key}") as Texture2D;

            Rect rect = new Rect(0, 0, champImage.width, champImage.height);
            Sprite image = Sprite.Create(champImage, rect, new Vector2(0.5f, 0.5f));

            champImage_ui.sprite = image;

            for(int i = 0; i<5; i++)
            {
                Texture2D skillImage = Resources.Load($"Textures/Player/{key}/{key}_skill_{i+1}") as Texture2D;
                
                rect = new Rect(0, 0, skillImage.width, skillImage.height);
                image = Sprite.Create(skillImage, rect, new Vector2(0.5f, 0.5f));

                skill_image[i].sprite = image;
            }
        }
        levelText.text = $"{playerStatData.level}";
        SetHpBar();
        SetMpBar();
    }

    void SetHpBar()
    {
        float ratio = playerStatData.curHp / playerStatData.maxHp;
        hpBar.value = ratio;
        hpBarText.text = $"{playerStatData.curHp}    /    {playerStatData.maxHp}";
    }

    void SetMpBar()
    {
        float ratio = playerStatData.curMp / playerStatData.maxMp;
        mpBar.value = ratio;
        mpBarText.text = $"{playerStatData.curMp}    /    {playerStatData.maxMp}";
    }
    public void SetCoolDownImage(int index, float coolTime)
    {
        cool_image[index].GetComponent<CoolImageController>().coolTime = coolTime;
        cool_image[index].gameObject.SetActive(true);
    }
}
