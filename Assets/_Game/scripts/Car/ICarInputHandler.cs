using System;

public interface ICarInputHandler : ICarInput
{
    event Action OnExitPerformed;
    void Enable();
    void Disable();
}
