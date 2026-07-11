
namespace Infrastructure.Configurations
{
    public class DbConfiguration
    {
        public Dictionary<string, string> ConnectionStrings { get; set; } = new Dictionary<string, string>();
        public string? DefaultConnection
        {
            get => ConnectionStrings.GetValueOrDefault("DefaultConnection");
            set
            {
                if (value != null)
                {
                    ConnectionStrings["DefaultConnection"] = value;
                }
            }
        }
    }
}
