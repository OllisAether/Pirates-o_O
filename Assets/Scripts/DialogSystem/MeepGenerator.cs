using UnityEngine;

namespace DialogSystem
{
  [RequireComponent(typeof(AudioSource))]
  public class MeepGenerator : MonoBehaviour
  {
    [SerializeField] private AudioClip[] meepSounds;
    [SerializeField] private string soundableCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

    private AudioSource audioSource;

    private void Awake()
    {
      audioSource = GetComponent<AudioSource>();
    }

    private void OnValidate()
    {
      if (audioSource == null)
      {
        audioSource = GetComponent<AudioSource>();
      }
    }

    public void PlayMeep(string context, char character, float pitch = 1f)
    {
      if (!soundableCharacters.Contains(character.ToString())) return;

      AudioClip clip = GetMeepClip(context);
      audioSource.pitch = pitch;
      audioSource.PlayOneShot(clip);
    }

    private int GetMeepHash(string context)
    {
      return context.GetHashCode();
    }

    private AudioClip GetMeepClip(string context)
    {
      int hash = GetMeepHash(context);
      int index = Mathf.Abs(hash) % meepSounds.Length;
      return meepSounds[index];
    }
  }
}