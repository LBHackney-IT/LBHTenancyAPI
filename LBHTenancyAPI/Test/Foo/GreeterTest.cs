using NUnit.Framework;
using LBHTenancyAPI.Foo;

namespace LBHTenancyAPI.Test.Foo
{
    [TestFixture]
    public class GreeterTest
    {
        [Test]
        public void SaysHello()
        {
            Greeter greeter = new Greeter();
            Assert.AreEqual(greeter.SayHello(), "hello world");
        }
    }
}
