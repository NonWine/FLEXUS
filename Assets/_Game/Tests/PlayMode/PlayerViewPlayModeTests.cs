using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerViewPlayModeTests
{
    [UnityTest]
    public IEnumerator Awake_WhenOptionalTransformsAreNotAssigned_UsesOwnTransform()
    {
        var playerObject = new GameObject("PlayMode_PlayerView");

        try
        {
            var playerView = playerObject.AddComponent<PlayerView>();

            yield return null;

            Assert.That(playerView.LookAt, Is.SameAs(playerObject.transform));
            Assert.That(playerView.InteractionSource, Is.SameAs(playerObject.transform));
        }
        finally
        {
            Object.DestroyImmediate(playerObject);
        }
    }
}
