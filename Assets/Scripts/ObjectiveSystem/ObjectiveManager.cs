using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Utils;

namespace ObjectiveSystem
{
  public class ObjectiveManager : SingletonBehaviour<ObjectiveManager>
  {
    [SerializeField]
    private ObjectiveSet[] objectiveSets;
    private bool isDictionaryConstructed = false;
    private Dictionary<string, Objective> objectiveDictionary;

    private Dictionary<string, ActiveObjective> currentObjectives = new Dictionary<string, ActiveObjective>();
    private Dictionary<string, Objective> completedObjectives = new Dictionary<string, Objective>();

    [SerializeField]
    private UnityEvent<Objective> onObjectiveStarted = new UnityEvent<Objective>();
    [SerializeField]
    private UnityEvent<Objective> onObjectiveCompleted = new UnityEvent<Objective>();
    [SerializeField]
    private UnityEvent<ObjectiveStepInfo> onObjectiveStepStarted = new UnityEvent<ObjectiveStepInfo>();
    [SerializeField]
    private UnityEvent<ObjectiveStepInfo> onObjectiveStepCompleted = new UnityEvent<ObjectiveStepInfo>();

    public UnityEvent<Objective> OnObjectiveStarted => onObjectiveStarted;
    public UnityEvent<Objective> OnObjectiveCompleted => onObjectiveCompleted;
    public UnityEvent<ObjectiveStepInfo> OnObjectiveStepStarted => onObjectiveStepStarted;
    public UnityEvent<ObjectiveStepInfo> OnObjectiveStepCompleted => onObjectiveStepCompleted;

    private void Start()
    {
      ConstructObjectiveDictionary();
      onObjectiveCompleted.AddListener(_ => AutoStartObjectives());
      AutoStartObjectives();
    }
    private void Update()
    {
      
    }

    private void ConstructObjectiveDictionary()
    {
      if (isDictionaryConstructed)
      {
        Debug.LogWarning("Objective dictionary is already constructed. Skipping reconstruction.");
        return;
      }

      objectiveDictionary = new Dictionary<string, Objective>();
      foreach (ObjectiveSet set in objectiveSets)
      {
        foreach (Objective objective in set.Objectives)
        {
          if (!objectiveDictionary.ContainsKey(objective.Id))
          {
            objectiveDictionary.Add(objective.Id, objective);
          }
          else
          {
            Debug.LogWarning("Duplicate objective ID found: " + objective.Id + ". Skipping.");
          }
        }
      }

      isDictionaryConstructed = true;
    }

    public Objective GetObjective(string objectiveId)
    {
      if (!isDictionaryConstructed)
      {
        Debug.LogWarning("Objective dictionary not yet constructed. Constructing now...");
        ConstructObjectiveDictionary();
      }

      if (objectiveDictionary.TryGetValue(objectiveId, out Objective objective))
      {
        return objective;
      }
      else
      {
        Debug.LogWarning("Objective with ID " + objectiveId + " not found.");
        return null;
      }
    }

    public void AutoStartObjectives()
    {
      foreach (var kvp in objectiveDictionary)
      {
        Objective objective = kvp.Value;

        if (objective.AutoStart && CheckPrerequisites(objective))
        {
          if (currentObjectives.ContainsKey(objective.Id) || completedObjectives.ContainsKey(objective.Id)) continue;

          Debug.Log("Auto-starting objective: " + objective.DisplayName);
          StartObjective(objective);
        }
      }
    }

    public void StartObjective(Objective objective)
    {
      if (currentObjectives.ContainsKey(objective.Id))
      {
        Debug.LogWarning("Objective " + objective.DisplayName + " is already active.");
        return;
      }

      if (completedObjectives.ContainsKey(objective.Id))
      {
        Debug.LogWarning("Objective " + objective.DisplayName + " has already been completed.");
        return;
      }

      if (!CheckPrerequisites(objective))
      {
        Debug.LogWarning("Cannot start objective " + objective.DisplayName + ". Prerequisites not met.");
        return;
      }

      Debug.Log("Starting objective: " + objective.DisplayName);

      ActiveObjective activeObjective = new ActiveObjective(objective);
      currentObjectives.Add(objective.Id, activeObjective);
      onObjectiveStarted.Invoke(objective);

      activeObjective.InstantiateCurrentStep();
      onObjectiveStepStarted.Invoke(activeObjective.CurrentStepInfo);
    }

    public void AdvanceObjective(Objective objective)
    {
      if (!currentObjectives.TryGetValue(objective.Id, out ActiveObjective activeObjective))
      {
        Debug.LogWarning("Objective " + objective.DisplayName + " is not currently active.");
        return;
      }

      if (!activeObjective.CanAdvanceStep())
      {
        CompleteObjective(activeObjective);
        return;
      }

      Debug.Log("Advancing objective: " + objective.DisplayName);

      activeObjective.AdvanceStep();
      onObjectiveStepCompleted.Invoke(activeObjective.CurrentStepInfo);
      
      activeObjective.InstantiateCurrentStep();
      onObjectiveStepStarted.Invoke(activeObjective.CurrentStepInfo);
    }

    public void CompleteObjective(ActiveObjective activeObjective)
    {
      Debug.Log("Completing objective: " + activeObjective.Objective.DisplayName);

      Objective objective = activeObjective.Objective;

      currentObjectives.Remove(objective.Id);
      completedObjectives.Add(objective.Id, objective);

      onObjectiveCompleted.Invoke(objective);

      if (objective.NextObjective != null)
      {
        StartObjective(objective.NextObjective);
      }
    }

    public bool CheckPrerequisites(Objective objective)
    {
      foreach (Objective prereq in objective.PrerequisiteObjectives)
      {
        if (!completedObjectives.ContainsKey(prereq.Id))
        {
          return false;
        }
      }
      return true;
    }
  }
}
