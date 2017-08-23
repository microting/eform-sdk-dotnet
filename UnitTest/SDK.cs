using eFormCore;

using System;
using System.Linq;
using Xunit;

namespace UnitTest
{
    public class SDK
    {
        string serverConnectionString = "Data Source=DESKTOP-7V1APE5\\SQLEXPRESS;Initial Catalog=MicrotingTest;Integrated Security=True";
        int siteId1 = 3818;
        int siteId2 = 3823;
        int workerMUId = 1778;
        int unitMUId = 4938;

        #region test 001x basic
        [Fact]
        public void Test0001a_MustAlwaysPass()
        {
            //Arrange
            bool checkValueA = true;
            bool checkValueB = false;


            //Act
            checkValueB = true;


            //Assert
            Assert.Equal(checkValueA, checkValueB);
        }

        [Fact]
        public void Test0002a_Core_Initiate()
        {
            //Arrange
            Core checkValue = null;


            //Act
            checkValue = new Core();


            //Assert
            Assert.NotNull(checkValue);
        }

        //[Fact]
        //public void Test0003a_Core_Start_WithBlankExpection()
        //{
        //    //Arrange
        //    bool checkValueA = true;
        //    bool checkValueB = false;
        //    Core core = new Core();


        //    //Act
        //    try
        //    {
        //        core.Start("");
        //    }
        //    catch
        //    {
        //        checkValueB = true;
        //    }

        //    //Assert
        //    Assert.Equal(checkValueA, checkValueB);
        //}

        //[Fact]
        //public void Test0003b_Core_Start()
        //{
        //    //Arrange
        //    bool checkValueA = true;
        //    bool checkValueB = false;
        //    Core core = new Core();


        //    //Act
        //    checkValueB = core.Start(serverConnectionString);
            

        //    //Assert
        //    Assert.Equal(checkValueA, checkValueB);
        //}
        #endregion
    }
}