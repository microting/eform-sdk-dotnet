using System;
using System.Linq;
using Xunit;

namespace UnitTest
{
    public class SDK
    {
        [Fact]
        public void TestMethod1()
        {
            bool value = true;

            Assert.Equal(true, value);
        }
    }
}