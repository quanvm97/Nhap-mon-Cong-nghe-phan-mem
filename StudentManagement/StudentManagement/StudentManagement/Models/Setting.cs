using SQLite.Net.Attributes;

namespace StudentManagement.Models
{
    public class Setting
    {
        //bảng Setting gồm các cột Id, MaxStudentPerClass, MaxStudentAge, MinStudentAge, SubjectPassScore và IsInitData.
        [PrimaryKey]
        public int Id { get; set; }

        public int MaxStudentPerClass { get; set; }

        public int MaxStudentAge { get; set; }

        public int MinStudentAge { get; set; }

        public float SubjectPassScore { get; set; }

        public bool IsInitData { get; set; }
    }
}
