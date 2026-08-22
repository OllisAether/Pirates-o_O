using UnityEngine;
using UnityEngine.Events;

namespace InventorySystem
{
  public class OnItemUsed : MonoBehaviour
  {
    [SerializeField] private UnityEvent onItemUsed;

    public void UseItem()
    {
      Debug.Log($"Item {gameObject.name} used.");
      onItemUsed?.Invoke();
    }
  }
}