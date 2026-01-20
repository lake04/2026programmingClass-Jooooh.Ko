using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Enemy : Unit,IDamageable, ICollidable
{
    private PooledObject _pooledObject;
    public static List<Enemy> ActiveEnemies = new List<Enemy>(500);

    [Header("이동 설정")]
    [SerializeField] private float separationDistance = 0.5f;
    [SerializeField] private float separationForce = 1.2f;

    [SerializeField] private LayerMask enemyLayer;

    public Vector2 Position => transform.position;
    public float Radius => 0.4f;

    private void OnEnable()
    {
        ActiveEnemies.Add(this);
        Init();
    }

    private void Awake()
    {
        _pooledObject = GetComponent<PooledObject>();
         rb = GetComponent<Rigidbody2D>();
         spriteRenderer = GetComponent<SpriteRenderer>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;
    }
    public override void Init()
    {
        curHp = maxHp;

    }

    void Update()
    {
        Move();
    }


    protected override void Move()
    {
        if (FieldManager.Instance == null) return;

        Vector2 flowDir = FieldManager.Instance.GetDirection(transform.position);

        Vector2 separationDir = CalculateSeparation();

        Vector2 finalDir = (flowDir + separationDir * separationForce).normalized;

        if(finalDir != Vector2.zero)
        {
            rb.MovePosition(rb.position + finalDir * moveSpeed * Time.deltaTime);

            spriteRenderer.flipX = finalDir.x < 0;
        }
    }

    private Vector2 CalculateSeparation()
    {
        if (Time.frameCount % 2 == 0)
        {
            return Vector2.zero;
        }

        Vector2 separation = Vector2.zero;

        Collider2D[] neighbors = Physics2D.OverlapCircleAll(transform.position, separationDistance, enemyLayer);

        foreach (var neighbor in neighbors)
        {
            if (neighbor.gameObject == this.gameObject) continue;
            Vector2 diff = (Vector2)transform.position - (Vector2)neighbor.transform.position;
            float sqrDist = diff.sqrMagnitude;

            if(sqrDist < separationDistance * separationDistance && sqrDist > 0.001f)
            {
                separation += diff.normalized / Mathf.Sqrt(sqrDist);
            }
        }
        return separation.normalized;

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

    private void OnDisable()
    {
        ActiveEnemies.Remove(this);
        rb.linearVelocity = Vector3.zero;
    }

    public void OnCollide(ICollidable other)
    {

    }
}
