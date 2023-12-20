using System;
using System.Text;

namespace JavaFlorist.Helpers
{
	public class MyUtil
	{
        public static string UploadHinh(IFormFile Avatar, string folder)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "img", folder, Avatar.FileName);

                using (var myfile = new FileStream(fullPath, FileMode.CreateNew))
                {
                    Avatar.CopyTo(myfile);
                }
                return Avatar.FileName;
            }
            catch (Exception ex)
            {
                return string.Empty;
            }
        }
        public static string GenerateRamdomKey(int length = 5)
        {
            var pattern = @"qwertyuiopasdfghjKLZXVBNMIUYTF!";
            var sb = new StringBuilder();
            var rd = new Random();
            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }
            return sb.ToString();
        }
    }
}

