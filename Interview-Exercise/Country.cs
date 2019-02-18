using InterviewExercise;

namespace Interview_Exercise
{
    public class Country
    {
        string _code;

        public string Code
        {
            get
            {
                return _code;
            }

            set
            {
                if (string.IsNullOrEmpty(value) == false)
                {
                    _code = value.ToUpper();
                }
            }
        }

        public string Name { get; set; }

        public Country()
        {
            _code = string.Empty;
            Name = string.Empty;
        }

        public Country(string code, string name)
        {
            Code = code;
            Name = name;
        }
    }
}