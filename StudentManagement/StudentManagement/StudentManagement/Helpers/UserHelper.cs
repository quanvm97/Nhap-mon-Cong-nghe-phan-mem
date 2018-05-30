using StudentManagement.Interfaces;
using StudentManagement.Models;
using System.Collections.Generic;
using System.Linq;

namespace StudentManagement.Helpers
{
    public class UserHelper
    {
        private List<User> _users;
        private static UserHelper _instance;
        public static UserHelper Instance => _instance ?? (_instance = new UserHelper());

        public UserHelper()
        {
            InitData();
        }

        public bool Login(ISQLiteHelper db, string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.UserName.ToLower().Equals(username.Trim().ToLower())
                                                  && u.Password.Equals(password));
            if (user == null)
                return false;

            db.DeleteAll<User>();
            db.Insert(user);

            return true;
        }

        public bool ConfirmPassword(string name, string password)
        {
            var user = _users.FirstOrDefault(u => u.Name.ToLower().Equals(name.Trim().ToLower()));

            if (user == null)
                return false;

            if (user.Password.Equals(password))
                return true;

            return false;
        }

        public void Logout(ISQLiteHelper db)
        {
            db.DeleteAll<User>();
        }


        private void InitData()
        {
            _users = new List<User>
            {
                new User
                {
                    Id = 10336,
                    Name = "Phạm Văn Ngọc",
                    Avatar = "student_boy.png",
                    Role = RoleManager.StudentRole,
                    UserName = "ngocpv@gmail.com",
                    Password = "ngoc123",
                    ClassId = 2009,
                },
                new User
                {
                    Id = 10337,
                    Name = "Vũ Minh Hoàng",
                    Avatar = "student_boy.png",
                    Role = RoleManager.StudentRole,
                    UserName = "hoangvm@gmail.com",
                    Password = "hoang123",
                    ClassId = 2001
                },
                new User
                {
                    Id = 1,
                    Name = "Hoàng Thị Thảo",
                    Avatar = "teacher_1.png",
                    Role = RoleManager.TeacherRole,
                    UserName = "thaoht@gmail.com",
                    Password = "thao123",
                    ClassId = 2005
                },
                new User
                {
                    Id = 1,
                    Name = "Võ Minh Quân",
                    Avatar = "teacher_5.png",
                    Role = RoleManager.PrincipalRole,
                    UserName = "quanvm@gmail.com",
                    Password = "quan123"
                },
            };
        }
    }
}
