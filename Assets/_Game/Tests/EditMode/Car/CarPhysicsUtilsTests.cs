using NUnit.Framework;

public class CarPhysicsUtilsTests
{
    [TestCase(DriveType.FWD, 1f, 0f)]
    [TestCase(DriveType.RWD, 0f, 1f)]
    [TestCase(DriveType.AWD, 0.5f, 0.5f)]
    
    
    public void CalculateTorqueDistribution_KnownDriveTypes_ReturnExpectedShares(
        DriveType driveType,
        float expectedFront,
        float expectedRear)
    {
        CarPhysicsUtils.CalculateTorqueDistribution(driveType, out var frontShare, out var rearShare);

        Assert.That(frontShare, Is.EqualTo(expectedFront));
        Assert.That(rearShare, Is.EqualTo(expectedRear));
    }

    [Test]
    public void CalculateTorqueDistribution_UnknownDriveType_ReturnsEvenSplit()
    {
        var unknownDriveType = (DriveType)999;

        CarPhysicsUtils.CalculateTorqueDistribution(unknownDriveType, out var frontShare, out var rearShare);

        Assert.That(frontShare, Is.EqualTo(0.5f));
        Assert.That(rearShare, Is.EqualTo(0.5f));
    }
}
