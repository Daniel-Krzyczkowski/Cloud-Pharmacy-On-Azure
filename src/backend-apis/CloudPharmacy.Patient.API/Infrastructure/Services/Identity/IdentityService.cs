using Microsoft.Identity.Web;
using System.Security.Claims;

namespace CloudPharmacy.Patient.API.Infrastructure.Services.Identity
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
            var userId = _context.HttpContext.User.FindFirst(ClaimConstants.NameIdentifierId).Value;
            return userId;
        }

        public string GetUserFirstNameAndLastName()
        {
            var firstName = _context.HttpContext.User.FindFirst(ClaimTypes.GivenName).Value;
            var lastName = _context.HttpContext.User.FindFirst(ClaimTypes.Surname).Value;
            return $"{firstName} {lastName}";
        }
    }
}
