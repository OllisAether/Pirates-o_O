using UnityEngine;

public class ClampRotation : MonoBehaviour
{
  [SerializeField] private float minX = -90f;
  [SerializeField] private float maxX = 90f;
  [SerializeField] private float minY = -90f;
  [SerializeField] private float maxY = 90f;
  [SerializeField] private float minZ = -90f;
  [SerializeField] private float maxZ = 90f;

  private void LateUpdate()
  {
    Vector3 currentRotation = transform.localEulerAngles;

    // Convert angles greater than 180 to negative angles for proper clamping
    if (currentRotation.x > 180) currentRotation.x -= 360;
    if (currentRotation.y > 180) currentRotation.y -= 360;
    if (currentRotation.z > 180) currentRotation.z -= 360;

    currentRotation.x = Mathf.Clamp(currentRotation.x, minX, maxX);
    currentRotation.y = Mathf.Clamp(currentRotation.y, minY, maxY);
    currentRotation.z = Mathf.Clamp(currentRotation.z, minZ, maxZ);

    transform.localEulerAngles = currentRotation;
  }
}
