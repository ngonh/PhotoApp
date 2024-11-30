using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using OpenCvSharp;
using OpenCvSharp.XPhoto;
using Photo.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public Mat ImageOrigin
        {
            get => imageOrigin;
            set
            {
                imageOrigin = value;
                OnPropertyChanged(nameof(ImageOrigin));
            }
        }
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
        public Visibility OriginVisibility
        {
            get => originVisibility;
            set
            {
                originVisibility = value;
                OnPropertyChanged(nameof(OriginVisibility));
            }
        }
        public Visibility PictureStyleVisibility
        {
            get => pictureStyleVisibility;
            set
            {
                pictureStyleVisibility = value;
                OnPropertyChanged(nameof(PictureStyleVisibility));
            }
        }
        public Visibility WhiteBalanceVisibility
        {
            get => whiteBalanceVisibility;
            set
            {
                whiteBalanceVisibility = value;
                OnPropertyChanged(nameof(WhiteBalanceVisibility));
            }
        }
        public ObservableCollection<ColorItem> ColorCode
        {
            get => colorCode;
            set
            {
                colorCode = value;
                OnPropertyChanged(nameof(ColorCode));
            }
        }
        public int BorderThickness
        {
            get => borderThickness;
            set
            {
                borderThickness = value;
                OnPropertyChanged(nameof(BorderThickness));
            }
        }
        public ColorItem SelectedColor
        {
            get => selectedColor;
            set
            {
                selectedColor = value;
                OnPropertyChanged(nameof(SelectedColor));
            }
        }
        public int Span
        {
            get => span;
            set
            {
                span = value;
                OnPropertyChanged(nameof(Span));
            }
        }
        public int SliderValue
        {
            get => sliderValue;
            set
            {
                sliderValue = value;
                OnPropertyChanged(nameof(SliderValue));
                WhiteBalance();
            }
        }
        #endregion

        #region Constructor(s)
        public MainViewModel()
        {
            #region Initialize
            BorderThickness = 1;
            Span = 2;
            SelectedColor = new ColorItem() { Name = "Black", Value = Scalar.Black };

            ColorCode = new ObservableCollection<ColorItem>
            {
                new ColorItem { Name = "AliceBlue", Value = Scalar.AliceBlue },
                new ColorItem { Name = "AntiqueWhite", Value = Scalar.AntiqueWhite },
                new ColorItem { Name = "Aqua", Value = Scalar.Aqua },
                new ColorItem { Name = "Aquamarine", Value = Scalar.Aquamarine },
                new ColorItem { Name = "Azure", Value = Scalar.Azure },
                new ColorItem { Name = "Beige", Value = Scalar.Beige },
                new ColorItem { Name = "Bisque", Value = Scalar.Bisque },
                new ColorItem { Name = "Black", Value = Scalar.Black },
                new ColorItem { Name = "BlanchedAlmond", Value = Scalar.BlanchedAlmond },
                new ColorItem { Name = "Blue", Value = Scalar.Blue },
                new ColorItem { Name = "BlueViolet", Value = Scalar.BlueViolet },
                new ColorItem { Name = "Brown", Value = Scalar.Brown },
                new ColorItem { Name = "BurlyWood", Value = Scalar.BurlyWood },
                new ColorItem { Name = "CadetBlue", Value = Scalar.CadetBlue },
                new ColorItem { Name = "Chartreuse", Value = Scalar.Chartreuse },
                new ColorItem { Name = "Chocolate", Value = Scalar.Chocolate },
                new ColorItem { Name = "Coral", Value = Scalar.Coral },
                new ColorItem { Name = "CornflowerBlue", Value = Scalar.CornflowerBlue },
                new ColorItem { Name = "Cornsilk", Value = Scalar.Cornsilk },
                new ColorItem { Name = "Crimson", Value = Scalar.Crimson },
                new ColorItem { Name = "Cyan", Value = Scalar.Cyan },
                new ColorItem { Name = "DarkBlue", Value = Scalar.DarkBlue },
                new ColorItem { Name = "DarkCyan", Value = Scalar.DarkCyan },
                new ColorItem { Name = "DarkGoldenrod", Value = Scalar.DarkGoldenrod },
                new ColorItem { Name = "DarkGray", Value = Scalar.DarkGray },
                new ColorItem { Name = "DarkGreen", Value = Scalar.DarkGreen },
                new ColorItem { Name = "DarkKhaki", Value = Scalar.DarkKhaki },
                new ColorItem { Name = "DarkMagenta", Value = Scalar.DarkMagenta },
                new ColorItem { Name = "DarkOliveGreen", Value = Scalar.DarkOliveGreen },
                new ColorItem { Name = "DarkOrange", Value = Scalar.DarkOrange },
                new ColorItem { Name = "White", Value = Scalar.White },
                new ColorItem { Name = "DarkOrchid", Value = Scalar.DarkOrchid },
                new ColorItem { Name = "DarkRed", Value = Scalar.DarkRed },
                new ColorItem { Name = "DarkSalmon", Value = Scalar.DarkSalmon },
                new ColorItem { Name = "DarkSeaGreen", Value = Scalar.DarkSeaGreen },
                new ColorItem { Name = "DarkSlateBlue", Value = Scalar.DarkSlateBlue },
                new ColorItem { Name = "DarkSlateGray", Value = Scalar.DarkSlateGray },
                new ColorItem { Name = "DarkTurquoise", Value = Scalar.DarkTurquoise },
                new ColorItem { Name = "DarkViolet", Value = Scalar.DarkViolet },
                new ColorItem { Name = "DeepPink", Value = Scalar.DeepPink },
                new ColorItem { Name = "DeepSkyBlue", Value = Scalar.DeepSkyBlue },
                new ColorItem { Name = "DimGray", Value = Scalar.DimGray },
                new ColorItem { Name = "DodgerBlue", Value = Scalar.DodgerBlue },
                new ColorItem { Name = "Firebrick", Value = Scalar.Firebrick },
                new ColorItem { Name = "FloralWhite", Value = Scalar.FloralWhite },
                new ColorItem { Name = "ForestGreen", Value = Scalar.ForestGreen },
                new ColorItem { Name = "Fuchsia", Value = Scalar.Fuchsia },
                new ColorItem { Name = "Gainsboro", Value = Scalar.Gainsboro },
                new ColorItem { Name = "GhostWhite", Value = Scalar.GhostWhite },
                new ColorItem { Name = "Gold", Value = Scalar.Gold },
                new ColorItem { Name = "Goldenrod", Value = Scalar.Goldenrod },
                new ColorItem { Name = "Gray", Value = Scalar.Gray },
                new ColorItem { Name = "Green", Value = Scalar.Green },
                new ColorItem { Name = "GreenYellow", Value = Scalar.GreenYellow },
                new ColorItem { Name = "Honeydew", Value = Scalar.Honeydew },
                new ColorItem { Name = "HotPink", Value = Scalar.HotPink },
                new ColorItem { Name = "IndianRed", Value = Scalar.IndianRed },
                new ColorItem { Name = "Indigo", Value = Scalar.Indigo },
                new ColorItem { Name = "Ivory", Value = Scalar.Ivory },
                new ColorItem { Name = "Khaki", Value = Scalar.Khaki },
                new ColorItem { Name = "Lavender", Value = Scalar.Lavender },
                new ColorItem { Name = "LavenderBlush", Value = Scalar.LavenderBlush },
                new ColorItem { Name = "LawnGreen", Value = Scalar.LawnGreen },
                new ColorItem { Name = "LemonChiffon", Value = Scalar.LemonChiffon },
                new ColorItem { Name = "LightBlue", Value = Scalar.LightBlue },
                new ColorItem { Name = "LightCoral", Value = Scalar.LightCoral },
                new ColorItem { Name = "LightCyan", Value = Scalar.LightCyan },
                new ColorItem { Name = "LightGoldenrodYellow", Value = Scalar.LightGoldenrodYellow },
                new ColorItem { Name = "LightGray", Value = Scalar.LightGray },
                new ColorItem { Name = "LightGreen", Value = Scalar.LightGreen },
                new ColorItem { Name = "LightPink", Value = Scalar.LightPink },
                new ColorItem { Name = "LightSalmon", Value = Scalar.LightSalmon },
                new ColorItem { Name = "LightSeaGreen", Value = Scalar.LightSeaGreen },
                new ColorItem { Name = "LightSkyBlue", Value = Scalar.LightSkyBlue },
                new ColorItem { Name = "LightSlateGray", Value = Scalar.LightSlateGray },
                new ColorItem { Name = "LightSteelBlue", Value = Scalar.LightSteelBlue },
                new ColorItem { Name = "LightYellow", Value = Scalar.LightYellow },
                new ColorItem { Name = "Lime", Value = Scalar.Lime },
                new ColorItem { Name = "LimeGreen", Value = Scalar.LimeGreen },
                new ColorItem { Name = "Linen", Value = Scalar.Linen },
                new ColorItem { Name = "Magenta", Value = Scalar.Magenta },
                new ColorItem { Name = "Maroon", Value = Scalar.Maroon },
                new ColorItem { Name = "MediumAquamarine", Value = Scalar.MediumAquamarine },
                new ColorItem { Name = "MediumBlue", Value = Scalar.MediumBlue },
                new ColorItem { Name = "MediumOrchid", Value = Scalar.MediumOrchid },
                new ColorItem { Name = "MediumPurple", Value = Scalar.MediumPurple },
                new ColorItem { Name = "MediumSeaGreen", Value = Scalar.MediumSeaGreen },
                new ColorItem { Name = "MediumSlateBlue", Value = Scalar.MediumSlateBlue },
                new ColorItem { Name = "MediumSpringGreen", Value = Scalar.MediumSpringGreen },
                new ColorItem { Name = "MediumTurquoise", Value = Scalar.MediumTurquoise },
                new ColorItem { Name = "MediumVioletRed", Value = Scalar.MediumVioletRed },
                new ColorItem { Name = "MidnightBlue", Value = Scalar.MidnightBlue },
                new ColorItem { Name = "MintCream", Value = Scalar.MintCream },
                new ColorItem { Name = "MistyRose", Value = Scalar.MistyRose },
                new ColorItem { Name = "Moccasin", Value = Scalar.Moccasin },
                new ColorItem { Name = "NavajoWhite", Value = Scalar.NavajoWhite },
                new ColorItem { Name = "Navy", Value = Scalar.Navy },
                new ColorItem { Name = "OldLace", Value = Scalar.OldLace },
                new ColorItem { Name = "Olive", Value = Scalar.Olive },
                new ColorItem { Name = "OliveDrab", Value = Scalar.OliveDrab },
                new ColorItem { Name = "Orange", Value = Scalar.Orange },
                new ColorItem { Name = "OrangeRed", Value = Scalar.OrangeRed },
                new ColorItem { Name = "Orchid", Value = Scalar.Orchid },
                new ColorItem { Name = "PaleGoldenrod", Value = Scalar.PaleGoldenrod },
                new ColorItem { Name = "PaleGreen", Value = Scalar.PaleGreen },
                new ColorItem { Name = "PaleTurquoise", Value = Scalar.PaleTurquoise },
                new ColorItem { Name = "PaleVioletRed", Value = Scalar.PaleVioletRed },
                new ColorItem { Name = "PapayaWhip", Value = Scalar.PapayaWhip },
                new ColorItem { Name = "PeachPuff", Value = Scalar.PeachPuff },
                new ColorItem { Name = "Peru", Value = Scalar.Peru },
                new ColorItem { Name = "Pink", Value = Scalar.Pink },
                new ColorItem { Name = "Plum", Value = Scalar.Plum },
                new ColorItem { Name = "PowderBlue", Value = Scalar.PowderBlue },
                new ColorItem { Name = "Purple", Value = Scalar.Purple },
                new ColorItem { Name = "Red", Value = Scalar.Red },
                new ColorItem { Name = "RosyBrown", Value = Scalar.RosyBrown },
                new ColorItem { Name = "RoyalBlue", Value = Scalar.RoyalBlue },
                new ColorItem { Name = "SaddleBrown", Value = Scalar.SaddleBrown },
                new ColorItem { Name = "Salmon", Value = Scalar.Salmon },
                new ColorItem { Name = "SandyBrown", Value = Scalar.SandyBrown },
                new ColorItem { Name = "SeaGreen", Value = Scalar.SeaGreen },
                new ColorItem { Name = "SeaShell", Value = Scalar.SeaShell },
                new ColorItem { Name = "Sienna", Value = Scalar.Sienna },
                new ColorItem { Name = "Silver", Value = Scalar.Silver },
                new ColorItem { Name = "SkyBlue", Value = Scalar.SkyBlue },
                new ColorItem { Name = "SlateBlue", Value = Scalar.SlateBlue },
                new ColorItem { Name = "SlateGray", Value = Scalar.SlateGray },
                new ColorItem { Name = "Snow", Value = Scalar.Snow },
                new ColorItem { Name = "SpringGreen", Value = Scalar.SpringGreen },
                new ColorItem { Name = "SteelBlue", Value = Scalar.SteelBlue },
                new ColorItem { Name = "Tan", Value = Scalar.Tan },
                new ColorItem { Name = "Teal", Value = Scalar.Teal },
                new ColorItem { Name = "Thistle", Value = Scalar.Thistle },
                new ColorItem { Name = "Tomato", Value = Scalar.Tomato },
                new ColorItem { Name = "Turquoise", Value = Scalar.Turquoise },
                new ColorItem { Name = "Violet", Value = Scalar.Violet },
                new ColorItem { Name = "Wheat", Value = Scalar.Wheat },
                new ColorItem { Name = "White", Value = Scalar.White },
                new ColorItem { Name = "WhiteSmoke", Value = Scalar.WhiteSmoke },
                new ColorItem { Name = "Yellow", Value = Scalar.Yellow },
                new ColorItem { Name = "YellowGreen", Value = Scalar.YellowGreen }
            };

            OperationVisibility = Visibility.Collapsed;
            OriginVisibility = Visibility.Collapsed;
            CropVisibility = Visibility.Collapsed;
            RotateVisibility = Visibility.Collapsed;
            FlipVisibility = Visibility.Collapsed;
            PictureStyleVisibility = Visibility.Collapsed;
            WhiteBalanceVisibility = Visibility.Collapsed;
            #endregion

            #region CommonCommand(s)
            ImportImageCommand = new AsyncRelayCommand(ImportImageAsync);
            ExportImageCommand = new AsyncRelayCommand(SaveImageAsync);
            ReloadCommand = new RelayCommand(() =>
            {
                #region Common
                Span = 2;
                #endregion
                OriginVisibility = Visibility.Collapsed;

                Image = new Mat(ImagePath);
                flag = false;
            });
            CropCommand = new RelayCommand(() =>
            {
                #region Common
                Span = 2;
                #endregion

                #region Visible
                CropVisibility = Visibility.Visible;
                #endregion

                #region Collapsed
                RotateVisibility = Visibility.Collapsed;
                FlipVisibility = Visibility.Collapsed;
                PictureStyleVisibility = Visibility.Collapsed;
                OriginVisibility = Visibility.Collapsed;
                WhiteBalanceVisibility = Visibility.Collapsed;
                #endregion
            });
            RotateCommand = new RelayCommand(() =>
            {
                #region Common
                Span = 2;
                #endregion

                #region Visible
                RotateVisibility = Visibility.Visible;
                #endregion

                #region Collapsed
                CropVisibility = Visibility.Collapsed;
                FlipVisibility = Visibility.Collapsed;
                PictureStyleVisibility = Visibility.Collapsed;
                OriginVisibility = Visibility.Collapsed;
                WhiteBalanceVisibility = Visibility.Collapsed;
                #endregion
            });
            FlipCommand = new RelayCommand(() =>
            {
                #region Common
                Span = 2;
                #endregion

                #region Visible
                FlipVisibility = Visibility.Visible;
                #endregion

                #region Collapsed
                CropVisibility = Visibility.Collapsed;
                RotateVisibility = Visibility.Collapsed;
                PictureStyleVisibility = Visibility.Collapsed;
                OriginVisibility = Visibility.Collapsed;
                WhiteBalanceVisibility = Visibility.Collapsed;
                #endregion
            });
            PictureStyleCommand = new RelayCommand(() =>
            {
                #region Common
                Span = 2;
                #endregion

                #region Visible
                PictureStyleVisibility = Visibility.Visible;
                #endregion

                #region Collapsed
                FlipVisibility = Visibility.Collapsed;
                CropVisibility = Visibility.Collapsed;
                RotateVisibility = Visibility.Collapsed;
                OriginVisibility = Visibility.Collapsed;
                WhiteBalanceVisibility = Visibility.Collapsed;
                #endregion
            });
            CompareToOriginCommand = new RelayCommand(() =>
            {
                #region Common
                Span = 1;
                #endregion

                #region Visible
                OriginVisibility = Visibility.Visible;
                #endregion

                #region Collapsed
                FlipVisibility = Visibility.Collapsed;
                CropVisibility = Visibility.Collapsed;
                RotateVisibility = Visibility.Collapsed;
                PictureStyleVisibility = Visibility.Collapsed;
                WhiteBalanceVisibility = Visibility.Collapsed;
                #endregion
            });
            WhiteBalanceCommand = new RelayCommand(WhiteBalance);
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

            #region StyleLevelCommand(s)
            StyleLevel1Command = new RelayCommand(PictureStyleLevel1);
            StyleLevel2Command = new RelayCommand(PictureStyleLevel2);
            StyleLevel3Command = new RelayCommand(PictureStyleLevel3);
            StyleLevel4Command = new RelayCommand(PictureStyleLevel4);
            StyleLevel5Command = new RelayCommand(PictureStyleLevel5);
            StyleLevel6Command = new RelayCommand(PictureStyleLevel6);
            StyleLevel7Command = new RelayCommand(PictureStyleLevel7);
            StyleLevel8Command = new RelayCommand(PictureStyleLevel8);
            StyleLevel9Command = new RelayCommand(PictureStyleLevel9);
            StyleLevel10Command = new RelayCommand(PictureStyleLevel10);

            SetBorderCommand = new RelayCommand(SetBorder);
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
        public ICommand PictureStyleCommand { get; }
        public ICommand CompareToOriginCommand { get; }

        public ICommand CropLevel1Command { get; }
        public ICommand CropLevel2Command { get; }
        public ICommand CropLevel3Command { get; }
        public ICommand CropLevel4Command { get; }

        public ICommand RotateLevel1Command { get; }
        public ICommand RotateLevel2Command { get; }

        public ICommand FlipLevel1Command { get; }
        public ICommand FlipLevel2Command { get; }
        public ICommand FlipLevel3Command { get; }

        public ICommand StyleLevel1Command { get; }
        public ICommand StyleLevel2Command { get; }
        public ICommand StyleLevel3Command { get; }
        public ICommand StyleLevel4Command { get; }
        public ICommand StyleLevel5Command { get; }
        public ICommand StyleLevel6Command { get; }
        public ICommand StyleLevel7Command { get; }
        public ICommand StyleLevel8Command { get; }
        public ICommand StyleLevel9Command { get; }
        public ICommand StyleLevel10Command { get; }

        public ICommand SetBorderCommand { get; }
        public ICommand WhiteBalanceCommand { get; }
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
                    ImageOrigin = new Mat(file.Path);

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
                    flag = false;
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
        public void WhiteBalance()
        {
            #region Common
            Span = 2;
            #endregion

            #region Visible
            WhiteBalanceVisibility = Visibility.Visible;
            #endregion

            #region Collapsed
            FlipVisibility = Visibility.Collapsed;
            CropVisibility = Visibility.Collapsed;
            RotateVisibility = Visibility.Collapsed;
            PictureStyleVisibility = Visibility.Collapsed;
            OriginVisibility = Visibility.Collapsed;
            #endregion
            //int kelvin = 1000;
            Mat adjusted = AdjustColorTemperature(Image, SliderValue);
            Image = adjusted;
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
        public void PictureStyle(string filePath)
        {
            Mat borderPattern = Cv2.ImRead(filePath);

            if (borderPattern.Empty())
            {
                return;
            }

            int borderThickness = Image.Width / 30;

            int newWidth = Image.Width + 2 * borderThickness;
            int newHeight = Image.Height + 2 * borderThickness;

            Mat result = new Mat(newHeight, newWidth, MatType.CV_8UC3);

            for (int y = 0; y < newHeight; y += borderPattern.Rows)
            {
                for (int x = 0; x < newWidth; x += borderPattern.Cols)
                {
                    Rect roi = new Rect(x, y,
                        Math.Min(borderPattern.Cols, newWidth - x),
                        Math.Min(borderPattern.Rows, newHeight - y));

                    if (roi.X < borderThickness || roi.Y < borderThickness ||
                        roi.X + roi.Width > newWidth - borderThickness ||
                        roi.Y + roi.Height > newHeight - borderThickness)
                    {
                        Mat targetRegion = new Mat(result, roi);
                        borderPattern[0, targetRegion.Rows, 0, targetRegion.Cols].CopyTo(targetRegion);
                    }
                }
            }

            Rect imageRoi = new Rect(borderThickness, borderThickness, Image.Width, Image.Height);
            Mat centerRegion = new Mat(result, imageRoi);
            Image.CopyTo(centerRegion);

            Image = result;
        }
        public void PictureStyleLevel1()
        {
            PictureStyle("D:\\Assets\\Boder\\border1.png");
        }
        public void PictureStyleLevel2()
        {
            PictureStyle("D:\\Assets\\Boder\\border2.png");
        }
        public void PictureStyleLevel3()
        {
            PictureStyle("D:\\Assets\\Boder\\border3.png");
        }
        public void PictureStyleLevel4()
        {
            PictureStyle("D:\\Assets\\Boder\\border4.png");
        }
        public void PictureStyleLevel5()
        {
            PictureStyle("D:\\Assets\\Boder\\border5.png");
        }
        public void PictureStyleLevel6()
        {
            PictureStyle("D:\\Assets\\Boder\\border6.png");
        }
        public void PictureStyleLevel7()
        {
            PictureStyle("D:\\Assets\\Boder\\border7.png");
        }
        public void PictureStyleLevel8()
        {
            PictureStyle("D:\\Assets\\Boder\\border8.png");
        }
        public void PictureStyleLevel9()
        {
            PictureStyle("D:\\Assets\\Boder\\border9.png");
        }
        public void PictureStyleLevel10()
        {
            PictureStyle("D:\\Assets\\Boder\\border10.png");
        }
        public void SetBorder()
        {
            if (flag == true)
            {
                Mat imageWithBorder = new Mat();
                Cv2.CopyMakeBorder(matTemp, imageWithBorder, BorderThickness, BorderThickness,
                    BorderThickness, BorderThickness, BorderTypes.Constant, SelectedColor.Value);
                Image = imageWithBorder;
                return;
            }
            else
            {
                matTemp = Image;
                Mat imageWithBorder = new Mat();
                Cv2.CopyMakeBorder(matTemp, imageWithBorder, BorderThickness, BorderThickness,
                    BorderThickness, BorderThickness, BorderTypes.Constant, SelectedColor.Value);
                Image = imageWithBorder;
                flag = true;
            }
        }
        public static Mat AdjustColorTemperature(Mat src, int kelvin)
        {
            if (src.Empty())
                throw new ArgumentException("Ảnh nguồn không hợp lệ");

            // Lấy các hệ số cân bằng màu
            double[] balanceFactors = GetColorBalanceFactors(kelvin);

            // Chia ảnh thành các kênh
            var channels = Cv2.Split(src);

            // Áp dụng hệ số cân bằng màu cho từng kênh
            Cv2.Multiply(channels[0], new Scalar(balanceFactors[0]), channels[0]); // Blue
            Cv2.Multiply(channels[1], new Scalar(balanceFactors[1]), channels[1]); // Green
            Cv2.Multiply(channels[2], new Scalar(balanceFactors[2]), channels[2]); // Red

            // Hợp nhất lại thành ảnh kết quả
            Mat result = new Mat();
            Cv2.Merge(channels, result);

            // Giải phóng bộ nhớ các kênh trung gian
            foreach (var channel in channels)
                channel.Dispose();

            return result;
        }

        private static double[] GetColorBalanceFactors(int kelvin)
        {
            kelvin = Math.Clamp(kelvin, 1000, 40000);

            double temp = kelvin / 100.0;
            double red, green, blue;

            if (temp <= 66)
            {
                red = 255;
                green = temp <= 10 ? 0 : (99.4708025861 * Math.Log(temp) - 161.1195681661);
                blue = temp <= 19 ? 0 : (138.5177312231 * Math.Log(temp - 10) - 305.0447927307);
            }
            else
            {
                red = 329.698727446 * Math.Pow(temp - 60, -0.1332047592);
                green = 288.1221695283 * Math.Pow(temp - 60, -0.0755148492);
                blue = 255;
            }

            // Giới hạn giá trị trong khoảng [0, 255]
            red = Math.Clamp(red, 0, 255);
            green = Math.Clamp(green, 0, 255);
            blue = Math.Clamp(blue, 0, 255);

            // Trả về tỷ lệ cân bằng
            return new double[] { blue / 255.0, green / 255.0, red / 255.0 };
        }

        #endregion

        #region Private(s)
        private Mat image;
        private Mat imageOrigin;
        private Visibility cropVisibility;
        private Visibility operationVisibility;
        private Visibility rotateVisibility;
        private Visibility flipVisibility;
        private Visibility pictureStyleVisibility;
        private Visibility originVisibility;
        private Visibility whiteBalanceVisibility;
        private ObservableCollection<ColorItem> colorCode;
        private ColorItem selectedColor;
        private int borderThickness;
        private Mat matTemp;
        private bool flag = false;
        private int span;
        private int sliderValue = 1000;
        #endregion
    }
}
