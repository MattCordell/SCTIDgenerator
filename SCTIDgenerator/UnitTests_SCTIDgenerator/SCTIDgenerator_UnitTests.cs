using Microsoft.VisualStudio.TestTools.UnitTesting;
using SCTIDgenerator_library;
using System;
using System.Collections.Generic;

namespace UnitTests_SCTIDgenerator
{
    [TestClass]
    // COMMENT: BH - Don't like multiple classes in the one file. Separate the below classes into separate files with appropriate names
    public class IsValidNameSpace_UnitTests
    {
        [TestMethod]
        // COMMENT: BH - This is a little confusing, are you testing the Validation of a correct namespace or validating that it returns false
        // Be a little more descriptive with Unit Test names i.e. create two test 1 for valid 1 for invalid ValidateShortNameSpaceWithInvalidNamespace - ValidateShortNameSpaceWithValidNamespace
        // Maybe you are doing this with the Validate7DigitNameSpace method but its still confusing
        // Unit testing names need to be more descriptive so at glance devs know exactly what is being tested
        public void ValidateShortNameSpace()
        {
            SCTIDgenerator IDgenerator = new SCTIDgenerator(123456);
            bool validNameSpace = IDgenerator.IsValidNameSpace();
            Assert.IsFalse(validNameSpace);
        }

        [TestMethod]
        // COMMENT: BH - Same as above - valid / invalid
        public void ValidateLongNameSpace()
        {
            SCTIDgenerator IDgenerator = new SCTIDgenerator(12345678);
            bool validNameSpace = IDgenerator.IsValidNameSpace();
            Assert.IsFalse(validNameSpace);
        }

        [TestMethod]
        public void Validate7DigitNameSpace()
        {
            SCTIDgenerator IDgenerator = new SCTIDgenerator(1234567);
            bool validNameSpace = IDgenerator.IsValidNameSpace();
            Assert.IsTrue(validNameSpace);
        }

        [TestMethod]
        public void ValidateARangeOfNameSpaces()
        {
            // variable for random simulated namespace
            int ns;
            // rnd generates a random ns of required size
            Random rnd = new Random();

            //generate and test namespaces from 1 up to "cap"
            //cap is max int value / 10 (so as not to max out when multiplied by 10)
            //ideally a rand(long,long) would be nice. But this will do for a test
            int cap = int.MaxValue;
            cap = cap / 10;
            for (int i = 1; i < cap; i = i * 10)
            {
                ns = rnd.Next(i, i * 10 - 1);
                ValidateARandomlySizedNameSpace(ns);
                //increment up.
            }
        }

        private void ValidateARandomlySizedNameSpace(int ns)
        {
            SCTIDgenerator IDgenerator = new SCTIDgenerator(ns);
            bool validNameSpace = IDgenerator.IsValidNameSpace();

            //asserts differently depending on ns Size.
            if (ns.ToString().Length < 7)
            {
                Assert.IsFalse(validNameSpace);
            }
            else if (ns.ToString().Length > 7)
            {
                Assert.IsFalse(validNameSpace);
            }
            else
            {
                Assert.IsTrue(validNameSpace);
            }
        }



    }

    [TestClass]
    public class Verhoeff_UnitTests
    {
        [TestMethod]
        //Generate randoms numers, and Generate Verhoeff and self check
        // method loops until one of every possible candidate (i) has been tested, of j reaches 100;
        public void SelfValidateVerhoeffs0through9()
        {
            //random
            Random rnd = new Random();
            string seed = rnd.Next().ToString();

            //list of candidate check digits 0-9
            SortedSet<int> candidates = new SortedSet<int>();
            for (int i = 0; i < 10; i++)
            {
                candidates.Add(i);
            }

            // keep validating up to 1000 times, until each candidate has been tested.
            int j = 0;
            while (j < 1000)
            {
                int generatedCheckDigit = int.Parse(SCTIDgenerator_library.Verhoeff.generateVerhoeff(seed));

                // Remove Checkdigits from the Candidates as they're generated.
                if (candidates.Contains(generatedCheckDigit))
                {
                    candidates.Remove(generatedCheckDigit);
                }
                // Now actually check the Verhoeff against the newly generated value.
                SelfValidateVerhoeff(seed + generatedCheckDigit);

                // break out once all candidates have been tested.
                if (candidates.Count == 0)
                {
                    break;
                }
                j++;
            }
        }


        public void SelfValidateVerhoeff(string c)
        {
            Assert.IsTrue(Verhoeff.validateVerhoeff(c));
        }

        [TestMethod]
        public void ValidateAKnownVerhoeff()
        {
            // known SCTID = 48176007 has check digit 7.
            // all other check digits should fail...
            for (int i = 0; i < 9; i++)
            {
                if (i != 7 && Verhoeff.validateVerhoeff("4817600" + i))
                {
                    Assert.Fail();
                }
            }
            //... But pass when 7
            Assert.IsTrue(Verhoeff.validateVerhoeff("48176007"));
        }

        [TestMethod]
        public void GenerateAKnownVerhoeff()
        {
            //known SCTID = 48176007 has check digit 7.4817600 should generate same check digit
            Assert.IsTrue("7" == Verhoeff.generateVerhoeff("4817600"));
        }
    }

    [TestClass]
    public class Verhoefferise_UnitTests
    {
        [TestMethod]
        public void SelfCheckRandomSeed()
        {
            Random rnd = new Random();
            string seed = rnd.Next(int.MaxValue).ToString();
            Assert.IsTrue(Verhoeff.generateVerhoeff(seed) == Verhoeff.generateVerhoeff(seed));
        }


        [TestMethod]
        public void CheckAgainstKnownList()
        {

            //cycle through ExistingSNOMEDCTIds, checking check didgits
            //ExistingSNOMEDCTIds consist of ConceptIds for all top level concept in Jan 2014, and associate DescriptionIds, and Defining ReflationshipIds
            string[] ExistingSNOMEDCTIds = { "48176007", "71388002", "78621006", "105590001", "123037004", "123038009", "243796009", "254291000", "260787004", "272379006", "308916002", "362981000", "363787002", "370115009", "373873005", "404684003", "410607006", "419891008", "900000000000441003", "80268017", "118588011", "130458013", "169710016", "189056010", "190895018", "291656011", "364629017", "378526013", "388424018", "388425017", "388426016", "388427013", "388428015", "407503013", "452305016", "470725012", "482116013", "486911019", "491692013", "573283013", "645163010", "652584013", "724699017", "724710013", "754754016", "769964015", "785715019", "811548016", "819582012", "1199173018", "1212316016", "1225256013", "1458019012", "2148514019", "2156578010", "2466059019", "2472261015", "2571651013", "2573280016", "2575824015", "2576552011", "2579706018", "2609236017", "2610738018", "2615979011", "2616135016", "108642591000036118", "900000000000951010", "900000000000952015", "144487025", "144519022", "144611029", "145047021", "145312020", "145315022", "146186027", "146315028", "146410021", "146531021", "146535028", "146538026", "1019504021", "1019522024", "1713837029", "2472459022", "2565789025", "2840535024", "3792608028" };
            foreach (var Id in ExistingSNOMEDCTIds)
            {
                if (Verhoeff.validateVerhoeff(Id) != true)
                {
                    Assert.Fail(Id + " failed Verhoeff validation");
                    break;
                }
            }
            Assert.IsFalse(false);
        }


    }


    //Test Class to Valdidate Generated ConceptIds
    [TestClass]
    public class GenerateConceptId_UnitTests
    {
        long ArtificalConceptId = 0;
        int ns;

        [TestMethod]
        //Generate a random ConceptId, that can be used for the remaining tests
        public void GenerateArtificalConceptId()
        {
            Random rnd = new Random();
            ns = rnd.Next(1000000, 9999999);
            SCTIDgenerator IDgenerator = new SCTIDgenerator(ns);
            ArtificalConceptId = IDgenerator.GenerateConceptId();
        }

        [TestMethod]
        public void ArtificalConceptIdGenerated()
        {
            //GenerateArtificalConceptId();
            Random rnd = new Random();
            ns = rnd.Next(1000000, 9999999);
            SCTIDgenerator IDgenerator = new SCTIDgenerator(ns);
            ArtificalConceptId = IDgenerator.GenerateConceptId();

            Assert.AreNotEqual(ArtificalConceptId, 0);  // COMMENT: BH - What should it equal? Anyway of being more specific than just 0, An assert = true is strict compared to a negative condition
        }

        [TestMethod]
        //Generate a Random ID, and check it contains the seed namespace
        public void ConceptIdHasNameSpace()
        {
            GenerateArtificalConceptId();
            int SubstringIndexPosition = ArtificalConceptId.ToString().Length - 10;
            int containedNS = int.Parse(ArtificalConceptId.ToString().Substring(SubstringIndexPosition, 7));
            Assert.IsTrue(ns == containedNS);
        }

        [TestMethod]
        public void ConceptIdHasCheckDigit()
        {
            GenerateArtificalConceptId();
            Assert.IsTrue(Verhoeff.validateVerhoeff(ArtificalConceptId.ToString()));
        }

        [TestMethod]
        public void ConceptIdHasPartitionIdentifierof10()
        {
            GenerateArtificalConceptId();
            int SubstringIndexPosition = ArtificalConceptId.ToString().Length - 3;
            int PartionID = int.Parse(ArtificalConceptId.ToString().Substring(SubstringIndexPosition, 2));
            Assert.IsTrue(PartionID == 10);
        }

        [TestMethod]
        public void ConceptIdAtLeast11DigitsLong()
        {
            GenerateArtificalConceptId();
            Assert.IsTrue(ArtificalConceptId.ToString().Length > 10);
        }

        [TestMethod]
        public void ConceptIdLessThan19DigitsLong()
        {
            GenerateArtificalConceptId();
            Assert.IsTrue(ArtificalConceptId.ToString().Length < 19);
        }

    }


    //Test Class to Valdidate Generated DescriptionIds
    [TestClass]
    public class GenerateDescriptionId_UnitTests
    {
        long ArtificalDescriptionId = 0;
        int ns;

        [TestMethod]
        //Generate a random DescriptionId, that can be used for the remaining tests
        public void GenerateArtificalDescriptionId()
        {
            Random rnd = new Random();
            ns = rnd.Next(1000000, 9999999);
            SCTIDgenerator IDgenerator = new SCTIDgenerator(ns);
            ArtificalDescriptionId = IDgenerator.GenerateDescriptionId();
        }

        [TestMethod]
        public void ArtificalDescriptionIdGenerated()
        {
            GenerateArtificalDescriptionId();
            Assert.AreNotEqual(ArtificalDescriptionId, 0);
        }

        [TestMethod]
        //Generate a Random ID, and check it contains the seed namespace
        public void DescriptionIdHasNameSpace()
        {
            GenerateArtificalDescriptionId();
            int SubstringIndexPosition = ArtificalDescriptionId.ToString().Length - 10;
            int containedNS = int.Parse(ArtificalDescriptionId.ToString().Substring(SubstringIndexPosition, 7));
            Assert.IsTrue(ns == containedNS);
        }

        [TestMethod]
        public void DescriptionIdHasCheckDigit()
        {
            GenerateArtificalDescriptionId();
            Assert.IsTrue(Verhoeff.validateVerhoeff(ArtificalDescriptionId.ToString()));
        }

        [TestMethod]
        public void DescriptionIdHasPartitionIdentifierof11()
        {
            GenerateArtificalDescriptionId();
            int SubstringIndexPosition = ArtificalDescriptionId.ToString().Length - 3;
            int PartionID = int.Parse(ArtificalDescriptionId.ToString().Substring(SubstringIndexPosition, 2));
            Assert.IsTrue(PartionID == 11);
        }

        [TestMethod]
        public void DescriptionIdAtLeast11DigitsLong()
        {
            GenerateArtificalDescriptionId();
            Assert.IsTrue(ArtificalDescriptionId.ToString().Length > 10);
        }

        [TestMethod]
        public void DescriptionIdLessThan19DigitsLong()
        {
            GenerateArtificalDescriptionId();
            Assert.IsTrue(ArtificalDescriptionId.ToString().Length < 19);
        }

    }


    //Test Class to Valdidate Generated RelationshipIds
    [TestClass]
    public class GenerateRelationshipId_UnitTests
    {
        long ArtificalRelationshipId = 0;
        int ns;

        [TestMethod]
        //Generate a random RelationshipId, that can be used for the remaining tests
        public void GenerateArtificalRelationshipId()
        {
            Random rnd = new Random();
            ns = rnd.Next(1000000, 9999999);
            SCTIDgenerator IDgenerator = new SCTIDgenerator(ns);
            ArtificalRelationshipId = IDgenerator.GenerateRelationshipId();
        }

        [TestMethod]
        public void ArtificalRelationshipIdGenerated()
        {
            GenerateArtificalRelationshipId();
            Assert.AreNotEqual(ArtificalRelationshipId, 0);
        }

        [TestMethod]
        //Generate a Random ID, and check it contains the seed namespace
        public void RelationshipIdHasNameSpace()
        {
            GenerateArtificalRelationshipId();
            int SubstringIndexPosition = ArtificalRelationshipId.ToString().Length - 10;
            int containedNS = int.Parse(ArtificalRelationshipId.ToString().Substring(SubstringIndexPosition, 7));
            Assert.IsTrue(ns == containedNS);
        }

        [TestMethod]
        public void RelationshipIdHasCheckDigit()
        {
            GenerateArtificalRelationshipId();
            Assert.IsTrue(Verhoeff.validateVerhoeff(ArtificalRelationshipId.ToString()));
        }

        [TestMethod]
        public void RelationshipIdHasPartitionIdentifierof12()
        {
            GenerateArtificalRelationshipId();
            int SubstringIndexPosition = ArtificalRelationshipId.ToString().Length - 3;
            int PartionID = int.Parse(ArtificalRelationshipId.ToString().Substring(SubstringIndexPosition, 2));
            Assert.IsTrue(PartionID == 12);
        }

        [TestMethod]
        public void RelationshipIdAtLeast11DigitsLong()
        {
            GenerateArtificalRelationshipId();
            Assert.IsTrue(ArtificalRelationshipId.ToString().Length > 10);
        }

        [TestMethod]
        public void RelationshipIdLessThan19DigitsLong()
        {
            GenerateArtificalRelationshipId();
            Assert.IsTrue(ArtificalRelationshipId.ToString().Length < 19);
        }

    }


}
