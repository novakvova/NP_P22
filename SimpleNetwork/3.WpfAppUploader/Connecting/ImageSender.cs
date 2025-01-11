using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace _3.WpfAppUploader.Connecting
{
    public static class ImageSender
    {
        public static async Task<string> UploadImageAsync(string imagePath, HttpClient httpClient)
        {
            try
            {
                if (string.IsNullOrEmpty(imagePath) || !File.Exists(imagePath))
                {
                    throw new Exception("Invalid image path");
                }

                byte[] imageBytes = File.ReadAllBytes(imagePath);
                string base64Image = Convert.ToBase64String(imageBytes);

                var uploadData = new { Photo = $"{base64Image}" };
                var content = new StringContent(JsonSerializer.Serialize(uploadData), Encoding.UTF8, "application/json");

                HttpResponseMessage response = await httpClient.PostAsync("api/Galleries/upload", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseData = await response.Content.ReadAsStringAsync();
                    var imageUrl = JsonDocument.Parse(responseData).RootElement.GetProperty("image").GetString();

                    return new Uri(httpClient.BaseAddress, imageUrl).ToString();
                }
                else
                {
                    throw new Exception($"Error uploading image: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Upload error: {ex.Message}");
            }
        }

        public static BitmapImage GetImageFromServer(string imageUrl, HttpClient httpClient)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                {
                    throw new ArgumentException("Image URL cannot be null or empty.");
                }

                var fullUrl = new Uri(httpClient.BaseAddress, imageUrl);
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.UriSource = fullUrl;
                bitmapImage.EndInit();

                return bitmapImage;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error fetching image: {ex.Message}");
            }
        }
    }
}
