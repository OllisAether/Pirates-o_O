using UnityEngine;

public class BillboardImage : MonoBehaviour
{
  [SerializeField]
  private Texture2D imageTexture;
  [SerializeField]
  private float scale = 1f;
  [SerializeField]
  private bool consistentSize = false;

  void Awake()
  {
    if (imageTexture != null)
    {
      Renderer renderer = GetComponent<Renderer>();
      if (renderer != null)
      {
        renderer.material.mainTexture = imageTexture;
        renderer.material.SetFloat("_Scale", scale);
        renderer.material.SetFloat("_ConsistentSize", consistentSize ? 1.0f : 0.0f);
      }
    }
  }
}
