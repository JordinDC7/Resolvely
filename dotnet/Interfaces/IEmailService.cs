using Sabio.Models.Requests.Email;
using Sabio.Models.Requests.Users;
using Sabio.Services.Services;
using System.Threading.Tasks;

namespace Sabio.Services.Interfaces
{
    public interface IEmailService
    {
        public Task ContactUs(ContactUsRequest model);

        Task SendConfirm(UserAddRequest userModel, TokenAddRequest token);

        Task SendPassReset(UserBase userModel, TokenAddRequest token);
    }
}