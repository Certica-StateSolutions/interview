using NUnit.Framework;

namespace InterviewExercise.Tests.UnitTests
{
    [TestFixture]
    class InvalidCountryCodeTest : TestFixtureBase
    {
        private bool _actualResult;
        private ICountryCodeValidator iCountryCodeValidator;

        protected override void Arrange()
        {
            iCountryCodeValidator = new UserDefinedCountryCodeValidator();
        }

        protected override void Act()
        {
            _actualResult = iCountryCodeValidator.IsCountryCodeValid("AAZ");
        }

        [Test]
        public void Result_is_false()
        {
            Assert.That(_actualResult, Is.False);
        }
    }

    class ValidCountryCodeTest : TestFixtureBase
    {
        private bool _actualResult;
        private ICountryCodeValidator iCountryCodeValidator;

        protected override void Arrange()
        {
            iCountryCodeValidator = new UserDefinedCountryCodeValidator();
        }

        protected override void Act()
        {
            _actualResult = iCountryCodeValidator.IsCountryCodeValid("BBZ");
        }

        [Test]
        public void Result_is_true()
        {
            Assert.That(_actualResult, Is.True);
        }
    }
}
