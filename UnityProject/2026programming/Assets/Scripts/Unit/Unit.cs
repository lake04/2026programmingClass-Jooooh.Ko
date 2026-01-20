using UnityEngine;

public class Unit : MonoBehaviour
{
    protected float curHp;
    public float maxHp;
    public float moveSpeed;
    public float attackCooldown;
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


}
