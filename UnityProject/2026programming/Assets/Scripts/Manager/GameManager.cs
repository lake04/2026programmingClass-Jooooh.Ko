using NUnit.Framework;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private ManagerBase[] managers;
    public GameObject player;

    public override void Awake()
    {
        base.Awake();
        foreach ( ManagerBase manager in managers )
        {
            manager.Init();
        }
    }
}
