using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TimeEffortCore.Entities;
using TimeEffortCore.Exceptions;


namespace TimeEffortCore.Services
{
    public class UserService
    {
        private static time_trackerEntities1 db;
        public UserService()
        {
            db = new time_trackerEntities1();
        }


        public int? Authenticate(UserInfo userInfo)
        {
            var curHashedPassword = BuildPassword(userInfo.Username, userInfo.Password);
            var curUser = db.UserInfo.FirstOrDefault(u => u.Username == userInfo.Username && u.Password == curHashedPassword);
            return curUser == null ? (int?)null : curUser.ID;
        }

        public void Register(UserInfo userInfo)
        {
            if (IsUserDataValid(userInfo))
            {
                if (!IsUnique(userInfo))
                    throw new Exception("Email or Username already exist in the database");

                userInfo.Password = BuildPassword(userInfo.Username, userInfo.Password);
                db.UserInfo.Add(userInfo);
                db.SaveChanges();
            }
        }

        private string GenerateAPIToken(int id, string username, string password)
        {
            var toEncrypt = username + id + password;
            HashAlgorithm hash = new SHA256Managed();
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(toEncrypt);
            byte[] hashBytes = hash.ComputeHash(plainTextBytes);

            //in this string you got the encrypted password
            return Convert.ToBase64String(hashBytes);
        }
        public string APIAuthenticate(UserInfo userInfo)
        {
            var id = Authenticate(userInfo);
            if (!id.HasValue)
                return string.Empty;
            var token = GenerateAPIToken(id.Value, userInfo.Username, userInfo.Password);
            //db.UserInfo.First(u => u.ID == id).APItoken = token;
            db.SaveChanges();
            return token;
        }
        /// <summary>
        ///algorithm for Hash with Salt
        ///but the Salt is dinamic - unique for each user instance
        ///assuming that username is constant and never be changed and length is at least 5 chars
        ///create the password string before encryption takes first 3 chars of login then pwd then last 3 chars of login
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        private string BuildPassword(string userName, string password)
        {
            if (userName.Length < 3)
                throw new Exception("Your UserName is less than 3 characters");
            var result = userName.Substring(0, 3) + password + userName.Substring(userName.Length - 3);
            return EncodeString(result);
        }

        private string EncodeString(string curString)
        {
            //Before you hash a string you parse it into a byte array
            var curByteArray = Encoding.UTF8.GetBytes(curString);
            using (var sha = new SHA256CryptoServiceProvider())
            {
                var curHashedByteArray = sha.ComputeHash(curByteArray);
                //not string type because of the performance
                var mutableString = new StringBuilder();
                foreach (var curHashedByte in curHashedByteArray)
                {
                    //puts some characters to the upper case
                    mutableString.Append(curHashedByte.ToString("x2"));
                }
                return mutableString.ToString();
            }
        }
        public static bool IsUnique(UserInfo userInfo)
        {
            return !db.UserInfo.Any(u => u.Username == userInfo.Username || u.Email == userInfo.Email);
        }
        public bool IsUserDataValid(UserInfo userInfo)
        {
            if (userInfo.FirstName == null)
                throw new NoUserNameException("First name is not provided");
            if (userInfo.LastName == null)
                throw new NoUserNameException("Last name is not provided");
            if (userInfo.Username == null)
                throw new NoUserNameException("User Name is not provided");
            if (userInfo.Password == null)
                throw new NoUserPasswordException("Password is not provided");
            if (userInfo.Email == null)
                throw new NoUserEmailException("Email is not provided");
            if (userInfo.Phone == null || userInfo.Phone == null || userInfo.Phone == null)
                throw new ArgumentException("Phone, First Name or Last Name missing");
            return true;
        }
        //method to check if the user name exists or not
        public bool UserNameExists(string username)
        {
            return db.UserInfo.Any(u => u.Username == username);
        }

        public UserInfo GetUserByUsername(string username)
        {
            return db.UserInfo.FirstOrDefault(u => u.Username == username);
        }

        public IEnumerable<UserInfo> GetAllUsers()
        {
            var users = db.UserInfo.Where(u => u.Username != "");
            foreach (var user in users)
                user.Password = ":p I will not show passwords";
            return users;
        }

        //Positions
        public List<Position> GetAllPositions()
        {
            return db.Position.ToList();
        }

    }
}
