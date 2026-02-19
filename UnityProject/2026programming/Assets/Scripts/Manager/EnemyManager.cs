using UnityEngine;

public class EnemyManager : Singleton<EnemyManager>, IManager
{
    public void Init()
    {
        
    }

    void FixedUpdate()
    {
        var allEnemies = Enemy.ActiveEnemies;

        for (int i = 0; i < allEnemies.Count; i++)
        {
            var enemy = allEnemies[i];

            Vector2 flowDir = FieldManager.Instance.GetDirection(enemy.Position);

            //Vector2 separation = FieldManager.Instance.CalculateSeparation(enemy);

            Vector2 velocity = flowDir.normalized * enemy.moveSpeed;
            enemy.Rb.MovePosition(enemy.Position + velocity * Time.fixedDeltaTime);
        }
    }

   
}
