using System.Collections.Generic;
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

    [Header("카드 풀")]
    [SerializeField] private List<CardBase> cardPool = new List<CardBase>();

    public  void Init()
    {
        
    }
   

    void Start()
    {
        CardUIManager.Instance.SetCardPool(cardPool);
    }

    public void AddExp(float amount)
    {
        curExp += amount;
        levelSlider.value = curExp / maxExp;

        if (curExp >= maxExp)
        {
            LevelUp();
        }
    }

    private void LevelUp()
    {
        curExp -= maxExp;
        maxExp *= 1.2f;
        curLeve++;
        levelText.text = "Lv." + curLeve;
        levelSlider.value = curExp / maxExp;
        CardUIManager.Instance.ShowCards();
    }
}
