using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using System.Data.SQLite;

namespace SCTIDgenerator_library
{

    //SCTIDRepo is really simple database repo for storing assigned SCTIDs.
    //DB consists of a single table structured: SCTID,Bean,NameSpace,IdType
    //SCTID = an issued ID
    //Bean = is effectively a counter used to generate the ID
    //NameSpace = NameSpace used to generate ID. So Single database can be reused for different DBs.
    //IdType = for filtering Concept/Description/Relationship Ids


    public class SCTIDRepo
    {
        private const string RepoFile = "SCTIDRepo.db";
        private string connectionString = "data source=" + RepoFile + ";Version=3;Cache Size=2000;Page Size=32768;Synchronous=OFF;Journal Mode=WAL;";  // COMMENT: BH - inconsistant casing of global scoped variables this is lower case - next line is upper case
        private string RepDumpFile = "SCTID_Repository_Dump.txt"; // COMMENT: BH - consistantly case similar scoped variables

        //Constructor will set the current Directory to launch directory, and make sure a DB exists
        public SCTIDRepo(int initialiserNameSpace)
        {
            System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            // COMMENT: BH - Could break apart this method (cyclomatic complexity) its a bit large. First part is the "Check if exists" then create - following part is Init.

            //If the SCTIDRepo.db doesn't exist, attempt to create one.
            if (!File.Exists(RepoFile))
            {
                try
                {
                    SQLiteConnection.CreateFile(RepoFile);
                    const string CreateTableSchema = @"DROP TABLE IF EXISTS SCTIDs;
                                                       CREATE TABLE SCTIDs (SCTID LONG,Bean INTEGER,NameSpace INTEGER,IdType TEXT);
                                                        CREATE INDEX IF NOT EXISTS ix_SCTIDs_SCTID ON SCTIDs(SCTID);
                                                        CREATE INDEX IF NOT EXISTS ix_SCTIDs_Bean ON SCTIDs(Bean);
                                                        CREATE INDEX IF NOT EXISTS ix_SCTIDs_NameSpace ON SCTIDs(NameSpace);
                                                        CREATE INDEX IF NOT EXISTS ix_SCTIDs_IdType ON SCTIDs(IdType);"; // COMMENT: BH - as above consistantly case similar scoped variables. Inside a method should probably start with Lower case
                    using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
                    {
                        using (SQLiteCommand cmd = new SQLiteCommand(CreateTableSchema, cnn))
                        {
                            cnn.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("{0} Problem setting up the database:", e);
                }
            }

            //Check if the DB has been initialised for the given namesapce.
            //(initialising involves seeding with 3 SCTIDs having a Bean of 0

            string CheckIfInitialisedForNameSpace = "select count(*) from SCTIDs where NameSpace = @NameSpace";
            using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(CheckIfInitialisedForNameSpace, cnn))
                {
                    cmd.Parameters.AddWithValue("@NameSpace", initialiserNameSpace);
                    cnn.Open();
                    var NumberOfEntriesForNameSpace = int.Parse(cmd.ExecuteScalar().ToString());

                    //There should at least 3. More if the db has been used.
                    if (NumberOfEntriesForNameSpace < 3)
                    {

                        //initialise with same base 1 SCTIDs for the given namespace
                        ReserveId("1" + initialiserNameSpace + "10" + Verhoeff.generateVerhoeff("1" + initialiserNameSpace + "10"));
                        ReserveId("1" + initialiserNameSpace + "11" + Verhoeff.generateVerhoeff("1" + initialiserNameSpace + "11"));
                        ReserveId("1" + initialiserNameSpace + "12" + Verhoeff.generateVerhoeff("1" + initialiserNameSpace + "12"));
                    }
                }
            }


        }

        //'Beans' are the bit before a namespace, kinda like a counter for the SCTIDs.
        //This method returns the next 'bean' available for a given namespace and idType
        public int GetNextBean(string idtype, int ns)
        {
            string MaxBeanQuery = "select max(Bean) from SCTIDs where IdType = @IDType and NameSpace = @NameSpace;";
            using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(MaxBeanQuery, cnn))
                {
                    cmd.Parameters.AddWithValue("@IDType", idtype);
                    cmd.Parameters.AddWithValue("@NameSpace", ns);
                    cnn.Open();
                    var MaxCurrentBean = cmd.ExecuteScalar().ToString();

                    int NextBean = int.Parse(MaxCurrentBean) + 1;
                    return NextBean;
                }
            }
        }

        //takes an ID and inserts into the repo. Bean,Namespace, and IDtype Extracted from id
        public void ReserveId(string id)
        {
            int len = id.Length;
            //Checkdigit is the 2nd+3rd last two digits
            var idTypeCode = id.Substring(len - 3, 2);
            //namespace is the 7 digits, before the last 3.
            var ns = id.Substring(len - 10, 7);
            
            //bean is everything else before the first 10.
            string bean = id.Substring(0, len - 10);
            string IDType = null;

            //idTypeCode can be 10,11,12
            switch (idTypeCode)
            {
                case "10": IDType = "Concept";
                    break;
                case "11": IDType = "Description";
                    break;
                case "12": IDType = "Relationship";
                    break;
            }

            //query to update Repo
            string MaxBeanQuery = "INSERT INTO SCTIDs VALUES (@Id,@Bean,@NameSpace,@IDType);";

            using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(MaxBeanQuery, cnn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Bean", bean);
                    cmd.Parameters.AddWithValue("@NameSpace", ns);
                    cmd.Parameters.AddWithValue("@IDType", IDType);

                    cnn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
        }

        //Dump the Repo, for whatever reason;
        public void DumpRepository()
        {
            var getRepoContents = "select SCTID from SCTIDs;";

            using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
            {
                using (SQLiteCommand cmd = new SQLiteCommand(getRepoContents, cnn))
                {
                    cnn.Open();
                    using (SQLiteDataReader RepoContents = cmd.ExecuteReader())
                    {
                        System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
                        using (System.IO.StreamWriter file = new System.IO.StreamWriter(RepDumpFile, false))
                        {
                            while (RepoContents.Read())
                            {
                                file.WriteLine(RepoContents.GetValue(0).ToString());
                            }
                        }
                    }
                }
            }
        }


        //method for Importing a list of SCTIds previously assigned, into the Repo
        public void ImportAllocatedSCTIDs(string AllocatedIdFiles)
        {
            var UsedIds = File.ReadAllLines(AllocatedIdFiles);

            foreach (var id in UsedIds)
            {
                ReserveId(id);
            }
        }


    }
}
