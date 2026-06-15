using UnityEngine;

namespace InventorySystem
{
  [CreateAssetMenu(fileName = "ItemSet", menuName = "Inventory System/Item Set", order = 2)]
  public class ItemSet : ScriptableObject
  {
    [SerializeField]
    private Item[] items;
    public Item[] Items => items;
  }
}