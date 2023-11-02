﻿using System.Globalization;

namespace CourseRegisterApplication.Server
{
    public static class SeedData
    {
        private static string ROLES_FILE_PATH = "Resources/roles.txt";
        private static string USERS_FILE_PATH = "Resources/users.txt";
        private static string PROVINCES_FILE_PATH = "Resources/provinces.txt";
        private static string DISTRICTS_FILE_PATH = "Resources/districts.txt";
        private static string DEPARTMENTS_FILE_PATH = "Resources/departments.txt";
        private static string BRANCHES_FILE_PATH = "Resources/branches.txt";
        private static string PRIORITY_TYPES_FILE_PATH = "Resources/priority-types.txt";
        private static string STUDENTS_FILE_PATH = "Resources/students.txt";
        private static string STUDENT_PRIORITY_TYPES_FILE_PATH = "Resources/student-priority-types.txt";

        public static void Initialize(ModelBuilder modelBuilder)
        {
            InitializeRoles(modelBuilder);
            InitializeUsers(modelBuilder);
            InitializeProvinces(modelBuilder);
            InitializeDistricts(modelBuilder);
            InitializeDepartments(modelBuilder);
            InitializeBranches(modelBuilder);
            InitializePriorityTypes(modelBuilder);
            InitializeStudents(modelBuilder);
            InitializeStudentPriorityTypes(modelBuilder);
        }

        private static void InitializeStudentPriorityTypes(ModelBuilder modelBuilder)
        {
            var studentPriorityTypes = new List<StudentPriorityType>();

            if (File.Exists(STUDENT_PRIORITY_TYPES_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(STUDENT_PRIORITY_TYPES_FILE_PATH))
                {
                    string? studentPriorityTypeLine;

                    while ((studentPriorityTypeLine = sr.ReadLine()) != null)
                    {
                        string[]? studentPriorityTypeData = studentPriorityTypeLine!.Split(',');

                        studentPriorityTypes.Add(new StudentPriorityType
                        {
                            StudentId = int.Parse(studentPriorityTypeData[0].Trim()),
                            PriorityTypeId = int.Parse(studentPriorityTypeData[1].Trim()),
                        });
                    }

                    modelBuilder.Entity<StudentPriorityType>().HasData(studentPriorityTypes);
                }
            }
        }

        private static void InitializeStudents(ModelBuilder modelBuilder)
        {
            var students = new List<Student>();

            if (File.Exists(STUDENTS_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(STUDENTS_FILE_PATH))
                {
                    int studentId = 1;
                    string? studentLine;

                    while ((studentLine = sr.ReadLine()) != null)
                    {
                        string[]? studentData = studentLine!.Split(',');

                        students.Add(new Student
                        {
                            Id = studentId++,
                            StudentSpecificId = studentData[0].Trim(),
                            FullName = studentData[1].Trim(),
                            DateOfBirth = DateTime.ParseExact(studentData[2].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Gender = (studentData[3].Trim() == "Nam") ? Gender.Male : Gender.Female,
                            DistrictId = int.Parse(studentData[4].Trim()),
                            BranchId = int.Parse(studentData[5].Trim())
                        });
                    }

                    modelBuilder.Entity<Student>().HasData(students);
                }
            }
        }

        private static void InitializePriorityTypes(ModelBuilder modelBuilder)
        {
            var priorityTypes = new List<PriorityType>();

            if (File.Exists(PRIORITY_TYPES_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(PRIORITY_TYPES_FILE_PATH))
                {
                    int priorityTypeId = 1;
                    string? priorityTypeLine;

                    while ((priorityTypeLine = sr.ReadLine()) != null)
                    {
                        string[]? priorityTypeData = priorityTypeLine!.Split(',');

                        priorityTypes.Add(new PriorityType
                        {
                            Id = priorityTypeId++,
                            PriorityName = priorityTypeData[0],
                            TuitionDiscountRate = float.Parse(priorityTypeData[1])
                        });
                    }

                    modelBuilder.Entity<PriorityType>().HasData(priorityTypes);
                }
            }
        }

        private static void InitializeBranches(ModelBuilder modelBuilder)
        {
            var branchs = new List<Branch>();

            if (File.Exists(BRANCHES_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(BRANCHES_FILE_PATH))
                {
                    int branchId = 1;
                    string? branchLine;

                    while ((branchLine = sr.ReadLine()) != null)
                    {
                        string[]? branchData = branchLine!.Split(',');

                        branchs.Add(new Branch
                        {
                            Id = branchId++,
                            BranchSpecificId = branchData[0],
                            BranchName = branchData[1],
                            DepartmentId = int.Parse(branchData[2])
                        });
                    }

                    modelBuilder.Entity<Branch>().HasData(branchs);
                }
            }
        }

        private static void InitializeDepartments(ModelBuilder modelBuilder)
        {
            var departments = new List<Department>();

            if (File.Exists(DEPARTMENTS_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(DEPARTMENTS_FILE_PATH))
                {
                    int departmentId = 1;
                    string? departmentLine;

                    while ((departmentLine = sr.ReadLine()) != null)
                    {
                        string[]? departmentData = departmentLine!.Split(',');

                        departments.Add(new Department
                        {
                            Id = departmentId++,
                            DepartmentSpecificId = departmentData[0],
                            DepartmentName = departmentData[1]
                        });
                    }

                    modelBuilder.Entity<Department>().HasData(departments);
                }
            }
        }

        public static void InitializeRoles(ModelBuilder modelBuilder) 
        {
            var roles = new List<Role>();

            if (File.Exists(ROLES_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(ROLES_FILE_PATH))
                {
                    int roleId = 1;
                    string? roleLine;

                    while ((roleLine = sr.ReadLine()) != null)
                    {
                        string[]? roleData = roleLine!.Split(',');

                        roles.Add(new Role
                        {
                            Id = roleId++,
                            RoleName = roleData[0]
                        });
                    }

                    modelBuilder.Entity<Role>().HasData(roles);
                }
            }
        }

        public static void InitializeUsers(ModelBuilder modelBuilder)
        {
            var users = new List<User>();

            if (File.Exists(USERS_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(USERS_FILE_PATH))
                {
                    int userId = 1;
                    string? userLine;
                    
                    while ((userLine = sr.ReadLine()) != null)
                    {
                        string[]? userData = userLine!.Split(',');

                        users.Add(new User
                        {
                            Id = userId++,
                            Username = userData[0],
                            Password = userData[1],
                            Email = userData[2],
                            RoleId = int.Parse(userData[3])
                        });
                    }

                    modelBuilder.Entity<User>().HasData(users);
                }
            }
        }

        public static void InitializeProvinces(ModelBuilder modelBuilder)
        {
            var provinces = new List<Province>();

            if (File.Exists(PROVINCES_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(PROVINCES_FILE_PATH))
                {
                    int provinceId = 1;
                    string? provinceLine;

                    while ((provinceLine = sr.ReadLine()) != null)
                    {
                        string[]? provinceData = provinceLine!.Split(',');

                        provinces.Add(new Province
                        {
                            Id = provinceId++,
                            ProvinceName = provinceData[0]
                        });
                    }

                    modelBuilder.Entity<Province>().HasData(provinces);
                }
            }
        }

        public static void InitializeDistricts(ModelBuilder modelBuilder)
        {
            var districts = new List<District>();

            if (File.Exists(DISTRICTS_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(DISTRICTS_FILE_PATH))
                {
                    int districtId = 1;
                    string? districtLine;

                    while ((districtLine = sr.ReadLine()) != null)
                    {
                        string[]? districtData = districtLine!.Split(',');

                        districts.Add(new District
                        {
                            Id = districtId++,
                            DistrictName = districtData[0],
                            IsPriority = districtData[1] == " 1",
                            ProvinceId = int.Parse(districtData[2])
                        });
                    }

                    modelBuilder.Entity<District>().HasData(districts);
                }
            }
        }
    }
}
