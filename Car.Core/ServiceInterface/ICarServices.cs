using Car.Core.Dto;
using DomainCar = global::Car.Core.Domain.Car;

namespace Car.Core.ServiceInterface
{
    public interface ICarServices
    {
        Task<DomainCar> Create(CarDto dto);
        Task<DomainCar> DetailAsync(Guid id);
        Task<DomainCar> Delete(Guid id);
        Task<DomainCar> Update(CarDto dto);
    }
}
