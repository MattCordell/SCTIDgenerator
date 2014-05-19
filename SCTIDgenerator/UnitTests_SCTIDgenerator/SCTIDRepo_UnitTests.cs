using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using SCTIDgenerator_library;
using System.Data.SQLite;

namespace UnitTests_SCTIDgenerator
{
    //Repo creator
    //repo reader
    //repo writer

    [TestClass]
    public class SCTIDRepo_UnitTests
    {
        //Repo database file. Must be same as in class.        
        private const string RepoFile = "SCTIDRepo.db";
        private const string RepoDump = "AllocatedSCTIDs.txt";
        private string connectionString = "data source=" + RepoFile + ";Version=3;Cache Size=2000;Page Size=32768;Synchronous=OFF;Journal Mode=WAL;";

        [TestMethod]
        public void RepoIsCreatedSuccessfuly()
        {
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            if (File.Exists(RepoFile))
            {
                File.Delete(RepoFile);
            }
            SCTIDRepo Repo = new SCTIDRepo();
            Assert.IsTrue(File.Exists(RepoFile));
        }

        //Just destroy the repo
        [TestMethod]
        public void NukingRepoSucceeds()
        {
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            if (File.Exists(RepoFile))
            {
                File.Delete(RepoFile);
            }
            Assert.IsFalse(File.Exists(RepoFile));
        }

        [TestMethod]
        public void AbleToGetNextConceptBean()
        {
            SCTIDRepo Repo = new SCTIDRepo();
            int NextBean = Repo.GetNextConceptBean(1234567);

            Assert.AreSame(1, NextBean);           
        }

        [TestMethod]
        public void AbleToReserveConceptId()
        {
            var ID = "11234567100";
            SCTIDRepo Repo = new SCTIDRepo();
            try
            {
                Repo.ReserveId(ID);
                Assert.IsTrue(true);
            }
            catch (Exception)
            {
                Assert.Fail("Unable to reserve ID: {0}", ID);          
            }
        }

        [TestMethod]
        public void RepoIsSuccesfullyDumped()
        {
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            if (File.Exists(RepoDump))
            {
                File.Delete(RepoDump);
            }
            SCTIDRepo Repo = new SCTIDRepo();

            Repo.DumpRepository();

            Assert.IsTrue(File.Exists(RepoDump));
        }

        [TestMethod]
        public void RepoIsSuccesfullyImported()
        {
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            if (File.Exists(RepoDump))
            {
                File.Delete(RepoDump);
            }
            SCTIDRepo Repo = new SCTIDRepo();

            string usedIds = "TestFileDump.txt";
            File.WriteAllText(usedIds, "999991234567121");

            Repo.ImportAllocatedSCTIDs(usedIds);

            string RepoSizeQry = "select count(*) from SCTIDs;";
            int RepoSize;

            using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(RepoSizeQry, cnn))
                {
                    cnn.Open();
                    RepoSize = int.Parse(cmd.ExecuteScalar().ToString());
                }
            }

            Assert.IsTrue(RepoSize > 0);
        }
    }
}
