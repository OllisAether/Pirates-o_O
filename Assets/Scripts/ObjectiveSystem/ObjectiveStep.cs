using UnityEngine;

namespace ObjectiveSystem
{
  abstract public class ObjectiveStep : MonoBehaviour
  {
    public Objective ParentObjective { get; private set; }
    internal void Initialize(Objective parent)
    {
      ParentObjective = parent;
      OnStepStart();
    }

    private float stepProgress = 0f;
    public float StepProgress { get { return stepProgress; } protected set { stepProgress = Mathf.Clamp01(value); } }

    public abstract void OnStepStart();
    public abstract void OnStepComplete();

    protected void CompleteStep()
    {
      OnStepComplete();
      ObjectiveManager.Instance.AdvanceObjective(ParentObjective);
    }
  }
}