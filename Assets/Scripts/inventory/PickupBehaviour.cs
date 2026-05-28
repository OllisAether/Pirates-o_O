using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Inventory
{
  [RequireComponent(typeof(Collider))]
  public class PickupBehaviour : MonoBehaviour
  {
    private Collider pickupCollider;

    [SerializeField]
    private InventoryBehaviour inventory;

    [SerializeField]
    private LayerMask collectableLayer;

    [SerializeField]
    private InputActionReference pickupAction;
    [SerializeField]
    private InputActionReference dropItemAction;

    [SerializeField]
    private Transform dropItemPosition;
    [SerializeField]
    private GameObject dropItemPrefab;
    [SerializeField]
    private float dropForce = 4f;

    private List<Collectable> nearbyCollectables = new List<Collectable>();
    private Collectable currentCollectable;

    [Space(10)]
    [SerializeField]
    private UnityEvent<InventoryItem> onItemPickup = new UnityEvent<InventoryItem>();
    public UnityEvent<InventoryItem> OnItemPickup { get { return onItemPickup; } }
    [SerializeField]
    private UnityEvent<InventoryItem> onCollectableTargeted = new UnityEvent<InventoryItem>();
    public UnityEvent<InventoryItem> OnCollectableTargeted { get { return onCollectableTargeted; } }

    void Start()
    {
      if (inventory == null)
      {
        Debug.LogError("PickupBehaviour requires a reference to an InventoryBehaviour.");
        enabled = false;
        return;
      }

      pickupCollider = GetComponent<Collider>();
      pickupCollider.isTrigger = true;
      pickupCollider.includeLayers = collectableLayer;

      if (pickupAction != null)
      {
        pickupAction.action.Enable();
        pickupAction.action.performed += ctx => TryPickup();
      }

      if (dropItemAction != null)
      {
        dropItemAction.action.Enable();
        dropItemAction.action.performed += ctx => DropCurrentItem();
      }
    }

    void Update()
    {
      if (nearbyCollectables.Count > 0)
      {
        float[] distances = new float[nearbyCollectables.Count];
        for (int i = 0; i < nearbyCollectables.Count; i++)
        {
          distances[i] = Vector3.Distance(transform.position, nearbyCollectables[i].transform.position);
        }

        int closestIndex = 0;
        for (int i = 1; i < distances.Length; i++)
        {
          if (distances[i] < distances[closestIndex])
          {
            closestIndex = i;
          }
        }

        currentCollectable = nearbyCollectables[closestIndex];
        onCollectableTargeted.Invoke(currentCollectable.ItemData);
      } else {
        currentCollectable = null;
        onCollectableTargeted.Invoke(null);
      }
    }

    void OnTriggerEnter(Collider other)
    {
      if (collectableLayer == (collectableLayer | (1 << other.gameObject.layer)))
      {
        Collectable collectable = other.GetComponent<Collectable>();
        if (collectable != null)
        {
          nearbyCollectables.Add(collectable);
        }
      }
    }

    void OnTriggerExit(Collider other)
    {
      if (collectableLayer == (collectableLayer | (1 << other.gameObject.layer)))
      {
        Collectable collectable = other.GetComponent<Collectable>();
        if (collectable != null)
        {
          nearbyCollectables.Remove(collectable);
        }
      }
    }

    public void TryPickup()
    {
      if (currentCollectable != null)
      {
        InventoryItem itemData = currentCollectable.ItemData;
        if (inventory.AddItem(itemData))
        {
          onItemPickup.Invoke(itemData);
          Destroy(currentCollectable.gameObject);
          nearbyCollectables.Remove(currentCollectable);
          currentCollectable = null;
          onCollectableTargeted.Invoke(null);
        }
      }
    }

    public void DropCurrentItem()
    {
      InventoryItem item = inventory.CurrentItem;
      if (item == null) return;
      if (dropItemPrefab == null) return;
      if (dropItemPosition == null) return;

      GameObject droppedItem = Instantiate(dropItemPrefab, dropItemPosition.position, Quaternion.identity);
      Collectable collectable = droppedItem.GetComponent<Collectable>();
      if (collectable != null)
      {
        collectable.SetItemData(item);
      }
      Rigidbody rb = droppedItem.GetComponent<Rigidbody>();
      if (rb != null)
      {
        rb.AddForce(dropItemPosition.transform.forward * dropForce, ForceMode.Impulse);
      }

      inventory.RemoveItem(inventory.CurrentItemIndex);
    }
  }
}