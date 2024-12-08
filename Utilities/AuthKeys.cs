using Microsoft.IdentityModel.Tokens;

namespace MenuOnlineUdemy.Utilities
{
    public class AuthKeys
    {
        public const string IssuerName = "member01";
        private const string KeySection = "Authentication:Schemes:Bearer:SigningKeys";
        private const string KeySection_Issuer = "Issuer";
        private const string KeySection_Value = "Value";

        public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration)
            => GetKey(configuration, IssuerName);

        public static IEnumerable<SecurityKey> GetKey(IConfiguration configuration, string issuer)
        {
            var signingKey = configuration.GetSection(KeySection)
                .GetChildren()
                .SingleOrDefault(key => key[KeySection_Issuer] == issuer);

            if (signingKey is not null && signingKey[KeySection_Value] is string keyValue)
            {
                yield return new SymmetricSecurityKey(Convert.FromBase64String(keyValue));
            }
        }

        public static IEnumerable<SecurityKey> GetAllKey(IConfiguration configuration)
        {
            var signingKeys = configuration.GetSection(KeySection)
                .GetChildren();

            foreach (var signingKey in signingKeys)
            {

                if (signingKey is not null && signingKey[KeySection_Value] is string keyValue)
                {
                    yield return new SymmetricSecurityKey(Convert.FromBase64String(keyValue));
                }
            }
        }
    }
}
