using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public class JinxSkill : BaseSkill
{

    public GameObject wSkillPrefab;
    private GameObject wSkill;
    public Transform wSkillFirePos;

    public GameObject rSkillPrefab;
    private GameObject rSkill;
    public Transform rSkillFirePos;

    public GameObject eSkillPrefab;
    private GameObject eSkill;
    public Transform eSkillFirePos;


    void Start()
    {
        animator = gameObject.GetComponent<Animator>();
        controller = gameObject.GetComponent<BaseController>();
        GetSkill("Jinx");
        stat = gameObject.GetComponent<Stat>();
        statData = Managers.Data.GetStatData("Jinx");
    }

    void Update()
    {
        if(IsSpell_Q)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                IsSpell_Q = false;
            }
        }
        if(IsSpell_W)
        {
            if(animator.GetCurrentAnimatorStateInfo(0).IsName("SPELL_2")
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                IsSpell_W = false;
            }
        }
        if (IsSpell_R)
        {
            if (animator.GetCurrentAnimatorStateInfo(0).IsName("SPELL_4")
                && animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f)
            {
                IsSpell_R = false;
            }
        }
    }

    public override void Active_q()
    {
        if (q_spell_cool) return;
        if (stat.curMp < skillData.qMp) return;
        StartCoroutine(Q_Spell_CoolDown());

        IsSpell_Q = true;
        
        bool launcher = gameObject.GetComponent<JinxController>().isLauncher;
        if (!launcher) // �̴ϰ� -> ��ó
        {
            animator.Play("M_TO_R");

            controller.GetComponent<AudioSource>().PlayOneShot(controller.spellSounds[2]);
            gameObject.GetComponent<JinxController>().isLauncher = true;
        }
        else
        {
            animator.Play("R_TO_M");

            controller.GetComponent<AudioSource>().PlayOneShot(controller.spellSounds[3]);
            gameObject.GetComponent<JinxController>().isLauncher = false;
        }
    }
    IEnumerator qSkill()
    {
        yield return new WaitForSeconds(1.0f);
        IsSpell_Q = false;
    }
    public override void Active_w()
    {
        
        if (w_spell_cool) return;
        if (stat.curMp < skillData.wMp) return;
        stat.curMp -= skillData.wMp;
        StartCoroutine(W_Spell_CoolDown());
        IsSpell_W = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        Physics.Raycast(ray, out hit, 100.0f);
        transform.LookAt(hit.point);
        controller.GetComponent<AudioSource>().PlayOneShot(controller.voices[4]);
        controller.GetComponent<AudioSource>().PlayOneShot(controller.spellSounds[4]);
        animator.Play("SPELL_2");
        //Vector3 mousePosition = Input.mousePosition;
        //Vector3 pos =  Camera.main.ScreenToWorldPoint(mousePosition);
        //transform.LookAt(pos);
    }
    public void W_Bullet_Fire()
    {
        wSkill = Instantiate(wSkillPrefab, wSkillFirePos.position, wSkillFirePos.rotation);
        wSkill.GetComponent<HS_ProjectileMover1>().layer = (int)Define.Layer.PLAYER | (int)Define.Layer.BOT;
        wSkill.GetComponent<HS_ProjectileMover1>().damage = 100;
        wSkill.GetComponent<HS_ProjectileMover1>().Jinx = gameObject;
    }
    public override void Active_e()
    {
        if (e_spell_cool) return;
        if (stat.curMp < skillData.eMp) return;
        StartCoroutine(E_Spell_CoolDown());
        stat.curMp -= skillData.eMp;
        IsSpell_E = true;
        controller.GetComponent<AudioSource>().PlayOneShot(controller.voices[5]);
        animator.Play("SPELL_3");
        eSkill = Instantiate(eSkillPrefab, eSkillFirePos.position,eSkillFirePos.rotation);
        Destroy(eSkill, 7.0f);
    }
    public override void Active_r()
    {
        if (r_spell_cool) return;
        if (stat.curMp < skillData.rMp) return;
        StartCoroutine(R_Spell_CoolDown());
        stat.curMp -= skillData.rMp;
        IsSpell_R = true;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        controller.GetComponent<AudioSource>().PlayOneShot(controller.voices[6]);
        controller.GetComponent<AudioSource>().PlayOneShot(controller.spellSounds[5]);
        Physics.Raycast(ray, out hit, 100.0f);
        transform.LookAt(hit.point);
        animator.Play("SPELL_4");
    }
    public void R_Missile_Fire()
    {
        rSkill = Instantiate(rSkillPrefab, rSkillFirePos.position, rSkillFirePos.rotation);
        rSkill.GetComponent<HS_ProjectileMover1>().layer = (int)Define.Layer.PLAYER | (int)Define.Layer.BOT;
        rSkill.GetComponent<HS_ProjectileMover1>().damage = 300;

        rSkill.GetComponent<HS_ProjectileMover1>().Jinx = gameObject;
        
    }
   
}
