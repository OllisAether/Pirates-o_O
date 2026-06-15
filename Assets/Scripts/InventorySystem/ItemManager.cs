using System.Collections.Generic;
using UnityEngine;

namespace InventorySystem
{
  public class ItemManager : SingletonBehaviour<ItemManager>
  {
    [SerializeField]
    private ItemSet[] itemSets;

    public ItemSet[] ItemSets => itemSets;

    [SerializeField]
    private int defaultStackLimit = 10;
    public int DefaultStackLimit => defaultStackLimit;

    private bool isDictionaryConstructed = false;
    private Dictionary<string, Item> itemDictionary = new Dictionary<string, Item>();

    private void Start()
    {
      ConstructItemDictionary();
    }

    private void Update()
    {
    }

    private void ConstructItemDictionary()
    {
      if (isDictionaryConstructed)
      {
        Debug.LogWarning("Item dictionary is already constructed. Skipping reconstruction.");
        return;
      }

      foreach (var set in itemSets)
      {
        foreach (var item in set.Items)
        {
          if (!itemDictionary.ContainsKey(item.Id))
          {
            itemDictionary.Add(item.Id, item);
          }
          else
          {
            Debug.LogWarning("Duplicate itemID found: " + item.Id + ". Skipping.");
          }
        }
      }
      isDictionaryConstructed = true;
    }

    public Item GetItemByID(string itemID)
    {
      if (!isDictionaryConstructed)
      {
        Debug.LogWarning("Item dictionary not yet constructed. Constructing now...");
        ConstructItemDictionary();
      }

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