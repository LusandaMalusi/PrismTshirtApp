using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using Tshirt.Models;
using Tshirt.Service.Interfaces;

namespace Tshirt.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private IDatabase _database;

        private DelegateCommand _placeorderCommand ;
        public DelegateCommand PlaceOrderCommand =>
            _placeorderCommand ?? (_placeorderCommand = new DelegateCommand(ExecutePlaceOrderCommand));


        private DelegateCommand _viewOrderCommand;
        public DelegateCommand ViewOrderCommand =>
            _viewOrderCommand ?? (_viewOrderCommand = new DelegateCommand(ExecuteViewOrderCommand));

        public TshirtProperties TshirtOrder { get; set; }

       public async void ExecuteViewOrderCommand()
        {
            await NavigationService.NavigateAsync("OrderList");
        }

       public async void ExecutePlaceOrderCommand()
        {
            await NavigationService.NavigateAsync("PlaceOrder");

        }
       public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";
        }

       public override void Initialize(INavigationParameters parameters)
        {
            base.Initialize(parameters);
            TshirtOrder = new TshirtProperties();
        }
    }
}
