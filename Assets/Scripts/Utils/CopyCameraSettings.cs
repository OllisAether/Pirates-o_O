using UnityEngine;

public class CopyCameraSettings : MonoBehaviour
{
  [SerializeField]
  private Camera sourceCamera;

  [SerializeField]
  private Camera targetCamera;

  void Start()
  {
    
  }

  void Update()
  {
    if (sourceCamera != null && targetCamera != null)
    {
      targetCamera.fieldOfView = sourceCamera.fieldOfView;
      targetCamera.nearClipPlane = sourceCamera.nearClipPlane;
      targetCamera.farClipPlane = sourceCamera.farClipPlane;
      targetCamera.orthographic = sourceCamera.orthographic;
      targetCamera.orthographicSize = sourceCamera.orthographicSize;
      targetCamera.usePhysicalProperties = sourceCamera.usePhysicalProperties;
      targetCamera.sensorSize = sourceCamera.sensorSize;
      targetCamera.lensShift = sourceCamera.lensShift;
      targetCamera.focalLength = sourceCamera.focalLength;
      targetCamera.aperture = sourceCamera.aperture;
      targetCamera.focusDistance = sourceCamera.focusDistance;
      targetCamera.gateFit = sourceCamera.gateFit;
    }
  }
}
