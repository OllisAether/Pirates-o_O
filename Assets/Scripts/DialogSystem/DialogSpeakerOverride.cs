using System;
using UnityEngine;

namespace DialogSystem
{
  [Serializable]
  public class DialogSpeakerOverride
  {
    [SerializeField] private bool overrideName = false;
    public bool OverrideName => overrideName;
    [SerializeField] private string name = "";
    public string Name => name;
    [SerializeField] private bool overridePitch = false;
    public bool OverridePitch => overridePitch;
    [SerializeField] private float pitch = 1f;
    public float Pitch => pitch;

    public DialogSpeakerOverride() { }
    public DialogSpeakerOverride(bool overrideName = false, string name = "", bool overridePitch = false, float pitch = 1f)
    {
      this.overrideName = overrideName;
      this.name = name;
      this.overridePitch = overridePitch;
      this.pitch = pitch;
    }
  }
}