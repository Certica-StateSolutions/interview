using Interview_Exercise;
using System;

namespace InterviewExercise
{
    public class CountryNotFoundException : Exception
    {
        public CountryNotFoundException() { }
        public CountryNotFoundException(string countryCode) : base(string.Format("Country cannot be found. Country Code: {0}", countryCode)) { }
    }
}
