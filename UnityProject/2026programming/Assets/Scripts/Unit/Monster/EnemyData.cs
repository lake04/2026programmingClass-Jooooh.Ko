using UnityEngine;

[CreateAssetMenu(fileName = "EnemyData", menuName = "ScriptableObjects/UnitData")]
public class EnemyData : ScriptableObject
{
    public float curHp;
    public float moveSpeed;
    public float attackCooldown;
    public float damage;
}
