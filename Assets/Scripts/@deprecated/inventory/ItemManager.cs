using System.Collections.Generic;
using UnityEngine;

namespace Inventory
{
  public class ItemManager : SingletonBehaviour<ItemManager>
  {
    [SerializeField]
    private ItemSet[] itemSets;
    public ItemSet[] ItemSets => itemSets;

    private Dictionary<string, InventoryItem> itemDictionary = new Dictionary<string, InventoryItem>();

    new void Awake()
    {
      base.Awake();

      foreach (var set in itemSets)
      {
        foreach (var item in set.Items)
        {
          if (!itemDictionary.ContainsKey(item.ItemID))
          {
            itemDictionary.Add(item.ItemID, item);
          }
          else
          {
            Debug.LogWarning("Duplicate itemID found: " + item.ItemID + ". Skipping.");
          }
        }
      }
    }

    public InventoryItem GetItemByID(string itemID)
    {
      if (itemDictionary.ContainsKey(itemID))
      {
        return itemDictionary[itemID];
      }
      else
      {
        Debug.LogError("Item not found for itemID: " + itemID);
        return null;
      }
    }
  }
}