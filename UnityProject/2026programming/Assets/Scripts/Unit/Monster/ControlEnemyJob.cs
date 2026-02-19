using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Jobs;

[BurstCompile]
public struct ControlEnemyJob : IJobParallelForTransform
{
    [ReadOnly] public NativeArray<Vector2> FlowDirections;
    [ReadOnly] public NativeArray<float> Speeds;
    public float DeltaTime;

    public void Execute(int index, TransformAccess transform)
    {
        float3 currentPos = transform.position;

        float2 moveDir = FlowDirections[index];
        float moveSpeed = Speeds[index];

        if (math.lengthsq(moveDir) > 0.001f)
        {
            float2 velocity = math.normalize(moveDir) * moveSpeed * DeltaTime;

            currentPos.xy += velocity;

            transform.position = currentPos;
        }
    }

}
