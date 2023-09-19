using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{ 
    void Awake()  //여기서 실행안되어도 부모에서 실행가능
    {
        Init();
        Managers.Audio.PlayBGM("BGM");
        Managers.Audio.PlayClip("Announcer/소환사의 협곡에 오신것을 환영합니다");
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.SetText("소환사의 협곡에 오신것을 환영합니다.");

    }
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.GAME;
        //Managers.Data.LoadData();
        //Managers.Pool.CreatePooling();
    }
    public override void Clear()
    {

    }
}
