using System.Drawing;
using System.Text.RegularExpressions;

namespace Fruit_N12.Areas.Admin.Models
{
    public static class UploadOnRootAndConvertToUrLImageModel
    {
        public static string UploadOnRootAndConvertToUrlImage(string imageBase64)
        {
            // Loại bỏ phần tiền tố "data:image/xxx;base64," nếu có
            if (imageBase64.Contains("data:image"))
            {
                int commaIndex = imageBase64.IndexOf(',');
                imageBase64 = imageBase64.Substring(commaIndex + 1);
            }

            // Chuyển chuỗi Base64 thành mảng byte
            byte[] imageBytes = Convert.FromBase64String(imageBase64);

            // Đường dẫn thư mục lưu ảnh
            var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/img/product");

            // Kiểm tra nếu thư mục chưa tồn tại thì tạo thư mục
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Lấy phần mở rộng của file (ví dụ: .jpg, .png) từ chuỗi Base64
            string extension = GetImageExtension(imageBase64);

            // Tạo tên file duy nhất cho ảnh
            string fileName = Guid.NewGuid().ToString() + extension;

            // Đường dẫn hoàn chỉnh đến file hình ảnh
            string filePath = Path.Combine(uploadsFolder, fileName);

            // Lưu mảng byte vào file
            File.WriteAllBytes(filePath, imageBytes);

            // Trả về tên file hoặc đường dẫn URL để hiển thị ảnh
            return $"/assets/img/product/{fileName}";
        }

        // Hàm xác định phần mở rộng của hình ảnh từ chuỗi Base64
        private static string GetImageExtension(string base64String)
        {
            if (base64String.Contains("data:image/jpeg"))
                return ".jpg";
            if (base64String.Contains("data:image/png"))
                return ".png";
            if (base64String.Contains("data:image/gif"))
                return ".gif";
            return ".jpg"; // Default
        }


        public static bool IsBase64String(string str)
        {

            try
            {
                // Loại bỏ phần tiền tố "data:image/xxx;base64," nếu có
                if (str.Contains("data:image"))
                {
                    int commaIndex = str.IndexOf(',');
                    str = str.Substring(commaIndex + 1);
                }

                // Giải mã Base64 thành byte[]
                byte[] imageBytes = Convert.FromBase64String(str);

                // Kiểm tra xem có thể chuyển đổi thành hình ảnh không
                using (MemoryStream ms = new MemoryStream(imageBytes))
                {
                    using (Image img = Image.FromStream(ms))
                    {
                        return true; // Nếu không lỗi, đây là hình ảnh
                    }
                }
            }
            catch
            {
                return false; // Nếu lỗi, không phải hình ảnh
            }
        }
    }
}
