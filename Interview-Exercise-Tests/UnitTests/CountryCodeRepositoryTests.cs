using System;
using Moq;
using NUnit.Framework;

using Interview_Exercise;

namespace InterviewExercise.Tests.UnitTests
{
    [TestFixture]
    public class CountryCodeRepositoryTests
    {
        public abstract class CountryCodeRepositoryTestBase : TestFixtureBase
        {
            protected bool result = false;
            protected CountryCodeRepository repo = new CountryCodeRepository();

            protected override void Arrange()
            {
                //clear anything loaded from local storage.
                repo.Clear();

                repo.Add(new Country("asd", "Test Country"));
            }

            public override void RunOnceAfterAll()
            {
                repo.Clear();
                base.RunOnceAfterAll();
            }

            [Test]
            public void TestForSuccess()
            {
                Assert.That(result, Is.True);
            }
        }

        public class CountryCodeRepositoryAddTest : CountryCodeRepositoryTestBase
        {
            protected override void Act()
            {
                int oldCount = repo.Count;
                repo.Add(new Country("new", "Test Country 2"));
                result = (repo.Count == (oldCount + 1));
            }
        }

        public class CountryCodeRepositoryDeleteTest : CountryCodeRepositoryTestBase
        {
            protected override void Act()
            {
                int oldCount = repo.Count;

                repo.Delete("asd");

                result = (repo.Count == (oldCount - 1));
            }
        }

        public class CountryCodeRepositoryUpdateTest : CountryCodeRepositoryTestBase
        {
            protected override void Act()
            {
                Country testCountry = new Country("asd", "New Name");
                repo.Update(testCountry);

                testCountry = repo.Get("asd");
                result = testCountry.Name.Equals("New Name");
            }
        }

        public class CountryCodeRepositoryGetTest : CountryCodeRepositoryTestBase
        {
            protected override void Act()
            {
                Country testCountry = repo.Get("asd");

                result = testCountry.Code.Equals("ASD") && testCountry.Name.Equals("Test Country");
            }
        }

        public class CountryCodeRepositoryClearTest : CountryCodeRepositoryTestBase
        {
            protected override void Act()
            {
                int oldCount = repo.Count;

                repo.Clear();

                result = oldCount == 1 && repo.Count == 0;
            }
        }
    }
}
