using UnityEngine;
using UnityEngine.Events;

namespace Interactable
{
  [RequireComponent(typeof(Collider))]
  public class InteractionTrigger : MonoBehaviour
  {
    public UnityEvent<Collider> OnTriggerEntered { get; } = new UnityEvent<Collider>();
    public UnityEvent<Collider> OnTriggerExited { get; } = new UnityEvent<Collider>();

    private void Reset()
    {
      Collider collider = GetComponent<Collider>();
      collider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
      OnTriggerEntered.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
      OnTriggerExited.Invoke(other);
    }
  }
}