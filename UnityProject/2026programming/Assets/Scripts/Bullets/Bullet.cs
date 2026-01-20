using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour, ICollidable
{
    private PooledObject _pooledObject;

    public static List<Bullet> ActiveBullets = new List<Bullet>(500);

    public Vector2 Position => transform.position;

    public float Radius => 0.4f;

    public float damage = 1f;

    public int pierce;

    private void OnEnable()
    {
        ActiveBullets.Add(this);
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void OnDisable()
    {
        //rb.linearVelocity = Vector3.zero;
        ActiveBullets.Remove(this);
    }

    public void OnCollide(ICollidable other)
    {
        throw new System.NotImplementedException();
    }

    public void Deactivate()
    {
       
    }
}
