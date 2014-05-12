using Assets.Scripts.Core.Editor.Sync.Drive;
using Assets.Scripts.Core.Editor.Sync.Filesystem;
using Assets.Tests.Editor.Core.Unit.Attributes;
using Assets.Tests.Editor.Core.Unit.Executors;

namespace Assets.Tests.Editor.Sync.Drive
{
    [TestFixture]
    public class DriveServiceTest
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
        public void DrivePath_FakeDriveExists_ReturnsTrue()
        {
            Assert.IsTrue(DriveService.Instance.DrivePath.Exists());
        }

        [Test]
        public void IsProjectValid_FakeDriveExists_ReturnsTrue()
        {
            Assert.IsTrue(DriveService.Instance.IsProjectValid);
        }
    }
}