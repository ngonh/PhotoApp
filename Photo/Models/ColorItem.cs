using CommunityToolkit.Mvvm.ComponentModel;
using OpenCvSharp;

namespace Photo.Models
{
    public class ColorItem : ObservableObject
    {
        public string Name
        {
            get => name;
            set { name = value; OnPropertyChanged(nameof(Name)); }
        }
        public Scalar Value
        {
            get => value;
            set { this.value = value; OnPropertyChanged(nameof(Value)); }
        }

        private string name;
        private Scalar value;
    }
}
