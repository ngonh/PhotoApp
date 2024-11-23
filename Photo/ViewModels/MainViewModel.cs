using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Storage.Pickers;
using WinRT.Interop;

namespace Photo.ViewModels
{
    public class MainViewModel : ViewModelBase
    {
        #region Properties
        public string ImagePath { get; set; }
        public Mat Image
        {
            get => image;
            set
            {
                image = value;
                OnPropertyChanged(nameof(Image));
            }
        }
        public Visibility CropVisibility
        {
            get => cropVisibility;
            set
            {
                cropVisibility = value;
                OnPropertyChanged(nameof(CropVisibility));
            }
        }
        public Visibility OperationVisibility
        {
            get => operationVisibility;
            set
            {
                operationVisibility = value;
                OnPropertyChanged(nameof(OperationVisibility));
            }
        }
        public Visibility RotateVisibility
        {
            get => rotateVisibility;
            set
            {
                rotateVisibility = value;
                OnPropertyChanged(nameof(RotateVisibility));
            }
        }
        public Visibility FlipVisibility
        {
            get => flipVisibility;
            set
            {
                flipVisibility = value;
                OnPropertyChanged(nameof(FlipVisibility));
            }
        }
        #endregion

        #region Constructor(s)
        public MainViewModel()
        {
            #region Initialize
            OperationVisibility = Visibility.Collapsed;

            CropVisibility = Visibility.Collapsed;
            RotateVisibility = Visibility.Collapsed;
            FlipVisibility = Visibility.Collapsed;
            #endregion

            #region CommonCommand(s)
            ImportImageCommand = new AsyncRelayCommand(ImportImageAsync);
            ExportImageCommand = new AsyncRelayCommand(SaveImageAsync);
            ReloadCommand = new RelayCommand(() => 
            {
                Image = new Mat(ImagePath);
            });
            CropCommand = new RelayCommand(() =>
            {
                #region Visible
                CropVisibility = Visibility.Visible;
                #endregion

                #region Collapsed
                RotateVisibility = Visibility.Collapsed;
                FlipVisibility = Visibility.Collapsed;
                #endregion
            });
            RotateCommand = new RelayCommand(() =>
            {
                #region Visible
                RotateVisibility = Visibility.Visible;
                #endregion

                #region Collapsed
                CropVisibility = Visibility.Collapsed;
                FlipVisibility = Visibility.Collapsed;
                #endregion
            });
            FlipCommand = new RelayCommand(() =>
            {
                #region Visible
                FlipVisibility = Visibility.Visible;
                #endregion

                #region Collapsed
                CropVisibility = Visibility.Collapsed;
                RotateVisibility = Visibility.Collapsed;
                #endregion
            });

            #region CropLevelCommand(s)
            CropLevel1Command = new RelayCommand(CropImageLevel1);
            CropLevel2Command = new RelayCommand(CropImageLevel2);
            CropLevel3Command = new RelayCommand(CropImageLevel3);
            CropLevel4Command = new RelayCommand(CropImageLevel4);
            #endregion

            #region RotateLevelCommand(s)
            RotateLevel1Command = new RelayCommand(RotateLevel1);
            RotateLevel2Command = new RelayCommand(RotateLevel2);
            #endregion

            #region FlipLevelCommand(s)
            FlipLevel1Command = new RelayCommand(FlipLevel1);
            FlipLevel2Command = new RelayCommand(FlipLevel2);
            FlipLevel3Command = new RelayCommand(FlipLevel3);
            #endregion
            #endregion
        }
        #endregion

        #region Command(s)
        public ICommand ImportImageCommand { get; }
        public ICommand ExportImageCommand { get; }
        public ICommand ReloadCommand { get; }
        public ICommand CropCommand { get; }
        public ICommand RotateCommand { get; }
        public ICommand FlipCommand { get; }

        public ICommand CropLevel1Command { get; }
        public ICommand CropLevel2Command { get; }
        public ICommand CropLevel3Command { get; }
        public ICommand CropLevel4Command { get; }

        public ICommand RotateLevel1Command { get; }
        public ICommand RotateLevel2Command { get; }

        public ICommand FlipLevel1Command { get; }
        public ICommand FlipLevel2Command { get; }
        public ICommand FlipLevel3Command { get; }
        #endregion

        #region Method(s)
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
                    Image = new Mat(file.Path);

                    string assetsPath = @"D:\Assets";
                    if (!Directory.Exists(assetsPath))
                    {
                        Directory.CreateDirectory(assetsPath);
                    }

                    string fileName = $"imported_{DateTime.Now:yyyyMMddHHmmss}{Path.GetExtension(file.Path)}";
                    string destinationPath = Path.Combine(assetsPath, fileName);

                    using (var sourceStream = await file.OpenStreamForReadAsync())
                    using (var destinationStream = new FileStream(destinationPath, FileMode.Create))
                    {
                        await sourceStream.CopyToAsync(destinationStream);
                    }

                    ImagePath = file.Path;
                    if (Image != null) OperationVisibility = Visibility.Visible;

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
        public async Task SaveImageAsync()
        {
            try
            {
                var picker = new FileSavePicker();
                picker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;

                picker.FileTypeChoices.Add("JPEG Image", new List<string> { ".jpg" });
                picker.FileTypeChoices.Add("PNG Image", new List<string> { ".png" });
                picker.FileTypeChoices.Add("Bitmap Image", new List<string> { ".bmp" });
                picker.FileTypeChoices.Add("GIF Image", new List<string> { ".gif" });
                picker.FileTypeChoices.Add("TIFF Image", new List<string> { ".tiff" });
                picker.SuggestedFileName = "ExportedImage";

                var hwnd = WindowNative.GetWindowHandle(App.MainWindow);
                InitializeWithWindow.Initialize(picker, hwnd);
                var file = await picker.PickSaveFileAsync();

                if (file != null)
                {
                    byte[] imageData = Image.ToBytes();

                    using (var stream = await file.OpenStreamForWriteAsync())
                    {
                        await stream.WriteAsync(imageData, 0, imageData.Length);
                    }

                    ContentDialog dialog = new ContentDialog
                    {
                        Title = "Thành công",
                        Content = $"Ảnh đã được lưu tại: {file.Path}",
                        CloseButtonText = "OK",
                        XamlRoot = App.MainWindow.Content.XamlRoot
                    };
                    await dialog.ShowAsync();
                }
            }
            catch (Exception ex)
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = $"Có lỗi xảy ra khi lưu ảnh: {ex.Message}",
                    CloseButtonText = "OK",
                    XamlRoot = App.MainWindow.Content.XamlRoot
                };
                await dialog.ShowAsync();
            }
        }

        public void CropImage(int p1, int p2)
        {
            int originalWidth = Image.Width;
            int originalHeight = Image.Height;

            int targetHeight = originalWidth * p2 / p1;
            if (targetHeight > originalHeight)
            {
                targetHeight = originalHeight;
                originalWidth = targetHeight * p1 / p2;
            }
            int x = (Image.Width - originalWidth) / 2;
            int y = (Image.Height - targetHeight) / 2;
            Rect roi = new Rect(x, y, originalWidth, targetHeight);
            Mat cropped = new Mat(Image, roi);

            Image = cropped;
        }
        public void CropImageLevel1()
        {
            CropImage(16, 9);
        }
        public void CropImageLevel2()
        {
            CropImage(4, 3);
        }
        public void CropImageLevel3()
        {
            CropImage(3, 4);
        }
        public void CropImageLevel4()
        {
            CropImage(1, 1);
        }
        public void RotateLevel1()
        {
            Mat rotatedClockwise = new Mat();
            Cv2.Rotate(Image, rotatedClockwise, RotateFlags.Rotate90Clockwise);
            Image = rotatedClockwise;
        }
        public void RotateLevel2()
        {
            Mat rotatedClockwise = new Mat();
            Cv2.Rotate(Image, rotatedClockwise, RotateFlags.Rotate90Counterclockwise);
            Image = rotatedClockwise;
        }
        public void FlipLevel1()
        {
            Mat dst = new Mat();
            Cv2.Flip(Image, dst, FlipMode.X);
            Image = dst;
        }
        public void FlipLevel2()
        {
            Mat dst = new Mat();
            Cv2.Flip(Image, dst, FlipMode.Y);
            Image = dst;
        }
        public void FlipLevel3()
        {
            Mat dst = new Mat();
            Cv2.Flip(Image, dst, FlipMode.XY);
            Image = dst;
        }
        #endregion

        #region Private(s)
        private Mat image;
        private Visibility cropVisibility;
        private Visibility operationVisibility;
        private Visibility rotateVisibility;
        private Visibility flipVisibility;
        #endregion
    }
}
