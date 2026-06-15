using UnityEngine;

namespace ObjectiveSystem
{
  [CreateAssetMenu(fileName = "New Objective Set", menuName = "Objective System/Objective Set")]
  public class ObjectiveSet : ScriptableObject
  {
    [SerializeField]
    private Objective[] objectives;

    public Objective[] Objectives => objectives;
  }
}