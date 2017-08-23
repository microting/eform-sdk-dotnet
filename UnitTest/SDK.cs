using System;
using System.Linq;
using Xunit;

namespace UnitTest
{
    public class SDK
    {
        [Fact]
        public void Test0001_MustAlwaysPass()
        {
            //Arrange
            bool checkValueA = true;
            bool checkValueB = false;


            //...
            //Act
            checkValueB = true;


            //...
            //Assert
            Assert.Equal(checkValueA, checkValueB);
        }

        [Fact]
        public void Test0002_InitiateCore()
        {
            //Arrange
            bool value = true;


            //...
            //Act


            //...
            //Assert
            Assert.Equal(true, value);
        }
    }
}