using UnityEngine;
using System;

namespace DialogSystem
{
  [Serializable]
  public class TimingOverrides {
    [SerializeField] private float postDelay;
    public float PostDelay => postDelay;
    [SerializeField] private bool overridePitch;
    public bool OverridePitch => overridePitch;
    [SerializeField] private float pitchOverride;
    public float PitchOverride => pitchOverride;
    [SerializeField] private bool overrideDelay;
    public bool OverrideDelay => overrideDelay;
    [SerializeField] private float delayOverride;
    public float DelayOverride => delayOverride;
    [SerializeField] private CharacterOverrideDelay[] characterOverrides;
    public CharacterOverrideDelay[] CharacterOverrides => characterOverrides;

    public TimingOverrides(
      float postDelay = 0f,
      bool overridePitch = false,
      float pitchOverride = 1f,
      bool overrideDelay = false,
      float delayOverride = 0.05f,
      CharacterOverrideDelay[] characterOverrides = null)
    {
      this.postDelay = postDelay;
      this.overridePitch = overridePitch;
      this.pitchOverride = pitchOverride;
      this.overrideDelay = overrideDelay;
      this.delayOverride = delayOverride;
      this.characterOverrides = characterOverrides ?? new CharacterOverrideDelay[0];
    }
  }
}