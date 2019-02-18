using System;

namespace InterviewExercise
{
    public class CountryNameIsInvalidException : Exception
    {
        public CountryNameIsInvalidException() : base("Country name is null or empty.") { }
    }
}
