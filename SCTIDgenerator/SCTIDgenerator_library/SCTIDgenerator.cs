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

        //All extensions require a namespace to seed SCTIDgenerator
        public SCTIDgenerator(int ns)
        {
            ExtenstionNameSpace = ns;
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
        public long Verhoefferise(string p)
        {
            string VerhoefferisedResult;
            VerhoefferisedResult = Verhoeff.generateVerhoeff(p);
            return long.Parse(VerhoefferisedResult);
        }

        //Generate a ConceptId. Ultimately needs to validate against some repo of used Ids
        public long GenerateConceptId()
        {
            Random rnd = new Random();
            int foo = rnd.Next(1,99999999);
            string generatedIdPart = foo.ToString() + ExtenstionNameSpace.ToString() + "10";
            return Verhoefferise(generatedIdPart);
        }

        //Generate a ConceptId. Ultimately needs to validate against some repo of used Ids
        public long GenerateDescriptionId()
        {
            Random rnd = new Random();
            int foo = rnd.Next(1, 99999999);
            string generatedIdPart = foo.ToString() + ExtenstionNameSpace.ToString() + "11";
            return Verhoefferise(generatedIdPart);
        }

        //Generate a ConceptId. Ultimately needs to validate against some repo of used Ids
        public long GenerateRelationshipId()
        {
            Random rnd = new Random();
            int foo = rnd.Next(1, 99999999);
            string generatedIdPart = foo.ToString() + ExtenstionNameSpace.ToString() + "12";
            return Verhoefferise(generatedIdPart);
        }
    }
}
