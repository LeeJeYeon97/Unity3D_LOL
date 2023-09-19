using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Transform red_ChampSpawnPos;
    public Transform blue_ChampSpawnPos;
    private GameObject blue_nexus;
    private GameObject red_nexus;
    public GameObject bigExpPrefab;
    GameObject bigExp;
    public GameObject hud;
    public GameObject victoryUI;
    public GameObject defeatUI;

    public Text textUI;

    private void Awake()
    {
        Managers.Data.LoadData();
        Managers.Pool.CreatePooling();
        ChampPick champ = GameObject.FindObjectOfType<ChampPick>();
        Instantiate(champ.GetComponent<ChampPick>().player);
    }

    private void Start()
    {
        blue_nexus = GameObject.FindGameObjectWithTag("BLUE_NEXUS");
        red_nexus = GameObject.FindGameObjectWithTag("RED_NEXUS");
    }

    private void Update()
    {
        if(blue_nexus.GetComponent<Stat>().curHp <=0) // 패배
        {
            hud.SetActive(false);
            defeatUI.SetActive(true);
            if (bigExp == null)
            {
                bigExp = Instantiate(bigExpPrefab, blue_nexus.transform.position, Camera.main.transform.rotation);
                float time = bigExp.GetComponent<ParticleSystem>().main.duration;
                Managers.Audio.PlayClip("Turret/포탑터지는소리");
                //Managers.Audio.PlayClip("Turret/넥서스 터지는 소리");
                Time.timeScale = 0;
            }
        }
        else if(red_nexus.GetComponent<Stat>().curHp <=0) //승리
        {
            hud.SetActive(false);

            victoryUI.SetActive(true);
            if (bigExp == null)
            {
                bigExp = Instantiate(bigExpPrefab, red_nexus.transform.position, Camera.main.transform.rotation);
                float time = bigExp.GetComponent<ParticleSystem>().main.duration;
                //Managers.Audio.PlayClip("Turret/넥서스 터지는 소리");
                Managers.Audio.PlayClip("Turret/포탑터지는소리");
                Time.timeScale = 0;
            }
           
        }

    }

    public void SetText(string text)
    {
        textUI.text = text;
        textUI.gameObject.SetActive(true);

        StartCoroutine("TextHide");
    }
    IEnumerator TextHide()
    {
        yield return new WaitForSeconds(3.0f);
        textUI.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        Time.timeScale = 1;
        Managers.Audio.PlayClip("clickSound");
        SceneManager.LoadScene("Start");
    }
}
