using System.Collections.Generic;
using UnityEngine;



public enum Weather
{
    Sunny,
    Rainy,
    Snowy
}
public enum TimePeriod
{
    Dawn,
    Noon,
    Dusk,
    Night
}

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary,
}

public static class MyColors
{
    /// <summary>
    /// レアリティに紐づく色を保持する辞書
    /// </summary>
    private static readonly Dictionary<Rarity, Color> rarityToColorMap = new ()
    {
        { Rarity.Common, new Color(0.8f, 0.8f, 0.8f) },       // グレー
        { Rarity.Uncommon, new Color(0.3f, 1f, 0.3f) },      // 緑
        { Rarity.Rare, new Color(0.2f, 0.4f, 1f) },          // 青
        { Rarity.Epic, new Color(0.7f, 0.3f, 0.9f) },        // 紫
        { Rarity.Legendary, new Color(1f, 0.8f, 0f) }        // 金
    };

    /// <summary>
    /// レアリティに対応する色を取得する
    /// </summary>
    /// <param name="rarity">レアリティ</param>
    /// <returns>紐づけられた色</returns>
    public static Color GetRarityColor(Rarity rarity)
    {
        if (rarityToColorMap.TryGetValue(rarity, out var color))
        {
            return color;
        }

        // デフォルトの色を返す（例: 白）
        return Color.white;
    }
}