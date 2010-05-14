using System.IO;
using DDT.SimpleHttpServer.Storage;
using Xunit;

namespace DDT.SimpleHttpServerTests.Storage
{
    public abstract class FileStorageTest
    {
        protected abstract IFileStorage CreateFileStorage();

        [Fact]
        public void FileExists_WithRealFile_ReturnsTrue()
        {
            var filesystem = CreateFileStorage();

            var result = filesystem.FileExists("filE.txt");

            Assert.True(result);
        }

        [Fact]
        public void FileExists_WithRealFileInSubfolder_ReturnsTrue()
        {
            var filesystem = CreateFileStorage();

            var result = filesystem.FileExists(@"SubDir\file.txt");

            Assert.True(result);
        }

        [Fact]
        public void FileExists_WithFakeFile_ReturnsFalse()
        {
            var filesystem = CreateFileStorage();

            var result = filesystem.FileExists("fake.txt");

            Assert.False(result);
        }

        [Fact]
        public void FileExists_WithRealFileAboveBaseNamespace_ReturnsFalse()
        {
            var filesystem = CreateFileStorage();

            var result = filesystem.FileExists(@"..\file.txt");

            Assert.False(result);
        }

        [Fact]
        public void GetFile_WithRealFileName_ReturnsFileContent()
        {
            var filesystem = CreateFileStorage();

            var result = filesystem.GetFile("file.txt");

            var reader = new StreamReader(result);
            var fileContent = reader.ReadToEnd();

            Assert.Equal("hello", fileContent);
        }

        [Fact]
        public void GetFile_WithFakeFileName_ThrowsFileNotFound()
        {
            var filesystem = CreateFileStorage();

            Assert.Throws<FileNotFoundException>(() => filesystem.GetFile("fake.txt"));
        }
    }
}