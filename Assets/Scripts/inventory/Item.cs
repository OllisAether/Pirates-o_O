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
    private IUsable useBehavior;
    public IUsable UseBehavior { get { return useBehavior; } }
  }
}
