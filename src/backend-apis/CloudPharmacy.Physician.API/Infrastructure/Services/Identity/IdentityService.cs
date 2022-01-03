using Microsoft.Identity.Web;

namespace CloudPharmacy.Physician.API.Infrastructure.Services.Identity
{
    public interface IIdentityService
    {
        string GetUserIdentity();
        string GetUserFirstNameAndLastName();
    }

    internal class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _context;

        public IdentityService(IHttpContextAccessor context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public string GetUserIdentity()
        {
            var userId = _context.HttpContext.User.FindFirst(ClaimConstants.ObjectId).Value;
            return userId;
        }

        public string GetUserFirstNameAndLastName()
        {
            var firstNameAndLastName = _context.HttpContext.User.FindFirst(ClaimConstants.Name).Value;
            return firstNameAndLastName;
        }
    }
}
