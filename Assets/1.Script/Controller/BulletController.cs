using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    GameObject target = null;
    private float damage;

    public float speed = 10f;
    public float hitOffset = 0f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;

    Color color;
    Color flashColor;
    Color hitColor;
    private AudioClip clip;
    GameObject shooter;

    private void Start()
    {
    }
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
            if (dir.magnitude <= 0.1f)
            {
                Detach();
                gameObject.SetActive(false);

                if(clip != null)
                {
                    
                    Managers.Audio.PlayClip(clip,shooter.GetComponent<AudioSource>());
                }

            }
        }
    }
    
    void Detach()
    {
        if (target.GetComponent<Stat>().curHp <= 0) 
            return;


        target.GetComponent<BaseController>().OnDamaged(damage, null);
    
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

            var hitColorPs = hitInstance.GetComponentsInChildren<ParticleSystem>();
            foreach(var ps in hitColorPs)
            {
                var main = ps.main;
                main.startColor = hitColor;
            }

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

    public void Fire(string key,GameObject target, float attack,Transform firePos,GameObject go = null)
    {
        this.target = target;
        this.damage = attack;
        gameObject.SetActive(true);
        transform.position = firePos.position;

        if (go != null)
            shooter = go;

        if (key == "Bullet_1" || key == "Bullet_2" ||key == "Bullet_3")
        { 
            switch (target.GetComponent<IsType>().team)
            {
                case Define.Team.BLUE:
                    color = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    flashColor = new Color(1.0f, 0.4f, 0.4f, 1.0f);
                    hitColor = new Color(1.0f, 0.0f, 0.0f, 1.0f);
                    break;
                case Define.Team.RED:
                    color = new Color(0.0f, 0.0f, 1.0f, 1.0f);
                    flashColor = new Color(0.4f, 0.4f, 1.0f, 1.0f);
                    hitColor = new Color(0.0f, 0.0f, 0.4f, 1.0f);
                    break;
            }
            if(key == "Bullet_3")
            {
                clip = Resources.Load("Sounds/Turret/포탑 맞는소리") as AudioClip;
            }
        }
        SetParticle();
    }

    void SetParticle()
    {
        var psSystems = gameObject.GetComponentsInChildren<ParticleSystem>();
        foreach (var ps in psSystems)
        {
            if (ps.gameObject.name == "Black" || ps.gameObject.name =="Darkness") continue;
            var main = ps.main;
            main.startColor = color;
        }

        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            //Destroy flash effect depending on particle Duration time

            var flashPs = flashInstance.GetComponentsInChildren<ParticleSystem>();
            
            foreach(var p in flashPs )
            {
                var psmain = p.main;
                psmain.startColor = flashColor;
            }


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
