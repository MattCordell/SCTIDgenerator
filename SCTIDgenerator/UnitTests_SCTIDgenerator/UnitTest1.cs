﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SCTIDgenerator_library;
using System.Collections.Generic;

namespace UnitTests_SCTIDgenerator
{
    [TestClass]
    public class IsValidNameSpace_UnitTests
    {
        [TestMethod]
        public void ValidateShortNameSpace()
        {
            SCTIDgenerator IDgenerator = new SCTIDgenerator(123456);
            bool validNameSpace = IDgenerator.IsValidNameSpace();
            Assert.IsFalse(validNameSpace);
        }

        [TestMethod]
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
            cap = cap/10;
            for (int i = 1; i < cap; i = i * 10)
            {
                ns = rnd.Next(i,i*10-1);
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

                //Remove Checkdigits from the Candidates as they're generated.
                if (candidates.Contains(generatedCheckDigit))
                {
                    candidates.Remove(generatedCheckDigit);
                }
                // Now actually check the Verhoeff against the newly generated value.
                SelfValidateVerhoeff(seed + generatedCheckDigit);
                
                //break out once all candidates have been tested.
                if (candidates.Count == 0)
                {
                    break;
                }
                j++;
            }
        }

        
        public void SelfValidateVerhoeff(string c)
        {            
            Assert.IsTrue(SCTIDgenerator_library.Verhoeff.validateVerhoeff(c));
        }

        [TestMethod]
        public void ValidateAKnownVerhoeff()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void GenerateAKnownVerhoeff()
        {
            Assert.IsTrue(false);
        }
    }

    [TestClass]
    public class Verhoefferise_UnitTests
    {
        [TestMethod]
        public void SelfCheckRandomSeed()
        {
            Random rnd = new Random();
            int seed = rnd.Next(int.MaxValue);
            CheckFedInteger(seed);
        }

        private void CheckFedInteger(int seed)
        {

            long hoffedValue;
            SCTIDgenerator IDgenerator = new SCTIDgenerator(seed);

            //first crack at generating a checkdigit for the seed
            hoffedValue = IDgenerator.Verhoefferise(long.Parse(seed.ToString()));
            char firstPass = hoffedValue.ToString()[hoffedValue.ToString().Length - 1];
            //second crack at generating a checkdigit for the seed
            hoffedValue = IDgenerator.Verhoefferise(long.Parse(seed.ToString()));
            char secondPass = hoffedValue.ToString()[hoffedValue.ToString().Length - 1];

            //the two check digits generated should be identical.
            Assert.Equals(firstPass, secondPass);
        }

        [TestMethod]
        public void CheckAgainstKnownList()
        {
            //populate array with top level SNOMED CTconcepts.
            //cycle through, generating check didgits
            Assert.IsTrue(false);
        }

        
    }

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
            int ns = rnd.Next(1000000, 9999999);
            SCTIDgenerator IDgenerator = new SCTIDgenerator(ns);
            ArtificalConceptId = IDgenerator.GenerateConceptId();
            Assert.AreNotEqual(ArtificalConceptId, 0);
        }

        [TestMethod]
        public void ConceptIdHasNameSpace()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void ConceptIdHasCheckDigit()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void ConceptIdHasPartitionIdentifierof10()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void ConceptIdAtLeast11DigitsLong()
        {
            Assert.IsTrue(false);
        }

        [TestMethod]
        public void ConceptIdLessThan19DigitsLong()
        {
            Assert.IsTrue(false);
        }
    
    }

}