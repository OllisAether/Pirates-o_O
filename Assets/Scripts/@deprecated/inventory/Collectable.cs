using Inventory;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
  [SerializeField]
  private InventoryItem itemData;
  public InventoryItem ItemData => itemData;

  [SerializeField]
  private UnityEvent<InventoryItem> onItemDataChanged = new UnityEvent<InventoryItem>();
  public UnityEvent<InventoryItem> OnItemDataChanged => onItemDataChanged; 

  public void SetItemData(InventoryItem newData)
  {
    itemData = newData;
    onItemDataChanged.Invoke(itemData);
  }
}
