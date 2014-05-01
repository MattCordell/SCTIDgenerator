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



        public long Verhoefferise(long p)
        {
            throw new NotImplementedException();
        }

        public long GenerateConceptId()
        {
            throw new NotImplementedException();
        }
    }
}
