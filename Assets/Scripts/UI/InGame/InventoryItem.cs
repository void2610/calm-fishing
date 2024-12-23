using ScriptableObject;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI amountText;
    
    private ItemData _itemData;
    
    public void Init(ItemData itemData)
    {
        _itemData = itemData;
        image.sprite = itemData.sprite;
        amountText.text = "0";
    }
    
    public void UpdateAmount(int amount)
    {
        amountText.text = amount.ToString();
    }
}
