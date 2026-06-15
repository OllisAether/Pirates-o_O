using UnityEngine;

namespace ObjectiveSystem
{
  public class ActiveObjective
  {
    public Objective Objective { get; private set; }
    public int CurrentStepIndex { get; private set; }
    public ObjectiveStepInfo CurrentStepInfo { get; private set; }

    public bool UseHiddenOverride { get; private set; } = false;
    public bool HiddenOverride { get; private set; } = false;

    public bool IsHidden {
      get
      {
        if (UseHiddenOverride)
        {
          return HiddenOverride;
        }
        return Objective.HiddenObjective;
      }
    }

    private ObjectiveStep currentStepInstance;
    public ObjectiveStep CurrentStepInstance => currentStepInstance;

    public ActiveObjective(Objective objective)
    {
      Objective = objective;
      CurrentStepIndex = 0;
      SetCurrentStepInfo();
    }

    private void SetCurrentStepInfo()
    {
      if (CurrentStepIndex < Objective.Steps.Length)
      {
        CurrentStepInfo = Objective.Steps[CurrentStepIndex];
      }
      else
      {
        CurrentStepInfo = null;
      }
    }

    public bool CanAdvanceStep()
    {
      return CurrentStepIndex < Objective.Steps.Length - 1;
    }
    public bool AdvanceStep()
    {
      if (CurrentStepIndex >= Objective.Steps.Length)
      {
        Debug.LogWarning("Cannot advance step. Already at the end of steps for objective: " + Objective.DisplayName);
        return false;
      }

      CurrentStepIndex++;
      SetCurrentStepInfo();
      return true;
    }
  
    public ObjectiveStep InstantiateCurrentStep()
    {
      DestroyCurrentStep();

      if (CurrentStepInfo != null)
      {
        var stepInstance = Object.Instantiate(CurrentStepInfo.StepPrefab);
        var stepComponent = stepInstance.GetComponent<ObjectiveStep>();
        if (stepComponent == null)
        {
          Debug.LogError("Step prefab " + CurrentStepInfo.StepPrefab.name + " does not have an ObjectiveStep component.");
          Object.Destroy(stepInstance);
          return null;
        }

        stepComponent.Initialize(Objective);
      }
      return currentStepInstance;
    }

    public void DestroyCurrentStep()
    {
      if (currentStepInstance != null)
      {
        Object.Destroy(currentStepInstance);
        currentStepInstance = null;
      }
    }
  
    public void SetHiddenOverride(bool hidden)
    {
      UseHiddenOverride = true;
      HiddenOverride = hidden;
    }

    public void ClearHiddenOverride()
    {
      UseHiddenOverride = false;
    }
  }
}
