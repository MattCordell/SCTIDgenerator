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
            private string connectionString = "data source=" + RepoFile + ";Version=3;Cache Size=2000;Page Size=32768;Synchronous=OFF;Journal Mode=WAL;";

            //Constructor will set the current Directory to launch directory, and make sure a DB exists
            public SCTIDRepo()
            {
                System.IO.Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

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
                                                        CREATE INDEX IF NOT EXISTS ix_SCTIDs_IdType ON SCTIDs(IdType);
                                                        INSERT INTO SCTIDs VALUES (01234567101,0,1234567,'Concept');
                                                        INSERT INTO SCTIDs VALUES (01234567111,0,1234567,'Description');
                                                        INSERT INTO SCTIDs VALUES (01234567121,0,1234567,'Relationship');";
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
            }

            public int GetNextBean(string p)
            {
                string MaxBeanQuery = "select max(Bean) from SCTIDs where IdType = @IDType;";

                using (SQLiteConnection cnn = new SQLiteConnection(connectionString))
                {
                    using (SQLiteCommand cmd = new SQLiteCommand(MaxBeanQuery,cnn))
                    {
                        cmd.Parameters.AddWithValue("@IDType", p);
                        cnn.Open();
                        var MaxCurrentBean = cmd.ExecuteScalar();                        
                        
                        int NextBean = int.Parse(MaxCurrentBean.ToString()) + 1;
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
                        cmd.Parameters.AddWithValue("@Id",id);
                        cmd.Parameters.AddWithValue("@Bean",bean);
                        cmd.Parameters.AddWithValue("@NameSpace",ns);
                        cmd.Parameters.AddWithValue("@IDType",IDType);
                        
                        cnn.Open();
                        cmd.ExecuteNonQuery();                        
                    }
                }
            }

        }
}
