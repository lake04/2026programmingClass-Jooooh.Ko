using System.Collections;
using UnityEditor.Overlays;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public ObjectPool enemyPool;
    public float spawnInterval = 2f;
    public Transform[] spawnPoints;
    public int enemyCount;

    [Header("Wave Settings")]
    [SerializeField] private WaveData[] waveDatas; 
    [SerializeField] private float[] waveTime; 
    [SerializeField] private int curWave = 0;

    public float timer = 0;
    private WaitForSeconds wait;

    private void Start()
    {
        wait = new WaitForSeconds(spawnInterval);
        StartCoroutine(SpawnRoutine());
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (curWave + 1 < waveDatas.Length && curWave < waveTime.Length)
        {
            if (timer >= waveTime[curWave])
            {
                curWave++;
            }
        }
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
        if (spawnPoints.Length == 0 || waveDatas.Length == 0)
        {
            return;
        }

        int waveIdx = Mathf.Min(curWave, waveDatas.Length - 1);
        WaveData currentWaveData = waveDatas[waveIdx];

        if (currentWaveData.enemyDatas.Length == 0)
        {
            return;
        }

        int spawnIdx = Random.Range(0, spawnPoints.Length);

        PooledObject pooled = enemyPool.GetPooledObject();
        pooled.transform.position = spawnPoints[spawnIdx].position;
        enemyCount++;

        int enemyIdx = Random.Range(0, currentWaveData.enemyDatas.Length);
        EnemyData data = currentWaveData.enemyDatas[enemyIdx];

        if (pooled.TryGetComponent<Enemy>(out Enemy enemy))
        {
            enemy.Init(data);
        }
    }
}