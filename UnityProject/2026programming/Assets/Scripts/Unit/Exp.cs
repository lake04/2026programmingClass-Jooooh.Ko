using System.Collections.Generic;
using UnityEngine;

public class Exp : MonoBehaviour, ICollidable
{
    public static List<Exp> ActiveGems = new List<Exp>(300);

    [Header("¼³Á¤")]
    [SerializeField] private float radius = 0.2f;
    [SerializeField] private float magnetRange = 3.0f;
    [SerializeField] private float moveSpeed = 5.0f;
    public float expAmount = 1f;

    private Transform _playerTransform;
    private PooledObject _pooledObject;
    private Rigidbody2D rb;
    private bool _isFlying = false;

    public Vector2 Position => rb.position;
    public float Radius => radius;

    private void Awake()
    {
        _pooledObject = GetComponent<PooledObject>();
        rb = GetComponent<Rigidbody2D>();

        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;

    }

    private void OnEnable()
    {
        ActiveGems.Add(this);
        _isFlying = false;
    }

    private void Start()
    {
        if (GameManager.Instance != null && GameManager.Instance.player != null)
        {
            _playerTransform = GameManager.Instance.player.transform;

        }
    }

    void Update()
    {
        if (_playerTransform == null)
        {
            return;
        }

        if (!_isFlying && Time.frameCount % 5 != 0)
        {
            return;
        }

        float distSq = ((Vector2)_playerTransform.position - rb.position).sqrMagnitude;

        if (!_isFlying && distSq < magnetRange * magnetRange)
        {
            _isFlying = true;
        }

        if (_isFlying)
        {
            MoveToPlayer();
        }
    }

    public void InitPos(Vector2 targetPos)
    {
        transform.position = targetPos;
        rb.position = targetPos;

        rb.linearVelocity = Vector2.zero;
        _isFlying = false;
    }

    private void MoveToPlayer()
    {
        Vector2 newPos = Vector2.MoveTowards(rb.position, _playerTransform.position, moveSpeed * Time.deltaTime);
        rb.MovePosition(newPos);

        moveSpeed += 0.1f;
    }

    public void OnCollide(ICollidable other)
    {
        if (other is Player)
        {
            Collect();
        }
    }

    private void Collect()
    {
        LevelMamanger.Instance.AddExp(expAmount);
        Deactivate();
    }

    public void Deactivate()
    {
        moveSpeed = 5.0f; 
        _pooledObject.Release();
    }

    private void OnDisable()
    {
        ActiveGems.Remove(this);
    }
}