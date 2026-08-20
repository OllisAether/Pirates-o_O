using DialogSystem;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

namespace ObjectiveSystem
{
  public class ConditionalObjectiveHandler : MonoBehaviour
  {
    [SerializeField] private Objective[] requiredObjectives;
    [SerializeField] private UnityEvent onAccepted;
    [SerializeField] private UnityEvent onRejected;

    public void HandleConditionalObjective()
    {
      foreach (var objective in requiredObjectives)
      {
        if (!ObjectiveManager.Instance.IsObjectiveCompleted(objective.Id))
        {
          onRejected?.Invoke();
          return;
        }
      }

      onAccepted?.Invoke();
    }
  }
}