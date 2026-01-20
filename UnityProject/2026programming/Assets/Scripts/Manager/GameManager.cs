using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private ManagerBase[] managers;
    public Player player;
    private CollisionSystem _collisionSystem;

    public float curExp = 0;
    private float maxExp = 10f;
    public int curLeve = 1;

    public ObjectPool expPool;

    public override void Awake()
    {
        base.Awake();
        foreach ( ManagerBase manager in managers )
        {
            manager.Init();
        }
    }

    private void Start()
    {
        _collisionSystem = CollisionSystem.Get();

        if (_collisionSystem == null)
        {
            _collisionSystem = new CollisionSystem();
        }
    }

    private void Update()
    {
        UpdateLogic();
    }

    private void UpdateLogic()
    {
        if(_collisionSystem != null)
        {
            _collisionSystem.LogicUpdate();
        }
    }

    public void AddExp(float amount)
    {
        curExp += amount;
        if (curExp >= maxExp)
        {
            curExp -= maxExp;
            maxExp *= 1.2f;
            curLeve++;
        }
    }
}
