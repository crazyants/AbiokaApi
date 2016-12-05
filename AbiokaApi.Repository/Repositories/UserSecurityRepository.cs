using AbiokaApi.Domain;
using AbiokaApi.Domain.Repositories;
using AbiokaApi.Repository.DatabaseObjects;
using AbiokaApi.Repository.Mappings;
using System.Linq;

namespace AbiokaApi.Repository.Repositories
{
    internal class UserSecurityRepository : Repository<UserSecurity, UserSecurityDB>, IUserSecurityRepository
    {
        public UserSecurity GetByEmail(string email) {
            var dbUser = Query.Where(u => u.Email.ToLowerInvariant() == email.ToLowerInvariant()).FirstOrDefault();
            if (dbUser == null)
                return null;

            var result = (UserSecurity)DBObjectMapper.ToDomainObject(dbUser);
            return result;
        }
    }
}