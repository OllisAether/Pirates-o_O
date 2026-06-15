using UnityEngine;

namespace ObjectiveSystem
{
  [System.Serializable]
  public class ObjectiveStepInfo
  {
    [SerializeField]
    private GameObject stepPrefab;

    public GameObject StepPrefab => stepPrefab;
    [SerializeField]
    private string stepName;
    public string StepName => stepName;
  }
}