using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trade.Logic.Requests;
using trade.Shared.Model.Dtos;

namespace trade.Logic.Services
{
    public interface IUserServices
    {
        Task<CudResponseDto> Register(RegisterUserRequest request);
    }

    public class UserServices : IUserServices
    {
        public Task<CudResponseDto> Register(RegisterUserRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
