using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SCTIdGeneratorLibrary
{
    public class SCTIdGenerator
    {
        private int Namespace;
        private SCTIdRepository Repo;

        // All extensions require a namespace to seed SCTIDgenerator
        public SCTIdGenerator(int ns)
        {
            Namespace = ns;
            Repo = new SCTIdRepository(Namespace);
        }

        //Validate the Set Namespace is 7 digits exactly.
        public bool IsValidNamespace()
        {
            if (Namespace.ToString().Length == 7)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //takes string p, and appends a check digit., and converts it to a long
        //p will be a cocatentation of rnd+namespace+partitionId
        internal long Verhoefferise(string p)
        {
            string VerhoefferisedResult;
            VerhoefferisedResult = p + Verhoeff.GenerateVerhoeff(p);
            return long.Parse(VerhoefferisedResult);
        }

        //Generate a ConceptId, based on the intialising NameSpace
        public long GenerateConceptId()
        {
            int foo = Repo.GetNextBean("Concept", Namespace);
            string generatedIdPart = foo.ToString() + Namespace.ToString() + "10";

            return Verhoefferise(generatedIdPart);
        }

        //Generate a ConceptId, based on the intialising NameSpace
        public long GenerateDescriptionId()
        {
            int foo = Repo.GetNextBean("Description", Namespace);
            string generatedIdPart = foo.ToString() + Namespace.ToString() + "11";

            return Verhoefferise(generatedIdPart);
        }

        //Generate a ConceptId, based on the intialising NameSpace
        public long GenerateRelationshipId()
        {
            int foo = Repo.GetNextBean("Relationship", Namespace);
            string generatedIdPart = foo.ToString() + Namespace.ToString() + "12"; // COMMENT: BH - Use stringBuilder instead - http://stackoverflow.com/questions/73883/string-vs-stringbuilder you know this one

            return Verhoefferise(generatedIdPart);
        }
    }
}
