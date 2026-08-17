using UnityEngine;
using System;
using System.Collections.Generic;

namespace DialogSystem
{
  [Serializable]
  public class TypewriterSegment
  {
    [SerializeField]
    [TextArea(3, 10)]
    private string text;
    public string Text => text;

    [Space]
    [SerializeField] private TimingOverrides timingOverrides;
    public TimingOverrides TimingOverrides => timingOverrides;

    public TypewriterSegment(
      string text,
      TimingOverrides timingOverrides = null)
    {
      this.text = text;
      this.timingOverrides = timingOverrides ?? new TimingOverrides();
    }
  }
}