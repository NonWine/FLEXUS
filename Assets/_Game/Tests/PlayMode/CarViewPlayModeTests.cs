using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class CarViewPlayModeTests
{
    [UnityTest]
    public IEnumerator InteractionMethods_WhenCalled_RaiseExpectedEvents()
    {
        var carObject = new GameObject("PlayMode_CarView");
        var interactor = new GameObject("PlayMode_Interactor");

        try
        {
            var carView = carObject.AddComponent<CarView>();
            GameObject receivedInteractor = null;
            var interactedCalls = 0;
            var showUxCalls = 0;
            var hideUxCalls = 0;

            carView.OnInteractedEvent += value =>
            {
                interactedCalls++;
                receivedInteractor = value;
            };
            carView.OnShowUxEvent += () => showUxCalls++;
            carView.OnHideUxEvent += () => hideUxCalls++;

            yield return null;

            carView.Interact(interactor);
            carView.ShowUx();
            carView.HideUx();

            Assert.That(interactedCalls, Is.EqualTo(1));
            Assert.That(receivedInteractor, Is.SameAs(interactor));
            Assert.That(showUxCalls, Is.EqualTo(1));
            Assert.That(hideUxCalls, Is.EqualTo(1));
        }
        finally
        {
            Object.DestroyImmediate(carObject);
            Object.DestroyImmediate(interactor);
        }
    }
}
