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
            


            Console.ReadKey();
        }
    }
}
