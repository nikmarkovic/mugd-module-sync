using System;
using System.IO;

namespace Assets.Scripts.Core.Editor.Sync.Filesystem
{
    public static class Values
    {
        public static string ProjectPath
        {
            get { return Directory.GetCurrentDirectory(); }
        }

        public static string ProjectName
        {
            get { return ProjectPath.Name(); }
        }

        public static string Timestamp
        {
            get { return DateTime.Now.ToString("yyyyMMddHHmmssffff"); }
        }
    }
}