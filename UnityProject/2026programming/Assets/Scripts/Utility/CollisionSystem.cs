using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem
{
    private static CollisionSystem _instance;
    public static CollisionSystem Get() => _instance;

    private float playerRadius = 0.5f;
    private float enemyRadius = 0.4f;

    public CollisionSystem()
    {
        _instance = this;
    }

    public void LogicUpdate()
    {
        CheckEnemyWithBullets();
        CheckPlayerWithEnemies();
    }

    private bool IsColliding(Vector2 posA, float radA, Vector2 posB, float radB)
    {
        float combinedRadius = radA + radB;
        return (posA - posB).sqrMagnitude <= (combinedRadius * combinedRadius);
    }

    public void CheckEnemyWithBullets()
    {
        var enemies = Enemy.ActiveEnemies;
        var bullets = Bullet.ActiveBullets; 

        for (int i = bullets.Count - 1; i >= 0; i--)
        {
            var bullet = bullets[i];

            for (int j = enemies.Count - 1; j >= 0; j--)
            {
                if (IsColliding(bullet.Position, bullet.Radius, enemies[j].Position, enemies[j].Radius))
                {
                    if (enemies[j] is IDamageable damageable)
                    {
                        damageable.TakeDamage(bullet.damage);
                    }

                    bullet.OnCollide(enemies[j]);
                    enemies[j].OnCollide(bullet);

                    if (bullet.pierce <= 0)
                    {
                        bullet.Deactivate();
                        break; 
                    }
                }
            }
        }
    }

    private void CheckPlayerWithEnemies()
    {
        if (GameManager.Instance.player == null) return;

        var playerObj = GameManager.Instance.player.GetComponent<ICollidable>();
        if (playerObj == null) return;

        Vector2 playerPos = playerObj.Position;
        var enemies = Enemy.ActiveEnemies;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (IsColliding(playerPos, playerRadius, enemies[i].Position, enemies[i].Radius))
            {
                // 플레이어 피격 처리
                // Player.OnDamage();
            }
        }
    }
}