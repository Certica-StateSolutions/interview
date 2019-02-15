using System;
using System.Linq;

namespace Interview_Exercise
{
    public class Country
    {
        private string code;
        public string Name;

        public string Code
        {
            get
            {
                return this.code;
            }
            private set
            {
                if (value != null && value.Length == 3)
                {
                    //enforce upper case only
                    value = value.ToUpper();

                    if (!value.All(char.IsLetter))
                    {
                        throw new Exception("Country Code can only be letters.");
                    }

                    //check for user defined codes (AAA to AAZ, QMA to QZZ, XAA to XZZ, and ZZA to ZZZ)
                    if (value.StartsWith("AA") || // AAA to AAZ
                        value[0] == 'X' || // XAA to XZZ
                        value.StartsWith("ZZ") || //ZZA to ZZZ
                        (value[0] == 'Q' && value[1] >= 'M')) //QMA to QZZ
                    {
                        //throw exception
                        throw new Exception("User-assigned Country Code is not a valid value.");
                    }

                    this.code = value;
                }
                else
                {
                    throw new Exception("Country code cannot be null and must be exactly three (3) characters.");
                }
            }
        }

        public Country(string Code, string Name)
        {
            this.Code = Code;
            this.Name = Name;
        }
    }
}