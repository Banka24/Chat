using Chat.ClientApp.Models;
using Chat.ClientApp.Services.Contracts;
using System.ComponentModel;

namespace Chat.ClientApp.Services
{
   /// <summary>
   /// Класс для работы с навигацией между представлениями.
   /// </summary>
   public class NavigationService : INavigationService
   {
       private readonly Stack<IViewModel> _viewModels = new();
       private IViewModel _startViewModel = null!;

       /// <summary>
       /// Метод для загрузки начального представления.
       /// </summary>
       /// <param name="viewModel">Начальное представление.</param>
       public void LoadStartViewModel(IViewModel viewModel)
       {
           if (_startViewModel == null)
           {
               _startViewModel = viewModel;
               _viewModels.Push(_startViewModel);
           }
       }

       /// <summary>
       /// Метод для возврата на предыдущее представление.
       /// </summary>
       public void GoBack()
       {
           if (_viewModels.Count <= 1) return;
           _viewModels.Pop();
           _startViewModel.CurrentViewModel = (INotifyPropertyChanged)_viewModels.Peek();
       }

       /// <summary>
       /// Метод для перехода к новому представлению.
       /// </summary>
       /// <param name="viewModel">Новое представление.</param>
       public void NavigateTo(IViewModel viewModel)
       {
           _viewModels.Push(viewModel);
           if (_startViewModel == null) return;
           _startViewModel.CurrentViewModel = (INotifyPropertyChanged)_viewModels.Peek();
       }
   }

}