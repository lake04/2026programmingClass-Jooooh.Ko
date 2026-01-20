using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPool enemyPool; 
    public float spawnInterval = 2f;

    private IEnumerator spawnEnemy;
    private WaitForSeconds wait;

    public Transform[] spawnPoints;
    public int enemyCount;

    private void Awake()
    {
        spawnPoints = GetComponentsInChildren<Transform>();
    }

    private void Start()
    {
        spawnEnemy = SpawnRoutine();
        wait = new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            SpawnEnemy();
            yield return wait;
        }
    }

    private void SpawnEnemy()
    {
        PooledObject pooled = enemyPool.GetPooledObject();
        pooled.transform.position = spawnPoints[Random.Range(1, spawnPoints.Length)].position;
        enemyCount++;
        pooled.GetComponent<Enemy>().Init();
    }
}
