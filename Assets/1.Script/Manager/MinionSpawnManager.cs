using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class MinionSpawnManager : MonoBehaviour
{
    [SerializeField]
    float minionSpawnTime = 50.0f;
    [SerializeField]
    float minionSpawnDelay = 1.0f;

    bool isSpawnPlaying = false;
    bool isSound = false;
    public Transform red_spawnPos;
    public Transform blue_spawnPos;
    

    private void Update()
    {
        minionSpawnTime -= Time.deltaTime;
        if (minionSpawnTime <= 0)
        {
            if(!isSound)
            {
                Managers.Audio.PlayClip("Announcer/미니언들이 생성되었습니다");
                GameManager gm = GameObject.Find("GameManager").GetComponent<GameManager>();
                gm.SetText("미니언들이 생성되었습니다.");
                isSound = true;
            }
            if(!isSpawnPlaying)
            {
                StartCoroutine(Spawn());
            }
            
            minionSpawnTime = 30.0f;
        }
    }
    IEnumerator Spawn()
    {
        
        isSpawnPlaying = true;
        for (int i = 1; i <= 3; i++)
        {
            string red_key = $"RedMinion_{i}";
            string blue_key = $"BlueMinion_{i}";

            if(i == 3)
            {
                foreach (GameObject minion in Managers.Pool.totalMinions[red_key])
                {
                    if (!minion.activeSelf)
                    {
                        minion.SetActive(true);
                        break;
                    }
                }
                foreach (GameObject minion in Managers.Pool.totalMinions[blue_key])
                {
                    if (!minion.activeSelf)
                    {
                        minion.SetActive(true);
                        break;
                    }
                }
                isSpawnPlaying = false;
                yield break;
            }

            for (int j = 0; j < 3; j++)
            {
                foreach (GameObject minion in Managers.Pool.totalMinions[red_key])
                {
                    if (!minion.activeSelf)
                    {
                        minion.SetActive(true);
                        break;
                    }
                }
                foreach (GameObject minion in Managers.Pool.totalMinions[blue_key])
                {
                    if (!minion.activeSelf)
                    {
                        minion.SetActive(true);
                        break;
                    }
                }

                

                yield return new WaitForSeconds(minionSpawnDelay);
            }
        }
        
    }
}
