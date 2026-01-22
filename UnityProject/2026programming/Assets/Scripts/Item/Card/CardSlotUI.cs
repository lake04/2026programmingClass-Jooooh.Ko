using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardSlotUI : MonoBehaviour
{
    public Image icon;
    public Text nameText;
    public Text descriptionText;
    public Button button;

    private CardBase card;
    public cardType cardType;

    public void Setup(CardBase newCard)
    {
        card = newCard;
        cardType = card.cardType;
        icon.sprite = card.icon;
        nameText.text = card.cardName;
        descriptionText.text = card.description;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => OnClick());
    }

    public void OnClick()
    {
        CardUIManager.Instance.ChooseCard(card);
    }
}
