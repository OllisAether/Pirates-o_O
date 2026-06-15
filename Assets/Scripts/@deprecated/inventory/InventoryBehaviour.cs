using System.Collections.Generic;
using System.Linq;
using Persistence;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Inventory
{
  public class InventoryBehaviour : MonoBehaviour, ISerializable
  {
    private InventoryItem currentItem;
    public InventoryItem CurrentItem => currentItem;
    private int currentItemIndex = 0;
    public int CurrentItemIndex => currentItemIndex;

    [SerializeField]
    private InventoryItem[] inventoryItems;
    public InventoryItem[] InventoryItems => inventoryItems;

    [SerializeField]
    private int inventorySize = 10;

    [SerializeField]
    private InputActionReference useItemAction;
    [SerializeField]
    private InputActionReference switchItemAction;

    // === Events ===
    // Theres a lot bc why not
    [Space(10)]
    [SerializeField]
    private UnityEvent<InventoryItem[]> onInventoryChanged = new UnityEvent<InventoryItem[]>();
    public UnityEvent<InventoryItem[]> OnInventoryChanged => onInventoryChanged;
    [SerializeField]
    private UnityEvent<InventoryItem> onItemUsed = new UnityEvent<InventoryItem>();
    public UnityEvent<InventoryItem> OnItemUsed => onItemUsed;
    [SerializeField]
    private UnityEvent<InventoryItem> onItemAdded = new UnityEvent<InventoryItem>();
    public UnityEvent<InventoryItem> OnItemAdded => onItemAdded;
    [SerializeField]
    private UnityEvent<InventoryItem> onItemRemoved = new UnityEvent<InventoryItem>();
    public UnityEvent<InventoryItem> OnItemRemoved => onItemRemoved;
    [SerializeField]
    private UnityEvent<InventoryItem> onCurrentItemChanged = new UnityEvent<InventoryItem>();
    public UnityEvent<InventoryItem> OnCurrentItemChanged => onCurrentItemChanged;
    [SerializeField]
    private UnityEvent onInventoryFull = new UnityEvent();
    public UnityEvent OnInventoryFull => onInventoryFull;

    [SerializeField]
    private Transform itemHoldPoint;
    public Transform ItemHoldPoint => itemHoldPoint;
    [SerializeField]
    private Transform cameraRoot;

    private GameObject[] heldItemObjects;
    

    void Start()
    {
      if (inventoryItems == null)
      {
        inventoryItems = new InventoryItem[inventorySize];

        OnInventoryChanged.Invoke(inventoryItems);
      } else if (inventoryItems.Length != inventorySize)
      {
        Debug.LogWarning("Inventory items array length does not match inventory size. Resizing array.");
        System.Array.Resize(ref inventoryItems, inventorySize);
      }

      heldItemObjects = new GameObject[inventorySize];

      if (useItemAction != null)
      {
        useItemAction.action.Enable();
        useItemAction.action.performed += ctx => UseCurrentItem();
      }

      if (switchItemAction != null)
      {
        switchItemAction.action.Enable();
        switchItemAction.action.performed += ctx => SwitchItem(ctx.ReadValue<float>() > 0 ? 1 : -1);
      }

      SwitchItem(1);
    }

    void Update()
    {
    }

    public void UseCurrentItem()
    {
      if (currentItem == null) return;

      var itemObject = heldItemObjects[currentItemIndex];
      if (itemObject == null) return;

      var useBehaviour = itemObject.GetComponent<UseBehaviour>();
      if (useBehaviour == null) return;

      Debug.Log("Using item: " + currentItem.DisplayName);

      useBehaviour.Use();

      onItemUsed.Invoke(currentItem);
    }

    public void SwitchItem(int direction)
    {
      if (inventoryItems.Length == 0)
      {
        currentItem = null;
        currentItemIndex = -1;

        UpdateActiveHeldItem();

        onCurrentItemChanged.Invoke(null);
        return;
      }

      int i = 0;
      int newIndex = currentItemIndex;
      do
      {
        newIndex = (newIndex + direction + inventoryItems.Length) % inventoryItems.Length;
        if (inventoryItems[newIndex] != null)
        {
          currentItemIndex = newIndex;
          currentItem = inventoryItems[currentItemIndex];

          UpdateActiveHeldItem();

          onCurrentItemChanged.Invoke(currentItem);
          return;
        }
        i++;
      } while (newIndex != currentItemIndex && i < inventoryItems.Length);

      currentItem = null;
      currentItemIndex = -1;

      UpdateActiveHeldItem();

      onCurrentItemChanged.Invoke(null);
    }

    public bool AddItem(InventoryItem item)
    {
      for (int i = 0; i < inventoryItems.Length; i++)
      {
        if (inventoryItems[i] == null)
        {
          inventoryItems[i] = item;
          currentItemIndex = i;
          currentItem = item;
          
          if (item.HoldingPrefab != null && itemHoldPoint != null)
          {
            if (heldItemObjects[i] != null)
            {
              Destroy(heldItemObjects[i]);
            }

            var itemObject = Instantiate(item.HoldingPrefab, itemHoldPoint);
            var useBehaviour = itemObject.GetComponent<UseBehaviour>();
            if (useBehaviour != null)
            {
              useBehaviour.SetPlayerTransform(transform);
              useBehaviour.SetPlayerCameraRoot(cameraRoot);
            }
            heldItemObjects[i] = itemObject;
          }

          UpdateActiveHeldItem();

          onItemAdded.Invoke(item);
          onCurrentItemChanged.Invoke(currentItem);
          onInventoryChanged.Invoke(inventoryItems);
          return true;
        }
      }

      onInventoryFull.Invoke();
      return false;
    }

    public void RemoveItem(int index)
    {
      if (index >= 0 && index < inventoryItems.Length && inventoryItems[index] != null)
      {
        InventoryItem removedItem = inventoryItems[index];
        inventoryItems[index] = null;

        if (heldItemObjects[index] != null)
        {
          Destroy(heldItemObjects[index]);
          heldItemObjects[index] = null;
        }

        onItemRemoved.Invoke(removedItem);
        onInventoryChanged.Invoke(inventoryItems);

        if (currentItemIndex == index)
        {
          SwitchItem(1);
        }
      }
    }

    private void UpdateActiveHeldItem()
    {
      for (int i = 0; i < heldItemObjects.Length; i++)
      {
        if (heldItemObjects[i] != null)
        {
          heldItemObjects[i].SetActive(i == currentItemIndex);
        }
      }
    }

    public void SetInventoryItems(InventoryItem[] items)
    {
      if (items.Length > inventorySize)
      {
        Debug.LogError("Cannot set inventory items: array length exceeds inventory size.");
        return;
      }

      if (items.Length < inventorySize)
      {
        Debug.LogWarning("Setting inventory items with an array smaller than inventory size. Remaining slots will be set to null.");
        items = items.Concat(new InventoryItem[inventorySize - items.Length]).ToArray();
      }

      inventoryItems = items;

      for (int i = 0; i < inventoryItems.Length; i++)
      {
        if (inventoryItems[i] != null && inventoryItems[i].HoldingPrefab != null && itemHoldPoint != null)
        {
          if (heldItemObjects[i] != null)
          {
            Destroy(heldItemObjects[i]);
          }

          var itemObject = Instantiate(inventoryItems[i].HoldingPrefab, itemHoldPoint);
          var useBehaviour = itemObject.GetComponent<UseBehaviour>();
          if (useBehaviour != null)
          {
            useBehaviour.SetPlayerTransform(transform);
            useBehaviour.SetPlayerCameraRoot(cameraRoot);
          }
          heldItemObjects[i] = itemObject;
        }
        else
        {
          if (heldItemObjects[i] != null)
          {
            Destroy(heldItemObjects[i]);
            heldItemObjects[i] = null;
          }
        }
      }

      UpdateActiveHeldItem();
      onInventoryChanged.Invoke(inventoryItems);
    }

    public string Serialize()
    {
      return JsonUtility.ToJson(new InventoryData(InventoryItems.Select(i => i != null ? i.ItemID : null).ToArray()));
    }

    public void Deserialize(string jsonData)
    {
      InventoryData data = JsonUtility.FromJson<InventoryData>(jsonData);
      if (data != null)
      {
        Debug.Log("Deserialized inventory data: " + jsonData);
        SetInventoryItems(data.items.Select(i => !string.IsNullOrEmpty(i) ? ItemManager.Instance.GetItemByID(i) : null).ToArray());
      }
    }
  }
}