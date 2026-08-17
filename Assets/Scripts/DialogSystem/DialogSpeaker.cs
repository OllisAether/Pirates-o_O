using UnityEngine;

namespace DialogSystem
{
  [CreateAssetMenu(fileName = "New Dialog Speaker", menuName = "Dialog System/Dialog Speaker")]
  public class DialogSpeaker : ScriptableObject { 
    [SerializeField] private string speakerName = "";
    public string Name => speakerName;
    [SerializeField] private float pitch = 1.0f;
    public float Pitch => pitch;
  }
}