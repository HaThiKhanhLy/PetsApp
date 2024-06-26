using System.Collections.Generic;
using PetsApp.Models;

namespace PetsApp.ViewModel
{
    public class CheckoutVM : ObservableObject
    {
        private List<CartItem> _cartItems;
        public List<CartItem> CartItems
        {
            get { return _cartItems; }
            set { SetProperty(ref _cartItems, value); }
        }

        public double TotalPrice => CartItems?.Sum(item => item.Pet.Price * item.Quantity) ?? 0;

        public double ShippingFee { get; set; } // Thêm logic tính phí vận chuyển nếu cần

        public double TotalOrder => TotalPrice + 20;

        public CheckoutVM(List<CartItem> cartItems)
        {
            CartItems = cartItems;
        }
    }
}