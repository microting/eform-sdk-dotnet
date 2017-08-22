using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace eFormSDK.Tests
{
    class Tests
    {
        [Fact]
        public void FirstTest()
        {
            bool value = true;

            Assert.Equal(true, value);
        }
    }
}
