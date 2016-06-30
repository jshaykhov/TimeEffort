using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TimeEffortCore.Entities;
using TimeEffortCore.Exceptions;

namespace TimeEffortCore.Services
{
    public class AllDBServices
    {
        private static time_trackerEntities1 db;

        public AllDBServices()
        {
            ContextSet();
        }

        private void ContextSet()
        {
            db = new time_trackerEntities1();

        }

        #region Access Services

        public bool IsInvolved(int userId, int projectId)
        {
            return db.Access.Any(x => x.UserID == userId && x.ProjectID == projectId);
        }
        public bool IsInvolved(string username, int projectId)
        {
            return db.Access.Any(x => x.UserInfo.Username == username && x.ProjectID == projectId);
        }

        public List<Project> GetProjectsByUser(string username)
        {
            List<Project> returningList = new List<Project>();

            var user = db.UserInfo.FirstOrDefault(u => u.Username == username);
            var projects = db.Project.ToList();


            foreach (Project p in projects)
            {
                if (IsInvolved(user.ID, p.ID) && p.ManagerID != user.ID)
                    returningList.Add(p);
            }

            return returningList;

        }

        //Getting all
        public List<Access> GetAllAccesses()
        {
            return db.Access.ToList();
        }

        public void Insert(Access item)
        {
            db.Access.Add(item);
            db.SaveChanges();
        }
        public Access GetAccessById(int Id)
        {
            var item = db.Access.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Appointment does not exist");
            return item;
        }
        //EDITING
        public void UpdateAccess(Access item)
        {
            var dbItem = db.Access.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new Exception("Appointment does not exist");
            //dbItem.UserID = item.UserID;
            var projectItem = db.Project.FirstOrDefault(p => p.ID == item.ProjectID);

            if (item.DateFrom > projectItem.EndDate)
                throw new Exception("Date From cannot be later than " + projectItem.EndDate.ToShortDateString());

            if (item.DateTo < projectItem.StartDate || item.DateTo > projectItem.EndDate)
                throw new Exception("Date To should be between " + projectItem.StartDate.ToShortDateString() + " and " + projectItem.EndDate.ToShortDateString());

            if (item.DateFrom >= item.DateTo)
                throw new Exception("Date To cannot be earlier than or equal to Date From");
            //dbItem.ProjectID = item.ProjectID;

            dbItem.RoleID = item.RoleID;
            dbItem.DateFrom = item.DateFrom;
            dbItem.DateTo = item.DateTo;

            db.SaveChanges();
        }

        //DELETING THE APPOINTMENT
        public void DeleteAccess(int itemId)
        {
            var item = db.Access.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete an appointment");
            db.Access.Remove(item);
            db.SaveChanges();
        }

        #endregion

        #region Project Services
        public void DeleteProject(int itemId)
        {
            // var notification = new Notification();
            var item = db.Project.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a project");
            db.Project.Remove(item);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                db.Dispose();
                ContextSet();
                throw new Exception("You cannot delete this record.");
            }
        }

        //Projects
        public List<Project> GetAllProjects()
        {
            var list = db.Project.ToList();
            var removing = db.Project.SingleOrDefault(x => x.ID == 0);
            if (removing != null)
                list.Remove(removing);
            return list;
        }

        public Project GetProjectById(int Id)
        {
            var item = db.Project.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Project does not exist");
            return item;
        }

        public void Insert(Project item)
        {
            //try
            //{
            db.Project.Add(item);
            db.SaveChanges();
            GenerateNotification(item);
            //}
            //catch (Exception e)
            //{

            //}
        }

        public void Update(Project item)
        {

            var dbItem = db.Project.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Project does not exist");

            dbItem.Name = item.Name;
            dbItem.ContractUSD = item.ContractUSD;
            dbItem.ContractUZS = item.ContractUZS;
            dbItem.ManagerID = item.ManagerID;
            dbItem.StartDate = item.StartDate;
            dbItem.EndDate = item.EndDate;
            dbItem.Status = item.Status;
            db.SaveChanges();
            // UpdateNotification(item);
        }

        //users


        public List<Project> GetProjectsByManager(int projectManagerId)
        {
            return db.Project.Where(p => p.ManagerID == projectManagerId).ToList();
        }

        public Project GetProjectByCode(string code)
        {
            return db.Project.FirstOrDefault(p => p.Code.Equals(code));
        }


        //CODE GENERATION

        public List<string> GetNextCode()
        {
            var p = db.Project.AsEnumerable().LastOrDefault();
            string i = "";
            List<string> returning = new List<string>();
            if (p != null) { 
                if(p.Code.Length > 11)
                    i = p.Code.Substring(8, 3);
            }
            if (i.Equals(""))
                i = "000";
            int incrementor = 0;
            bool successful = int.TryParse(i, out incrementor);
            if (successful)
            {
                incrementor += 1;
                i = incrementor.ToString();
                if (i.Length == 1)
                    i = "00" + incrementor;
                else if (i.Length == 2)
                    i = "0" + incrementor;
                else if (i.Length == 3)
                    i = incrementor.ToString();
                else
                    i = "";

                if (i != "")
                {
                    //returning.Add("PJ" + " " + DateTime.Now.Year + "-" + i + "-I");
                    returning.Add("PJ" + " " + DateTime.Now.Year + "-" + i + "-R");
                    returning.Add("PJ" + " " + DateTime.Now.Year + "-" + i + "-H");
                    returning.Add("PJ" + " " + DateTime.Now.Year + "-" + i + "-M");
                    returning.Add("PJ" + " " + DateTime.Now.Year + "-" + i + "-B");
                }
                return returning;
            }
            else
                return returning;
        }
        //GENERATE NOTIFICATION
        public void GenerateNotification(Project project)
        {

            var notification = new Notification();
            notification.ProjectId = project.ID;
            if (DateTime.Today < project.StartDate)
            {
                notification.MESSAGE = "Today is the start date of project " + db.Project.FirstOrDefault(x => x.ID == project.ID).Name + " (code: " + db.Project.FirstOrDefault(x => x.ID == project.ID).Code + "). Please change its status to 'Active'";
                notification.ISREAD = false;
                notification.Date = project.StartDate;
                notification.TOID = db.UserInfo.FirstOrDefault(x => x.Position.Name == "Monitor").ID;
                db.Notification.Add(notification);
                db.SaveChanges();
            }
            if (DateTime.Today <= project.EndDate)
            {
                notification.MESSAGE = "Today is the end date of project " + db.Project.FirstOrDefault(x => x.ID == project.ID).Name + " (code: " + db.Project.FirstOrDefault(x => x.ID == project.ID).Code + "). Please change its status to 'Completed'";
                notification.ISREAD = false;
                notification.Date = project.EndDate;
                notification.TOID = db.UserInfo.FirstOrDefault(x => x.Position.Name == "Monitor").ID;
                db.Notification.Add(notification);
                db.SaveChanges();
            }
        }
        #endregion

        #region User Service
        public int? Authenticate(UserInfo userInfo)
        {
            var curHashedPassword = BuildPassword(userInfo.Username, userInfo.Password);
            UserInfo curUser = null;
            try
            {
                curUser = db.UserInfo.FirstOrDefault(u => u.Username == userInfo.Username && u.Password == curHashedPassword);
            }
            catch (Exception e)
            {
                return curUser == null ? (int?)null : curUser.ID;
            }

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

        public bool ChangePassword(UserInfo userInfo)
        {
            try
            {
                var dbItem = db.UserInfo.FirstOrDefault(p => p.Username == userInfo.Username);
                if (dbItem == null)
                    throw new ArgumentNullException("User does not exist");

                userInfo.Password = BuildPassword(userInfo.Username, userInfo.Password);
                dbItem.Password = userInfo.Password;
                db.SaveChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
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

        public List<UserInfo> GetAllUsers()
        {
            var users = db.UserInfo.Where(u => u.Username != "");
            return users.ToList();
        }

        //get by id
        public UserInfo GetUserById(int Id)
        {
            var item = db.UserInfo.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("User does not exist");
            return item;
        }
        //Delete
        public void DeleteUser(int itemId)
        {
            var item = db.UserInfo.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a user");
            db.UserInfo.Remove(item);
            db.SaveChanges();
        }
        //UPDATE
        public void Update(UserInfo item)
        {
            var dbItem = db.UserInfo.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("User does not exist");

            if (dbItem.PositionID == 1)
                return;

            dbItem.FirstName = item.FirstName;
            dbItem.LastName = item.LastName;
            dbItem.Email = item.Email;
            dbItem.Phone = item.Phone;
            dbItem.Address = item.Address;
            dbItem.Major = item.Major;
            dbItem.Project = item.Project;
            dbItem.Username = item.Username;
            dbItem.PositionID = item.PositionID;
            dbItem.Address = item.Address;
            dbItem.Major = item.Major;

            db.SaveChanges();
        }
        #endregion

        #region Workload Service
        public void Insert(Workload item)
        {
            if (item == null)
                throw new ArgumentNullException("You cannot insert a null Insert");

            db.Workload.Add(item);
            db.SaveChanges();
        }

        public List<Workload> GetAllWorkloads()
        {
            return db.Workload.ToList();
        }

        public List<Workload> GetAllbyUserAndDate(int userId, DateTime date)
        {
            return db.Workload.Where(w => w.UserID == userId && w.Date == date).ToList();
        }

        public Workload GetWorkloadById(int id)
        {
            var item = db.Workload.FirstOrDefault(w => w.ID == id);
            if (item == null)
                throw new ArgumentNullException("Workload does not exist");
            return item;
        }
        public UserInfo GetUserByName(string name)
        {
            var item = db.UserInfo.FirstOrDefault(u => u.Username == name);
            if (item == null)
                throw new ArgumentNullException("Workload does not exist");
            return item;
        }

        public List<Workload> GetWorkloadsByUser(string name)
        {
            var user = GetUserByName(name);
            return db.Workload.Include("WorkloadType").Include("Project").Where(w => w.UserID == user.ID).ToList();
        }

        public void UpdateApproveStatus(int id, bool master, bool pm, bool cto)
        {
            var dbItem = db.Workload.FirstOrDefault(w => w.ID == id);
            if (dbItem == null)
                throw new ArgumentNullException("Workload does not exist");

            db.SaveChanges();
        }
        public void Update(Workload item)
        {
            var dbItem = db.Workload.FirstOrDefault(w => w.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Workload does not exist");

            dbItem.Date = item.Date;
            dbItem.UserID = item.UserID;
            dbItem.ProjectID = item.ProjectID;
            dbItem.Duration = item.Duration;
            dbItem.Note = item.Note;
            dbItem.WorkloadTypeID = item.WorkloadTypeID;

            db.SaveChanges();
        }

        public void DeleteWorkload(int id)
        {
            var item = db.Workload.FirstOrDefault(w => w.ID == id);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a null service");
            db.Workload.Remove(item);
            db.SaveChanges();
        }
        //User
        public int GetUserIdByUsername(string username)
        {
            var user = db.UserInfo.FirstOrDefault(u => u.Username == username);
            if (user == null)
                throw new ArgumentNullException("User not found");
            return user.ID;
        }

        public List<Project> GetAllInvolvedUserPMProjects(string username)
        {
            var user = db.UserInfo.FirstOrDefault(u => u.Username == username);
            List<Project> list = new List<Project>();
            foreach (Access item in db.Access.Where(x => x.UserID == user.ID).ToList())
            {
                list.Add(item.Project);
            }
            foreach (Project item in db.Project.Where(x => x.ManagerID == user.ID).ToList())
            {
                list.Add(item);
            }

            return list;
        }

        public List<Project> GetAllInvolvedUserPMProjects(string username, DateTime from, DateTime to)
        {
            var user = db.UserInfo.FirstOrDefault(u => u.Username == username);
            List<Project> list = new List<Project>();
            foreach (Access item in db.Access.Where(x => x.UserID == user.ID && ((x.DateFrom >= from && x.DateFrom <= to) || (x.DateTo <= to && x.DateTo >= from) || (x.DateFrom <= from && x.DateTo >= to))).ToList())
            {
                if(item.ProjectID != 0)
                    list.Add(item.Project);
            }
            foreach (Project item in db.Project.Where(x => x.ManagerID == user.ID && !x.Status.Equals("Completed") && ((x.StartDate >= from && x.StartDate <= to) || (x.EndDate <= to && x.EndDate >= from) || (x.StartDate <= from && x.EndDate >= to))).ToList())
            {
                if(item.ID != 0)
                    list.Add(item);
            }

            return list;
        }

        public bool IsAccessibleOnDate(DateTime date, int projectId, int userId)
        {

            if (db.Access.Any(x => x.UserID == userId && x.ProjectID == projectId && (x.DateFrom <= date && x.DateTo >= date)))
                return true;
            else
                return db.Project.Any(x => x.ManagerID == userId && (x.StartDate <= date && x.EndDate >= date));
        }
        #endregion




        #region Customer Service
        public void DeleteCustomer(int itemId)
        {
            var item = db.Customer.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a customer");
            db.Customer.Remove(item);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                db.Dispose();
                ContextSet();
                throw new Exception("You cannot delete this record.");
            }
        }

        public List<Customer> GetAllCustomers()
        {
            return db.Customer.ToList();
        }

        public Customer GetCustomerById(int Id)
        {
            var item = db.Customer.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Customer does not exist");
            return item;
        }

        public void Insert(Customer item)
        {
            if (db.Customer.Any(o => o.TIN == item.TIN))
            {
                throw new Exception("THE TIN YOU ENTERED ALREADY EXISTS IN THE DATABASE");
            }
            else
            {
                db.Customer.Add(item);
                db.SaveChanges();
            }
        }

        public void Update(Customer item)
        {
            var dbItem = db.Customer.FirstOrDefault(p => p.ID == item.ID);
            if (db.Customer.Any(o => o.TIN == item.TIN))

                if (dbItem == null)
                    throw new Exception("Customer does not exist");
            if (item.ID == 0)
                return;
            dbItem.Name = item.Name;
            dbItem.Address = item.Address;
            dbItem.ContactPhone = item.ContactPhone;
            dbItem.GenDirector = item.GenDirector;
            dbItem.ContactPerson = item.ContactPerson;
            dbItem.TIN = item.TIN;
            db.Entry(dbItem).State = EntityState.Modified;
            db.SaveChanges();
        }
        #endregion

        #region Position Service
        public void DeletePosition(int itemId)
        {
            var item = db.Position.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a position");

            db.Position.Remove(item);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                db.Dispose();
                ContextSet();
                throw new Exception("You cannot delete this record.");
            }
        }

        public List<Position> GetAllPositions()
        {
            return db.Position.ToList();
        }

        public Position GetPositionById(int Id)
        {
            var item = db.Position.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Position does not exist");
            return item;
        }

        public void Insert(Position item)
        {
            db.Position.Add(item);
            db.SaveChanges();
        }

        public void Update(Position item)
        {
            var dbItem = db.Position.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Position does not exist");
            dbItem.Name = item.Name;

            db.SaveChanges();
        }
        #endregion

        #region Role Service
        public void DeleteRole(int itemId)
        {
            var item = db.Role.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a role");
            if (db.Access.FirstOrDefault(p => p.RoleID == item.ID) != null)
            {
                throw new Exception("You cannot delete this record. Project role is in use!");
            }
            db.Role.Remove(item);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                db.Dispose();
                ContextSet();
                throw new Exception("You cannot delete this record.");
            }
        }

        public List<Role> GetAllRoles()
        {
            return db.Role.ToList();
        }

        public Role GetRoleById(int Id)
        {
            var item = db.Role.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Role does not exist");
            return item;
        }

        public void Insert(Role item)
        {
            db.Role.Add(item);
            db.SaveChanges();
        }

        public void Update(Role item)
        {
            var dbItem = db.Role.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Role does not exist");
            dbItem.Name = item.Name;

            db.SaveChanges();
        }
        #endregion

        #region Workload Type Service


        //WorkloadType
        public List<WorkloadType> GetAllWorkloadTypes()
        {
            return db.WorkloadType.ToList();
        }

        public void DeleteWorkloadType(int itemId)
        {
            var item = db.WorkloadType.FirstOrDefault(p => p.ID == itemId);
            if (item == null)
                throw new ArgumentNullException("You cannot delete a type");
            db.WorkloadType.Remove(item);
            try
            {
                db.SaveChanges();
            }
            catch (Exception e)
            {
                db.Dispose();
                ContextSet();
                throw new Exception("You cannot delete this record.");
            }
        }


        public WorkloadType GetWorkloadTypeById(int Id)
        {
            var item = db.WorkloadType.FirstOrDefault(p => p.ID == Id);
            if (item == null)
                throw new ArgumentNullException("Type does not exist");
            return item;
        }

        public void Insert(WorkloadType item)
        {
            db.WorkloadType.Add(item);
            db.SaveChanges();
        }

        public void Update(WorkloadType item)
        {
            var dbItem = db.WorkloadType.FirstOrDefault(p => p.ID == item.ID);
            if (dbItem == null)
                throw new ArgumentNullException("Type does not exist");
            dbItem.Name = item.Name;

            db.SaveChanges();
        }
        #endregion

        #region Notification Service
        public List<Notification> GetAllNotifications()
        {
            try
            {
                return db.Notification.ToList();
            }
            catch { return new List<Notification>(); }
        }
        public bool MarkNotificationAsReadSuccessfully(int id)
        {
            var notification = db.Notification.FirstOrDefault(x => x.ID == id);
            if (notification == null)
                return false;
            else
                notification.ISREAD = true;
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception e) { return false; }
        }

        public bool DeleteSelectedNotificationSuccessfully(int id)
        {
            var notification = db.Notification.FirstOrDefault(x => x.ID == id);
            if (notification == null)
                return false;
            else
                db.Notification.Remove(notification);
            try
            {
                db.SaveChanges();
                return true;
            }
            catch (Exception e) { return false; }
        }

        public bool MarkAllAsReadSuccessfully(string username)
        {
            var userId = GetUserIdByUsername(username);
            foreach (var i in db.Notification.Where(x=>x.TOID==userId).ToList())
            {
                i.ISREAD = true;                
            }

            try { db.SaveChanges(); return true; }
            catch { return false; }
        }
        #endregion


        /// <summary>
        /// Gets all notifications for User ID up to specified Date
        /// </summary>
        /// <param name="userId">Id of the user to search by</param>
        /// <param name="upToDate">The max date the notifications for which will be get</param>
        /// <param name="OnlyUnread">Returning notifications should be unread or all of them?. True by default (only new notifications)</param>
        /// <returns>List of Notifications</returns>
        public List<Notification> GetNotificationsForUser(int userId, DateTime upToDate, bool OnlyUnread = true)
        {
            var returning = new List<Notification>();
            ContextSet();
            if (OnlyUnread)
            {
                if (db.Notification.ToList().Count != 0) // && x.ISREAD == false
                    returning = db.Notification.Where(x => x.TOID == userId && x.Date <= upToDate && x.ISREAD == false).ToList();
            }
            else
            {
                var temp = db.Notification.Where(x => x.TOID == userId && x.Date <= upToDate).ToList();
                if (temp != null)
                    returning = temp.ToList();
            }
            return returning;
        }
    }
}
