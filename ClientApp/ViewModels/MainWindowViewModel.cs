using System.ComponentModel;

namespace ClientApp.ViewModels
{
   /// <summary>
   /// Класс ViewModel для главного окна приложения.
   /// </summary>
   public class MainWindowViewModel : ViewModelBase
   {
       private INotifyPropertyChanged _currentViewModel = null!;

       /// <summary>
       /// Свойство, которое содержит текущую ViewModel.
       /// </summary>
       public override INotifyPropertyChanged CurrentViewModel
       {
           get => _currentViewModel;
           set => SetProperty(ref _currentViewModel, value, nameof(CurrentViewModel));
       }

       /// <summary>
       /// Конструктор класса MainWindowViewModel.
       /// </summary>
       public MainWindowViewModel()
       {
           NavigationService.LoadStartViewModel(this);
           NavigationService.NavigateTo(new StartViewModel());
       }
   }

}