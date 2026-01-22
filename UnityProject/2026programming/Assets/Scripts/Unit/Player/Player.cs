using UnityEngine;
using System.Collections;

public class Player : Unit, IDamageable , ICollidable
{
    public enum PlayerState { Idle, Move, Attack }
    [Header("State")]
    public PlayerState currentState = PlayerState.Idle;

    private Vector2 moveInput;

    public Vector2 Position => rb.position;

    public float Radius => 0.4f;

    [Header("공격 설정")]
    public ObjectPool bulletPool;
    private IEnumerator ieAttackRoutine;
    private WaitForSeconds attackWait;
    [SerializeField] private float serarchRange = 15f;
    private WaitForSeconds searchWait = new WaitForSeconds(0.2f);

    private  void Awake()
    {
        Init();

        ieAttackRoutine = IE_AttackRoutine();
        attackWait = new WaitForSeconds(attackCooldown);
    }
    private void Start()
    {
        StartCoroutine(ieAttackRoutine);
    }

    private  void Update()
    {
        HandleInput();
    }

    private  void FixedUpdate()
    {
       Move();
     
    }

    public override void Init()
    {
        base.Init();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void HandleInput()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spriteRenderer.flipX = (mousePos.x < transform.position.x);
    }

    protected override void Move()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    private IEnumerator IE_AttackRoutine()
    {
        while (true)
        {
            Enemy target = GetNearestEnemy();

            if (target != null)
            {
                currentState = PlayerState.Attack;
                FireBullet(target);
                yield return attackWait;
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

        for(int i =0; i< enemies.Count; i++)
        {
            float distSq = (enemies[i].Position - this.Position).sqrMagnitude;
            if (distSq < minDistSq && distSq < serarchRange)
            {
                minDistSq = distSq;
                neatest = enemies[i];
            }
        }
        return neatest;
    }

    private void FireBullet(Enemy target)
    {
        if (bulletPool == null) return;

        PooledObject pooled = bulletPool.GetPooledObject();
        pooled.transform.position = rb.position;

        if(pooled.TryGetComponent(out Bullet bullet))
        {
           Vector2 fireDir = (target.Position - this.Position).normalized;

            float angle = Mathf.Atan2(fireDir.y, fireDir.x) * Mathf.Rad2Deg;
            pooled.transform.rotation = Quaternion.Euler(0, 0, angle - 90);

            bullet.InitBullet(fireDir, damage);
        }
    }

    public void TakeDamage(float amount)
    {
        curHp -= amount;

        if (curHp <= 0) Die();
    }

    protected virtual void Die()
    {
        
        Debug.Log("플레이어 사망");
    }

    public void AddHP(int amount)
    {
        maxHp += amount;
        curHp += amount;
    }
    public void AddStats(int hp, int dmg, float speed)
    {
        maxHp += hp;
        damage += dmg;
        moveSpeed += speed;
    }

    public void Healingv(int _hp)
    {
        if (curHp < maxHp)
        {
            curHp += _hp;
            if (curHp > maxHp)
            {
                curHp = maxHp;
            }
        }
        else
        {
            curHp = maxHp;
        }
    }

    public void OnCollide(ICollidable other)
    {
        
    }
}