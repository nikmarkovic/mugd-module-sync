using System.IO;
using Assets.Scripts.Core.Editor.Sync.Filesystem;
using UnityEngine;

namespace Assets.Scripts.Core.Editor.Sync.Drive
{
    public sealed class DriveService
    {
        public static readonly DriveService Instance = new DriveService();
        private string _path;

        private DriveService()
        {
            _path = PlayerPrefs.GetString("Assets.Scripts.Core.Editor.Sync.Drive.DriveService", string.Empty);
        }

        public string ProjectPath
        {
            get { return Path.Combine(DrivePath, Values.ProjectName); }
        }

        public string DrivePath
        {
            get { return _path; }
            set
            {
                if (_path == value) return;
                PlayerPrefs.SetString("Assets.Scripts.Core.Editor.Sync.Drive.DriveService", value);
                _path = value;
            }
        }

        public bool IsProjectValid
        {
            get
            {
                return ProjectPath.Exists() &&
                       Path.Combine(ProjectPath, "Assets").Exists() &&
                       Path.Combine(ProjectPath, "VersionControl").Exists();
            }
        }
    }
}