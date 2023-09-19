using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{ 
    void Awake()  //���⼭ ����ȵǾ �θ𿡼� ���డ��
    {
        Init();
        Managers.Audio.PlayBGM("BGM");
        Managers.Audio.PlayClip("Announcer/��ȯ���� ��� ���Ű��� ȯ���մϴ�");
        GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
        gm.SetText("��ȯ���� ��� ���Ű��� ȯ���մϴ�.");

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
