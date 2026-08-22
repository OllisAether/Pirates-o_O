using UnityEngine;
using System;

namespace DialogSystem
{
  [Serializable]
  public class DialogSegment
  {
    [SerializeField] private DialogSpeaker speaker;
    public DialogSpeaker Speaker => speaker;
    [SerializeField] private DialogSpeakerOverride speakerOverride;
    public DialogSpeakerOverride SpeakerOverride => speakerOverride;
    [SerializeField] private TypewriterSegment[] typewriterSegments;
    public TypewriterSegment[] TypewriterSegments => typewriterSegments;
    [SerializeField] private bool autoAdvance = false;
    public bool AutoAdvance => autoAdvance;
    [SerializeField] private string[] dialogEvents;
    public string[] DialogEvents => dialogEvents;

    public DialogSegment(
      DialogSpeaker speaker,
      TypewriterSegment[] typewriterSegments,
      DialogSpeakerOverride speakerOverride = null)
    {
      this.speaker = speaker;
      this.speakerOverride = speakerOverride ?? new DialogSpeakerOverride();
      this.typewriterSegments = typewriterSegments;
    }
  }
}