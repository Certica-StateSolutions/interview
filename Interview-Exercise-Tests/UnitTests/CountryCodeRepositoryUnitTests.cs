using Interview_Exercise;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;

namespace InterviewExercise.Tests.UnitTests
{
    [TestFixture]
    public class CountryCodeRepositoryUnitTestsBase : TestFixtureBase
    {
        protected IDictionary<string, Country> _countryDictionary;
        protected ICountryCodeValidator _countryCodeValidator;
        protected IRepository _countryCodeRepository;
        protected Mock<IPersistRepository> _persistRepositoryMock;

        protected override void Arrange()
        {
            _countryDictionary = new Dictionary<string, Country>();
            _countryCodeValidator = new UserDefinedCountryCodeValidator();
            _persistRepositoryMock = new Mock<IPersistRepository>();
            _countryCodeRepository = new CountryCodeRepository(_countryDictionary, _countryCodeValidator, _persistRepositoryMock.Object);
        }
    }

    [TestFixture]
    public class Country_not_added_twice_test : CountryCodeRepositoryUnitTestsBase
    {
        Country _country;
        string _countryCode;

        protected override void Arrange()
        {
            base.Arrange();
            _countryCode = "SAD";
            _country = new Country() { Code = _countryCode, Name = "Smallville" };
            _countryDictionary.Add(_countryCode, _country);
        }

        protected override void Act()
        {
            _countryCodeRepository.Add(_country);
        }

        [Test]
        public void Country_already_exists_exception_should_be_thrown()
        {
            AssertAll(
                () => Assert.That(_countryDictionary.Keys.Contains(_countryCode)),
                () => Assert.That(ActualException.Message, Is.Not.Null),
                () => Assert.That(ActualException.Message, Is.EqualTo(string.Format("Country already exists. Country Code: {0}, Country Name: {1}", _countryCode, _country.Name))));
        }
    }

    [TestFixture]
    public class Country_with_missing_name_test : CountryCodeRepositoryUnitTestsBase
    {
        Country _country;
        string _countryCode;

        protected override void Arrange()
        {
            base.Arrange();
            _countryCode = "SAD";
            _country = new Country() { Code = _countryCode };
        }

        protected override void Act()
        {
            _countryCodeRepository.Add(_country);
        }

        [Test]
        public void Country_name_is_invalid_exception_should_be_thrown()
        {
            AssertAll(
                () => Assert.That(ActualException.Message, Is.Not.Null),
                () => Assert.That(ActualException.Message, Is.EqualTo("Country name is null or empty.")));
        }
    }

    [TestFixture]
    public class Add_country_successfully_test : CountryCodeRepositoryUnitTestsBase
    {
        Country _country;
        string _countryCode;

        protected override void Arrange()
        {
            base.Arrange();
            _countryCode = "YAY";
            _country = new Country() { Code = _countryCode, Name = "Q-Continuum" };
        }

        protected override void Act()
        {
            _countryCodeRepository.Add(_country);
        }

        [Test]
        public void Dictionary_should_contain_key_for_code()
        {
            Assert.That(_countryDictionary.Keys.Contains(_countryCode));
        }
    }

    [TestFixture]
    public class Clear_the_repository_test : CountryCodeRepositoryUnitTestsBase
    {
        Country _country;

        protected override void Arrange()
        {
            base.Arrange();
            _country = new Country() { Code = "LOL" };
            _countryDictionary.Add(_country.Code, _country);

            _country = new Country() { Code = "YAY" };
            _countryDictionary.Add(_country.Code, _country);

            _country = new Country() { Code = "HUH" };
            _countryDictionary.Add(_country.Code, _country);
        }

        protected override void Act()
        {
            _countryCodeRepository.Clear();
        }

        [Test]
        public void Dictionary_should_be_empty()
        {
            Assert.That(_countryDictionary.Count == 0);
        }
    }

    [TestFixture]
    public class Delete_existing_country_test : CountryCodeRepositoryUnitTestsBase
    {
        Country _country;
        string _countryCode;

        protected override void Arrange()
        {
            base.Arrange();
            _countryCode = "OUI";
            _country = new Country() { Code = _countryCode };
            _countryDictionary.Add(_countryCode, _country);
        }

        protected override void Act()
        {
            _countryCodeRepository.Delete(_countryCode);
        }

        [Test]
        public void Dictionary_should_not_contain_key_for_code()
        {
            Assert.That(_countryDictionary.Keys.Contains(_countryCode) == false);
        }
    }

    [TestFixture]
    public class Attempt_to_delete_country_that_does_not_exist_test : CountryCodeRepositoryUnitTestsBase
    {
        string _countryCode;

        protected override void Arrange()
        {
            base.Arrange();
            _countryCode = "NOT";
        }

        protected override void Act()
        {
            _countryCodeRepository.Delete(_countryCode);
        }

        [Test]
        public void Country_not_found_exception_should_be_thrown()
        {
            AssertAll(
                () => Assert.That(ActualException.Message, Is.Not.Null),
                () => Assert.That(ActualException.Message, Is.EqualTo(string.Format("Country cannot be found. Country Code: {0}", _countryCode))));
        }
    }

    [TestFixture]
    public class Update_country_successfully_test : CountryCodeRepositoryUnitTestsBase
    {
        Country _countryOld;
        Country _countryNew;
        string _countryCode;
        string _oldName;
        string _newName;

        protected override void Arrange()
        {
            base.Arrange();
            _countryCode = "BOO";
            _oldName = "OLD";
            _newName = "NEW";
            _countryOld = new Country() { Code = _countryCode, Name = _oldName };
            _countryNew = new Country() { Code = _countryCode, Name = _newName };
            _countryDictionary.Add(_countryCode, _countryOld);
        }

        protected override void Act()
        {
            _countryCodeRepository.Update(_countryNew);
        }

        [Test]
        public void Dictionary_should_have_new_name_for_code_key()
        {
            AssertAll(
                () => Assert.That(_countryDictionary.Keys.Contains(_countryCode)),
                () => Assert.That(_countryDictionary[_countryCode], Is.Not.Null),
                () => Assert.That(_countryDictionary[_countryCode].Name, Is.Not.Null),
                () => Assert.That(_countryDictionary[_countryCode].Name, Is.EqualTo(_newName)));
        }
    }



    [TestFixture]
    public class Update_with_missing_name_test : CountryCodeRepositoryUnitTestsBase
    {
        Country _country;
        string _countryCode;

        protected override void Arrange()
        {
            base.Arrange();
            _countryCode = "BOO";
            _country = new Country() { Code = _countryCode };
        }

        protected override void Act()
        {
            _countryCodeRepository.Update(_country);
        }

        [Test]
        public void Country_name_is_invalid_exception_should_be_thrown()
        {
            AssertAll(
                () => Assert.That(ActualException.Message, Is.Not.Null),
                () => Assert.That(ActualException.Message, Is.EqualTo("Country name is null or empty.")));
        }
    }

    [TestFixture]
    public class Attempt_to_update_noexistant_country_test : CountryCodeRepositoryUnitTestsBase
    {
        Country _country;
        string _countryCode;

        protected override void Arrange()
        {
            base.Arrange();
            _countryCode = "BOO";
            _country = new Country() { Code = _countryCode, Name = "Earth2" };
        }

        protected override void Act()
        {
            _countryCodeRepository.Update(_country);
        }

        [Test]
        public void Country_not_found_exception_should_be_thrown()
        {
            AssertAll(
                () => Assert.That(ActualException.Message, Is.Not.Null),
                () => Assert.That(ActualException.Message, Is.EqualTo(string.Format("Country cannot be found. Country Code: {0}", _countryCode))));
        }
    }
}
