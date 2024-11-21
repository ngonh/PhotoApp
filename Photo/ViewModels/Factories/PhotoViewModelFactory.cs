using Photo.Defines;
using System;

namespace Photo.ViewModels.Factories
{
    public class PhotoViewModelFactory : IPhotoViewModelFactory
    {
        private readonly CreateViewModel<HomeViewModel> _createHomeViewModel;

        public PhotoViewModelFactory(CreateViewModel<HomeViewModel> createHomeViewModel)
        {
            _createHomeViewModel = createHomeViewModel;
        }
        public ViewModelBase CreateViewModel(ViewType viewType)
        {
            switch (viewType)
            {
                case ViewType.Home:
                    return _createHomeViewModel();
                default:
                    throw new ArgumentException("The ViewType does not have a ViewModel.", "viewType");
            }
        }
    }
}
