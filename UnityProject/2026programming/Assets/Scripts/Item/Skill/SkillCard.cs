using System.Collections.Generic;
using UnityEngine;

public enum SkillType { Projectile, Aura, Orbit , Lightning}

[CreateAssetMenu(fileName = "NewSkillCard", menuName = "Cards/Skill")]
public class SkillCard : CardBase
{
    public SkillType skillType;
    public GameObject gameObject;
    public float damage;
    public float cooldown;
    public float speed;
    public int projectileCount;
    public float range; 

    public List<SkillLevelData> levelUpModifiers;
}

[System.Serializable]
public class SkillLevelData
{
    public string upgradeDescription;
    public float damageMod;
    public float cooldownMod;
}