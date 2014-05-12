using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Assets.Scripts.Core.Editor.Sync.Drive;
using Assets.Scripts.Core.Editor.Sync.Filesystem;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.Core.Editor.Sync
{
    public class SyncWindow : EditorWindow
    {
        private static string _path;
        private static List<IDriveDetection> _autodetectModules;

        private void OnGUI()
        {
            GUILayout.Label("Drive path", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            _path = EditorGUILayout.TextField(_path);
            if (GUILayout.Button("..."))
            {
                _path = EditorUtility.OpenFolderPanel("Drive Folder", "", "");
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Separator();
            if (_autodetectModules.Count > 0)
            {
                GUILayout.Label("Detect path...");
                EditorGUILayout.BeginHorizontal();
                _autodetectModules.ForEach(module =>
                {
                    if (!GUILayout.Button(module.DriveName)) return;
                    try
                    {
                        _path = module.DrivePath;
                    }
                    catch
                    {
                        _path = string.Empty;
                    }
                });
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.Separator();
            if (!GUILayout.Button("Save")) return;
            DriveService.Instance.DrivePath = _path;
            GetWindow(typeof (SyncWindow), true, "Sync Settings").Close();
        }

        [MenuItem("Edit/Sync Settings...")]
        public static void OpenSyncSettings()
        {
            _path = DriveService.Instance.DrivePath;
            var assembly = Assembly.GetCallingAssembly();
            _autodetectModules = assembly
                .GetTypes()
                .Where(type => type.GetInterfaces().Contains(typeof (IDriveDetection)))
                .Select(module => assembly.CreateInstance(module.FullName) as IDriveDetection).ToList();

            GetWindow(typeof (SyncWindow), true, "Sync Settings");
        }

        [MenuItem("Assets/Sync Project")]
        public static void SyncProject()
        {
            var status = DefaultSyncService.Instance.Sync() ? "Sync completed." : "Sync failed.";
            Debug.Log(status);
        }

        [MenuItem("Assets/Sync Project", true)]
        public static bool ValidateSyncProject()
        {
            if ("Assets".IsSymLink() || !DriveService.Instance.DrivePath.Exists()) return false;
            return !DriveService.Instance.ProjectPath.Exists() || DriveService.Instance.IsProjectValid;
        }

        [MenuItem("Assets/Backup Project")]
        public static void BackupProject()
        {
            var status = DefaultSyncService.Instance.Backup() ? "Backup completed." : "Backup failed.";
            Debug.Log(status);
        }

        [MenuItem("Assets/Backup Project", true)]
        public static bool ValidateBackupProject()
        {
            return DriveService.Instance.IsProjectValid && "Assets".IsSymLink();
        }
    }
}