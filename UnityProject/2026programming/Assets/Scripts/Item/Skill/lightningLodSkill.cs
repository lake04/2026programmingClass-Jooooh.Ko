using UnityEngine;
using System.Collections;

public class lightningLodSkill : SkillBase
{
    public ObjectPool  lightningPool;


    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void Init(SkillCard data)
    {
        base.Init(data);
        StartCoroutine(IE_AttackRoutine());
    }

    public override void OnCollide(ICollidable other)
    {

    }



    private IEnumerator IE_AttackRoutine()
    {
        Debug.Log("Lightning Lod공격 시작");
        while (true)
        {
            Enemy target = GetNearestEnemy();

            if (target != null)
            {
                Attack(target);
                yield return skillData.cooldown;
            }
            else
            {
                yield return new WaitForSeconds(0.2f);
            }
        }
    }

    private Enemy GetNearestEnemy()
    {
        Enemy neatest = null;
        float minDistSq = float.MaxValue;
        var enemies = Enemy.ActiveEnemies;

        for (int i = 0; i < enemies.Count; i++)
        {
            float distSq = (enemies[i].Position - this.Position).sqrMagnitude;
            if (distSq < minDistSq && distSq < skillData.range)
            {
                minDistSq = distSq;
                neatest = enemies[i];
            }
        }
        return neatest;
    }

    public void Attack(Enemy target)
    {
        if (lightningPool == null) return;

        PooledObject pooled = lightningPool.GetPooledObject();

        if (pooled.TryGetComponent(out Bullet bullet))
        {
            Debug.Log("Lightning Lod Skill Attack!");
            pooled.transform.position = target.Position;
            bullet.InitBullet(Vector2.zero,skillData.damage,0,1f);
        }
    }

    public override void LevelUp()
    {
        base.LevelUp();
    }
}
