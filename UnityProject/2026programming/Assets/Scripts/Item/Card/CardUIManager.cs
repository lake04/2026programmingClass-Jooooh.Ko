using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardUIManager : Singleton<CardUIManager>,IManager
{
    public GameObject panel;
    public CardSlotUI[] slots;

    private List<CardBase> allCardPool = new List<CardBase>();
    public List<CardBase> currentCards = new List<CardBase>();

    private bool isClick = false;


    public void SetCardPool(List<CardBase> cards)
    {
        allCardPool = cards;
    }

    public void ShowCards()
    {
        panel.SetActive(true);
        isClick = false;
        currentCards.Clear();
        List<int> usedIndices = new List<int>();
        for (int i = 0; i < slots.Length; i++)
        {
            int index;
            do
            {
                index = Random.Range(0, allCardPool.Count);
            } while (usedIndices.Contains(index));

            usedIndices.Add(index);
            currentCards.Add(allCardPool[index]);
        }
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].Setup(currentCards[i]);
            slots[i].gameObject.SetActive(false);
            slots[i].button.interactable = false;
        }

        ShowCardSequence();
        isClick = true;
    }

    private void ShowCardSequence()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].gameObject.SetActive(true);
            slots[i].button.interactable = true;
        }
        Time.timeScale = 0f;

    }

    public void ChooseCard(CardBase chosen)
    {
        if (!isClick) return;
        isClick = false;

        Debug.Log($"{chosen.cardName} 선택됨");
        panel.SetActive(false);
        Time.timeScale = 1f;

        if (chosen is SkillCard skillCard)
        {
            SkillManager.Instance.AddOrUpgradeSkill(skillCard);
        }
        else if (chosen is StatCard statCard)
        {
            switch (statCard.cardType)
            {
                case cardType.stat:
                    GameManager.instance.player.AddStats(statCard.playerHP, statCard.playerDamage, statCard.playerSpeed);
                    break;
                case cardType.Healing:
                    GameManager.instance.player.Healingv(statCard.playerHP);
                    break;
            }
        }
        else
        {
            Debug.LogWarning("알 수 없는 카드 타입입니다.");
        }
    }

    public void Init()
    {
        panel.SetActive(false);
    }
}
