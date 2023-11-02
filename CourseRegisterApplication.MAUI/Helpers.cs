namespace CourseRegisterApplication_SE214   
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
    }
}
