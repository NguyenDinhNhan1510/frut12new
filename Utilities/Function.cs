using System.Security.Cryptography;
using System.Text;

namespace Fruit_N12.Utilities
{
    public class Function
    {

        public static int _UserID = 0;
        public static string _UserName = String.Empty;
        public static string _Email = String.Empty;
        public static string _Message = String.Empty;
        public static string _MessageEmail = String.Empty;
        public static string TitleSlugGenerationAlias(string title)
        {
            return SlugGenerator.SlugGenerator.GenerateSlug(title);
        }

        public static string MD5Hash(string text)
        {
            MD5 md5 = new MD5CryptoServiceProvider();
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(text));
            byte[] result = md5.Hash;
            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                strBuilder.Append(result[i].ToString("x2"));
            }
            return strBuilder.ToString();
        }

        public static string MD5Password(string? text)
        {
            string str = MD5Hash(text);
            //Lặp thêm 5 lần mã hóa xâu đảm bảo tính bảo mật 
            // Mỗi lần lặp nhân đôi xâu mã hóa, ở giữa thêm "_"

            for (int i = 0; i < 5; i++)
            {
                str = MD5Hash(str + "_" + str);
            }
            return str;
        }

        public static bool IsLogin()
        {
            if (string.IsNullOrEmpty(Function._UserName) || string.IsNullOrEmpty(Function._Email) || (Function._UserID <= 0))
                return false;
            return true;
        }

    }
}
