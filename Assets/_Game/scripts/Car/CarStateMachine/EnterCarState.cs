

public class EnterCarState : CarState
{
    
    public override void Enter()
    {
        ChangeState<DrivingCarState>();
    }

    public override void Exit()
    {
        
    }
}