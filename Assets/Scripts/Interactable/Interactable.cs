using UnityEngine;
using UnityEngine.Events;

namespace Interactable
{
  public class Interactable : MonoBehaviour
  {
    [SerializeField]
    private bool invokeOnInteractMessage = true;

    [SerializeField]
    private Vector3 interactionPromptOffset = new Vector3(0, 0, 0);
    public Vector3 InteractionPromptOffset => interactionPromptOffset;

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