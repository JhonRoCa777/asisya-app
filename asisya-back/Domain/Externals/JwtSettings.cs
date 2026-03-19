
namespace Domain.Externals
{
    public class JwtSettings
    {
        public string Key { get; set; } = string.Empty;

        public int ExpireMinutes { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
