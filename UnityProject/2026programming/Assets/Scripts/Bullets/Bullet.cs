using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ICollidable
{
    public static List<Bullet> ActiveBullets = new List<Bullet>(200);

    public float damage = 10f;
    public int pierce = 0;
    public float speed = 10f;
    [SerializeField] private float radius = 0.2f;

    private HashSet<int> _hitTargets = new HashSet<int>(10);
    private PooledObject _pooledObject;

    public Vector2 Position => transform.position;
    public float Radius => radius;

    private void Awake()
    {
        _pooledObject = GetComponent<PooledObject>();
    }

    private void OnEnable()
    {
        ActiveBullets.Add(this);
        _hitTargets.Clear();
    }

    private void Update()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        if (transform.position.sqrMagnitude > 400f) 
        {
            Deactivate();
        }
    }

    public void InitBullet(Vector2 dir, float playerDamage, int playerPierce = 0)
    {
        damage = playerDamage;
        pierce = playerPierce;
    }

    public void OnCollide(ICollidable other)
    {
        int id = other.GetHashCode();
        //if (_hitTargets.Contains(id)) return;
        //_hitTargets.Add(id);

        if (pierce <= 0)
        {
            Deactivate();
        }
        else
        {
            pierce--;
        }
    }

    public void Deactivate()
    {
        if (_pooledObject != null)
            _pooledObject.Release();
        else
            gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        ActiveBullets.Remove(this);
    }
}