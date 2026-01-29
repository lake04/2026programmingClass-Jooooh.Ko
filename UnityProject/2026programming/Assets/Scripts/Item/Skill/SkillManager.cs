using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>, IManager
{
    public List<SkillBase> activeSkills = new List<SkillBase>();


    public List<AreaSkill> activeAuras = new List<AreaSkill>();



    public void Init()
    {
        activeSkills.Clear();
        activeAuras.Clear();
    }

    public void AddOrUpgradeSkill(SkillCard cardData)
    {
        SkillBase existingSkill = activeSkills.Find(s => s.skillData.cardName == cardData.cardName);

        if (existingSkill != null)
        {
            existingSkill.LevelUp();
        }
        else
        {
            CreateNewSkill(cardData);
        }
    }

    private void CreateNewSkill(SkillCard cardData)
    {
        GameObject skillObj = Instantiate(cardData.gameObject);

        skillObj.transform.SetParent(GameManager.Instance.player.transform);
        skillObj.transform.localPosition = Vector3.zero;
        skillObj.SetActive(true);

        SkillBase newSkill = null;

        switch (cardData.skillType)
        {
            case SkillType.Aura:
                newSkill = skillObj.AddComponent<AreaSkill>();
                activeAuras.Add(newSkill as AreaSkill);
                break;
            case SkillType.Lightning:
                newSkill = skillObj.GetComponent<lightningLodSkill>();
                activeSkills.Add(newSkill as lightningLodSkill);
                break;
            
        }

        if (newSkill != null)
        {
            newSkill.Init(cardData);
            activeSkills.Add(newSkill);
        }
    }

}