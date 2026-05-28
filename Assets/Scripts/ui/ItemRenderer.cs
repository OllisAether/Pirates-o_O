using Inventory;
using UnityEngine;

public class ItemRenderer : MonoBehaviour
{
  [SerializeField]
  private Transform itemContainer;

  [SerializeField]
  private InventoryBehaviour inventory;
  [SerializeField]
  private InventoryUi inventoryUi;

  void Start()
  {
    inventory.OnInventoryChanged.AddListener(UpdateItemDisplay);
    inventoryUi.OnActiveItemChanged.AddListener(UpdateActiveItemDisplay);

    UpdateItemDisplay(inventory.InventoryItems);
  }

  private void UpdateActiveItemDisplay(int index)
  {
    for (int i = 0; i < itemContainer.childCount; i++)
    {
      var child = itemContainer.GetChild(i);
      child.gameObject.SetActive(i == index);
    }
  }

  private void UpdateItemDisplay(InventoryItem[] items)
  {
    // Instantiate new items based on inventory
    for (int i = 0; i < items.Length; i++)
    {
      var item = items[i];

      if (item != null && item.ItemPrefab != null)
      {
        if (i < itemContainer.childCount)
        {
          var existingChild = itemContainer.GetChild(i);
          if (existingChild != null && existingChild.name == i + item.ItemID)
          {
            continue;
          }
          else
          {
            Destroy(existingChild.gameObject);
          }
        }

        var itemGameObject = Instantiate(item.ItemPrefab, itemContainer);
        foreach (Transform child in itemGameObject.GetComponentsInChildren<Transform>(true))
        {
          child.gameObject.layer = gameObject.layer;
        }

        itemGameObject.name = i + item.ItemID;
        itemGameObject.SetActive(true);
      }
    }

    UpdateActiveItemDisplay(inventoryUi.ActiveItemIndex);
  }
}
