using UnityEngine;
using UnityEngine.Events;

namespace InventorySystem
{
  public class Collectable : MonoBehaviour
  {
    [SerializeField]
    private Item item;
    [SerializeField]
    private int quantity = 1;
    [SerializeField]
    private AudioSource collectAudioSource;

    public Item Item { get => item; }

    [SerializeField]
    private UnityEvent onCollected;
    public UnityEvent OnCollected { get => onCollected; }

    public void Collect(GameObject collector)
    {
      var inventory = collector.GetComponentInChildren<InventoryAbility>();
      var holdItemAbility = collector.GetComponentInChildren<HoldItemAbility>();

      if (inventory == null)
      {
        Debug.LogWarning($"{collector.name} does not have an inventory to collect {item.DisplayName}");
        return;
      }
      
      if (!inventory.HasFreeSlotsFor(item))
      {
        Debug.LogWarning($"{collector.name} does not have enough inventory space to collect {item.DisplayName}");
        return;
      }

      Debug.Log($"{collector.name} collected {item.DisplayName}");

      if (collectAudioSource != null)
      {
        collectAudioSource.Play();
        collectAudioSource.transform.SetParent(null);
        Destroy(collectAudioSource.gameObject, collectAudioSource.clip.length);
      }

      ItemSlot addedSlot;
      inventory.AddItem(item, quantity, out _, out addedSlot);
      if(item.Holdable && holdItemAbility != null && addedSlot != null)
      {
        holdItemAbility.HoldItem(addedSlot);
      }
      Destroy(gameObject);
    }
  }
}