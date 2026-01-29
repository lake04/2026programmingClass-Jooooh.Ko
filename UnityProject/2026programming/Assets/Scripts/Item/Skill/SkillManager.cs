using System.Collections.Generic;
using UnityEngine;

public class SkillManager : Singleton<SkillManager>, IManager
{
    public ObjectPool activeSkillPool;
    public ObjectPool activeAurasPool;

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
        GameObject skillObj = activeSkillPool.GetPooledObject().gameObject;

        skillObj.transform.SetParent(GameManager.Instance.player.transform);
        skillObj.transform.localPosition = Vector3.zero;
        skillObj.SetActive(true); 

        SkillBase newSkill = skillObj.GetComponent<SkillBase>();

        if (newSkill != null)
        {
            newSkill.Init(cardData);
            activeSkills.Add(newSkill);

            if (newSkill is AreaSkill areaSkill)
            {
                activeAuras.Add(areaSkill);
            }
        }
    }

}