using trade.Shared.Enum;
using trade.Shared.Model.BaseModel;

namespace trade.InfraModel.DataAccess
{
    public class User : BaseEntity
    {
        public string PassWordHash { get; set; }
        public string Email { get; set; }
        public RoleEnum Role { get; set; }
        public string Name { get; set; }
        public string Phone { get; set; }
        public string BankName { get; set; }
        public string BankAccountNumber { get; set; }
        public ICollection<Token> Tokens { get; set; }
        public ICollection<Store> Stores { get; set; }
    }
}
