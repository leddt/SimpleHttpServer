using DDT.SimpleHttpServer.Storage;

namespace DDT.SimpleHttpServerTests.Storage
{
    public class DirectoryFileStorageTest : FileStorageTest
    {
        protected override IFileStorage CreateFileStorage()
        {
            return new DirectoryFileStorage(@"Storage\TestPhysical");
        }
    }
}