using ObjectiveSystem;
using UnityEngine;

namespace DialogSystem
{
  [CreateAssetMenu(fileName = "New Dialog Tree", menuName = "Dialog System/Dialog Tree")]
  public class DialogTree : ScriptableObject
  {
    [SerializeField] private bool showBlackBars = false;
    public bool ShowBlackBars => showBlackBars;
    [SerializeField] private DialogSegment[] dialogSegments;
    public DialogSegment[] DialogSegments => dialogSegments;
    [SerializeField] private DialogResponse[] responses;
    public DialogResponse[] Responses => responses;
    [SerializeField] private Objective objectiveToStart;
    public Objective ObjectiveToStart => objectiveToStart;
  }
}