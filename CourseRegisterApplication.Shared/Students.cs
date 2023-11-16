namespace CourseRegisterApplication.Shared
{
    public class Students
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Phone { get; set; }

        public Students(string name, string id, string phone)
        {
            Name = name;
            Id = id;
            Phone = phone;
        }
    }
}
