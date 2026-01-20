using UnityEngine;

public class Unit : MonoBehaviour
{
    protected float curHp;
    public float maxHp;
    public float moveSpeed;
    public float attackCooldown;
    public float damage;
    protected Rigidbody2D rb;
    protected SpriteRenderer spriteRenderer;



    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public virtual void Init()
    {
        curHp = maxHp;
    }

    protected virtual void Move()
    {

    }


}
