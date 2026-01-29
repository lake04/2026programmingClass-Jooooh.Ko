using Unity.VisualScripting;
using UnityEngine;

public class AreaSkill : SkillBase
{
    private float attackTimer;
    private SpriteRenderer auraRenderer;

    public bool IsReadyToAttack => attackTimer >= skillData.cooldown;

    public override void Init(SkillCard data)
    {
        base.Init(data);
        currentRange = skillData.range;

        auraRenderer = GetComponentInChildren<SpriteRenderer>();

        if (auraRenderer == null)
        {
            auraRenderer.AddComponent<SpriteRenderer>();
        }

        UpdateVisuals();
    }

    private void Update()
    {
        attackTimer += Time.deltaTime;
    }

    public void ResetAttackTimer() => attackTimer = 0f;

    public override void LevelUp()
    {
        base.LevelUp(); 
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (auraRenderer != null)
        {
            transform.localScale = Vector3.one * (currentRange * 2f);
        }
    }

    public override void OnCollide(ICollidable other)
    {
      
    }
}