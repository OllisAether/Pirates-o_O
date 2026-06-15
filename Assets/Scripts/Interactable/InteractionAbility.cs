using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Interactable
{
  public class InteractionAbility : MonoBehaviour
  {
    [SerializeField]
    private GameObject interactor;

    [SerializeField]
    private LayerMask interactableLayerMask;

    [SerializeField]
    private Transform interactionPrompt;

    private List<Interactable> nearbyInteractables = new List<Interactable>();
    private Interactable nearestInteractable;

    [SerializeField]
    private InteractionTrigger[] interactionTriggers;

    private void Start()
    {
      if (interactionPrompt != null)
      {
        interactionPrompt.SetParent(null);
      }

      foreach (var trigger in interactionTriggers)
      {
        trigger.OnTriggerEntered.AddListener(InteractionTriggerEnter);
        trigger.OnTriggerExited.AddListener(InteractionTriggerExit);
      }
    }

    private void Update()
    {
      if (nearbyInteractables.Count > 0)
      {
        RemoveDestroyedInteractables();
        CalculateNearestInteractable();
      }

      UpdateInteractionPrompt();
    }

    private void InteractionTriggerEnter(Collider other)
    {
      if (interactableLayerMask == (interactableLayerMask | (1 << other.gameObject.layer)))
      {
        Interactable interactable = other.GetComponent<Interactable>();

        if (interactable != null)
        {
          nearbyInteractables.Add(interactable);
        }
      }
    }

    private void InteractionTriggerExit(Collider other)
    {
      if (interactableLayerMask == (interactableLayerMask | (1 << other.gameObject.layer)))
      {
        Interactable interactable = other.GetComponent<Interactable>();
        if (interactable != null)
        {
          nearbyInteractables.Remove(interactable);

          if (interactable == nearestInteractable)
          {
            nearestInteractable = null;
          }
        }
      }
    }

    private void RemoveDestroyedInteractables()
    {
      nearbyInteractables.RemoveAll(interactable => interactable == null);
    }

    private void CalculateNearestInteractable()
    {
      float nearestDistance = float.MaxValue;
      nearestInteractable = null;

      foreach (var interactable in nearbyInteractables)
      {
        float distance = Vector3.Distance(transform.position, interactable.transform.position);
        if (distance < nearestDistance)
        {
          nearestDistance = distance;
          nearestInteractable = interactable;
        }
      }
    }

    private void UpdateInteractionPrompt()
    {
      if (nearestInteractable != null)
      {
        if (interactionPrompt != null)
        {
          interactionPrompt.gameObject.SetActive(true);
          interactionPrompt.position = CalculateIndicatorPosition(nearestInteractable);
        }
      }
      else
      {
        if (interactionPrompt != null)
        {
          interactionPrompt.gameObject.SetActive(false);
        }
      }
    }

    private Vector3 CalculateIndicatorPosition(Interactable interactable)
    {
      var colliders = interactable.GetComponentsInChildren<Collider>();

      var bounds = colliders[0].bounds;
      for (var i = 1; i < colliders.Length; ++i)
      {
        bounds.Encapsulate(colliders[i].bounds);
      }

      return new Vector3(
        bounds.center.x,
        bounds.max.y,
        bounds.center.z
      );
    }

    public void OnInteract()
    {
      if (nearestInteractable != null)
      {
        nearestInteractable.Interact(interactor != null ? interactor : gameObject);
      }
    }
  }
}