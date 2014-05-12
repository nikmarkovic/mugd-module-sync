namespace Assets.Scripts.Core.Editor.Sync
{
    public interface ISyncService
    {
        bool Sync();

        bool Backup();
    }
}