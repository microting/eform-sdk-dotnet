using eFormCore;
using eFormShared;
using System;
using System.Linq;
using Xunit;

namespace UnitTest
{
    public class SDK
    {
  

        int siteId1     = 2001;
        int siteId2     = 2002;
        int workerMUId  = 2003;
        int unitMUId    = 345678;

        Tools t = new Tools();

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

        [Fact]
        public void Test0003a_Core_StartSqlOnly_WithBlankExpection()
        {
            //Arrange
            string checkValueA = "serverConnectionString is not allowed to be null or empty";
            string checkValueB = "some other text";
            Core core = new Core();

            //Act
            try
            {
                checkValueB = core.StartSqlOnly("").ToString();
            }
            catch (Exception ex)
            {
                checkValueB = ex.InnerException.Message;
            }

            //Assert
            Assert.Equal(checkValueA, checkValueB);
        }

        [Fact]
        public void Test0003b_Core_StartSqlOnly()
        {
            //Arrange
            string checkValueA = "True";
            string checkValueB = "False";
            Core core = new Core();
            
            //Act
            try
            {
                string serverConnectionString = "Persist Security Info=True;server=localhost;database=microtingMySQL;uid=root;password=";

                try
                {
                    if (Environment.MachineName == "DESKTOP-7V1APE5")
                    {
                        //serverConnectionString = "Data Source=DESKTOP-7V1APE5\\SQLEXPRESS;Initial Catalog=MicrotingTestNew;Integrated Security=True";
                        serverConnectionString = "Persist Security Info=True;server=localhost;database=microtingMySQL;uid=root;password=1234";
                    }
                }
                catch { }

                AdminTools at = new AdminTools(serverConnectionString);
                at.DbSetup("unittest");
                checkValueB = core.StartSqlOnly(serverConnectionString).ToString();
            }
            catch (Exception ex)
            {
                checkValueB = t.PrintException(t.GetMethodName() + " failed", ex);
            }

            //Assert
            Assert.Equal(checkValueA, checkValueB);
        }
        #endregion
    }
}