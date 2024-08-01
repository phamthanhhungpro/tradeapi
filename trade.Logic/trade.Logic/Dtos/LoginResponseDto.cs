using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using trade.InfraModel.DataAccess;

namespace trade.Logic.Dtos
{
    public class LoginResponseDto
    {
        public string Token { get; set; }
        public User User { get; set; }
    }
}
