using Interview_Exercise;
using NUnit.Framework;

namespace InterviewExercise.Tests.UnitTests
{
    [TestFixture]
    class CountryCodeTests : TestFixtureBase
    {
        private Country _country;
        private string _expectedResult;

        protected override void Arrange()
        {
            _country = new Country();
            _expectedResult = "ABC";
        }

        protected override void Act()
        {
            _country.Code = "AbC";
        }

        [Test]
        public void Code_is_converted_to_upper_case()
        {
            Assert.That(_country.Code == _expectedResult);
        }
    }
}
