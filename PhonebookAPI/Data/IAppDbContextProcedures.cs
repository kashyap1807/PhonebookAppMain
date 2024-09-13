using PhonebookAPI.Dtos;

namespace PhonebookAPI.Data
{
    public interface IAppDbContextProcedures
    {
        List<AllContactSP> GetAllContact();

        List<GetAllContactByCountryDto> GetAllContactByCountry();

        List<GetAllContactByGenderDto> GetAllContactByGender();

        List<GetAllContactByStateIdDto> GetAllContactByStateId(int stateId);
        List<GetAllContactByMonthDto> GetAllContactByMonth(int month);
    }
}
