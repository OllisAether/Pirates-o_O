using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Utils
{
  [RequireComponent(typeof(VideoPlayer), typeof(RawImage))]
  public class VideoImage : MonoBehaviour
  {
    [SerializeField] private string streamingAssetName = "";

    private VideoPlayer videoPlayer;
    private RawImage rawImage;
    
    private void Start()
    {
      videoPlayer = GetComponent<VideoPlayer>();
      rawImage = GetComponent<RawImage>();

      videoPlayer.prepareCompleted += OnVideoPrepared;
      videoPlayer.errorReceived += OnVideoErrorReceived;

      if (!string.IsNullOrEmpty(streamingAssetName))
      {
        string videoPath = System.IO.Path.Combine(Application.streamingAssetsPath, streamingAssetName);
        videoPlayer.url = videoPath;
      }

      videoPlayer.Prepare();
      rawImage.color = Color.clear;
    }

    private void OnVideoPrepared(VideoPlayer player)
    {
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

      rawImage.texture = player.texture;
      rawImage.color = Color.white;
    }

    private void OnVideoErrorReceived(VideoPlayer player, string message)
    {
      rawImage.color = Color.red;
      
      // Log text in rawImage to indicate error
      var text = new GameObject("ErrorText", typeof(RectTransform), typeof(CanvasRenderer), typeof(Text));
      text.transform.SetParent(rawImage.transform, false);
      var textComponent = text.GetComponent<Text>();
      textComponent.text = "Video Error: " + message;
      textComponent.alignment = TextAnchor.MiddleCenter;
      textComponent.color = Color.white;
      textComponent.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
      textComponent.resizeTextForBestFit = true;
      textComponent.resizeTextMinSize = 10;
      textComponent.resizeTextMaxSize = 100;
      textComponent.rectTransform.anchorMin = Vector2.zero;
      textComponent.rectTransform.anchorMax = Vector2.one;
      textComponent.rectTransform.offsetMin = Vector2.zero;
      textComponent.rectTransform.offsetMax = Vector2.zero;

      Debug.LogError($"VideoPlayer error: {message}");
    }
  }
}
