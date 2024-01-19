using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Domain.Users;
using Sabio.Models.Requests.Users;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using System.Threading.Tasks;

namespace Sabio.Services.Services
{
    public class UserService : IUserService
    {
        private IAuthenticationService<int> _authenticationService;
        private IDataProvider _dataProvider;
        private ILookUpService _lookUpService;
        private IEmailService _emailService;


        public UserService(IAuthenticationService<int> authService
            , ILookUpService lookUpService
            , IDataProvider dataProvider
            , IEmailService emailService
            )
        {
            _lookUpService = lookUpService;
            _authenticationService = authService;
            _dataProvider = dataProvider;
            _emailService = emailService;
        }

        public async Task<bool> LogInAsync(LogInAddRequest model)
        {
            bool isSuccessful = false;

            IUserAuthData response = Get(model.Email, model.Password);



            if (response != null)
            {
                await _authenticationService.LogInAsync(response);
                isSuccessful = true;
            }

            return isSuccessful;
        }

        public int Create(UserAddRequest model)
        {
            int userId = 0;

            string password = model.Password;
            string salt = BCrypt.BCryptHelper.GenerateSalt();
            string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, salt);

            TokenAddRequest newToken = new TokenAddRequest()
            {
                TokenId = Guid.NewGuid().ToString(),
                TokenType = 1
            };

            _dataProvider.ExecuteNonQuery(
                    storedProc: "[dbo].[Users_Insert]"
                    , inputParamMapper: paramCollection =>
                    {
                        paramCollection.AddWithValue("@Email", model.Email);
                        paramCollection.AddWithValue("@FirstName", model.FirstName);
                        paramCollection.AddWithValue("@LastName", model.LastName);
                        paramCollection.AddWithValue("@Mi", model.Mi);
                        paramCollection.AddWithValue("@AvatarUrl", model.AvatarUrl);
                        paramCollection.AddWithValue("@Password", hashedPassword);
                        paramCollection.AddWithValue("@Token", newToken.TokenId);
                        paramCollection.AddWithValue("@TokenType", newToken.TokenType);
                        paramCollection.AddWithValue("@RoleId", model.RoleId);

                        SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                        idOut.Direction = ParameterDirection.Output;

                        paramCollection.Add(idOut);
                    }
                    , returnParameters: returnCollection =>
                    {
                        object oId = returnCollection["@Id"].Value;
                        int.TryParse(oId.ToString(), out userId);
                    });

            _emailService.SendConfirm(model, newToken);

            return userId;
        }

        public UserBase ForgotPassTokenRequest(string email)
        {
            UserBase user = null;
            TokenAddRequest newToken = null;
            user = GetCurrentUserByEmail(email);

            if (user != null)
            {
                newToken = new TokenAddRequest()
                {
                    TokenId = Guid.NewGuid().ToString(),
                    TokenType = 2
                };

                InsertUserToken(user.Id, newToken);
                _emailService.SendPassReset(user, newToken);

            }
            return user;
        }

        public bool ChangePassword(PasswordUpdateRequest userPassUpdate)
        {
            TokenUpdateRequest tokenConfirm = ConfirmByToken(userPassUpdate.Token);
            bool result = false;

            if (tokenConfirm != null)
            {
                if (tokenConfirm.UserId != 0 && tokenConfirm.TokenType == 2)
                {
                    string password = userPassUpdate.Password;
                    string salt = BCrypt.BCryptHelper.GenerateSalt();
                    string hashedPassword = BCrypt.BCryptHelper.HashPassword(password, salt);

                    _dataProvider.ExecuteNonQuery(
                storedProc: "[dbo].[Users_UpdatePassword]"
                , inputParamMapper: paramCollection =>
                {
                    paramCollection.AddWithValue("@Id", tokenConfirm.UserId);
                    paramCollection.AddWithValue("@Password", hashedPassword);
                }
                , returnParameters: null);

                    DeleteUserToken(userPassUpdate.Token);

                    result = true;
                }
            }
            return result;
        }

        public void UpdateAvatar(string avatarUrl, int id)
        {

            _dataProvider.ExecuteNonQuery(storedProc: "[dbo].[Users_UpdateAvatar]", inputParamMapper: paramCollection =>
            {
                paramCollection.AddWithValue("@Id", id);
                paramCollection.AddWithValue("@AvatarUrl", avatarUrl);
            }, returnParameters: null);

            return;
        }

        public List<User> SelectAll()
        {
            List<User> userList = null;

            _dataProvider.ExecuteCmd(
                storedProc: "[dbo].[Users_SelectAll]"
                , inputParamMapper: null
                , singleRecordMapper: (reader, set) =>
                {
                    User user = null;
                    int index = 0;
                    user = FullUserMapper(reader, out index);

                    if (userList == null)
                    {
                        userList = new List<User>();
                    }

                    userList.Add(user);

                }
                , returnParameters: null);

            return userList;
        }

        public InitialUser SelectById(int id)
        {
            InitialUser user = null;

            _dataProvider.ExecuteCmd(
                storedProc: "[dbo].[Users_SelectById]"
                , inputParamMapper: (paramCollection) =>
                {
                    paramCollection.AddWithValue("@Id", id);
                }
                , singleRecordMapper: (reader, set) =>
                {
                    int index = 0;
                    user = InitialUserMapper(reader, ref index);
                }
                , returnParameters: null);

            return user;
        }

        public void InsertUserToken(int userId, TokenAddRequest tokenReq)
        {
            _dataProvider.ExecuteNonQuery(
                storedProc: "[dbo].[UserTokens_Insert]"
                , inputParamMapper: paramCollection =>
                {
                    paramCollection.AddWithValue("@Id", userId);
                    paramCollection.AddWithValue("@TokenType", tokenReq.TokenType);
                    paramCollection.AddWithValue("@Token", tokenReq.TokenId);
                }
                , returnParameters: null
                );

            return;
        }


        public bool ConfirmAccount(string token)
        {
            TokenUpdateRequest tokenConfirm = ConfirmByToken(token);

            if (tokenConfirm.UserId != 0 && tokenConfirm.TokenType == 1)
            {
                int statusId = 1;

                UpdateIsConfirmed(tokenConfirm.UserId);
                UpdateUserStatus(tokenConfirm.UserId, statusId);
                DeleteUserToken(tokenConfirm.TokenId);

                return true;
            }
            return false;
        }

        private UserBase GetCurrentUserByEmail(string email)
        {
            UserBase user = null;

            _dataProvider.ExecuteCmd(
                storedProc: "[dbo].[Users_SelectByEmail]"
                , inputParamMapper: paramCollection =>
                {
                    paramCollection.AddWithValue("@Email", email);
                }
                , singleRecordMapper: (reader, set) =>
                {
                    int index = 0;
                    user = new UserBase();
                    user.Id = reader.GetSafeInt32(index++);
                    user.Email = reader.GetSafeString(index++);
                    user.Role = reader.GetSafeString(index++);
                    user.TenantId = "Resolvely Tenant";
                    user.AvatarUrl = reader.GetSafeString(index++);
                }
                , returnParameters: null);

            return user;
        }

        public InitialUser InitialUserMapper(IDataReader reader, ref int index)
        {
            InitialUser user = new InitialUser();

            user.Id = reader.GetSafeInt32(index++);
            user.FirstName = reader.GetSafeString(index++);
            user.LastName = reader.GetSafeString(index++);
            user.Mi = reader.GetSafeString(index++);
            user.AvatarUrl = reader.GetSafeString(index++);
            user.Email = reader.GetSafeString(index++);
            user.Role = reader.GetSafeString(index++);

            return user;
        }

        public User FullUserMapper(IDataReader reader, out int index)
        {
            User user = new User() { Status = new LookUp() };
            index = 0;

            user.Id = reader.GetSafeInt32(index++);
            user.FirstName = reader.GetSafeString(index++);
            user.LastName = reader.GetSafeString(index++);
            user.Mi = reader.GetSafeString(index++);
            user.AvatarUrl = reader.GetSafeString(index++);
            user.Email = reader.GetSafeString(index++);
            user.IsConfirmed = reader.GetSafeBool(index++);
            user.Status = _lookUpService.MapSingleLookUp(reader, ref index);
            user.DateCreated = reader.GetSafeDateTime(index++);
            user.DateModified = reader.GetSafeDateTime(index++);
            user.Role = _lookUpService.MapSingleLookUp(reader, ref index);

            return user;
        }

        private void DeleteUserToken(string tokenId)
        {
            _dataProvider.ExecuteNonQuery(
                storedProc: "[dbo].[UserTokens_Delete_ByToken]"
                , inputParamMapper: paramCollection =>
                {
                    paramCollection.AddWithValue("@Token", tokenId);
                }
                , returnParameters: null
                );

            return;
        }

        private TokenUpdateRequest ConfirmByToken(string tokenIncoming)
        {
            TokenUpdateRequest tokenUpdateReq = null;

            _dataProvider.ExecuteCmd(
                 storedProc: "[dbo].[UserTokens_Select_ByToken]"
                 , inputParamMapper: paramCollection =>
                 {
                     paramCollection.AddWithValue("@Token", tokenIncoming);
                 }
                 , singleRecordMapper: (reader, set) =>
                 {
                     tokenUpdateReq = new TokenUpdateRequest();
                     int index = 1;

                     tokenUpdateReq.UserId = reader.GetSafeInt32(index++);
                     tokenUpdateReq.TokenType = reader.GetSafeInt32(index++);
                     tokenUpdateReq.TokenId = tokenIncoming;
                 }
                 , returnParameters: null
             );


            return tokenUpdateReq;
        }

        public void UpdateUserStatus(int id, int statusId)
        {
            _dataProvider.ExecuteNonQuery(
                storedProc: "[dbo].[Users_UpdateStatus]"
                , inputParamMapper: paramCollection =>
                {
                    paramCollection.AddWithValue("@Id", id);
                    paramCollection.AddWithValue("@StatusId", statusId);
                }
                , returnParameters: null);

            return;
        }

        private void UpdateIsConfirmed(int userId)
        {
            _dataProvider.ExecuteNonQuery(
            storedProc: "[dbo].[Users_Confirm]"
            , inputParamMapper: paramCollection =>
            {
                paramCollection.AddWithValue("@Id", userId);
            }
            , returnParameters: null);

            return;
        }
        public void UpdateUserInfo(BaseUserProfileUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Users_Update]";
            _dataProvider.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", userId);
                col.AddWithValue("@FirstName", model.FirstName);
                col.AddWithValue("@LastName", model.LastName);
                col.AddWithValue("@Email", model.Email);
            });
        }

        private IUserAuthData Get(string email, string password)
        {
            string passwordFromDb = "";
            bool isConfirmed = false;
            UserBase user = null;

            _dataProvider.ExecuteCmd(
                storedProc: "[dbo].[Users_SelectPass_ByEmail]"
                , inputParamMapper: (paramCollection) =>
                {
                    paramCollection.AddWithValue("@Email", email);
                }
                , singleRecordMapper: (reader, set) =>
                {
                    passwordFromDb = reader.GetSafeString(0);
                    isConfirmed = reader.GetSafeBool(1);
                }
                , returnParameters: null);

            bool isValidCredentials = BCrypt.BCryptHelper.CheckPassword(password, passwordFromDb);

            if (isValidCredentials && isConfirmed)
            {
                user = GetCurrentUserByEmail(email);
            }
            else if (isValidCredentials && !isConfirmed)
            {
                throw new Exception("Email address not confirmed! Please check your email and try again.");
            }

            return user;
        }
    }
}

