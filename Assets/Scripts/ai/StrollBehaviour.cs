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
    public Transform[] Waypoints => waypoints;
    

    [SerializeField]
    private int startingWaypoint = 0;
    [SerializeField]
    private float waypointIdleTime = 0f;

    [SerializeField]
    private StrollMode strollMode = StrollMode.Cycle;
    public StrollMode StrollMode => strollMode;

    [SerializeField]
    private ThirdPersonControllerAI controller;

    public int CurrentWaypointIndex { get; set; } = 0;

    private StateMachine stateMachine = new StateMachine();

    private Vector3 reachedDestination;
    public Vector3 ReachedDestination => reachedDestination;

    void Start()
    {
      controller.OnDestinationReached.AddListener(OnDestinationReached);

      stateMachine.CurrentState = new GoToWaypointState(this, waypoints[startingWaypoint], waypointIdleTime);
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

    private Transform pauseTarget;
    internal bool isPaused = false;
    public void PauseStroll()
    {
      pauseTarget = controller.Target;
      controller.Target = null;
      isPaused = true;
    }

    public void ResumeStroll()
    {
      controller.Target = pauseTarget;
      isPaused = false;
    }
  }

  public class GoToWaypointState : State
  {
    private StrollBehaviour strollBehaviour;
    private Transform targetWaypoint;
    private float waypointIdleTime = 0f;

    private const float waypointReachedThreshold = 0.1f;

    public GoToWaypointState(StrollBehaviour strollBehaviour, Transform targetWaypoint, float waypointIdleTime = 0f)
    {
      this.strollBehaviour = strollBehaviour;
      this.targetWaypoint = targetWaypoint;
      this.waypointIdleTime = waypointIdleTime;
    }

    public override void OnEnter()
    {
      strollBehaviour.SetTarget(targetWaypoint);
    }

    private bool AtDestination => strollBehaviour.ReachedDestination != null && Vector3.Distance(strollBehaviour.ReachedDestination, targetWaypoint.position) < waypointReachedThreshold;
    float idleTimer = 0f;
    public override void OnUpdate(float deltaTime)
    {
      if (strollBehaviour.isPaused) return;

      if (AtDestination)
      {
        idleTimer += deltaTime;
      } else
      {
        idleTimer = 0f;
      }
    }

    public override State RequestStateTransition()
    {
      if (strollBehaviour.isPaused) return null;

      if (AtDestination && idleTimer >= waypointIdleTime)
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

        var nextState = new GoToWaypointState(strollBehaviour, strollBehaviour.Waypoints[strollBehaviour.CurrentWaypointIndex], waypointIdleTime);

        return nextState;
      }
      return null;
    }
  }
}
