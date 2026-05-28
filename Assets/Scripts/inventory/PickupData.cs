using Unity.Properties;
using UnityEngine;

namespace Inventory
{
  [CreateAssetMenu(fileName = "New Pickup Data", menuName = "Inventory/Pickup Data")]
  public class PickupData : ScriptableObject
  {

    [CreateProperty]
    public InventoryItem targetItem;
    [CreateProperty]
    public bool hasTargetItem;
  }
}