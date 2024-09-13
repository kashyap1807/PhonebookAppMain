using PhonebookAPI.Dtos;

namespace PhonebookAPI.Services.Contract
{
    public interface IStateService
    {
        ServiceResponse<IEnumerable<StateDto>> GetStateByCountryId(int id);
    }
}
