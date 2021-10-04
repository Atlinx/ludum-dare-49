using System.Collections;
using System.Collections.Generic;
using Gameplay.Buildings;
using UnityEngine;

public class ShopManager : MonoBehaviour
{

    public List<BuildingData> shopItems = new List<BuildingData>();

    public List<ShopItem> ShopSlots = new List<ShopItem>();

    public void Init()
    {
        foreach (var shopItem in ShopSlots)
        {
            shopItem.Init();
        }
    }

}
