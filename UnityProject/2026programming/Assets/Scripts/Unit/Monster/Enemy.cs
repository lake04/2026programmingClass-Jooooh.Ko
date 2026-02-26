using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : Unit, IDamageable, ICollidable
{
    private PooledObject _pooledObject;

    public static List<Enemy> ActiveEnemies = new List<Enemy>(500);

    [Header("이동 설정")]
    [SerializeField] private float separationDistance = 0.5f;
    [SerializeField] private float separationForce = 1.2f;
    [SerializeField] private LayerMask enemyLayer;

    private static Collider2D[] _neighborResults = new Collider2D[10];

    public Vector2 Position => transform.position;
    public float Radius => 0.3f;

    private static ContactFilter2D _enemyFilter;
    private static bool _filterInitialized = false;

    private bool isKnockback = false;
    private Vector2 knockback = Vector2.zero;
    [SerializeField] private float friction = 5f;

    private FlashSprite flashSprite;


    public void OnCollide(ICollidable other)
    {
        
    }

    private void Awake()
    {
        _pooledObject = GetComponent<PooledObject>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        flashSprite = GetComponent<FlashSprite>();
    }

    private void OnEnable()
    {
        Init();
    }

    public  void Init(EnemyData enemyData)
    {
        maxHp = enemyData.curHp;
        curHp = maxHp;
        moveSpeed = enemyData.moveSpeed;
        damage = enemyData.damage;
        attackCooldown = enemyData.attackCooldown;
    }

    void Update()
    {
        if (FieldManager.Instance != null)
        {
            FieldManager.Instance.RegisterEnemy(this, Position);
        }
    }

    void FixedUpdate() 
    {
        if (knockback.sqrMagnitude > 0.01f)
        {
            knockback = Vector2.Lerp(knockback, Vector2.zero, Time.fixedDeltaTime * friction);
        }
        else
        {
            knockback = Vector2.zero;
            isKnockback = false;
        }
    }

    private Vector2 CalculateSeparation(Vector2 myPos)
    {
        Vector2 push = Vector2.zero;
        Vector2Int gPos = FieldManager.Instance.WorldToGrid(myPos);

        for (int x = -1; x <= 1; x++)
        {
            for(int y = -1; y<= 1; y++)
            {
                int nx = gPos.x + x;
                int ny = gPos.y + y;

                if(FieldManager.Instance.IsInsideGrid(nx, ny))
                {
                    var neighbors = FieldManager.Instance.enemyGrid[nx, ny];
                    for (int i = 0; i < neighbors.Count; i++)
                    {
                        if (neighbors[i] == this) continue;

                        Vector2 diff = myPos - (Vector2)neighbors[i].rb.position;
                        float sqrDist = diff.sqrMagnitude;

                        if (sqrDist < separationDistance * separationDistance && sqrDist > 0.001f)
                        {
                            push += diff.normalized;
                        }
                    }
                }
               
            }
        }
        return push;
    }

    public  void TakeDamage(float amount,float power = 0f)
    {
        curHp -= amount;
        flashSprite.Flash();


        Vector2 dir = (Position - (Vector2)FieldManager.Instance.player.position).normalized;

        if (power > 0f)
        {
            knockback = dir * power;
            isKnockback = true;
        }

        if (curHp <= 0) Die();
    }

    public void Die()
    {
        PooledObject exp = LevelMamanger.Instance.expPool.GetPooledObject();
        exp.transform.position = transform.position;

        if (exp.TryGetComponent(out Exp expScript))
        {
            expScript.InitPos(transform.position);
        }

        EnemyManager.Instance.UnregisterEnemy(this.transform);

        _pooledObject.Release();
    }

    public Vector2 GetCurrentVelocity(Vector2 flowDir)
    {
        return (flowDir * moveSpeed) + knockback;
    }


    private void OnDisable()
    {
        ActiveEnemies.Remove(this);
        spriteRenderer.color = Color.red;
        flashSprite.StopFlash();
        isKnockback = false;
    }
}