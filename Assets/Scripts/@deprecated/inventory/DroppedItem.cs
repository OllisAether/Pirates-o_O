using UnityEngine;

namespace Inventory
{
  [RequireComponent(typeof(Collectable))]
  public class DroppedItem : MonoBehaviour
  {
    [SerializeField]
    private Transform meshContainer;

    private Collectable collectable;

    void Start()
    {
      collectable = GetComponent<Collectable>();

      collectable.OnItemDataChanged.AddListener(UpdateModel);

      UpdateModel(collectable.ItemData);
    }

    void UpdateModel(InventoryItem item)
    {
      foreach (Transform child in meshContainer)
      {
        Destroy(child.gameObject);
      }

      if (item != null && item.ItemPrefab != null)
      {
        Instantiate(item.ItemPrefab, meshContainer);
      }
    }
  }
}