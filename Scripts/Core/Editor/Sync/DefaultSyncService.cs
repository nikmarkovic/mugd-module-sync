using System.IO;
using Assets.Scripts.Core.Editor.Sync.Drive;
using Assets.Scripts.Core.Editor.Sync.Filesystem;
using Assets.Scripts.Core.Editor.Sync.Unity;
using UnityEditor;

namespace Assets.Scripts.Core.Editor.Sync
{
    public class DefaultSyncService : ISyncService
    {
        public static readonly DefaultSyncService Instance = new DefaultSyncService();

        private DefaultSyncService()
        {
        }

        public bool Sync()
        {
            if (!DriveService.Instance.IsProjectValid && DriveService.Instance.ProjectPath.Exists()) return false;
            Utils.ChangeVersionControlMode(Utils.VersionControlMods.Meta_Files, Utils.AssetSerializationMods.Force_Text);
            AssetDatabase.Refresh();
            CopyProjectToDrive();
            SyncProject();
            AssetDatabase.Refresh();
            Utils.ClearLog();
            return true;
        }

        public bool Backup()
        {
            if (!DriveService.Instance.IsProjectValid) return false;
            var packageName = Values.ProjectName + Values.Timestamp + ".unitypackage";
            var backupFolder = Path.Combine(DriveService.Instance.ProjectPath, "VersionControl");
            AssetDatabase.ExportPackage("Assets",
               Path.Combine(backupFolder, packageName),
               ExportPackageOptions.Recurse | ExportPackageOptions.IncludeDependencies | ExportPackageOptions.IncludeLibraryAssets);
            return true;
        }

        private static void CopyProjectToDrive()
        {
            if (DriveService.Instance.ProjectPath.Exists()) return;
            "Assets".CopyDirectoryTo(Path.Combine(DriveService.Instance.ProjectPath, "Assets"));
            Path.Combine(DriveService.Instance.ProjectPath, "VersionControl").CreateDirectory();
        }

        private static void SyncProject()
        {
            "Assets".Delete();
            Path.Combine(DriveService.Instance.ProjectPath, "Assets").CreateSymLinkTo(Values.ProjectPath);
        }
    }
}