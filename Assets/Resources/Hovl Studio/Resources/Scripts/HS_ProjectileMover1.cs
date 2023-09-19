using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HS_ProjectileMover1 : MonoBehaviour
{
    public float speed = 15f;
    public bool UseFirePointRotation;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);
    public GameObject hit;
    public GameObject flash;
    private Rigidbody rb;
    public GameObject[] Detached;

    public GameObject Jinx;

    public int layer;
    public float damage;
    public AudioClip clip;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (flash != null)
        {
            //Instantiate flash effect on projectile position
            var flashInstance = Instantiate(flash, transform.position, Quaternion.identity);
            flashInstance.transform.forward = gameObject.transform.forward;

            //Destroy flash effect depending on particle Duration time
            var flashPs = flashInstance.GetComponent<ParticleSystem>();
            if (flashPs != null)
            {
                Destroy(flashInstance, flashPs.main.duration);
            }
            else
            {
                var flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(flashInstance, flashPsParts.main.duration);
            }
        }

        
    }

    void FixedUpdate ()
    {
        
		if (speed != 0)
        {
            rb.velocity = transform.forward * speed;
            //transform.position += transform.forward * (speed * Time.deltaTime);
        }
        Vector3 dir = Jinx.transform.position - transform.position;
        if(dir.magnitude > 30.0f)
        {
            Destroy(gameObject);
        }
	}

    //https ://docs.unity3d.com/ScriptReference/Rigidbody.OnCollisionEnter.html
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != (int)Define.Layer.BOT)
        {
            return;
        }

        other.transform.GetComponent<BaseController>().OnDamaged(damage, Jinx);
        //Lock all axes movement and rotation
        rb.constraints = RigidbodyConstraints.FreezeAll;
        speed = 0;

        //Vector3 contact = other.ClosestPoint(transform.position);
            
        Quaternion rot = Quaternion.FromToRotation(Vector3.up, transform.position);

        //Spawn hit effect on collision
        if (hit != null)
        {
            var hitInstance = Instantiate(hit, transform.position, rot);
            if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
            else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
            else { hitInstance.transform.LookAt(transform.position); }

            //Destroy hit effects depending on particle Duration time
            var hitPs = hitInstance.GetComponent<ParticleSystem>();
            if (hitPs != null)
            {
                Destroy(hitInstance, hitPs.main.duration);
                BaseController controller = Jinx.GetComponent<BaseController>();
                controller._audio.PlayOneShot(clip);
            }
            else
            {
                var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                Destroy(hitInstance, hitPsParts.main.duration);
                BaseController controller = Jinx.GetComponent<BaseController>();
                controller._audio.PlayOneShot(clip);
            }
        }
        foreach (var detachedPrefab in Detached)
        {
            if (detachedPrefab != null)
            {
                detachedPrefab.transform.parent = null;
                Destroy(detachedPrefab, 1);
            }
        }

        Destroy(gameObject);
    }
}
    //void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.layer == (int)Define.Layer.WALL)
    //    {
    //        Destroy(gameObject);
    //    }
    //    if (collision.gameObject.layer!= (int)Define.Layer.BOT)
    //    {
    //        return;
    //    }
    //
    //    collision.transform.GetComponent<BaseController>().OnDamaged(damage, Jinx);
    //    //Lock all axes movement and rotation
    //    rb.constraints = RigidbodyConstraints.FreezeAll;
    //    speed = 0;
    //
    //    ContactPoint contact = collision.contacts[0];
    //    Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
    //    Vector3 pos = contact.point + contact.normal * hitOffset;
    //
    //    //Spawn hit effect on collision
    //    if (hit != null)
    //    {
    //        var hitInstance = Instantiate(hit, pos, rot);
    //        if (UseFirePointRotation) { hitInstance.transform.rotation = gameObject.transform.rotation * Quaternion.Euler(0, 180f, 0); }
    //        else if (rotationOffset != Vector3.zero) { hitInstance.transform.rotation = Quaternion.Euler(rotationOffset); }
    //        else { hitInstance.transform.LookAt(contact.point + contact.normal); }
    //
    //        //Destroy hit effects depending on particle Duration time
    //        var hitPs = hitInstance.GetComponent<ParticleSystem>();
    //        if (hitPs != null)
    //        {
    //            Destroy(hitInstance, hitPs.main.duration);
    //            BaseController controller = Jinx.GetComponent<BaseController>();
    //            controller._audio.PlayOneShot(clip);
    //        }
    //        else
    //        {
    //            var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
    //            Destroy(hitInstance, hitPsParts.main.duration);
    //            BaseController controller = Jinx.GetComponent<BaseController>();
    //            controller._audio.PlayOneShot(clip);
    //        }
    //    }
    //    foreach (var detachedPrefab in Detached)
    //    {
    //        if (detachedPrefab != null)
    //        {
    //            detachedPrefab.transform.parent = null;
    //            Destroy(detachedPrefab, 1);
    //        }
    //    }
    //
    //    Destroy(gameObject);
    //}
