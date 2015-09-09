using System;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Security.Claims;
using Ironhide.Users.Domain.Entities;

namespace Ironhide.Users.Domain.Services
{
    public class JwtFactory : ITokenFactory
    {
        readonly ITimeProvider _timeProvider;
        readonly ITokenExpirationProvider _tokenExpirationProvider;
        readonly IIdentityGenerator<Guid> _identityGenerator;
        readonly IKeyProvider _keyProvider;

        public JwtFactory(ITimeProvider timeProvider,
                                  IIdentityGenerator<Guid> identityGenerator,
                                  ITokenExpirationProvider tokenExpirationProvider,
                                  IKeyProvider keyProvider)
        {
            _timeProvider = timeProvider;
            _identityGenerator = identityGenerator;
            _tokenExpirationProvider = tokenExpirationProvider;
            _keyProvider = keyProvider;
        }

        #region ITokenFactory Members

        public string Create(User executor)
        {
            var id = _identityGenerator.Generate();

            var now = _timeProvider.Now();
            DateTime expirationDateTime = _tokenExpirationProvider.GetExpiration(now);

            var tokenHandler = new JwtSecurityTokenHandler();
            var symmetricKey = _keyProvider.GetKey();

            var claims = executor.UserRoles.Select(x => new Claim(ClaimTypes.Role, x.Description)).ToList();
            claims.Add(new Claim(ClaimTypes.Name, executor.Name));
            claims.Add(new Claim("userguid", executor.Id.ToString()));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                TokenIssuerName = "self",
                AppliesToAddress = "http://www.example.com",
                Lifetime = new Lifetime(now, expirationDateTime),
                SigningCredentials = new SigningCredentials(
                    new InMemorySymmetricSecurityKey(symmetricKey),
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256"),


            };
            
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return tokenString;
        }

        #endregion
    }
}