using UnityEngine;

public class Enemy : Unit,IDamageable
{
    private PooledObject _pooledObject;

    private void Awake()
    {
        _pooledObject = GetComponent<PooledObject>();
         rb = GetComponent<Rigidbody2D>();
         spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public override void Init()
    {
        curHp = maxHp;
       

    }

    public void TakeDamage(float amount)
    {
        curHp -= amount;

        if (curHp <= 0) Die();
    }

    public void Die()
    {
        _pooledObject.Release();
    }

    private void OnEnable()
    {
        rb.linearVelocity = Vector3.zero;
    }


}
