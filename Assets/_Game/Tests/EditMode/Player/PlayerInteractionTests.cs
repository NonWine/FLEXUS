using System;
using NUnit.Framework;
using UnityEngine;

public class PlayerInteractionTests
{
    [Test]
    public void OnInteract_WhenCurrentInteractableExists_CallsInteractWithPlayerGameObject()
    {
        var playerObject = new GameObject("PlayerInteractionTest_Player");

        try
        {
            var playerView = playerObject.AddComponent<PlayerView>();
            var input = new FakePlayerInput();
            var scanner = new FakeInteractionScanner();
            var interactable = new FakeInteractable();
            scanner.Current = interactable;

            var playerInteraction = new PlayerInteraction(playerView, input, scanner);
            playerInteraction.Initialize();

            input.RaiseInteract();

            Assert.That(interactable.InteractCalls, Is.EqualTo(1));
            Assert.That(interactable.LastInteractor, Is.SameAs(playerObject));

            playerInteraction.Dispose();
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(playerObject);
        }
    }

    [Test]
    public void OnInteract_WhenCurrentInteractableIsNull_DoesNotCallInteract()
    {
        var playerObject = new GameObject("PlayerInteractionTest_Player");

        try
        {
            var playerView = playerObject.AddComponent<PlayerView>();
            var input = new FakePlayerInput();
            var scanner = new FakeInteractionScanner
            {
                Current = null
            };
            var playerInteraction = new PlayerInteraction(playerView, input, scanner);
            playerInteraction.Initialize();

            Assert.DoesNotThrow(() => input.RaiseInteract());
            playerInteraction.Dispose();
        }
        finally
        {
            UnityEngine.Object.DestroyImmediate(playerObject);
        }
    }

    private sealed class FakePlayerInput : IPlayerInput
    {
        public event Action OnInteract;
        public Vector2 MoveInput => Vector2.zero;
        public bool IsSprinting => false;

        public void RaiseInteract()
        {
            OnInteract?.Invoke();
        }
    }

    private sealed class FakeInteractionScanner : IInteractionScanner
    {
        public IInteractable Current { get; set; }
        public IInteractable CurrentInteractable => Current;
        public event Action<IInteractable> OnInteractableChanged
        {
            add { }
            remove { }
        }
    }

    private sealed class FakeInteractable : IInteractable
    {
        public int InteractCalls { get; private set; }
        public GameObject LastInteractor { get; private set; }

        public void Interact(GameObject interactor)
        {
            InteractCalls++;
            LastInteractor = interactor;
        }

        public void ShowUx()
        {
        }

        public void HideUx()
        {
        }
    }
}
