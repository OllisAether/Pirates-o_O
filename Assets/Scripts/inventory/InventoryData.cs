using System;

namespace Inventory
{
  [Serializable]
  public class InventoryData
  {
    public string[] items;

    public InventoryData(string[] items)
    {
      this.items = items;
    }
  }
}