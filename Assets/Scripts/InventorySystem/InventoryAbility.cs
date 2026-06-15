using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace InventorySystem
{

  public class InventoryAbility : MonoBehaviour
  {
    [SerializeField]
    private int inventorySize = 20;

    [SerializeField]
    private List<ItemSlot> itemSlots;
    public List<ItemSlot> ItemSlots { get => itemSlots; }

    [SerializeField]
    private UnityEvent<List<ItemSlot>> onInventoryChanged;
    public UnityEvent<List<ItemSlot>> OnInventoryChanged { get => onInventoryChanged; }

    [SerializeField]
    private UnityEvent<Item, int> onItemAdded;
    public UnityEvent<Item, int> OnItemAdded { get => onItemAdded; }
    [SerializeField]
    private UnityEvent<Item, int> onItemRemoved;
    public UnityEvent<Item, int> OnItemRemoved { get => onItemRemoved; }
    private void Start()
    {
      itemSlots = new List<ItemSlot>();
    }

    public void SetInventory(ItemSlot[] slots)
    {
      itemSlots.Clear();
      for (int i = 0; i < Mathf.Min(slots.Length, inventorySize); i++)
      {
        itemSlots.Add(slots[i]);
      }
      onInventoryChanged.Invoke(itemSlots);
    }

    public bool HasFreeSlotsFor(Item item, int quantity = 1)
    {
      var stackLimit = item.IsStackable ? (
        item.UseCustomStackLimit ? item.CustomStackLimit : ItemManager.Instance.DefaultStackLimit
      ) : 1;

      int freeSpace = 0;
      for (int i = 0; i < inventorySize; i++)
      {
        if (i < itemSlots.Count)
        {
          var slot = itemSlots[i];
          if (slot.Item == item)
          {
            freeSpace += stackLimit - slot.Quantity;
          }
        }
        else
        {
          freeSpace += stackLimit;
        }

        if (freeSpace >= quantity)
        {
          return true;
        }
      }

      return false;
    }

    public bool AddItem(Item item, int quantity = 1) => AddItem(item, quantity, out _, out _);
    public bool AddItem(Item item, int quantity, out int itemsAdded) => AddItem(item, quantity, out itemsAdded, out _);
    public bool AddItem(Item item, int quantity, out int itemsAdded, out ItemSlot addedSlot)
    {
      itemsAdded = 0;
      addedSlot = null;

      var stackLimit = item.IsStackable ? (
        item.UseCustomStackLimit ? item.CustomStackLimit : ItemManager.Instance.DefaultStackLimit
      ) : 1;

      while (itemsAdded < quantity)
      {
        var slot = itemSlots.Find(s => s.Item == item && s.Quantity < stackLimit);
        if (slot != null && slot.Quantity < stackLimit)
        {
          int addable = Mathf.Min(stackLimit - slot.Quantity, quantity - itemsAdded);
          slot.SetQuantity(slot.Quantity + addable);
          itemsAdded += addable;
          if (addedSlot == null)
          {
            addedSlot = slot;
          }
        }
        else
        {
          if (itemSlots.Count >= inventorySize)
          {
            Debug.LogWarning("Inventory is full. Could not add all items.");
            return false;
          }

          int addable = Mathf.Min(stackLimit, quantity - itemsAdded);
          ItemSlot newSlot = new ItemSlot(item, addable);
          itemSlots.Add(newSlot);
          itemsAdded += addable;
          if (addedSlot == null)
          {
            addedSlot = newSlot;
          }
        }
      }

      onInventoryChanged.Invoke(itemSlots);
      onItemAdded.Invoke(item, itemsAdded);
      return true;
    }

    public bool RemoveItem(Item item, int quantity = 1) => RemoveItem(item, quantity, out _);
    public bool RemoveItem(Item item, int quantity, out int itemsRemoved)
    {
      itemsRemoved = 0;

      while (itemsRemoved < quantity)
      {
        var slot = itemSlots.Find(s => s.Item == item);
        if (slot != null)
        {
          int removable = Mathf.Min(slot.Quantity, quantity - itemsRemoved);
          slot.SetQuantity(slot.Quantity - removable);
          itemsRemoved += removable;

          if (slot.Quantity <= 0)
          {
            itemSlots.Remove(slot);
          }
        }
        else
        {
          Debug.LogWarning("Not enough items to remove.");
          return false;
        }
      }

      onItemRemoved.Invoke(item, itemsRemoved);
      onInventoryChanged.Invoke(itemSlots);
      return true;
    }

    public bool HasItem(Item item, int quantity = 1)
    {
      int totalQuantity = 0;
      foreach (var slot in itemSlots)
      {
        if (slot.Item == item)
        {
          totalQuantity += slot.Quantity;
          if (totalQuantity >= quantity)
          {
            return true;
          }
        }
      }
      return false;
    }

    public void ClearInventory()
    {
      Debug.Log("Clearing inventory of " + gameObject.name);
      itemSlots.Clear();
      onInventoryChanged.Invoke(itemSlots);
    }

    private void Update()
    {
    }
  }
}