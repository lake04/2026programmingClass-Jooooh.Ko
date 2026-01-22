using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelMamanger : Singleton<LevelMamanger>, IManager
{
    [Header("레벨")]
    public float curExp = 0;
    private float maxExp = 10f;
    public int curLeve = 1;

    public Slider levelSlider;
    [SerializeField] private TMP_Text levelText;

    public ObjectPool expPool;

    public  void Init()
    {
        Debug.Log("level 매니저1");
    }
   

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void AddExp(float amount)
    {
        curExp += amount;
        levelSlider.value = curExp / maxExp;

        if (curExp >= maxExp)
        {
            curExp -= maxExp;
            maxExp *= 1.2f;
            curLeve++;
            levelText.text = "Lv." + curLeve;
            levelSlider.value = curExp / maxExp;
        }
    }
}
