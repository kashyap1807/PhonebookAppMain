using PhonebookAPI.Data.Contract;
using PhonebookAPI.Dtos;
using PhonebookAPI.Services.Contract;
using System.IO;

namespace PhonebookAPI.Services.Implementation
{
    public class StateService : IStateService
    {
        private readonly IStateRepository _stateRepository;

        public StateService(IStateRepository stateRepository)
        {
            _stateRepository = stateRepository;
        }

        public ServiceResponse<IEnumerable<StateDto>> GetStateByCountryId(int id)
        {
            var response = new ServiceResponse<IEnumerable<StateDto>>();
            try
            {
                var states = _stateRepository.GetStatesBytCountryId(id);

                if (states != null && states.Any())
                {
                    var stateDtos = new List<StateDto>();
                    foreach (var state in states)
                    {
                        stateDtos.Add(new StateDto()
                        {
                            StateId = state.StateId,
                            StateName = state.StateName,
                            CountryId = state.CountryId,
                            //Country = new Models.Country()
                            //{
                            //    CountryId = state.CountryId,
                            //    CountryName = state.Country.CountryName
                            //}
                        });
                    }
                    response.Data = stateDtos;

                }
                else
                {
                    response.Success = false;
                    response.Message = "No record found";
                }
            }
            catch (Exception ex)
            {
                response.Success= false;
                response.Message = ex.Message;
            }
            

            return response;
        }
    }
}
