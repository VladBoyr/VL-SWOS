namespace Swos.CareerMod.Domain
{
    public interface ICareerService
    {
        Task NewSeason();
    }

    public class CareerService : ICareerService
    {
        public Task NewSeason()
        {
            throw new NotImplementedException();
        }
    }
}