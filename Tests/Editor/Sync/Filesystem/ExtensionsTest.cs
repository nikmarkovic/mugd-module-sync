using Assets.Scripts.Core.Editor.Sync.Filesystem;
using Assets.Tests.Editor.Core.Unit.Attributes;
using Assets.Tests.Editor.Core.Unit.Executors;

namespace Assets.Tests.Editor.Sync.Filesystem
{
    [TestFixture]
    public class ExtensionsTest
    {
        [SetUp]
        public void SetUp()
        {
            @"TestFolder/Directory1".CreateDirectory();
            @"TestFolder/Directory1/Directory1-1".CreateDirectory();
            @"TestFolder/Directory2".CreateDirectory();
        }

        [TearDown]
        public void TearDown()
        {
            @"TestFolder".DeleteDirectory();
        }

        [Test]
        public void Exists_DirectoryExists_ReturnsTrue()
        {
            Assert.IsTrue(@"TestFolder/Directory1".Exists());
        }

        [Test]
        public void Exists_DirectoryNotExists_ReturnsFalse()
        {
            Assert.IsFalse(@"TestFolder/Direecctory".Exists());
        }

        [Test]
        public void CreateDirectory_DirectoryExists_ReturnsExistingDirectory()
        {
            Assert.IsTrue(@"TestFolder/Directory1".CreateDirectory().FullName.Exists());
        }

        [Test]
        public void CreateDirectory_NewDirectory_ReturnsNewDirectory()
        {
            Assert.IsTrue(@"TestFolder/DirectoryNew".CreateDirectory().FullName.Exists());
        }

        [Test]
        public void CopyDirectoryTo_SourceAndTargetExists()
        {
            @"TestFolder/Directory1".CopyDirectoryTo(@"TestFolder/Directory2");
            Assert.IsTrue(@"TestFolder/Directory2/Directory1-1".Exists());
        }

        [Test]
        public void CopyDirectoryTo_TargetNotExists()
        {
            @"TestFolder/Directory1".CopyDirectoryTo(@"TestFolder/Directory2/Directory1");
            Assert.IsTrue(@"TestFolder/Directory2/Directory1/Directory1-1".Exists());
        }

        [Test]
        public void CopyDirectoryTo_SoruceIsEmpty()
        {
            @"TestFolder/Directory1/Directory1-1".CopyDirectoryTo(@"TestFolder/Directory2/");
            Assert.IsTrue(@"TestFolder/Directory2/".Exists());
        }

        [Test]
        public void CopyDirectoryTo_SoruceNotExists()
        {
            @"TestFolder/Directory3".CopyDirectoryTo(@"TestFolder/Directory2/");
            Assert.IsFalse(@"TestFolder/Directory2/Directory3".Exists());
        }

        [Test]
        public void DeleteDirectory_DirectoryExists()
        {
            @"TestFolder/Directory1".DeleteDirectory();
            Assert.IsFalse(@"TestFolder/Directory1".Exists());
        }

        [Test]
        public void Delete_Directory()
        {
            @"TestFolder/Directory1".Delete();
            Assert.IsFalse(@"TestFolder/Directory1".Exists());
        }

        [Test]
        public void Name_DirectoryExists_ReturnsDirectoryName()
        {
            Assert.AreEqual("Directory1", @"TestFolder/Directory1".Name());
        }

        //[Test]
        public void CreateSymLinkTo_DeleteSymLink()
        {
            @"TestFolder/Directory1".CreateSymLinkTo(@"TestFolder/Directory2");
            Assert.IsTrue(@"TestFolder/Directory2/Directory1".IsSymLink());
            @"TestFolder/Directory2/Directory1".DeleteSymLink();
            Assert.IsFalse(@"TestFolder/Directory2/Directory1".IsSymLink());
        }
    }
}