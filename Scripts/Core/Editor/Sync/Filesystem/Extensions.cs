using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Assets.Scripts.Core.Editor.Sync.Filesystem
{
    public static class Extensions
    {
        public static bool Exists(this string directory)
        {
            return Directory.Exists(directory);
        }

        public static DirectoryInfo CreateDirectory(this string directory)
        {
            return directory.Exists() ? new DirectoryInfo(directory) : Directory.CreateDirectory(directory);
        }

        public static void CopyDirectoryTo(this string source, string target)
        {
            if (!source.Exists()) return;
            var sourceDirectory = new DirectoryInfo(source);
            var targetDirectory = target.CreateDirectory();
            sourceDirectory.GetFiles().ToList().ForEach(file => file.CopyTo(Path.Combine(target, file.Name), true));
            sourceDirectory.GetDirectories()
                .ToList()
                .ForEach(
                    directory =>
                        directory.FullName.CopyDirectoryTo(targetDirectory.CreateSubdirectory(directory.Name).FullName));
        }

        public static void DeleteDirectory(this string directory)
        {
            try
            {
                Directory.Delete(directory, true);
            }
            catch
            {
                directory.DeleteSymLink();
            }
        }

        public static void Delete(this string directory)
        {
            if (directory.IsSymLink())
            {
                directory.DeleteSymLink();
            }
            else
            {
                directory.DeleteDirectory();
            }
        }

        public static string FullPath(this string directory)
        {
            return directory.ToDirectoryInfo().FullName;
        }

        public static string Name(this string directory)
        {
            return directory.ToDirectoryInfo().Name;
        }

        public static DirectoryInfo ToDirectoryInfo(this string directory)
        {
            return new DirectoryInfo(directory);
        }

#if UNITY_STANDALONE_WIN

        public static bool IsSymLink(this string directory)
        {
            if (!directory.Exists()) return false;
            var attributes = File.GetAttributes(directory);
            return attributes != FileAttributes.Directory;
        }

        public static void CreateSymLinkTo(this string source, string target)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments =
                    "/c cd /d" + "\"" + target.FullPath() + "\"" + " & mklink " + "/d" + " " + source.Name() + " " +
                    "\"" + source.FullPath() + "\"",
                Verb = "runas",
                WindowStyle = ProcessWindowStyle.Normal,
                UseShellExecute = true
            };
            var process = Process.Start(processStartInfo);
            if (process != null) process.WaitForExit();
        }

        public static void DeleteSymLink(this string directory)
        {
            var processStartInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                Arguments = "/c rmdir \"" + directory.FullPath() + "\"",
            };
            if (!directory.Exists()) return;
            var process = Process.Start(processStartInfo);
            if (process != null) process.WaitForExit();
        }

#endif

#if UNITY_STANDALONE_OSX

        public static bool IsSymLink(this string directory)
        {
            throw new NotImplementedException(); 
        }

        public static void CreateSymLink(this string source, string target)
        {
            throw new NotImplementedException(); 
        }

        public static void DeleteSymLink(this string directory)
        {
            throw new NotImplementedException(); 
        }

#endif
    }
}