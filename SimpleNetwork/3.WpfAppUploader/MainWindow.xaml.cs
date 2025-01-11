using System.Net.Http;
using System.Windows;
using _3.WpfAppUploader.Connecting;

namespace _3.WpfAppUploader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private HttpClient _httpClient;
        public MainWindow()
        {
            InitializeComponent();
        }
        private async void connectBT_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _httpClient = new HttpClient { BaseAddress = new Uri($"{this.portTB.Text}") };

                HttpResponseMessage response = await _httpClient.PostAsync("api/Galleries/upload", null); //для тесту

                if (response.StatusCode != System.Net.HttpStatusCode.NotFound)
                {
                    System.Windows.MessageBox.Show("Connected to the server!", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    ChangeControlState();
                }
                else
                {
                    throw new Exception("Invalid port");
                }
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void loadImagePathBT_Click(object sender, RoutedEventArgs e)
        {
            using (var openFileDialog = new System.Windows.Forms.OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.jpg;*.jpeg;*.png;";
                openFileDialog.Title = "Select an Image File";

                if (openFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.imagePathTB.Text = openFileDialog.FileName;
                }
            }
        }

        private async void uploadBT_Click(object sender, RoutedEventArgs e)
        {
            string imagePath = imagePathTB.Text;

            try
            {
                this.imageLabel.Source = ImageSender.GetImageFromServer(
                    await ImageSender.UploadImageAsync(imagePath, _httpClient), _httpClient
                    );
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ChangeControlState()
        {
            this.connectBT.IsEnabled = !this.connectBT.IsEnabled;
            this.loadImagePathBT.IsEnabled = !this.loadImagePathBT.IsEnabled;
            this.uploadBT.IsEnabled = !this.uploadBT.IsEnabled;

            this.imagePathTB.IsEnabled = !this.imagePathTB.IsEnabled;
            this.portTB.IsEnabled = !this.portTB.IsEnabled;
        }
    }
}