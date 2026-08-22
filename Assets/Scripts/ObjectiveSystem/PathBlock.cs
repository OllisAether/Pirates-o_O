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
    }

    private void OnCollisionEnter(Collision other)
    {
      Debug.Log($"Collision detected with {other.gameObject.name}");
      if (other.gameObject.CompareTag(playerTag))
      {
        onRejected?.Invoke();
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