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
            int ns = 1223221;
            SCTIDgenerator foo = new SCTIDgenerator(ns);
            SCTIDRepo Repo = new SCTIDRepo(ns);

            Console.WriteLine("NameSpace =" + ns.ToString());


            string bar = foo.GenerateConceptId().ToString();
            Console.WriteLine("ConceptId =" + bar);
            Repo.ReserveId(bar);
            bar = foo.GenerateConceptId().ToString();
            Console.WriteLine("ConceptId =" + bar);
            Repo.ReserveId(bar);
            bar = foo.GenerateConceptId().ToString();
            Console.WriteLine("ConceptId =" + bar);
            Repo.ReserveId(bar);
            
            Console.WriteLine();
            bar = foo.GenerateDescriptionId().ToString();
            Console.WriteLine("DescriptionId =" + bar);
            Repo.ReserveId(bar);
            
            Console.WriteLine();
            bar = foo.GenerateRelationshipId().ToString();
            Console.WriteLine("RelationshipId =" + bar);
            Repo.ReserveId(bar);
            
            string codeString  = "0123456789";
   
            string beep = codeString.Substring(codeString.Length - 6, 3);
            Console.WriteLine("This should be 456 : " + beep);
            Console.WriteLine("48176007 has check digit = " + Verhoeff.generateVerhoeff("4817600"));

            Repo.DumpRepository();
            Console.WriteLine("Repo Dumped.");

            Console.ReadKey();
        }


    }
}
