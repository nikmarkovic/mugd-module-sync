using System.IO;
using System.Linq;
using Assets.Scripts.Core.Editor.Sync;
using Assets.Scripts.Core.Editor.Sync.Drive;
using Assets.Scripts.Core.Editor.Sync.Filesystem;
using Assets.Tests.Editor.Core.Unit.Attributes;
using Assets.Tests.Editor.Core.Unit.Executors;

namespace Assets.Tests.Editor.Sync
{
    [TestFixture]
    public class DefaultSyncServiceTest
    {
        private string _originalDriveSettings;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            @"FakeDrive".CreateDirectory();
            (string.Format("{0}/{1}/{2}", "FakeDrive", Values.ProjectName, "Assets")).CreateDirectory();
            (string.Format("{0}/{1}/{2}", "FakeDrive", Values.ProjectName, "ProjectSettings")).CreateDirectory();
            (string.Format("{0}/{1}/{2}", "FakeDrive", Values.ProjectName, "VersionControl")).CreateDirectory();

            _originalDriveSettings = DriveService.Instance.DrivePath;
            DriveService.Instance.DrivePath = @"FakeDrive".FullPath();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            @"FakeDrive".DeleteDirectory();
            DriveService.Instance.DrivePath = _originalDriveSettings;
        }

        [Test]
        public void Backup_ProjectIsValid_ReturnsTrue()
        {
            var result = DefaultSyncService.Instance.Backup();
            Assert.IsTrue(result);

            var vc = Path.Combine(DriveService.Instance.ProjectPath, "VersionControl");
            var numberOfBackupFiles = vc.ToDirectoryInfo().GetFiles().Count(file => file.Extension == ".unitypackage");
            Assert.IsTrue(numberOfBackupFiles == 1);
        }
    }
}