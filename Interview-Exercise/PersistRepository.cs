using Interview_Exercise;
using System;
using System.IO;

namespace InterviewExercise
{
    public class PersistRepository : IPersistRepository
    {
        void IPersistRepository.Add(Country country)
        {
            CreateBaseDirectoryIfNecessary();
            string filePath = BuildFullPath(country);
            using (var sw = File.AppendText(filePath))
            {
                sw.Write(BuildCountryEntry(country.Code, country.Name));
            }
        }

        void IPersistRepository.Clear()
        {
            if (IsBaseDirectoryThere())
            {
                string[] files = Directory.GetFiles(GetBasePath());
                foreach (string file in files)
                    File.Delete(file);
            }
        }

        void IPersistRepository.Delete(string countryCode)
        {
            if (IsBaseDirectoryThere())
            {
                string filePath = BuildFullPath(countryCode);
                if (File.Exists(filePath))
                {
                    string fileData = File.ReadAllText(filePath);
                    int startIndex = fileData.IndexOf(countryCode);
                    if (startIndex != -1)
                    {
                        string newFileContent = fileData.Remove(startIndex, startIndex + 8); // 8 = 3 letters plus 1 comma multiplied by 2 
                        File.WriteAllText(filePath, newFileContent);
                    }
                    else throw new CountryNotFoundException(countryCode);
                }
                else throw new CountryNotFoundException(countryCode);
            }
        }

        Country IPersistRepository.Get(string countryCode)
        {
            if (IsBaseDirectoryThere())
            {
                string filePath = BuildFullPath(countryCode);
                if (File.Exists(filePath))
                {
                    string fileData = File.ReadAllText(filePath);
                    int startIndex = fileData.IndexOf(countryCode);
                    if (startIndex != -1)
                    {
                        string entry = fileData.Substring(startIndex, startIndex + 8); // 8 = 3 letters plus 1 comma multiplied by 2 
                        string[] tokens = entry.Split(new char[] { ',' });
                        return new Country(tokens[0], tokens[1]);
                    }
                    else throw new CountryNotFoundException(countryCode);
                }
                else throw new CountryNotFoundException(countryCode);
            }

            throw new CountryNotFoundException();
        }

        void IPersistRepository.Update(Country country)
        {
            if (IsBaseDirectoryThere())
            {
                string filePath = BuildFullPath(country);
                if (File.Exists(filePath))
                {
                    string fileData = File.ReadAllText(filePath);
                    int startIndex = fileData.IndexOf(country.Code);
                    if (startIndex != -1)
                    {
                        fileData = fileData.Remove(startIndex, startIndex + 8); // 8 = 3 letters plus 1 comma multiplied by 2 
                        fileData = fileData.Insert(startIndex, BuildCountryEntry(country));
                        File.WriteAllText(filePath, fileData);
                    }
                    else throw new CountryNotFoundException(country.Code);
                }
                else throw new CountryNotFoundException(country.Code);
            }
        }

        private string GetBasePath()
        {
            string dataPath = Path.Combine(
                System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "Interview-Exercise");
            return dataPath;
        }

        private static string BuildCountryEntry(string code, string name)
        {
            return string.Format("{0},{1},", code, name);
        }

        private static string BuildCountryEntry(Country country)
        {
            return BuildCountryEntry(country.Code, country.Name);
        }

        private string BuildFileName(string countryCode)
        {
            return string.Format("{0}.csv", countryCode[0]);
        }

        private string BuildFileName(Country country)
        {
            return BuildFileName(country.Code);
        }

        private string BuildFullPath(string countryCode)
        {
            return string.Format("{0}\\{1}", GetBasePath(), BuildFileName(countryCode));
        }

        private string BuildFullPath(Country country)
        {
            return BuildFullPath(country.Code);
        }

        private void CreateBaseDirectoryIfNecessary()
        {
            var basePath = GetBasePath();
            if (Directory.Exists(basePath) == false)
                Directory.CreateDirectory(basePath);
        }

        private bool IsBaseDirectoryThere()
        {
            return Directory.Exists(GetBasePath());
        }
    }
}
