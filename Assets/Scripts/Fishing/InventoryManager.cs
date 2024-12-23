using UnityEngine;
using ScriptableObject;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    [SerializeField] private ItemDataList allItemDataList;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        
        allItemDataList.Register();
    }

    public void AddRandomItem()
    {
        var itemData = allItemDataList.GetRandomItemData();
        Debug.Log($"アイテムを追加しました: {itemData.displayName}");
    }
}
