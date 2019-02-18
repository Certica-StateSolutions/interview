using Interview_Exercise;

namespace InterviewExercise
{
    public interface IRepository
    {
        void Add(Country country);
        void Update(Country country);
        void Delete(string countryCode);
        Country Get(string countryCode);
        void Clear();
    }
}
