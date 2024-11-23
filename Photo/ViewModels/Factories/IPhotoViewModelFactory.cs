using Photo.Defines;

namespace Photo.ViewModels.Factories
{
    public interface IPhotoViewModelFactory
    {
        ViewModelBase CreateViewModel(ViewType viewType);
    }
}
