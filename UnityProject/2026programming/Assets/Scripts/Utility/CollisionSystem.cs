using System.Collections.Generic;
using UnityEngine;

public class CollisionSystem
{
    private static CollisionSystem _instance;
    public static CollisionSystem Get() => _instance;

    private float playerRadius = 0.5f;

    public CollisionSystem()
    {
        _instance = this;
    }

    public void LogicUpdate()
    {
        CheckEnemyWithBullets(); 
        CheckPlayerWithEnemies();
        CheckPlayerWithExp();
        CheckEnemyWithAura();
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
                        damageable.TakeDamage(bullet.damage,bullet.power);
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
        var playerDamageable = GameManager.Instance.player.GetComponent<IDamageable>();

        if (playerObj == null || playerDamageable == null) return;

        Vector2 playerPos = playerObj.Position;
        var enemies = Enemy.ActiveEnemies;

        for (int i = enemies.Count - 1; i >= 0; i--)
        {
            if (IsColliding(playerPos, playerRadius, enemies[i].Position, enemies[i].Radius))
            {
                playerDamageable.TakeDamage(enemies[i].damage);

                playerObj.OnCollide(enemies[i]);
                enemies[i].OnCollide(playerObj);
            }
        }
    }

    public void CheckPlayerWithExp()
    {
        var player = GameManager.Instance.player;
        var gems = Exp.ActiveGems;
        if (player == null || gems.Count == 0) return;

        for (int i = gems.Count - 1; i >= 0; i--)
        {
            float combinedRadius = player.Radius + gems[i].Radius;
            if ((player.Position - gems[i].Position).sqrMagnitude <= combinedRadius * combinedRadius)
            {
                gems[i].OnCollide(player);
            }
        }
    }

    public void CheckEnemyWithAura()
    {
        var auras = SkillManager.Instance.activeAuras;
        var enemies = Enemy.ActiveEnemies;

        for (int i = 0; i < auras.Count; i++)
        {
            var aura = auras[i];

            if (!aura.IsReadyToAttack) continue;

            for (int j = enemies.Count - 1; j >= 0; j--)
            {
                if (IsColliding(aura.Position, aura.Radius, enemies[j].Position, enemies[j].Radius))
                {
                    if (enemies[j] is IDamageable damageable)
                    {
                        damageable.TakeDamage(aura.skillData.damage, 0);
                    }

                    aura.OnCollide(enemies[j]);
                    enemies[j].OnCollide(aura);
                }
            }
            aura.ResetAttackTimer();
        }
    }
}