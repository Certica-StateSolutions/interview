using Interview_Exercise;
using NUnit.Framework;
using System;
using System.IO;

namespace InterviewExercise.Tests.IntegrationTests
{
    public class PersistRepositoryTests
    {
        [TestFixture]
        public class PersistRepositoryTestsBase : TestFixtureBase
        {
            protected IPersistRepository _persistRepository;
            protected Country _country;
            protected string _countryCode;

            protected override void Arrange()
            {
                _persistRepository = new PersistRepository();
                _country = new Country();
            }

            public override void RunOnceAfterAll()
            {
                base.RunOnceAfterAll();
                Directory.Delete(GetBasePath(), true);
            }

            public string GetBasePath()
            {
                string dataPath = Path.Combine(
                    System.Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "Interview-Exercise");
                return dataPath;
            }

            public string GetFileData(string countryCode)
            {
                return File.ReadAllText(string.Format("{0}\\{1}.csv", GetBasePath(), _country.Code[0]));
            }

            public string BuildCountryEntry(string code, string name)
            {
                return string.Format("{0},{1},", code, name);
            }
        }

        [TestFixture]
        public class Add_to_file_test : PersistRepositoryTestsBase
        {
            string _fileData;

            protected override void Arrange()
            {
                base.Arrange();
                _country.Code = "ABC";
                _country.Name = "Mr.Ed";
            }

            protected override void Act()
            {
                _persistRepository.Add(_country);
                _fileData = GetFileData(_country.Code);
            }

            [Test]
            public void New_entry_should_be_in_csv_file()
            {
                AssertAll(
                    () => Assert.That(Directory.Exists(GetBasePath()), Is.True),
                    () => Assert.That(_fileData.Contains(string.Format("{0},{1},", _country.Code, _country.Name)), Is.True));
            }
        }

        [TestFixture]
        public class Clear_repository_test : PersistRepositoryTestsBase
        {
            protected override void Arrange()
            {
                base.Arrange();
                string basePath = GetBasePath();
                Directory.CreateDirectory(basePath);
                File.Create(string.Format("{0}\\{1}", basePath, "A.csv")).Dispose(); //Call dispose to release the lock on the newly created file
                File.Create(string.Format("{0}\\{1}", basePath, "B.csv")).Dispose();
                File.Create(string.Format("{0}\\{1}", basePath, "C.csv")).Dispose();
            }

            protected override void Act()
            {
                _persistRepository.Clear();
            }

            [Test]
            public void Target_directory_should_be_empty()
            {
                AssertAll(
                    () => Assert.That(Directory.Exists(GetBasePath()), Is.True),
                    () => Assert.That(Directory.GetFiles(GetBasePath()).Length, Is.EqualTo(0)));
            }
        }

        [TestFixture]
        public class Delete_country_entry_test : PersistRepositoryTestsBase
        {
            string _fileData;
            string _countryEntry;

            protected override void Arrange()
            {
                base.Arrange();
                string basePath = GetBasePath();
                Directory.CreateDirectory(basePath);
                string filePath = string.Format("{0}\\{1}", basePath, "B.csv");
                File.Create(filePath).Dispose(); //Call dispose to release the lock on the newly created file
                _country.Code = "BAD";
                _country.Name = "MAN";
                _countryCode = _country.Code;
                _countryEntry = BuildCountryEntry(_country.Code, _country.Name);
                using (var sw = File.AppendText(filePath))
                {
                    sw.Write(_countryEntry);
                }
            }

            protected override void Act()
            {
                _persistRepository.Delete(_countryCode);
                _fileData = GetFileData(_country.Code);
            }

            [Test]
            public void Entry_should_not_exist_in_file()
            {
                Assert.That(_fileData.Contains(_countryEntry), Is.False);
            }
        }

        [TestFixture]
        public class Get_country_info_test : PersistRepositoryTestsBase
        {
            string _countryName;
            string _countryEntry;

            protected override void Arrange()
            {
                base.Arrange();
                string basePath = GetBasePath();
                Directory.CreateDirectory(basePath);
                string filePath = string.Format("{0}\\{1}", basePath, "O.csv");
                File.Create(filePath).Dispose(); //Call dispose to release the lock on the newly created file
                _country.Code = "ONE";
                _countryName = "TWO";
                _country.Name = _countryName;
                _countryCode = _country.Code;
                _countryEntry = BuildCountryEntry(_country.Code, _country.Name);
                using (var sw = File.AppendText(filePath))
                {
                    sw.Write(_countryEntry);
                }
            }

            protected override void Act()
            {
                _country = _persistRepository.Get(_countryCode);
            }

            [Test]
            public void Should_be_able_to_retrieve_the_name_of_the_country()
            {
                Assert.That(_country.Name == _countryName, Is.True);
            }
        }

        [TestFixture]
        public class Update_country_entry_test : PersistRepositoryTestsBase
        {
            string _fileData;
            string _countryName;
            string _countryEntry;
            Country _countryUpdate;

            protected override void Arrange()
            {
                base.Arrange();
                string basePath = GetBasePath();
                Directory.CreateDirectory(basePath);
                string filePath = string.Format("{0}\\{1}", basePath, "P.csv");
                File.Create(filePath).Dispose(); //Call dispose to release the lock on the newly created file
                _country.Code = "POW";
                _countryName = "WOW";
                _country.Name = _countryName;
                _countryCode = _country.Code;

                _countryUpdate = new Country() { Code = _country.Code, Name = "UPD" };

                _countryEntry = BuildCountryEntry(_country.Code, _country.Name);
                using (var sw = File.AppendText(filePath))
                {
                    sw.Write(_countryEntry);
                }
            }

            protected override void Act()
            {
                _persistRepository.Update(_countryUpdate);
                _fileData = GetFileData(_country.Code);
            }

            [Test]
            public void Country_entry_data_should_be_updated()
            {
                Assert.That(_fileData.Contains(BuildCountryEntry(_countryUpdate.Code, _countryUpdate.Name)), Is.True);
            }
        }
    }
}
