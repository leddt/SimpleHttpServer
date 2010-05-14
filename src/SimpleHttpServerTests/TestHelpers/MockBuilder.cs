using Rhino.Mocks;

namespace DDT.SimpleHttpServerTests.TestHelpers
{
    public class MockBuilder<TMock> where TMock : class
    {
        public TMock Mock { get; private set; }

        public MockBuilder()
        {
            Mock = MockRepository.GenerateMock<TMock>();
        }

        public static implicit operator TMock(MockBuilder<TMock> mockBuilder)
        {
            return mockBuilder.Mock;
        }
    }
}