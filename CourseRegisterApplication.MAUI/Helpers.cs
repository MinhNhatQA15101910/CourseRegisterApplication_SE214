namespace CourseRegisterApplication.MAUI
{
    public static class Helpers
    {
        public static string EncryptData(string data)
        {
            try
            {
                byte[] encDataByte = Encoding.UTF8.GetBytes(data!);
                string encodeData = Convert.ToBase64String(encDataByte);
                return encodeData;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in base64Encode " + ex.Message);
            }
        }

        public static string GeneratePassword()
        {
            string password = "";
            for (int i = 0; i < 10; i++)
            {
                Random random = new Random();
                password += random.Next(0, 9).ToString();
            }

            return password;
        }

        public static string FormatDateTime(DateTime dateTime)
        {
            int day = dateTime.Day;

            if (day == 1 || day == 21 || day == 31)
            {
                return "Today is " + dateTime.ToString("dddd, MMMM d\"st\", yyyy");
            }

            if (day == 2 || day == 22)
            {
                return "Today is " + dateTime.ToString("dddd, MMMM d\"nd\", yyyy");
            }

            if (day == 3 || day == 23)
            {
                return "Today is " + dateTime.ToString("dddd, MMMM d\"rd\", yyyy");
            }

            return "Today is " + dateTime.ToString("dddd, MMMM d\"th\", yyyy");
        }

		public static int CalculateAge(DateTime dateOfBirth)
		{
			DateTime currentDate = DateTime.Now;
			int age = currentDate.Year - dateOfBirth.Year;
			if (currentDate.Month < dateOfBirth.Month || (currentDate.Month == dateOfBirth.Month && currentDate.Day < dateOfBirth.Day))
			{
				age--;
			}
			return age;
		}
	}
}
