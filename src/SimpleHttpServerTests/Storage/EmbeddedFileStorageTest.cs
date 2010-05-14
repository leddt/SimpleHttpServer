using DDT.SimpleHttpServer.Storage;

namespace DDT.SimpleHttpServerTests.Storage
{
    public class EmbeddedFileStorageTest : FileStorageTest
    {
        protected override IFileStorage CreateFileStorage()
        {
            return new EmbeddedFileStorage(GetType().Assembly, "DDT.SimpleHttpServerTests.Storage.TestEmbedded");
        }
    }
}