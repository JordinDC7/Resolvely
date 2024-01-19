using System.Collections.Generic;

namespace Sabio.Models
{
    public interface IUserAuthData
    {
        int Id { get; }
        string Email { get; }
        string Role { get; }
        object TenantId { get; }

    }
}