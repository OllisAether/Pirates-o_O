using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class PunchPlayer : MonoBehaviour
{

  [SerializeField]
  private float punchForce = 10f;

  [SerializeField]
  private float waitForPunchDuration = 0.5f;

  void Start()
  {
    
  }

  void Update()
  {
    
  }
  
  void OnTriggerEnter(Collider other)
  {
    if (other.CompareTag("Player"))
    {
      var playerController = other.GetComponent<CharacterController>();
      StartCoroutine(punchCoroutine(playerController));
    }
  }

  private IEnumerator punchCoroutine(CharacterController playerController)
  {
    yield return new WaitForSeconds(waitForPunchDuration);

    playerController.Move(playerController.transform.rotation * transform.forward * punchForce * Time.deltaTime);
  }
}
