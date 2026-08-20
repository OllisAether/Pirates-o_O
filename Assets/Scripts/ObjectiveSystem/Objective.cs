using UnityEngine;

namespace ObjectiveSystem
{

  [CreateAssetMenu(fileName = "New Objective", menuName = "Objective System/Objective")]
  public class Objective : ScriptableObject
  {
    [SerializeField]
    private string id;
    [SerializeField]
    private string displayName;
    [SerializeField]
    private string description;
    [SerializeField]
    private bool hiddenObjective;
    [SerializeField]
    private bool autoStart;
    [SerializeField]
    private Objective[] prerequisiteObjectives;

    [SerializeField]
    private ObjectiveStepInfo[] steps;

    [SerializeField]
    private Objective nextObjective;

    public string Id => id;
    public string DisplayName => displayName;
    public string Description => description;
    public bool HiddenObjective => hiddenObjective;
    public bool AutoStart => autoStart;
    public Objective[] PrerequisiteObjectives => prerequisiteObjectives;
    public ObjectiveStepInfo[] Steps => steps;

    public Objective NextObjective => nextObjective;
  }
}