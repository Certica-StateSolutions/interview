using Interview_Exercise;
using System;
using System.Collections.Generic;
using System.Text;

namespace InterviewExercise
{
    public class CountryAlreadyExistsException : Exception
    {
        public CountryAlreadyExistsException() { }

        public CountryAlreadyExistsException(Country country) : base(string.Format("Country already exists. Country Code: {0}, Country Name: {1}", country.Code, country.Name)) { }
    }
}
