using System;
using Moq;
using NUnit.Framework;

using Interview_Exercise;

namespace InterviewExercise.Tests.UnitTests
{
    [TestFixture]
    public class CountryTests
    {
        public class CountryCodeTest : TestFixtureBase
        {
            [Test]
            public void TestCountryCodeCase()
            {
                Country testCountry = new Country("asd", "Test Country");

                Assert.AreEqual(testCountry.Code, "ASD");
            }
        }
    }
}
