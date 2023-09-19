using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JinxBulletController : MonoBehaviour
{
    GameObject target = null;
    private float damage;

    public float speed = 10f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    GameObject jinx;

    void Update()
    {
        if(gameObject.activeSelf == false)
        {
            return;
        }
        if (target != null)
        {
            Vector3 dir = target.transform.position - transform.position;
            transform.position += dir.normalized * speed * Time.deltaTime;
            transform.LookAt(target.transform);
            if (dir.magnitude <= 0.1f)
            {
                gameObject.SetActive(false);
                Detach();
                if (jinx.GetComponent<JinxController>().isLauncher)
                {
                    Managers.Audio.PlayClip("Champ/JinxCannonHit");
                    
                }
                
                
            }
        }
    }
    void CannonDetach()
    {
        if (target.GetComponent<Stat>().curHp <= 0)
            return;
        //int mask = (int)Define.Layer.BOT | (int)Define.Layer.RED_MINION;
        //Collider[] colliders = Physics.OverlapSphere(transform.position, 3.0f,mask);
        //
        //Debug.Log(colliders.Length);

        //foreach(Collider collider in colliders)
        //{
        //    collider.GetComponent<BaseController>().OnDamaged(damage, jinx);
        //}
        //target.GetComponent<BaseController>().OnDamaged(damage, jinx);

        Vector3 dir = target.transform.position - transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, dir);
        Vector3 pos = target.transform.position + dir * hitOffset;

        //Spawn hit effect on collision
        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(target.transform.position + dir); }

            //Destroy hit effects depending on particle Duration time
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
    }
    void Detach()
    {
        if (target.GetComponent<Stat>().curHp <= 0) 
            return;


        target.GetComponent<BaseController>().OnDamaged(damage, jinx);
        
        Vector3 dir = target.transform.position - transform.position;
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, dir);
        Vector3 pos = target.transform.position + dir * hitOffset;
    
        //Spawn hit effect on collision
        if (hit != null)
        {
            var hitInstance = Instantiate(hit, pos, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(target.transform.position + dir); }

            //Destroy hit effects depending on particle Duration time
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
            }
        }
    }

    public void Fire(GameObject target, float attack,Transform firePos,GameObject Jinx)
    {
        this.target = target;
        damage = attack;
        gameObject.SetActive(true);
        transform.position = firePos.position;
        jinx = Jinx;
        
        SetParticle();
    }

    void SetParticle()
    {

        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            //Destroy flash effect depending on particle Duration time

            var flashPs = flashInstance.GetComponentsInChildren<ParticleSystem>();
            
            var flashParticle = flashInstance.GetComponent<ParticleSystem>();
            //var main = flashParticle.main;
            if (flashParticle != null)
            {
                Destroy(flashInstance, flashParticle.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }
    }
}
