
using InterviewExercise;
using System.Collections.Generic;

namespace Interview_Exercise
{
    public class CountryCodeRepository : IRepository
    {
        IDictionary<string, Country> _countryDictionary;
        ICountryCodeValidator _countryCodeValidator;
        IPersistRepository _persistRepository;

        public CountryCodeRepository(IDictionary<string, Country> dictionary, ICountryCodeValidator codeValidator, IPersistRepository persistRepository)
        {
            _countryDictionary = dictionary;
            _countryCodeValidator = codeValidator;
            _persistRepository = persistRepository;
        }

        void IRepository.Add(Country country)
        {
            if (_countryCodeValidator.IsCountryCodeValid(country.Code) == true)
            {
                if (string.IsNullOrEmpty(country.Name))
                    throw new CountryNameIsInvalidException();
                else if (_countryDictionary.Keys.Contains(country.Code) == false)
                {
                    _persistRepository.Add(country);
                    _countryDictionary.Add(country.Code, country);
                }
                else throw new CountryAlreadyExistsException(country);
            }
            else throw new UserDefinedCountryCodeException(country);
        }

        void IRepository.Clear()
        {
            _persistRepository.Clear();
            _countryDictionary.Clear();
        }

        void IRepository.Delete(string countryCode)
        {
            if (_countryDictionary.Keys.Contains(countryCode) == true)
            {
                _persistRepository.Delete(countryCode);
                _countryDictionary.Remove(countryCode);
            }
            else throw new CountryNotFoundException(countryCode);
        }

        Country IRepository.Get(string countryCode)
        {
            if (_countryDictionary.Keys.Contains(countryCode) == true)
                return _countryDictionary[countryCode];
            else throw new CountryNotFoundException(countryCode);
        }

        void IRepository.Update(Country country)
        {
            if (string.IsNullOrEmpty(country.Name))
                throw new CountryNameIsInvalidException();
            else if (_countryDictionary.Keys.Contains(country.Code) == true)
            {
                _persistRepository.Update(country);
                _countryDictionary[country.Code] = country;
            }
            else throw new CountryNotFoundException(country.Code);
        }
    }
}
