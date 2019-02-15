using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;


namespace Interview_Exercise
{
    public class CountryCodeRepository
    {
        private static readonly string dataPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
           "Interview-Exercise");

        //collection to hold Country objects in memory
        private Dictionary<string, Country> countries = new Dictionary<string, Country>();

        public int Count { get { return countries.Count; } private set { } }

        public CountryCodeRepository()
        {
            //create data directory if it does not already exist.
            if (!Directory.Exists(dataPath))
            {
                Directory.CreateDirectory(dataPath);
            }

            //enumerate current stored data files and deserialize
            IEnumerable<string> dataFiles = Directory.EnumerateFiles(dataPath, "*.csv");
            foreach (string csvFile in dataFiles)
            {
                string[] currentCountries = File.ReadAllLines(csvFile);

                foreach (string countryLine in currentCountries)
                {
                    string[] countryData = countryLine.Split(',');
                    
                    if (countryData.Length == 2)
                    {
                        try
                        {
                            Country newCountry = new Country(countryData[0], countryData[1]);
                            countries.Add(newCountry.Code, newCountry);
                        }
                        catch (Exception e)
                        {
                            //log error rather than throw exception and halt execution
                            Console.WriteLine(string.Format("Invalid Country data found in {0}: {1}", csvFile, e.ToString()));
                        }
                    }
                    else
                    {
                        //bad data, log error
                        Console.WriteLine(string.Format("Invalid Country data found in {0}, not enough parameters to deserialize", csvFile));
                    }
                }
            }
        }
        
        public void Add(Country country)
        {
            if (!countries.ContainsKey(country.Code))
            {
                countries.Add(country.Code, country);
                UpdateStorage(country);
            }
            else
            {
                throw new Exception("Country already exists in current repository");
            }
        }
        
        public void Update(Country country)
        {
            if (countries.ContainsKey(country.Code))
            {
                countries[country.Code] = country;
                UpdateStorage(country);
            }
            else
            {
                throw new Exception("Country does not exist in current repository");
            }
        }
        
        public void Delete(string countryCode)
        {
            string upperCode = countryCode.ToUpper();
            if (countries.ContainsKey(upperCode))
            {
                UpdateStorage(countries[upperCode]);
                countries.Remove(upperCode);
            }
            else
            {
                throw new Exception("Country does not exist in current repository");
            }
        }
        
        public Country Get(string countryCode)
        {
            string upperCode = countryCode.ToUpper();
            if (countries.ContainsKey(upperCode))
            {
                return countries[upperCode];
            }
            else
            {
                throw new Exception("Country does not exist in current repository");
            }
        }
        
        public void Clear()
        {
            //clear objects in memory
            countries.Clear();

            //remove local storage
            var dataFiles = Directory.EnumerateFiles(dataPath, "*.csv");

            foreach (string csvFile in dataFiles)
            {
                File.Delete(csvFile);
            }
        }
        
        private void UpdateStorage(Country country)
        {
            //update all specific entry in csv
            string dataFile = Path.Combine(dataPath, country.Code[0] + ".csv");

            //retrieve all Country objects that start with the first letter of the country we are updating.
            List<Country> dataSet = countries.Where(i => i.Key[0] == country.Code[0]).ToDictionary(
                i => i.Key, i => i.Value).Values.ToList();

            //delete current data set.
            if (File.Exists(dataFile))
            {
                File.Delete(dataFile);
            }

            //rewrite whole data set.
            foreach (Country newCountrySet in dataSet)
            {
                using (StreamWriter dataWriter = new StreamWriter(dataFile))
                {
                    dataWriter.WriteLine(string.Format("{0},{1}", newCountrySet.Code, newCountrySet.Name));
                }
            }
        }
    }
}