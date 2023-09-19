using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager
{
    public void Fire(string key,GameObject target,float attack,Transform firePos,GameObject go = null)
    {
        if (Managers.Pool.totalBullets[key].Count <= 0)
            return;
        
        foreach(GameObject bullet in Managers.Pool.totalBullets[key])
        {
            if(!bullet.activeSelf)
            {
                bullet.GetComponent<BulletController>().Fire(key, target, attack, firePos,go);
                return;
            }
        }   
    }
    public void JinxFire(string key, GameObject target, float attack, Transform firePos,GameObject Jinx)
    {
        if (Managers.Pool.totalBullets[key].Count <= 0)
            return;

        foreach (GameObject bullet in Managers.Pool.totalBullets[key])
        {
            if (!bullet.activeSelf)
            {

                bullet.GetComponent<JinxBulletController>().Fire(target, attack, firePos,Jinx);
                return;
            }
        }
    }
}
