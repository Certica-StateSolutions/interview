using System.Linq;

namespace InterviewExercise
{
    public class UserDefinedCountryCodeValidator : ICountryCodeValidator
    {
        private static string[] _userCodeStart = new string[] { "AAA", "QMA", "XAA", "ZZA" };
        private static string[] _userCodeEnd = new string[] { "AAZ", "QZZ", "XZZ", "ZZZ" };

        bool ICountryCodeValidator.IsCountryCodeValid(string code)
        {
            if (string.IsNullOrEmpty(code)) return false;
            if (code.Length != 3) return false;
            if (IsCodeAllLetters(code) == false) return false;


            string codeAsUpperCase = code.ToUpper();
            return false ==
                (IsInUserCodeRange(_userCodeStart[0], _userCodeEnd[0], codeAsUpperCase) ||
                IsInUserCodeRange(_userCodeStart[1], _userCodeEnd[1], codeAsUpperCase) ||
                IsInUserCodeRange(_userCodeStart[2], _userCodeEnd[2], codeAsUpperCase) ||
                IsInUserCodeRange(_userCodeStart[3], _userCodeEnd[3], codeAsUpperCase));
        }

        private static bool IsCodeAllLetters(string code)
        {
            return code.All(x => char.IsLetter(x) == true);
        }

        private static bool IsLetterInAsciiRange(char start, char end, char value)
        {
            return (byte)start <= value && (byte)end >= value;
        }

        private static bool IsInUserCodeRange(string userCodeStart, string userCodeEnd, string code)
        {
            for (int i = 0; i < userCodeStart.Length; ++i)
            {
                if (IsLetterInAsciiRange(userCodeStart[i], userCodeEnd[i], code[i]) == false) return false;
            }

            return true;
        }
    }
}
