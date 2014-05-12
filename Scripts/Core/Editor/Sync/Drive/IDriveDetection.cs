namespace Assets.Scripts.Core.Editor.Sync.Drive
{
    public interface IDriveDetection
    {
        string DriveName { get; }

        string DrivePath { get; }
    }
}