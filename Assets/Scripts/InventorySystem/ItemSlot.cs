using System;
using UnityEngine;

namespace InventorySystem
{
  [Serializable]
  public class ItemSlot
  {
    public Item Item { get; private set; }
    public int Quantity { get; private set; }

    public ItemSlot(Item item, int quantity = 1)
    {
      Item = item;
      SetQuantity(quantity);
    }

    public void SetQuantity(int quantity)
    {
      var maxStack = Item.IsStackable ? (
        Item.UseCustomStackLimit ? Item.CustomStackLimit : ItemManager.Instance.DefaultStackLimit
      ) : 1;
      Quantity = Mathf.Clamp(quantity, 0, maxStack);
    }
  }
}