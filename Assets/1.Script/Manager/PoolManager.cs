using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PoolManager { 


    private GameObject poolRoot;

    public Dictionary<string, List<GameObject>> totalMinions = new Dictionary<string, List<GameObject>>();
    private int minionTypeSize = 3;


    public Dictionary<string, List<GameObject>> totalBullets = new Dictionary<string, List<GameObject>>();
    
    private int bulletPoolSize = 50;

    public List<GameObject> goldUIs = new List<GameObject>();
    private int goldUIPoolSize = 10;

    public void CreatePooling()
    {
        poolRoot = new GameObject("Pool_Root");
        MinionPooling();
        BulletPooling();
        GoldUIPooling();
    }

    private void MinionPooling()
    {
        GameObject root = new GameObject("MinionPool_Root");

        for (int i = 1; i <= minionTypeSize; i++)
        {
            List<GameObject> redminions = new List<GameObject>();

            GameObject redMinionPrefab = Resources.Load($"Prefabs/Minion/RedMinion_{i}") as GameObject;
            GameObject obj = new GameObject(redMinionPrefab.name);
            obj.transform.parent = root.transform;

            
            for (int j = 0; j < 50; j++)
            {
                GameObject redMinion = GameObject.Instantiate<GameObject>(redMinionPrefab);

                redMinion.name = redMinionPrefab.name + "_" + j;
                redMinion.SetActive(false);
                redMinion.transform.parent = obj.transform;

                redminions.Add(redMinion);
            }
            totalMinions.Add(obj.name, redminions);


            GameObject blueMinionPrefab = Resources.Load($"Prefabs/Minion/BlueMinion_{i}") as GameObject;
            List<GameObject> blueminions = new List<GameObject>();
            obj = new GameObject(blueMinionPrefab.name);
            obj.transform.parent = root.transform;

            for (int j = 0; j < 50; j++)
            {
                GameObject blueMinion = GameObject.Instantiate<GameObject>(blueMinionPrefab);
                blueMinion.name = blueMinionPrefab.name + "_" + j;

                blueMinion.SetActive(false);

                blueMinion.transform.parent = obj.transform;

                blueminions.Add(blueMinion);
            }

            totalMinions.Add(obj.name, blueminions);
        }
        root.transform.parent = poolRoot.transform;
    }

    private void BulletPooling()
    {
        GameObject root = new GameObject("BulletPool_Root");

        for(int i = 1; i<=5; i++)
        {
            GameObject prefab = Resources.Load<GameObject>($"Prefabs/Bullets/Bullet_{i}");

            List<GameObject> bullets = new List<GameObject>();

            GameObject bulletRoot = new GameObject();
            bulletRoot.name = $"{prefab.name}_Root";

            for (int j = 0; j < bulletPoolSize; j++)
            {
                GameObject go = GameObject.Instantiate(prefab);

                go.SetActive(false);
                go.name = prefab.name + "_" + j;
                go.transform.parent = bulletRoot.transform;
                bullets.Add(go);

            }
            bulletRoot.transform.parent = root.transform;
            totalBullets.Add(prefab.name, bullets);
        }

        root.transform.parent = poolRoot.transform;
    }
    private void GoldUIPooling()
    {
        GameObject root = new GameObject("GoldUIPool_Root");
        //root.AddComponent<RectTransform>();

        GameObject prefab = Resources.Load<GameObject>($"Prefabs/UI/GoldUI");

        for(int i = 0; i< goldUIPoolSize; i++)
        {
            GameObject go = GameObject.Instantiate(prefab);
            go.SetActive(false);
            go.transform.SetParent(root.transform);
            goldUIs.Add(go);
        }
        root.transform.parent = poolRoot.transform;
    }
}

