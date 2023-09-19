using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
    public GameObject boomEffect;
    public GameObject snareEffect;

    private Animator animator;
    private float dieTime = 5.0f;

    public void OnEnable()
    {
        animator = GetComponent<Animator>();
        animator.Play("IDLE");
    }

    void Update()
    {
        dieTime -= Time.deltaTime;
        if(dieTime <= 0)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE"))
            {
                animator.Play("ATTACK");
                GameObject go = Instantiate(boomEffect, transform);
                Destroy(go, 1.0f);
            }
        }
        // 애니메이션
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
        {
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != (int)Define.Layer.BOT)
        {
            return;
        }

        // 타겟 속박 로직 구현

        other.GetComponent<BaseController>().State = Define.State.SNARE;

        animator.Play("ATTACK");
        GameObject boom = Instantiate(boomEffect, transform);
        Destroy(boom, 1.0f);

        GameObject snare = Instantiate(snareEffect, other.transform);
        Destroy(snare, 1.5f);
    }
}
