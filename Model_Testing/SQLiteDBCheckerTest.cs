using System;
using System.IO;
using NUnit.Framework;
using CompMgr.Model.DBUtils;
using System.Data.SQLite;


namespace Model_Testing
{
    [TestFixture]
    public class SQLiteDBCheckerTest
    {
        SQLiteDataBaseCreator testDBCreator = new SQLiteDataBaseCreator();

        [Test]
        public void TestDBCreation()
        {
            string testFile = "test.db";


            testDBCreator.CreateDataBase(testFile);
            Assert.IsTrue(File.Exists(testFile));
        }
    }
}
