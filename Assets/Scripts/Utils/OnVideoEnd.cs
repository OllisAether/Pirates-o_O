using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Video;

namespace Utils
{
  public class OnVideoEnd : MonoBehaviour
  {
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] private UnityEvent onVideoEnd;
    public UnityEvent OnVideoEndEvent => onVideoEnd;

    private void Start()
    {
      if (videoPlayer == null)
      {
        videoPlayer = GetComponent<VideoPlayer>();
      }

      if (videoPlayer != null)
      {
        videoPlayer.loopPointReached += OnVideoEndReached;
      }
    }

    private void OnVideoEndReached(VideoPlayer player)
    {
      onVideoEnd?.Invoke();
    }
  }
}