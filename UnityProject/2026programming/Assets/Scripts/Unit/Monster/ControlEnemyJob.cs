using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

[BurstCompile]
public struct ControlEnemyJob : IJobParallelForTransform
{
    [ReadOnly] public NativeArray<Vector2> FinalVelocities;
    public float DeltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        float3 currentPos = transform.position;

        float2 velocity = FinalVelocities[index];

        currentPos.xy += velocity * DeltaTime;

       transform.position = currentPos;
    }

}
