using System.Reflection;
using UnityEditor;

namespace Assets.Scripts.Core.Editor.Sync.Unity
{
    public static class Utils
    {
        public enum AssetSerializationMods
        {
            Mixed,
            Force_Binary,
            Force_Text
        }

        public enum VersionControlMods
        {
            Disabled,
            Meta_Files,
            Asset_Server,
            Perforce
        }

        public static void ChangeVersionControlMode(VersionControlMods vcm, AssetSerializationMods asm)
        {
            var editorSettings =
                new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/EditorSettings.asset")[0]);
            editorSettings.FindProperty("m_ExternalVersionControlSupport").stringValue = vcm.ToString().Replace("_", " ");
            editorSettings.FindProperty("m_SerializationMode").intValue = (int) asm;
            editorSettings.ApplyModifiedProperties();
            editorSettings.Dispose();
        }

        public static void ClearLog()
        {
            var assembly = Assembly.GetAssembly(typeof (SceneView));
            var type = assembly.GetType("UnityEditorInternal.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}