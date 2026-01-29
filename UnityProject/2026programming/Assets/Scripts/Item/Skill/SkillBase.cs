using UnityEngine;

public abstract class SkillBase : MonoBehaviour, ICollidable
{
    public SkillCard skillData;
    protected float timer;
    public int currentLevel = 1;
    public Vector2 Position => transform.position;
    public float Radius => currentRange;
    protected float currentRange;


    public virtual void Init(SkillCard data)
    {
        skillData = data;
        currentRange = skillData.range;
    }

    public virtual void OnCollide(ICollidable other)
    {
       
    }

    public virtual void LevelUp()
    {
        currentLevel++;
    }
}
