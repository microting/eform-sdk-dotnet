using eFormSqlController;
using NUnit.Framework;

namespace eFormSDK.Integration.Tests
{
    [TestFixture]
    public class SqlControllerTest : DbTestFixture
    {
        [Test]
        public void CanPeepTwice()
        {
            DbContext.settings.Add(new settings()
            {
                id = 1,
                name = "Test1",
                value = "SomethingEls2e"
            });

            DbContext.SaveChanges();
        }
    }

}
