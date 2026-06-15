using UnityEngine;
using UnityEngine.Events;

namespace Interactable
{
  public class Interactable : MonoBehaviour
  {
    [SerializeField]
    private bool invokeOnInteractMessage = true;

    [SerializeField]
    private UnityEvent<GameObject> onInteract;
    public UnityEvent<GameObject> OnInteract { get => onInteract; }

    public void Interact(GameObject interactor)
    {
      Debug.Log($"{gameObject.name} interacted with by {interactor.name}");
      onInteract.Invoke(interactor);

      if (invokeOnInteractMessage)
      {
        SendMessage("OnInteract", interactor, SendMessageOptions.DontRequireReceiver);
      }
    }
  }
}