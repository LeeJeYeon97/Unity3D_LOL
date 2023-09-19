using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NexusController : BaseController
{
    public GameObject bigExpPrefab;
    GameObject bigExp;
    


    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
    }
    protected override void Update()
    {
        base.Update();
    }
    protected override void UpdateDie()
    {
        if (!dieSoundPlaying)
        {
            //Managers.Audio.PlayClip("Announcer/��ž�� �ı��߽��ϴ�");
            GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
            Managers.Audio.PlayClip("Turret/�ؼ��� ������ �Ҹ�", _audio);
            dieSoundPlaying = true;
        }
        hpBar.SetActive(false);
        _collider.enabled = false;
        
        if (bigExp == null)
        {
            bigExp = Instantiate(bigExpPrefab, transform.position, Camera.main.transform.rotation);
            float time = bigExp.GetComponent<ParticleSystem>().main.duration;
            Destroy(bigExp, time);
        }
    }
}
