using PetsApp.Models;
using PetsApp.Services;
using System.Collections.ObjectModel;

namespace PetsApp.ViewModel
{
    class OrderDetailsVM : ObservableObject
    {
        private readonly OrderDetailsServices _orderDetailsServices;
        private ObservableCollection<OrderDetailsModel> _orderDetails;

        public ObservableCollection<OrderDetailsModel> OrderDetails
        {
            get { return _orderDetails; }
            set
            {
                _orderDetails = value;
                OnPropertyChanged(nameof(OrderDetails));
            }
        }

        public OrderDetailsVM(int orderId)
        {
            _orderDetailsServices = new OrderDetailsServices();
            LoadOrderDetails(orderId);
        }

        private void LoadOrderDetails(int orderId)
        {
            OrderDetails = new ObservableCollection<OrderDetailsModel>((IEnumerable<OrderDetailsModel>)_orderDetailsServices.GetOrderDetailById(orderId));
        }
    }
}
