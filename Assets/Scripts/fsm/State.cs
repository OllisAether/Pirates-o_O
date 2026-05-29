namespace FSM
{
  public abstract class State
  {
    public virtual void OnEnter() { }
    public virtual void OnUpdate(float deltaTime) { }
    public virtual void OnExit() { }
    public virtual State RequestStateTransition() { return null; }
  }
}