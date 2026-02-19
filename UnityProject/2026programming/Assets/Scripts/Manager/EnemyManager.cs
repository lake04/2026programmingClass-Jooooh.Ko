using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

[BurstCompile]
public class EnemyManager : Singleton<EnemyManager>, IManager
{
    private TransformAccessArray _transformArray;

    private NativeArray<Vector2> _flowDirections;
    private NativeArray<float> _speeds;

    private const int MaxCapacity = 8000;

    public void Init()
    {
        _transformArray = new TransformAccessArray(MaxCapacity);
        _flowDirections = new NativeArray<Vector2>(MaxCapacity, Allocator.Persistent);
        _speeds = new NativeArray<float>(MaxCapacity, Allocator.Persistent);
    }

    void FixedUpdate()
    {
        int enemyCount = _transformArray.length;
        if (enemyCount == 0) return;

        for (int i = 0; i < enemyCount; i++)
        {
            Enemy enemy = Enemy.ActiveEnemies[i];
            Vector2 dir = FieldManager.Instance.GetDirection(enemy.Position);
            _flowDirections[i] = dir;
            _speeds[i] = enemy.moveSpeed;
        }

        var job = new ControlEnemyJob
        {
            FlowDirections = _flowDirections,
            Speeds = _speeds,
            DeltaTime = Time.fixedDeltaTime
            
        };

        JobHandle handle = job.Schedule(_transformArray);

        handle.Complete();
    }

    public void RegisterEnemy(Transform enemyTransform)
    {
        _transformArray.Add(enemyTransform);
    }

    public void UnregisterEnemy(Transform enemyTransform)
    {
        for (int i = 0; i < _transformArray.length; i++)
        {
            if (_transformArray[i] == enemyTransform)
            {
                _transformArray.RemoveAtSwapBack(i);

                int lastIdx = Enemy.ActiveEnemies.Count - 1;
                if (i < lastIdx)
                {
                    Enemy.ActiveEnemies[i] = Enemy.ActiveEnemies[lastIdx];
                }
                Enemy.ActiveEnemies.RemoveAt(lastIdx);

                break;
            }
        }
    }

    private void OnDestroy()
    {
        if (_transformArray.isCreated)
        {
            _transformArray.Dispose();
        }

        if (_flowDirections.IsCreated)
        {
            _flowDirections.Dispose();
        }

        if (_speeds.IsCreated)
        {
            _speeds.Dispose();
        }
    }

}
