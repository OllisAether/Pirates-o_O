using System;
using System.Collections.Generic;
using UnityEngine;

namespace ObjectiveSystem
{
  [Serializable]
  public class RequiredItem
  {
    [SerializeField] private string itemId;
    [SerializeField] private int quantity;

    public string ItemId => itemId;
    public int Quantity => quantity;
  }

  public enum CountingMethod
  {
    TotalCount,
    PickedUpCount
  }

  public class CollectItemStep : ObjectiveStep
  {
    [SerializeField]
    private RequiredItem[] requiredItems;
    [SerializeField]
    private CountingMethod countingMethod;

    private int[] currentCounts;

    private InventorySystem.InventoryAbility inventory;

    public override void OnStepStart()
    {
      var player = GameManager.PlayerManager.Instance.CurrentPlayer;
      if (player == null) {
        Debug.LogError("No player found in the scene. Please ensure there is a PlayerManager with a reference to the current player.");
        return;
      }

      inventory = player.GetComponent<InventorySystem.InventoryAbility>();

      if (inventory == null) {
        Debug.LogError("Player does not have an InventoryAbility component. Please add one to the player.");
        return;
      }

      currentCounts = new int[requiredItems.Length];

      if (countingMethod == CountingMethod.TotalCount)
      {
        inventory.OnInventoryChanged.AddListener(OnInventoryChanged);
        OnInventoryChanged(inventory.ItemSlots);
      }
      else if (countingMethod == CountingMethod.PickedUpCount)
      {
        inventory.OnItemAdded.AddListener(OnItemAdded);
      }
    }

    public override void OnStepComplete()
    {
      inventory.OnInventoryChanged.RemoveListener(OnInventoryChanged);
      inventory.OnItemAdded.RemoveListener(OnItemAdded);
    }

    private void OnInventoryChanged(List<InventorySystem.ItemSlot> slots)
    {
      for (int i = 0; i < currentCounts.Length; i++)
      {
        currentCounts[i] = 0;
      }

      foreach (var slot in slots)
      {
        if (slot.Item == null) continue;

        for (int i = 0; i < requiredItems.Length; i++)
        {
          var requiredItem = requiredItems[i];
          if (requiredItem.ItemId == slot.Item.Id)
          {
            currentCounts[i] += slot.Quantity;
          }
        }
      }

      UpdateProgress();
    }

    private void OnItemAdded(InventorySystem.Item item, int quantity)
    {
      if (item == null)
      {
        Debug.LogWarning("OnItemAdded called with null item.");
        return;
      }

      for (int i = 0; i < requiredItems.Length; i++)
      {
        var requiredItem = requiredItems[i];
        if (requiredItem.ItemId == item.Id)
        {
          currentCounts[i] += quantity;
        }
      }
    }

    private void UpdateProgress()
    {
      int totalRequired = 0;
      int totalCollected = 0;

      for (int i = 0; i < requiredItems.Length; i++)
      {
        var requiredItem = requiredItems[i];

        totalRequired += requiredItem.Quantity;
        totalCollected += Math.Min(currentCounts[i], requiredItem.Quantity);
      }

      StepProgress = (float)totalCollected / totalRequired;

      if (StepProgress >= 1f)
      {
        CompleteStep();
      }
    }
  }
}