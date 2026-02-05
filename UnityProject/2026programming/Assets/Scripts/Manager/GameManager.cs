using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private IManager[] managers;
    public Player player;
    private CollisionSystem _collisionSystem;
    [SerializeField] private TMP_Text timeText;

    public override void Awake()
    {
        base.Awake();

        var allScripts = GetComponentsInChildren<MonoBehaviour>(true);
        var mList = new List<IManager>();

        foreach (var script in allScripts)
        {
            if (script is IManager manager)
            {
                mList.Add(manager);
            }
        }
        managers = mList.ToArray();

        foreach (var m in managers)
        {
            m.Init();
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
        UpdateTime();
    }

    private void UpdateLogic()
    {
        if(_collisionSystem != null)
        {
            _collisionSystem.LogicUpdate();
        }
    }

    private void UpdateTime()
    {
        timeText.text = Time.timeSinceLevelLoad.ToString("F2");
    }
  
}
