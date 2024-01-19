using Sabio.Models.Domain.Users;
using Sabio.Models;
using System.Threading.Tasks;
using Sabio.Models.Requests.Users;
using System.Collections.Generic;
using Sabio.Services.Services;

namespace Sabio.Services
{
    public interface IUserService
    {
        Task<bool> LogInAsync(LogInAddRequest model);

        int Create(UserAddRequest model);

        UserBase ForgotPassTokenRequest(string email);

        bool ChangePassword(PasswordUpdateRequest userPassUpdate);

        void UpdateAvatar(string avatarUrl, int id);

        List<User> SelectAll();

        InitialUser SelectById(int id);

        bool ConfirmAccount(string token);

        void InsertUserToken(int userId, TokenAddRequest tokenReq);
        void UpdateUserInfo(BaseUserProfileUpdateRequest model, int userId);

        void UpdateUserStatus(int id, int statusId);
    }
}