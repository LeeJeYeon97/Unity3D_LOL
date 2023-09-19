using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PickScene : BaseScene
{
    public Button champ_1, champ_2, champ_3;
    public Button startButton;
    public Image pickImage;
    public Text error;

    private GameObject champPick;
    void Awake()
    {
        Init();
        champPick = GameObject.Find("ChampPick");
        if (champPick == null)
        {
            champPick = new GameObject("ChampPick");
            champPick.AddComponent<ChampPick>();
        }
        pickImage.gameObject.SetActive(false);
    }

    private void Start()
    {
        startButton.onClick.AddListener(OnClickStartButton);
        champ_1.onClick.AddListener(() => OnClickChampButton(Define.Champ.Garen));
        champ_2.onClick.AddListener(() => OnClickChampButton(Define.Champ.Jinx));
        champ_3.onClick.AddListener(() => OnClickChampButton(Define.Champ.Alistar));
    }
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.PICK;
    }
    public void OnClickStartButton()
    {   
        if(champPick.GetComponent<ChampPick>().player == null)
        {
            error.gameObject.SetActive(true);
            return;
        }
        else
        {
            Managers.Audio.PlayClip("clickSound");
            SceneManager.LoadScene("Game");
        }
        
    }
    public void OnClickChampButton(Define.Champ champType)
    {
        pickImage.gameObject.SetActive(true);
        error.gameObject.SetActive(false);
        switch (champType)
        {
            case Define.Champ.Garen:
                champPick.GetComponent<ChampPick>().player =
                    Resources.Load<GameObject>("Prefabs/Champion/Garen/Garen");
                pickImage.transform.position = champ_1.transform.position;
                break;
            case Define.Champ.Jinx:
                champPick.GetComponent<ChampPick>().player =
                    Resources.Load<GameObject>("Prefabs/Champion/Jinx/Jinx");
                pickImage.transform.position = champ_2.transform.position;
                break;
            case Define.Champ.Alistar:
                champPick.GetComponent<ChampPick>().player =
                    Resources.Load<GameObject>("Prefabs/Champion/Alistar/Alistar");
                pickImage.transform.position = champ_3.transform.position;
                break;
        }
    }

    public override void Clear()
    {
        
    }
}
