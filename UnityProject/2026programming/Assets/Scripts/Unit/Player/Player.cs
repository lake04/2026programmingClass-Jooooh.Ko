using UnityEngine;
using System.Collections;

public class Player : Unit, IDamageable
{
    public enum PlayerState { Idle, Move, Attack }
    [Header("State")]
    public PlayerState currentState = PlayerState.Idle;

    private Vector2 moveInput;

    private  void Awake()
    {
        Init();
    }

    private  void Update()
    {
        HandleInput();
    }

    private  void FixedUpdate()
    {
        switch (currentState)
        {
            case PlayerState.Idle:
            case PlayerState.Move:
                MoveLogic();
                break;
            case PlayerState.Attack:
                break;
        }
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

        if (currentState != PlayerState.Attack)
        {
            currentState = (moveInput.sqrMagnitude > 0) ? PlayerState.Move : PlayerState.Idle;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        spriteRenderer.flipX = (mousePos.x < transform.position.x);

        if (Input.GetButtonDown("Fire1") && currentState != PlayerState.Attack)
        {
            StartCoroutine(AttackRoutine());
        }
    }

    private void MoveLogic()
    {
        rb.linearVelocity = moveInput.normalized * moveSpeed;
    }

    private IEnumerator AttackRoutine()
    {
        currentState = PlayerState.Attack;
        Attack();

        yield return new WaitForSeconds(attackCooldown);

        currentState = PlayerState.Idle;
    }

    private void  Attack()
    {

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
}