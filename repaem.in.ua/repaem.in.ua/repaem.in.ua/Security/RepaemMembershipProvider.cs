using aspdev.repaem.Models.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace aspdev.repaem.App_Start
{
    //Может, ну его к псам, все сделать по своему?
    public class RepaemMembershipProvider : MembershipProvider
    {
        IUserService us;

        public override string ApplicationName
        {
            get
            {
                return "/";
            }
            set
            {
            }
        }

        public RepaemMembershipProvider()
        {
            us = DependencyResolver.Current.GetService<IUserService>();
        }
        public override bool ValidateUser(string username, string password)
        {
            return us.ValidateUser(username, password);
        }
        
        public override bool RequiresQuestionAndAnswer
        {
            get { return false; }
        }
        public override bool RequiresUniqueEmail
        {
            get { return true; }
        }

        //Не нужны
        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            return us.ChangePassword(username, oldPassword, newPassword);
        }
        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }
        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status)
        {
            throw new NotImplementedException();
        }
        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }
        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }
        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }
        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }
        public override int MaxInvalidPasswordAttempts
        {
            get { return 10; }
        }
        public override int MinRequiredNonAlphanumericCharacters
        {
            get { throw new NotImplementedException(); }
        }
        public override int MinRequiredPasswordLength
        {
            get { return 5; }
        }
        public override int PasswordAttemptWindow
        {
            get { return 6; }
        }
        public override MembershipPasswordFormat PasswordFormat
        {
            get { throw new NotImplementedException(); }
        }
        public override string PasswordStrengthRegularExpression
        {
            get { throw new NotImplementedException(); }
        }
        public override bool EnablePasswordReset
        {
            get { return true; }
        }
        public override bool EnablePasswordRetrieval
        {
            get { return false; }
        }
        public override bool UnlockUser(string userName)
        {
            throw new NotImplementedException();
        }
        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }
        public override int GetNumberOfUsersOnline()
        {
            return 0;
        }
    }
}