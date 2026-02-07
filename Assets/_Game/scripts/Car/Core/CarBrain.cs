using Zenject;

public class CarBrain : IInitializable
{
    private readonly CarStateMachine stateMachine;

    public CarBrain(CarStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }

    public void Initialize()
    {
    }
}
