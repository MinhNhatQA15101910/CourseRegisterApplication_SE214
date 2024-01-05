using CourseRegisterApplication.MAUI.IServices;

namespace CourseRegisterApplication.MAUI.Services
{
    public class StudentService : IStudentService
    {
        private readonly HttpClient _httpClient;
        private readonly IDistrictService _districtService;
        private readonly IPriorityTypeService _priorityTypeService;
        private readonly IStudentPriorityTypeService _studentPriorityTypeService;

        public StudentService(HttpClient httpClient, IDistrictService districtService, IPriorityTypeService priorityTypeService, IStudentPriorityTypeService studentPriorityTypeService)
        {
            _httpClient = httpClient;
            _districtService = districtService;
            _priorityTypeService = priorityTypeService;
            _studentPriorityTypeService = studentPriorityTypeService;
        }

        public async Task<Student> AddStudent(Student student, List<PriorityType> priorityTypes)
        {
            string apiUrl = GlobalConfig.STUDENT_BASE_URL;

            var json = JsonConvert.SerializeObject(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                // If student district is priority, add priority type with Id = 2.
                District district = await _districtService.GetDistrictById(student.DistrictId);
                if (district.IsPriority)
                {
                    List<PriorityType> priorityTypeList = (await _priorityTypeService.GetAllPriorityTypesAsync()).ToList();
                    PriorityType priorityType = priorityTypeList.First(pt => pt.Id == 2);

                    priorityTypes.Add(priorityType);

                    var temp = priorityTypes.Where(pt => pt.Id == 1);
                    if (temp.Any())
                    {
                        priorityTypes.Remove(temp.ElementAt(0));
                    }
                }

                // Add priority types
                foreach (var priorityType in priorityTypes)
                {
                    Student studentFromDatabase = await GetStudentBySpecificId(student.StudentSpecificId);

                    StudentPriorityType studentPriorityType = new()
                    {
                        PriorityTypeId = priorityType.Id,
                        StudentId = studentFromDatabase.Id
                    };

                    StudentPriorityType result = await _studentPriorityTypeService.AddStudentPriorityType(studentPriorityType);
                    if (result == null)
                    {
                        return null;
                    }
                }

                return student;
            }

            return null;
        }

        public async Task<List<Student>> GetAllStudents()
        {
            string apiUrl = GlobalConfig.STUDENT_BASE_URL;

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var studentList = JsonConvert.DeserializeObject<List<Student>>(jsonResponse);
                return studentList;
            }

            return null;
        }

        public async Task<List<Student>> GetFullInformationOfAllStudents()
        {
            string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}full/";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var studentList = JsonConvert.DeserializeObject<List<Student>>(jsonResponse);
                return studentList;
            }

            return null;
        }

        public async Task<Student> GetFullInformationOfStudentBySpecificId(string studentSpecificId)
        {
            string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}full/specificId/{studentSpecificId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Student>(jsonResponse);
            }

            return null;
        }

        public async Task<Student> GetStudentBySpecificId(string studentSpecificId)
        {
            string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}specificId/{studentSpecificId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<Student>(jsonResponse);
            }

            return null;
        }

        public async Task<List<Student>> GetStudentsByBranchId(int branchId)
        {
            string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}branch/{branchId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var studentList = JsonConvert.DeserializeObject<List<Student>>(jsonResponse);
                return studentList;
            }

            return null;
        }

        public async Task<List<Student>> GetStudentsByDistrictId(int districtId)
        {
            string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}district/{districtId}";

            var response = await _httpClient.GetAsync(new Uri(apiUrl));
            if (response.IsSuccessStatusCode)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                var studentList = JsonConvert.DeserializeObject<List<Student>>(jsonResponse);
                return studentList;
            }

            return null;
        }

        public async Task<bool> UpdateImageUrl(Student student, string newImageUrl)
        {
            student.ImageUrl = newImageUrl;

            string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}{student.Id}";

            var json = JsonConvert.SerializeObject(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateStudent(int studentId, Student student, List<PriorityType> priorityTypes)
        {
            string apiUrl = $"{GlobalConfig.STUDENT_BASE_URL}{studentId}";

            var json = JsonConvert.SerializeObject(student);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync(new Uri(apiUrl), content);

            if (response.IsSuccessStatusCode)
            {
                // If student district is priority, add priority type with Id = 2.
                District district = await _districtService.GetDistrictById(student.DistrictId);
                if (district.IsPriority)
                {
                    List<PriorityType> priorityTypeList = (await _priorityTypeService.GetAllPriorityTypesAsync()).ToList();
                    PriorityType priorityType = priorityTypeList.First(pt => pt.Id == 2);

                    priorityTypes.Add(priorityType);

                    var temp = priorityTypes.Where(pt => pt.Id == 1);
                    if (temp.Any())
                    {
                        priorityTypes.Remove(temp.ElementAt(0));
                    }
                }

                // Delete old priority types
                List<StudentPriorityType> studentPriorityTypes = await _studentPriorityTypeService.GetStudentPriorityTypesByStudentId(student.Id);
                foreach (var studentPriorityType in studentPriorityTypes)
                {
                    await _studentPriorityTypeService.DeleteStudentPriorityType(studentPriorityType.StudentId, studentPriorityType.PriorityTypeId);
                }

                // Add new priority types
                foreach (var priorityType in priorityTypes)
                {
                    StudentPriorityType studentPriorityType = new()
                    {
                        PriorityTypeId = priorityType.Id,
                        StudentId = student.Id
                    };

                    StudentPriorityType result = await _studentPriorityTypeService.AddStudentPriorityType(studentPriorityType);
                    if (result == null)
                    {
                        return false;
                    }
                }

                return true;
            }

            return false;
        }
    }
}
