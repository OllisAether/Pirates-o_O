using UnityEngine.Events;

namespace FSM
{
  public class StateMachine
  {
    private State currentState;
    public State CurrentState
    {
      get { return currentState; }
      set { TransitionToState(value); }
    }

    private UnityEvent<State> onStateChanged = new UnityEvent<State>();
    public UnityEvent<State> OnStateChanged => onStateChanged;

    public void TransitionToState(State newState)
    {
      if (currentState != null)
      {
        currentState.OnExit();
      }

      currentState = newState;
      onStateChanged.Invoke(currentState);

      currentState.OnEnter();
    }

    public void Update(float deltaTime)
    {
      if (currentState != null)
      {
        currentState.OnUpdate(deltaTime);
        State newState = currentState.RequestStateTransition();

        if (newState != null)
        {
          TransitionToState(newState);
        }
      }
    }
  }
}