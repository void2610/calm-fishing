using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amountText;
    [SerializeField] private Sprite unknownSprite;
    
    private ItemData _itemData;
    
    public void Init(ItemData itemData, int amount = 0)
    {
        _itemData = itemData;

        if (amount > 0)
        {
            image.sprite = itemData.sprite;
            amountText.text = amount.ToString();
        }
        else
        {
            image.sprite = unknownSprite;
            amountText.text = "";
        }
    }
    
    public void UpdateAmount(int amount)
    {
        if (amount <= 0) return;
        
        image.sprite = _itemData.sprite;
        amountText.text = amount.ToString();
    }
}
