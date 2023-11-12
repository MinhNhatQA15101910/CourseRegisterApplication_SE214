﻿namespace CourseRegisterApplication.Shared
{
    public class Student
    {
        public int Id { get; set; }
        public string? StudentSpecificId { get; set; }
        public string? FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public Gender Gender { get; set; }

        [ForeignKey("District")]
        public int DistrictId { get; set; }
        public District? District { get; set; }

        [ForeignKey("Branch")]
        public int BranchId { get; set; }
        public Branch? Branch { get; set; }

        public ICollection<StudentPriorityType>? PriorityTypes { get; }
    }
}
