using Xunit;
using LBHTenancyAPI.Foo;

namespace LBHTenancyAPITest.Foo
{
    public class GreeterTest
    {
        [Fact]
        public void SaysHello()
        {
            Greeter greeter = new Greeter();
            Assert.Equal("hello world", Greeter.SayHello());
        }
    }
}
