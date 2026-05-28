using UnityEngine;

namespace Inventory
{
  [CreateAssetMenu(fileName = "ItemSet", menuName = "Inventory/ItemSet", order = 2)]
  public class ItemSet : ScriptableObject
  {
    [SerializeField]
    private InventoryItem[] items;
    public InventoryItem[] Items { get { return items; } }
  }
}