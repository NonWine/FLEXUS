public interface ICarInput
{
    float Throttle { get; }
    float Steer { get; }
    float Brake { get; }
    bool IsHandbraking { get; }
}
