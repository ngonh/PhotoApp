using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.Storage;
using WinRT.Interop;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;

namespace Photo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        public string ImagePath { get; set; }   
        private ImageSource _image;
        public ImageSource Image
        {
            get => _image;
            set
            {
                _image = value;
                OnPropertyChanged(nameof(Image));
            }
        }

        public ICommand ImportImageCommand { get; }

        public MainViewModel()
        {
            ImportImageCommand = new AsyncRelayCommand(ImportImageAsync);
        }
        public async Task ImportImageAsync()
        {
            try
            {
                var picker = new FileOpenPicker();
                picker.ViewMode = PickerViewMode.Thumbnail;
                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                picker.FileTypeFilter.Add(".png");
                picker.FileTypeFilter.Add(".jpg");
                picker.FileTypeFilter.Add(".jpeg");

                var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
                InitializeWithWindow.Initialize(picker, hwnd);
                var file = await picker.PickSingleFileAsync();

                if (file != null)
                {
                    var bitmapImage = new BitmapImage();
                    using (var stream = await file.OpenAsync(FileAccessMode.Read))
                    {
                        bitmapImage.SetSource(stream);
                        var decoder = await Windows.Graphics.Imaging.BitmapDecoder.CreateAsync(stream);
                        var softwareBitmap = await decoder.GetSoftwareBitmapAsync();
                    }

                    // Tạo thư mục D:/Assets nếu chưa tồn tại
                    string assetsPath = @"D:\Assets";
                    if (!Directory.Exists(assetsPath))
                    {
                        Directory.CreateDirectory(assetsPath);
                    }

                    // Tạo tên file mới
                    string fileName = $"imported_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.Path)}";
                    string destinationPath = Path.Combine(assetsPath, fileName);

                    // Sao chép file
                    using (var sourceStream = await file.OpenStreamForReadAsync())
                    using (var destinationStream = new FileStream(destinationPath, FileMode.Create))
                    {
                        await sourceStream.CopyToAsync(destinationStream);
                    }

                    Image = bitmapImage;
                    ImagePath = file.Path;

                    // Thông báo thành công (tùy chọn)
                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Thành công",
                        Content = $"Đã lưu ảnh vào {destinationPath}",
                        CloseButtonText = "OK",
                        XamlRoot = App.MainWindow.Content.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                // Xử lý lỗi
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Có lỗi xảy ra khi import ảnh: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = App.MainWindow.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }
    }
}
