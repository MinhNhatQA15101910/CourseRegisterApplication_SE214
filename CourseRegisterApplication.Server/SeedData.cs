namespace CourseRegisterApplication.Server
{
    public static class SeedData
    {
        private static string USERS_FILE_PATH = "Resources/users.txt";
        private static string PROVINCES_FILE_PATH = "Resources/provinces.txt";
        private static string DISTRICTS_FILE_PATH = "Resources/districts.txt";
        private static string DEPARTMENTS_FILE_PATH = "Resources/departments.txt";
        private static string BRANCHES_FILE_PATH = "Resources/branches.txt";
        private static string PRIORITY_TYPES_FILE_PATH = "Resources/priority-types.txt";
        private static string STUDENTS_FILE_PATH = "Resources/students.txt";
        private static string STUDENT_PRIORITY_TYPES_FILE_PATH = "Resources/student-priority-types.txt";
        private static string SUBJECT_TYPES_FILE_PATH = "Resources/subject-types.txt";
        private static string SUBJECTS_FILE_PATH = "Resources/subjects.txt";
        private static string CURRICULUMS_FILE_PATH = "Resources/curriculums.txt";
        private static string SEMESTERS_FILE_PATH = "Resources/semesters.txt";
        private static string AVAILABLE_COURSES_FILE_PATH = "Resources/available-courses.txt";

        public static void Initialize(ModelBuilder modelBuilder)
        {
            InitializeUsers(modelBuilder);
            InitializeProvinces(modelBuilder);
            InitializeDistricts(modelBuilder);
            InitializeDepartments(modelBuilder);
            InitializeBranches(modelBuilder);
            InitializePriorityTypes(modelBuilder);
            InitializeStudents(modelBuilder);
            InitializeStudentPriorityTypes(modelBuilder);
            InitializeSubjectTypes(modelBuilder);
            InitializeSubjects(modelBuilder);
            InitializeCurriculums(modelBuilder);
            InitializeSemester(modelBuilder);
            InitializeAvailableCourses(modelBuilder);
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
                            Email = studentData[0].Trim() + "@gm.uit.edu.vn",
                            FullName = studentData[1].Trim(),
                            DateOfBirth = DateTime.ParseExact(studentData[2].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            Gender = (studentData[3].Trim() == "Nam") ? Gender.Male : Gender.Female,
                            DistrictId = int.Parse(studentData[4].Trim()),
                            BranchId = int.Parse(studentData[5].Trim()),
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
                            PriorityName = priorityTypeData[0].Trim(),
                            TuitionDiscountRate = float.Parse(priorityTypeData[1].Trim())
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
                            BranchSpecificId = branchData[0].Trim(),
                            BranchName = branchData[1].Trim(),
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
                            DepartmentSpecificId = departmentData[0].Trim(),
                            DepartmentName = departmentData[1].Trim()
                        });
                    }

                    modelBuilder.Entity<Department>().HasData(departments);
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

                        Role role = Role.Admin;
                        if (int.Parse(userData[3].Trim()) == 2)
                        {
                            role = Role.Accountant;
                        }
                        else if (int.Parse(userData[3].Trim()) == 3)
                        {
                            role = Role.Student;
                        }

                        users.Add(new User
                        {
                            Id = userId++,
                            Username = userData[0].Trim(),
                            Password = userData[1].Trim(),
                            Email = userData[2].Trim(),
                            Role = role
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
                            ProvinceName = provinceData[0].Trim()
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
                            DistrictName = districtData[0].Trim(),
                            IsPriority = districtData[1].Trim() == "1",
                            ProvinceId = int.Parse(districtData[2].Trim())
                        });
                    }

                    modelBuilder.Entity<District>().HasData(districts);
                }
            }
        }

        public static void InitializeSubjectTypes(ModelBuilder modelBuilder)
        {
            var subjectTypes = new List<SubjectType>();

            if (File.Exists(SUBJECT_TYPES_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(SUBJECT_TYPES_FILE_PATH))
                {
                    int subjectTypeId = 1;
                    string? subjectTypeLine;

                    while((subjectTypeLine = sr.ReadLine()) != null)
                    {
                        string[] subjectTypeData = subjectTypeLine!.Split(",");

                        subjectTypes.Add(new SubjectType
                        {
                            Id = subjectTypeId++,
                            Name = subjectTypeData[0].Trim(),
                            NumberOfLessons = int.Parse(subjectTypeData[1].Trim()),
                            LessonsCharge = double.Parse(subjectTypeData[2].Trim()),
                        });
                    }

                    modelBuilder.Entity<SubjectType>().HasData(subjectTypes);
                }
            }
        }

        public static void InitializeSubjects(ModelBuilder modelBuilder)
        {
            var subjects = new List<Subject>();

            if (File.Exists(SUBJECTS_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(SUBJECTS_FILE_PATH))
                {
                    int subjectId = 1;
                    string? subjectLine;

                    while ((subjectLine = sr.ReadLine()) != null)
                    {
                        string[] subjectData = subjectLine!.Split(",");

                        subjects.Add(new Subject { 
                            Id = subjectId++,
                            SubjectSpecificId = subjectData[0].Trim(),
                            Name = subjectData[1].Trim(),
                            NumberOfCredits = int.Parse(subjectData[2].Trim()),
                            TotalLessons = int.Parse(subjectData[3].Trim()),
                            TotalCharge = double.Parse(subjectData[4].Trim()),
                            SubjectTypeId = int.Parse(subjectData[5].Trim()),
                        });
                    }

                    modelBuilder.Entity<Subject>().HasData(subjects);
                }
            }
        }

        public static void InitializeCurriculums(ModelBuilder modelBuilder) {
            var curriculums = new List<Curriculum>();

            if (File.Exists(CURRICULUMS_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(CURRICULUMS_FILE_PATH))
                {
                    string? curriculumLine;

                    while ((curriculumLine = sr.ReadLine()) != null)
                    {
                        string[]? curriculumData = curriculumLine!.Split(',');

                        curriculums.Add(new Curriculum
                        {
                            BranchId = int.Parse(curriculumData[0].Trim()),
                            SubjectId = int.Parse(curriculumData[1].Trim()),
                            Semester = int.Parse(curriculumData[2].Trim())
                        });
                    }

                    modelBuilder.Entity<Curriculum>().HasData(curriculums);
                }
            }
        }

        public static void InitializeSemester(ModelBuilder modelBuilder)
        {
            var semesters = new List<Semester>();

            if (File.Exists(SEMESTERS_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(SEMESTERS_FILE_PATH))
                {
                    int semesterId = 1;
                    string? semesterLine;

                    while ((semesterLine = sr.ReadLine()) != null)
                    {
                        string[]? semesterData = semesterLine!.Split(',');

                        semesters.Add(new Semester
                        {
                            Id = semesterId++,
                            SemesterName = (int.Parse(semesterData[0]) == 1) ? SemesterName.FirstSemester : ((int.Parse(semesterData[0]) == 2) ? SemesterName.SecondSemester : SemesterName.SummerSemester),
                            Year = int.Parse(semesterData[1].Trim()),
                            StartRegistrationDate = DateTime.ParseExact(semesterData[2].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            EndRegistrationDate = DateTime.ParseExact(semesterData[3].Trim(), "dd/MM/yyyy", CultureInfo.InvariantCulture),
                            MinimumCredits = int.Parse(semesterData[4].Trim()),
                            MaximumCredits = int.Parse(semesterData[5].Trim()),
                        });
                    }

                    modelBuilder.Entity<Semester>().HasData(semesters);
                }
            }
        }

        public static void InitializeAvailableCourses(ModelBuilder modelBuilder)
        {
            var availableCourses = new List<AvailableCourse>();

            if (File.Exists(AVAILABLE_COURSES_FILE_PATH))
            {
                using (StreamReader sr = new StreamReader(AVAILABLE_COURSES_FILE_PATH))
                {
                    string? availableCourseLine;

                    while ((availableCourseLine = sr.ReadLine()) != null)
                    {
                        string[]? availableCourseData = availableCourseLine!.Split(',');

                        availableCourses.Add(new AvailableCourse
                        {
                            SemesterId = int.Parse(availableCourseData[0].Trim()),
                            SubjectId = int.Parse(availableCourseData[1].Trim()),
                        });
                    }

                    modelBuilder.Entity<AvailableCourse>().HasData(availableCourses);
                }
            }
        }
    }
}
