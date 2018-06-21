using Xunit;
using LBHTenancyAPI.Foo;

namespace LBHTenancyAPI.Test.Foo
{
    public class GreeterTest
    {
        [Fact]
        public void SaysHello()
        {
            Greeter greeter = new Greeter();
            Assert.Equal("hello world", greeter.SayHello());
        }
    }
}
