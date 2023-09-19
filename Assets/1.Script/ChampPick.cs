using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChampPick : MonoBehaviour
{
    public GameObject player;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }
}
