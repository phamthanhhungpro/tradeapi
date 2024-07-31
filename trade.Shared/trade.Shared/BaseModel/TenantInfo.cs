namespace trade.Shared.Model.BaseModel
{
    public class TenantInfo
    {
        public Guid UserId { get; set; }
        public string Role { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }

    }
}
