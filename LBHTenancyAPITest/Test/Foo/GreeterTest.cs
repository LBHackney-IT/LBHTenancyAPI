namespace LBHTenancyAPITest.Test.Foo
{
    using LBHTenancyAPI.Foo;
    using Xunit;

    public class GreeterTest
    {
        [Fact]
        public void SaysHello()
        {
            Assert.Equal("hello world", Greeter.SayHello());
        }
    }
}
