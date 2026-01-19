using NUnit.Framework;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private ManagerBase[] managers;


    private void Awake()
    {
        foreach( ManagerBase manager in managers )
        {
            manager.Awake();
            manager.Init();
        }
    }
}
