using FSM;
using StarterAssets;
using UnityEngine;

namespace AI {
  public enum StrollMode
  {
    Cycle,
    Random
  }

  public class StrollBehaviour : MonoBehaviour
  {
    [SerializeField]
    private Transform[] waypoints = new Transform[0];
    public Transform[] Waypoints { get { return waypoints; } }  

    [SerializeField]
    private int startingWaypoint = 0;

    [SerializeField]
    private StrollMode strollMode = StrollMode.Cycle;
    public StrollMode StrollMode { get { return strollMode; } }

    [SerializeField]
    private ThirdPersonControllerAI controller;

    public int CurrentWaypointIndex { get; set; } = 0;

    private StateMachine stateMachine = new StateMachine();

    private Vector3 reachedDestination;
    public Vector3 ReachedDestination { get { return reachedDestination; } }

    void Start()
    {
      controller.OnDestinationReached.AddListener(OnDestinationReached);

      stateMachine.CurrentState = new GoToWaypointState(this, waypoints[startingWaypoint]);
    }

    void Update()
    {
      stateMachine.Update(Time.deltaTime);
    }

    private void OnDestinationReached(Vector3 destination)
    {
      reachedDestination = destination;
      Debug.Log("Destination reached: " + destination);
    }
  
    internal void SetTarget(Transform target)
    {
      controller.Target = target;
    }
  }

  public class GoToWaypointState : State
  {
    private StrollBehaviour strollBehaviour;
    private Transform targetWaypoint;

    private const float waypointReachedThreshold = 0.1f;

    public GoToWaypointState(StrollBehaviour strollBehaviour, Transform targetWaypoint)
    {
      this.strollBehaviour = strollBehaviour;
      this.targetWaypoint = targetWaypoint;
    }

    public override void OnEnter()
    {
      strollBehaviour.SetTarget(targetWaypoint);
    }

    public override State RequestStateTransition()
    {
      if (strollBehaviour.ReachedDestination != null && Vector3.Distance(strollBehaviour.ReachedDestination, targetWaypoint.position) < waypointReachedThreshold)
      {
        switch (strollBehaviour.StrollMode)
        {
          case StrollMode.Cycle:
            strollBehaviour.CurrentWaypointIndex = (strollBehaviour.CurrentWaypointIndex + 1) % strollBehaviour.Waypoints.Length;
            break;
          case StrollMode.Random:
            strollBehaviour.CurrentWaypointIndex = Random.Range(0, strollBehaviour.Waypoints.Length);
            break;
        }

        var nextState = new GoToWaypointState(strollBehaviour, strollBehaviour.Waypoints[strollBehaviour.CurrentWaypointIndex]);

        return nextState;
      }
      return null;
    }
  }
}
