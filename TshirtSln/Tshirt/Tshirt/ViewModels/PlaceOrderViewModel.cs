using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Tshirt.Models;
using Tshirt.Service.Interfaces;

namespace Tshirt.ViewModels
{
    public class PlaceOrderViewModel : ViewModelBase
    {
        private IDatabase _database;

        private DelegateCommand _saveCommand;
        public DelegateCommand SaveCommand =>
            _saveCommand ?? (_saveCommand = new DelegateCommand(ExecuteSaveCommand));

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand(ExecuteCancelCommand));


        public TshirtProperties TshirtOrder { get; set; }

        public async void ExecuteCancelCommand()
        {
            await NavigationService.NavigateAsync("PlaceOrder");
        }

       
        public async void ExecuteSaveCommand()
        {
            await _database.SaveItemAsync(TshirtOrder);
            await NavigationService.NavigateAsync("OrderList");
        }

       

        public PlaceOrderViewModel(INavigationService navigationService, IDatabase database) : base(navigationService)
        {
            _database = database;
        }

        public async override void Initialize(INavigationParameters parameters)
        {
            base.OnNavigatedFrom(parameters);
           // var tshirtProperties = await _database.GetItemsAsync();
            TshirtOrder = new TshirtProperties();
        }


    }
}
