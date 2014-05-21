using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SCTIDgenerator_library;

namespace Sample_Application_SCTIDgenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            int ns = 1234567;
            SCTIDgenerator foo = new SCTIDgenerator(ns);
            Console.WriteLine("NameSpace =" + ns.ToString());
            Console.WriteLine("ConceptId ="+foo.GenerateConceptId());
            Console.WriteLine("DescriptionId =" + foo.GenerateDescriptionId());
            Console.WriteLine("RelationshipId =" + foo.GenerateRelationshipId());
            
            string codeString  = "0123456789";
   
            string beep = codeString.Substring(codeString.Length - 6, 3);
            Console.WriteLine("This should be 456 : " + beep);
            Console.WriteLine("48176007 has check digit = " + Verhoeff.generateVerhoeff("4817600"));

            SCTIDRepo Repo = new SCTIDRepo();
            Repo.ReserveId("100001234567101");
            Repo.ReserveId("110001234567111");
            Repo.ReserveId("120001234567121");

            Repo.DumpRepository();
            Console.WriteLine("Repo Dumped.");

            Console.ReadKey();
        }


    }
}
