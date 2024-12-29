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
            amountText.text = FormatAmount(amount);
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
        amountText.text = FormatAmount(amount);
    }
    
    // 数値をフォーマットするメソッド
    private static string FormatAmount(int amount)
    {
        if (amount >= 1_000_000) // 100万以上
            return (amount / 1_000_000f).ToString("0.0") + "M"; // 例: 1.5M
        if (amount >= 1_000) // 1000以上
            return (amount / 1_000f).ToString("0.0") + "K"; // 例: 1.0K
        return amount.ToString(); // そのまま表示
    }
}
