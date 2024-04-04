using System;
using System.Drawing;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Albom
{
    public partial class Form1 : Form
    {
        private const string ImageUrl1 = "https://source.unsplash.com/random";
        private const string ImageUrl2 = "https://source.unsplash.com/random";
        private const string DefaultImageUrl = "https://source.unsplash.com/random";
        private Timer timer;
        private int secondsLeft = 30;
        public Form1()
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            timer1 = new Timer();
            timer1.Interval = 30000; // Устанавливаем интервал таймера в 30 секунд (30000 миллисекунд)
            timer1.Tick += async (sender, e) => await UpdateImageAsync();
        }
        private async Task UpdateImageAsync()
        {
            Image image = await DownloadImageAsync(DefaultImageUrl);
            if (image != null)
            {
                // Удалите предыдущие изображения перед добавлением нового
                flowLayoutPanel1.Controls.Clear();
                AddImageToGallery(image);
                secondsLeft = 30;
            }
        }
        private async Task<Image> DownloadImageAsync(string imageUrl)
        {
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.GetAsync(imageUrl);
                    if (response.IsSuccessStatusCode)
                    {
                        var stream = await response.Content.ReadAsStreamAsync();
                        return Image.FromStream(stream);
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки изображения: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;
            }
        }

        private void AddImageToGallery(Image image)
        {
            PictureBox pictureBox = new PictureBox();
            pictureBox.Image = image;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom; // Установите режим масштабирования в Zoom
            pictureBox.Width = 1500; // Установите желаемую ширину
            pictureBox.Height = 840; // Установите желаемую высоту
            flowLayoutPanel1.Controls.Clear(); // Очистите все предыдущие изображения
            flowLayoutPanel1.Controls.Add(pictureBox); // Добавьте новое изображение
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            Image image = await DownloadImageAsync(ImageUrl1);
            if (image != null)
            {
                AddImageToGallery(image);
            }
        }

        private async void label1_Click(object sender, EventArgs e)
        {
            Image image = await DownloadImageAsync(ImageUrl2); // Загружаем новое изображение
            if (image != null)
            {
                AddImageToGallery(image); // Заменяем текущее изображение в галерее
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {
            timer1.Start(); // Запустить таймер при нажатии на лейбл
        }
    }
}
