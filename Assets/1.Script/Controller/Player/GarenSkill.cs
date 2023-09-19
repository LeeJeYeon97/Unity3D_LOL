using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class GarenSkill : BaseSkill
{
    float qSkillSpeedUpTime = 2.0f;

    public GameObject qSkill;

    private GameObject shieldPrefab;
    private GameObject shield;

    private float eSkillRange = 3.0f;

    bool rSkillHit = false;
    public GameObject rSkillPrefab;
    public GameObject rSkillEffectPrefab;
    GameObject rSkill;
    GameObject rSkillEffect;

    [SerializeField]
    Collider[] cols;
    private void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        GetSkill("Garen");
        stat = gameObject.GetComponent<Stat>();
        statData = Managers.Data.GetStatData("Garen");

        shieldPrefab = Resources.Load<GameObject>("Prefabs/Champion/Garen/Shield");
        shield = Instantiate(shieldPrefab);
        shield.SetActive(false);
        controller = GetComponent<BaseController>();
    }
    private void Update()
    {
        
        if(qSkill.activeSelf)
            qSkill.transform.Rotate(Vector3.forward, Time.deltaTime * 200, Space.Self);
        if(shield.activeSelf)
        {
            shield.transform.position = new Vector3(transform.position.x, 1, transform.position.z);
            shield.transform.Rotate(Vector3.forward, Time.deltaTime * 50);
        }

        if(rSkill != null)
        {
            if (rSkill.transform.position.y > 2.0f)
            {
                rSkill.transform.position += Vector3.down * 50 * Time.deltaTime;
            }
            else
            {
                if(!rSkillHit)
                {
                    target.GetComponent<BaseController>().OnDamaged(stat.attack, this.gameObject);
                    
                    rSkillHit = true;
                }
                if (rSkillEffect == null)
                {
                    Vector3 pos = new Vector3(rSkill.transform.position.x, 0, rSkill.transform.position.z);
                    rSkillEffect = Instantiate(rSkillEffectPrefab, pos, Quaternion.identity);
                    Destroy(rSkillEffect, 1.0f);
                }
                Destroy(rSkill, 1.0f);
            }
        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK_SPELL_4")
            && animator.GetCurrentAnimatorStateInfo(0).normalizedTime>= 1.0f)
        {
            gameObject.GetComponent<BaseController>().State = Define.State.IDLE;
        }
        
    }
    public override void Active_q()
    {
        if (q_spell_cool) return;
        StartCoroutine(qSkillAttackUp());
        controller._audio.PlayOneShot(controller.spellSounds[0]);
        controller._audio.PlayOneShot(controller.voices[4]);
        StartCoroutine(qSkillSpeedUp());
    }
    IEnumerator qSkillAttackUp()
    {
        IsSpell_Q = true;
        stat.attack = statData.attack + (statData.attack + 30);
        qSkill.SetActive(true);

        yield return new WaitForSeconds(skillData.qSkillDuration);

        qSkill.SetActive(false);
        stat.attack = statData.attack;
        IsSpell_Q = false;
        StartCoroutine(Q_Spell_CoolDown());
    }
    IEnumerator qSkillSpeedUp()
    {
        stat.moveSpeed = statData.moveSpeed + (statData.moveSpeed * 0.35f);
        yield return new WaitForSeconds(qSkillSpeedUpTime);
        stat.moveSpeed = statData.moveSpeed;
    }
    public override void Active_w()
    {
        if (w_spell_cool) return;
        StartCoroutine(wSkill());
        controller._audio.PlayOneShot(controller.spellSounds[2]);
        controller._audio.PlayOneShot(controller.voices[6]);

    }
    IEnumerator wSkill()
    {
        IsSpell_W = true;
        shield.SetActive(true);

        // TODO 방어력 올리기

        yield return new WaitForSeconds(skillData.wSkillDuration);
        // 방어력 되돌리기
        shield.SetActive(false);
        IsSpell_W = false;
        StartCoroutine(W_Spell_CoolDown());
    }
    public override void Active_e()
    {
        if (e_spell_cool) return;
        StartCoroutine(eSkill());
        controller._audio.PlayOneShot(controller.spellSounds[3]);
        controller._audio.PlayOneShot(controller.voices[7]);
    }
    IEnumerator eSkill()
    {
        IsSpell_E = true;
        IEnumerator co = eSkillAttack();
        StartCoroutine(co);

        yield return new WaitForSeconds(skillData.eSkillDuration);

        IsSpell_E = false;
        gameObject.GetComponent<BaseController>().State = Define.State.IDLE;
        StopCoroutine(co);
        StartCoroutine(E_Spell_CoolDown());
    }
    IEnumerator eSkillAttack()
    {
        IsType type = gameObject.GetComponent<IsType>();
        int layer = gameObject.GetComponent<BaseController>().MyLayer;
        while (true)
        {
            cols = Physics.OverlapSphere(transform.position, eSkillRange,layer);


            foreach (Collider col in cols)
            {
                IsType colType = col.gameObject.GetComponent<IsType>();
                if (colType == null) continue;

                switch (type.team)
                {
                    case Define.Team.BLUE:
                        if (colType.team == Define.Team.RED 
                            && col.gameObject.layer != (int)Define.Layer.RED_TURRET)
                        {
                            col.gameObject.GetComponent<BaseController>().OnDamaged(stat.attack, this.gameObject);
                        }
                        break;
                    case Define.Team.RED:
                        if (colType.team == Define.Team.BLUE
                            && col.gameObject.layer != (int)Define.Layer.BLUE_TURRET)
                        {
                            col.gameObject.GetComponent<BaseController>().OnDamaged(stat.attack, this.gameObject);
                        }
                        break;
                }
            }
            yield return new WaitForSeconds(0.5f);
        }
    }
    public override void Active_r()
    {
        if (r_spell_cool) return;
        IsSpell_R = true;
        rSkillHit = false;
        target = gameObject.GetComponent<BaseController>().Target;
        
        transform.LookAt(target.transform);
        animator.Play("ATTACK_SPELL_4");
        Vector3 pos = target.transform.position + (Vector3.up * 10);
        Quaternion rot = rSkillPrefab.transform.rotation;
        rSkill = Instantiate(rSkillPrefab, pos, rot);

        controller._audio.PlayOneShot(controller.spellSounds[4]);
        controller._audio.PlayOneShot(controller.voices[8]);
        StartCoroutine(R_Spell_CoolDown());
    }
}

