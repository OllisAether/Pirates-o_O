using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Utils
{
  [RequireComponent(typeof(VideoPlayer), typeof(RawImage))]
  public class VideoImage : MonoBehaviour
  {
    private VideoPlayer videoPlayer;
    private RawImage rawImage;
    
    private void Start()
    {
      videoPlayer = GetComponent<VideoPlayer>();
      rawImage = GetComponent<RawImage>();

      videoPlayer.prepareCompleted += OnVideoPrepared;
      videoPlayer.Prepare();
      rawImage.color = Color.clear;

      int videoWidth = (int)videoPlayer.width;
      int videoHeight = (int)videoPlayer.height;
      float aspectRatio = (float)videoWidth / videoHeight;

      var rectTransform = rawImage.rectTransform;

      if (aspectRatio > 1)
      {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, rectTransform.sizeDelta.x / aspectRatio);
      }
      else
      {
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.y * aspectRatio, rectTransform.sizeDelta.y);
      }
    }

    private void OnVideoPrepared(VideoPlayer player)
    {
      rawImage.texture = player.texture;
      rawImage.color = Color.white;
    }
  }
}
