using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JinxController : BaseController
{
    public GameObject minigunFirePos;
    public GameObject launcherFirePos;

    public bool isLauncher = false;
    string bulletKey;

    protected SkillData skillData;
    public AudioClip fireSound;
    public AudioClip cannonSound;
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        bulletKey = "Bullet_4";
        statData = Managers.Data.GetStatData("Jinx");
        skillData = Managers.Data.GetSkillData("Jinx");    
        stat.SetStat(statData);

        if (type.team == Define.Team.BLUE)
            layer = 1 << (int)Define.Layer.RED_TURRET | 1 << (int)Define.Layer.BOT | 1 << (int)Define.Layer.GROUND
                | 1 << (int)Define.Layer.RED_MINION;
        else if (type.team == Define.Team.RED)
            layer = 1 << (int)Define.Layer.BLUE_TURRET | 1 << (int)Define.Layer.BOT | 1 << (int)Define.Layer.GROUND
                | 1 << (int)Define.Layer.BLUE_MINION;

        base.Start();
        transform.position = spawnPos.position;
        GameObject go = Instantiate(spawnPrefab, transform);
        go.transform.position += Vector3.up;
        Destroy(go, 2.0f);

    }

    protected override void Update()
    {
        
        OnMouseClicked();
        OnKeyboard();

        base.Update();
    }
    protected override void OnKeyboard()
    {
        if (state == Define.State.DIE)
            return;
        if (skill.IsSpell_W || skill.IsSpell_R)
            return;

        base.OnKeyboard();
    }
    protected override void OnMouseClicked()
    {
        if (state == Define.State.DIE)
            return;
        if (skill.IsSpell_W || skill.IsSpell_R) 
            return;
        base.OnMouseClicked();
    }
    protected override void UpdateIdle()
    {
        if (state == Define.State.DIE)
            return;
        if (skill.IsSpell_W || skill.IsSpell_R || skill.IsSpell_Q)
            return;
        if (!isLauncher)
            animator.Play("IDLE");
        else
            animator.Play("IDLE_2");
    }
    protected override void UpdateMoving()
    {
        if(state == Define.State.DIE)
            return;
       
        if (skill.IsSpell_W || skill.IsSpell_R)
            return;
        Vector3 dir = destPos - transform.position;

        if (!isLauncher)
            animator.Play("RUN");
        else
            animator.Play("RUN_2");

        if (target != null)
        {
            if(dir.magnitude < stat.attackRange)
            {
                state = Define.State.ATTACK;
                return;
            }
        }
        // ¸ñÀûÁö µµÂø
        if (dir.magnitude <= 0.1f)
        {
            state = Define.State.IDLE;
        }
        agent.Move(dir.normalized * stat.moveSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 30 * Time.deltaTime);
    }
    protected override void UpdateAttack()
    {
        if (state == Define.State.DIE)
            return;
        if (skill.IsSpell_W || skill.IsSpell_W)
            return;

        if (target == null || target.GetComponent<Stat>().curHp <= 0)
        {
            state = Define.State.IDLE;
            return;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("RUN") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("IDLE_2") ||
            animator.GetCurrentAnimatorStateInfo(0).IsName("RUN_2"))
        {
            if (!isLauncher)
                animator.Play("ATTACK");
            else
                animator.Play("ATTACK_2");
        }
        transform.LookAt(target.transform);
    }

    protected override void UpdateDie()
    {
        if (!isLauncher)
            animator.Play("DIE");
        else
            animator.Play("DIE_2");

        if (!dieSoundPlaying)
        {
            Managers.Audio.PlayClip("Champ/JinxDie", _audio);
            dieSoundPlaying = true;
        }
        
        base.UpdateDie();
    }
    void Fire()
    {
        if (state == Define.State.DIE)
            return;
        if (!isLauncher)
        {
            bulletKey = "Bullet_4";
            _audio.PlayOneShot(fireSound);
            Managers.Bullet.JinxFire(bulletKey, Target, stat.attack, minigunFirePos.transform, gameObject);
        }
        else
        {
            stat.curMp -= skillData.qMp;
            bulletKey = "Bullet_5";
            _audio.PlayOneShot(cannonSound);
            Managers.Bullet.JinxFire(bulletKey, Target, stat.attack, launcherFirePos.transform, gameObject);
        }
    }
}
