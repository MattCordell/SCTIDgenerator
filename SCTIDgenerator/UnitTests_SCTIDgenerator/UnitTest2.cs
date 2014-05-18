using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using SCTIDgenerator_library;

namespace UnitTests_SCTIDgenerator
{
    //Repo creator
    //repo reader
    //repo writer

    [TestClass]
    public class UnitTest2
    {
        //Repo database file. Must be same as in class.        
        private const string RepoFile = "SCTIDRepo.db";                                  

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
            int NextBean = Repo.GetNextBean("Concept");

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
    }
}
