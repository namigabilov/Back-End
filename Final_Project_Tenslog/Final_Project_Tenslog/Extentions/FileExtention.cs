namespace Final_Project_Tenslog.Extentions
{
    public static class FileExtention
    {
        public static async Task<string> CreateFileAsync(this IFormFile file, IWebHostEnvironment env, params string[] folders)
        {
            string fileName = $"{Guid.NewGuid().ToString()}-{DateTime.Now.ToString("yyyyMMddHHmmssfff")}-{file.FileName}";

            string filePath = Path.Combine(env.WebRootPath);
            foreach (string folder in folders)
            {
                filePath = Path.Combine(filePath, folder);
            }
            filePath = Path.Combine(filePath, fileName);

            using (FileStream stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            return fileName;
        }
    }
}
