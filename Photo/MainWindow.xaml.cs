using Microsoft.UI.Xaml.Input;
using Point = Windows.Foundation.Point;

namespace Photo
{
    public sealed partial class MainWindow : Microsoft.UI.Xaml.Window
    {
        public MainWindow(object dataContext)
        {
            this.InitializeComponent();
            RootPanel.DataContext = dataContext;
        }
        private void scrollImageTarget_PointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            int delta = e.GetCurrentPoint(scrollImageTarget).Properties.MouseWheelDelta;

            double zoomFactor = delta > 0 ? 1.1 : 0.9;

            double currentWidth = mainImage.ActualWidth;
            double currentHeight = mainImage.ActualHeight;

            Point pointerPosition = e.GetCurrentPoint(mainImage).Position;

            double imageMouseX = pointerPosition.X;
            double imageMouseY = pointerPosition.Y;

            mainImage.Width = currentWidth * zoomFactor;
            mainImage.Height = currentHeight * zoomFactor;

            double newImageMouseX = imageMouseX * (mainImage.Width / currentWidth);
            double newImageMouseY = imageMouseY * (mainImage.Height / currentHeight);

            double offsetX = newImageMouseX - imageMouseX;
            double offsetY = newImageMouseY - imageMouseY;

            scrollImageTarget.ChangeView(scrollImageTarget.HorizontalOffset + offsetX, scrollImageTarget.VerticalOffset + offsetY, null);

            e.Handled = true;
        }
    }
}
