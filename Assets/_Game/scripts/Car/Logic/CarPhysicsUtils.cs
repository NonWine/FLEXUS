public static class CarPhysicsUtils
{
    public static void CalculateTorqueDistribution(DriveType driveType, out float frontShare, out float rearShare)
    {
        switch (driveType)
        {
            case DriveType.FWD:
                frontShare = 1f;
                rearShare = 0f;
                break;
            case DriveType.RWD:
                frontShare = 0f;
                rearShare = 1f;
                break;
            case DriveType.AWD:
                frontShare = 0.5f;
                rearShare = 0.5f;
                break;
            default:
                frontShare = 0.5f;
                rearShare = 0.5f;
                break;
        }
    }
}
