using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

/// <summary>
/// Item‚Ìî•ñ‚ğŠÇ—‚·‚éƒNƒ‰ƒX
/// </summary>

public class ItemManager : MonoBehaviour
{
    [SerializeField] ItemBase[] _items;

    private static ItemManager _instance = null;
    public static ItemManager Instance => _instance;

    private void Awake()
    {
        _instance = this;
    }

    public ItemBase RequestItem(string itemName)
    {
        return _items.First(i => i.Data.Name == itemName);
    }

    public void Save(ItemBase.ItemData data, GameObject obj)
    {

    }

    public void Delete()
    {

    }
}
