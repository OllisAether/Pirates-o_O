using DialogSystem;
using UnityEngine;
using UnityEngine.Events;

namespace ObjectiveSystem
{
  public class PathBlock : MonoBehaviour
  {
    [SerializeField] private Objective completedObjective;
    public Objective CompletedObjective => completedObjective;

    [SerializeField] private UnityEvent onRejected;

    [SerializeField] private string playerTag = "Player";

    private void Start()
    {
      ObjectiveManager.Instance.OnObjectiveCompleted.AddListener(OnObjectiveCompleted);

      var colliders = GetComponentsInChildren<Collider>();
      foreach (var collider in colliders)
      {
        collider.isTrigger = true;
      }
    }

    private void OnTriggerEnter(Collider other)
    {
      if (other.CompareTag(playerTag))
      {
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var collider in colliders)
        {
          collider.isTrigger = false;
        }

        onRejected?.Invoke();
      }
    }
    private void OnTriggerExit(Collider other)
    {
      if (other.CompareTag(playerTag))
      {
        var colliders = GetComponentsInChildren<Collider>();

        foreach (var collider in colliders)
        {
          collider.isTrigger = true;
        }
      }
    }

    private void OnObjectiveCompleted(Objective objective)
    {
      if (objective == completedObjective)
      {
        gameObject.SetActive(false);
      }
    }
  }
}