namespace DDT.SimpleHttpServerTests.TestHelpers
{
    public static class Mocked
    {
        public static MockedHttpContextBuilder HttpContext()
        {
            return new MockedHttpContextBuilder();
        }
    }
}