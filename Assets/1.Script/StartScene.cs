using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScene : BaseScene
{

    void Awake()
    {
        Init();
        Managers.Audio.Create();
    }
    protected override void Init()
    {
        base.Init();
        SceneType = Define.Scene.START;
        
    }
    public void OnClickButton()
    {
        Managers.Audio.PlayClip("clickSound");
        SceneManager.LoadScene("Pick");

    }
    public override void Clear()
    {

    }
}
