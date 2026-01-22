using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum cardType { stat,Healing,Item};

public abstract class CardBase : ScriptableObject
{
    public string cardName;
    public Sprite icon;
    public cardType cardType;
    [TextArea] public string description;
}

