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
        private const string RepoDump = "SCTID_Repository_Dump.txt";       

        [TestMethod]
        public void RepoIsCreatedSuccessfuly()
        {
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            if (File.Exists(RepoFile))
            {
                File.Delete(RepoFile);
            }
            SCTIDRepo Repo = new SCTIDRepo(1234567);
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
            SCTIDRepo Repo = new SCTIDRepo(1234567);

            Repo.ReserveId("88888881234567101");
            int NextBean = Repo.GetNextBean("Concept",1234567);

            Assert.AreEqual(8888889, NextBean);           
        }

        [TestMethod]
        public void AbleToReserveConceptId()
        {
            var ID = "11234567100";
            SCTIDRepo Repo = new SCTIDRepo(1234567);
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
            SCTIDRepo Repo = new SCTIDRepo(1234567);

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
            SCTIDRepo Repo = new SCTIDRepo(1234567);

            string usedIds = "TestFileDump.txt";
            Random foo = new Random();
            int RandomBean = foo.Next(999999);
            
            //write generate a random bean, and write a bogus ID to the file.
            File.WriteAllText(usedIds, RandomBean + "1234567121");
            
            //import that file
            Repo.ImportAllocatedSCTIDs(usedIds);
            //and get the next been..           
            //which should be 1 more than the random bean inserted above.
            Assert.IsTrue(Repo.GetNextBean("Relationship", 1234567) == RandomBean+1);
        }
    }
}
