using System.Collections.Generic;
using Persistence;
using UnityEngine;

namespace Inventory
{
  [CreateAssetMenu(fileName = "InventoryItem", menuName = "Inventory/InventoryItem", order = 1)]
  public class InventoryItem : ScriptableObject
  {
    [SerializeField]
    private string itemID;
    public string ItemID { get { return itemID; } }
    
    [SerializeField]
    private string displayName;
    public string DisplayName { get { return displayName; } }
    
    [SerializeField]
    private GameObject itemPrefab;
    public GameObject ItemPrefab { get { return itemPrefab; } }
    
    [SerializeField]
    [Tooltip("Prefab to use when the item is being held by the player. This object can have a use Behaviour attached to it for special interactions.")]
    private GameObject holdingPrefab;
    public GameObject HoldingPrefab { get { return holdingPrefab; } }
  }
}
