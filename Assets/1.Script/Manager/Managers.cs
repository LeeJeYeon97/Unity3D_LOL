using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Managers : MonoBehaviour
{
    static Managers _instance;
    static Managers Instance 
    {
        get { Init();  return _instance; }
    }
    DataManager _data = new DataManager();
    public static DataManager Data { get { return Instance._data; } }

    BulletManager _bullet = new BulletManager();
    public static BulletManager Bullet { get { return Instance._bullet; } }

    PoolManager _pool = new PoolManager();
    public static PoolManager Pool { get { return Instance._pool; } }

    AudioManager _audio = new AudioManager();
    public static AudioManager Audio { get { return Instance._audio; } }

    static void Init()
    {
        if (_instance == null )
        {
            GameObject obj = GameObject.Find("@Managers");
            if(obj == null ) 
            {
                obj = new GameObject("@Managers");
            }
            DontDestroyOnLoad(obj);
            _instance = obj.AddComponent<Managers>();
        }
    }
}
