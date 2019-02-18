using Interview_Exercise;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterviewExercise
{
    public class UserDefinedCountryCodeException : Exception
    {
        public UserDefinedCountryCodeException() { }

        public UserDefinedCountryCodeException(string countryCode) : base(string.Format("User defined country code is not supported: {0}", countryCode)) { }

        public UserDefinedCountryCodeException(Country country) : base(string.Format("User defined country code is not supported: {0}", country.Code)) { }
    }
}
