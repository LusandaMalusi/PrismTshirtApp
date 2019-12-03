using Newtonsoft.Json;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using Tshirt.Models;
using Tshirt.Service;
using Tshirt.Service.Interfaces;
using Xamarin.Essentials;

namespace Tshirt.ViewModels
{
    public class OrderListViewModel : ViewModelBase
    {
        private ObservableCollection<TshirtProperties> _orders;
        private IDatabase _database;

        private IPageDialogService _dialogService;


        public ObservableCollection<TshirtProperties> Orders
        {
            get { return _orders; }
            set { SetProperty(ref _orders, value); }
        }


        private DelegateCommand _confirmCommand;
        public DelegateCommand ConfirmCommand =>
            _confirmCommand ?? (_confirmCommand = new DelegateCommand(ExecuteConfirmCommand));

        public OrderListViewModel(INavigationService navigationService, IDatabase database, IPageDialogService dialogService) : base(navigationService)
        {
            _database = database;
            _dialogService = dialogService;
        }

        private async void ExecuteConfirmCommand()
        {

            var current = Connectivity.NetworkAccess;
            if (current == NetworkAccess.Internet)
            {
                await _dialogService.DisplayAlertAsync("Connection", "Internet is working", "ok");
            }
            if (current != NetworkAccess.Internet)
            {
                await _dialogService.DisplayAlertAsync("Connection", "Internet is not working", "ok");
            }
            var databaseContent = _database;

            // Orders = await databaseContent.GetUnSubmittedOrders();





            var stuff = new TshirtDatabase();
            var unsubmitted = await stuff.GetUnSubmittedOrders();

            //var tshirts = await _database.GetItemsAsync();
            //Orders = new ObservableCollection<TshirtProperties>(tshirts);
            var MyServerOrders = unsubmitted.Select(x => new TshirtProperties()
            {
                Name = x.Name,
                Gender = x.Gender,
                Tshirtsize = x.Tshirtsize,
                Datetime = x.Datetime,
                Tshirtcolor = x.Tshirtcolor,
                Shippingadress = x.Shippingadress
            }).ToList();
            var json = JsonConvert.SerializeObject(MyServerOrders);
            var client = new HttpClient();
            var url = "http://10.0.2.2:5000/Products";
            var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
            try
            {
                var response = await client.PostAsync(url, content);
                await _dialogService.DisplayAlertAsync("Response Message", response.ReasonPhrase, "ok");
                for (int i = 0; i < Orders.Count; i++)
                {
                    Orders[i].Posted = true;
                    await databaseContent.SaveItemAsync(Orders[i]);
                }
            }
            catch (Exception ex)
            {
                await _dialogService.DisplayAlertAsync("Exception", ex.Message, "ok");
            }
        }

           public async override void Initialize(INavigationParameters parameters)
           {
            base.OnNavigatedFrom(parameters);

            Orders = new ObservableCollection<TshirtProperties>(await _database.GetItemsAsync());
           }
    }
    
    
}