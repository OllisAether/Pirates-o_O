
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace InventorySystem
{
  [RequireComponent(typeof(InventoryAbility))]
  public class HoldItemAbility : MonoBehaviour
  {
    [SerializeField]
    private Transform holdPoint;
    [SerializeField]
    private Animator animator;

    private int currentSlotIndex = -1;
    private ItemSlot currentItemSlot;
    public ItemSlot CurrentItemSlot { get => currentItemSlot; }

    private UnityEvent<ItemSlot> onHoldItemChanged = new UnityEvent<ItemSlot>();
    public UnityEvent<ItemSlot> OnHoldItemChanged { get => onHoldItemChanged; }

    private InventoryAbility inventory;

    private Dictionary<ItemSlot, GameObject> heldItemObjects = new Dictionary<ItemSlot, GameObject>();

    private void Awake()
    {
      inventory = GetComponent<InventoryAbility>();
    }

    private void Start ()
    {
      FetchHoldableItems();
      UpdateHoldingItems();
      UpdateActiveHeldItem();
      inventory.OnInventoryChanged.AddListener(OnInventoryChanged);
    }

    private void OnInventoryChanged(List<ItemSlot> itemSlots)
    {
      FetchHoldableItems();
      UpdateHoldingItems();
      UpdateActiveHeldItem();
    }

    private List<ItemSlot> holdableItems = new List<ItemSlot>();
    private List<ItemSlot> FetchHoldableItems()
    {
      holdableItems.Clear();

      foreach (var slot in inventory.ItemSlots)
      {
        if (slot.Item != null && slot.Item.Holdable)
        {
          holdableItems.Add(slot);
        }
      }

      if (currentItemSlot != null && !holdableItems.Contains(currentItemSlot))
      {
        SetHoldItemIndex(-1);
      } else if (currentItemSlot != null)
      {
        SetHoldItemIndex(holdableItems.IndexOf(currentItemSlot));
      }

      return holdableItems;
    }

    private void UpdateHoldingItems()
    {
      var newHeldItemObjects = new Dictionary<ItemSlot, GameObject>();

      foreach (var slot in holdableItems)
      {
        if (heldItemObjects.ContainsKey(slot))
        {
          newHeldItemObjects[slot] = heldItemObjects[slot];
          heldItemObjects.Remove(slot);
        }
        else
        {
          var itemObject = Instantiate(slot.Item.HoldPrefab, holdPoint);
          newHeldItemObjects[slot] = itemObject;
          itemObject.SetActive(slot == currentItemSlot);
        }
      }

      foreach (var kvp in heldItemObjects)
      {
        Destroy(kvp.Value);
      }

      heldItemObjects = newHeldItemObjects;
    }

    private void UpdateActiveHeldItem()
    {
      foreach (var kvp in heldItemObjects)
      {
        kvp.Value.SetActive(kvp.Key == currentItemSlot);
      }

      animator.SetInteger("HoldItemType", currentItemSlot != null ? (int)currentItemSlot.Item.HoldItemType : 0);
    }

    private void SetHoldItemIndex(int index)
    {
      if (index < -1 || index >= holdableItems.Count)
      {
        Debug.LogWarning($"Invalid hold item index: {index}");
        return;
      }

      currentSlotIndex = index;
      currentItemSlot = index == -1 ? null : holdableItems[index];
      UpdateActiveHeldItem();

      onHoldItemChanged.Invoke(currentItemSlot);
    }

    public void NextHoldItem()
    {
      if (holdableItems.Count == 0)
      {
        SetHoldItemIndex(-1);
        return;
      }

      int nextIndex = (currentSlotIndex + 2) % (holdableItems.Count + 1) - 1;
      Debug.Log($"Switching to next hold item. New index: {nextIndex}");
      SetHoldItemIndex(nextIndex);
    }

    public void PreviousHoldItem()
    {
      if (holdableItems.Count == 0)
      {
        SetHoldItemIndex(-1);
        return;
      }

      int prevIndex = (currentSlotIndex + holdableItems.Count + 1) % (holdableItems.Count + 1) - 1;
      Debug.Log($"Switching to previous hold item. New index: {prevIndex}");
      SetHoldItemIndex(prevIndex);
    }

    public void OnSwitchItem(InputValue value)
    {
      var floatValue = value.Get<float>();

      if (floatValue > 0)
      {
        NextHoldItem();
      }
      else if (floatValue < 0)
      {
        PreviousHoldItem();
      }
    }

    public void HoldItem(ItemSlot itemSlot)
    {
      if (itemSlot == null)
      {
        SetHoldItemIndex(-1);
        Debug.Log("Stopped holding item.");
        return;
      }

      int index = holdableItems.IndexOf(itemSlot);
      if (index == -1)
      {
        Debug.LogWarning($"Item {itemSlot.Item.DisplayName} is not holdable or not in inventory.");
        return;
      }

      SetHoldItemIndex(index);
      Debug.Log($"Now holding item: {itemSlot.Item.DisplayName}");
    }
  
    public void OnUseItem(InputValue value)
    {
      if (currentItemSlot != null && currentItemSlot.Item != null)
      {
        heldItemObjects[currentItemSlot].GetComponent<OnItemUsed>()?.UseItem();
      }
    }
  }
}