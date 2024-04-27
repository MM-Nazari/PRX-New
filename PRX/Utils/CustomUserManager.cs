using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using PRX.Models.User;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace PRX.Utils
{
    public class CustomUserManager : UserManager<User>
    {
        public CustomUserManager(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger)
            : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }

        public async Task<User> FindByPhoneNumberAsync(string phoneNumber)
        {
            return await Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber);
        }
    }
}



