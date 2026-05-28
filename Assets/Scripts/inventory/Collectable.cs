using Inventory;
using UnityEngine;
using UnityEngine.Events;

public class Collectable : MonoBehaviour
{
  [SerializeField]
  private InventoryItem itemData;
  public InventoryItem ItemData { get { return itemData; } }

  [SerializeField]
  private UnityEvent<InventoryItem> onItemDataChanged = new UnityEvent<InventoryItem>();
  public UnityEvent<InventoryItem> OnItemDataChanged { get { return onItemDataChanged; } } 

  public void SetItemData(InventoryItem newData)
  {
    itemData = newData;
    onItemDataChanged.Invoke(itemData);
  }
}
