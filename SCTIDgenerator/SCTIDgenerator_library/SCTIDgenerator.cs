using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace SCTIDgenerator_library
{
    public class SCTIDgenerator
    {
        private int ExtenstionNameSpace;
        private SCTIDRepo Repo;

        // All extensions require a namespace to seed SCTIDgenerator
        public SCTIDgenerator(int ns)
        {
            ExtenstionNameSpace = ns;
            Repo = new SCTIDRepo(ExtenstionNameSpace);
        }

        //Validate the Set Namespace is 7 digits exactly.
        public bool IsValidNameSpace()
        {
            if (ExtenstionNameSpace.ToString().Length == 7)
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
            VerhoefferisedResult = p + Verhoeff.generateVerhoeff(p);
            return long.Parse(VerhoefferisedResult);
        }

        //Generate a ConceptId, based on the intialising NameSpace
        public long GenerateConceptId()
        {
            int foo = Repo.GetNextBean("Concept", ExtenstionNameSpace);
            string generatedIdPart = foo.ToString() + ExtenstionNameSpace.ToString() + "10";

            return Verhoefferise(generatedIdPart);
        }

        //Generate a ConceptId, based on the intialising NameSpace
        public long GenerateDescriptionId()
        {
            int foo = Repo.GetNextBean("Description", ExtenstionNameSpace);
            string generatedIdPart = foo.ToString() + ExtenstionNameSpace.ToString() + "11";

            return Verhoefferise(generatedIdPart);
        }

        //Generate a ConceptId, based on the intialising NameSpace
        public long GenerateRelationshipId()
        {
            int foo = Repo.GetNextBean("Relationship", ExtenstionNameSpace);
            string generatedIdPart = foo.ToString() + ExtenstionNameSpace.ToString() + "12"; // COMMENT: BH - Use stringBuilder instead - http://stackoverflow.com/questions/73883/string-vs-stringbuilder you know this one

            return Verhoefferise(generatedIdPart);
        }
    }
}
