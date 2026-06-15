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
  [SerializeField]
  private string itemLayerName = "Items";
  private const string EmptySlotName = "EmptySlot";

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
      itemContainer.GetChild(i).gameObject.SetActive(i == index);
    }
  }

  private void UpdateItemDisplay(InventoryItem[] items)
  {
    for (int i = itemContainer.childCount - 1; i >= 0; i--)
    {
      Destroy(itemContainer.GetChild(i).gameObject);
    }

    for (int i = 0; i < items.Length; i++)
    {
      var item = items[i];
      if (item != null && item.ItemPrefab != null)
      {
        var itemObject = Instantiate(item.ItemPrefab, itemContainer);
        SetLayerRecursively(itemObject, LayerMask.NameToLayer(itemLayerName));
        itemObject.SetActive(i == inventoryUi.ActiveItemIndex);
      }
      else
      {
        var emptyObject = new GameObject(EmptySlotName);
        emptyObject.transform.SetParent(itemContainer);
        emptyObject.transform.localPosition = Vector3.zero;
        emptyObject.transform.localRotation = Quaternion.identity;
        emptyObject.transform.localScale = Vector3.one;
        emptyObject.layer = LayerMask.NameToLayer(itemLayerName);
      }
    }

    UpdateActiveItemDisplay(inventoryUi.ActiveItemIndex);
  }

  private void SetLayerRecursively(GameObject obj, int layer)
  {
    obj.layer = layer;
    foreach (Transform child in obj.transform)
    {
      SetLayerRecursively(child.gameObject, layer);
    }
  }
}
