using IDP.Services;
using Microsoft.Extensions.DependencyInjection;


namespace IDP
{
    public static class IdentityServerBuilderExtensions
    {
        public static IIdentityServerBuilder AddUserStore(this IIdentityServerBuilder builder)
        {
            //builder.Services.AddSingleton<IUserRepository, UserRepository>();
            builder.AddProfileService<UserProfileService>();
            return builder;
        }
    }
}
