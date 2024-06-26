using PetsApp.Services;

namespace PetsApp.ViewModel
{
    public class CartVM : ObservableObject
    {
        private readonly CartServices _cartServices;
        private List<CartItem> _cartItems; // Add a property to store cart items

        public CartVM()
        {
            _cartServices = new CartServices();
            _cartItems = new List<CartItem>(); // Initialize the cart items list
        }

        public List<CartItem> CartItems
        {
            get => _cartItems;
            set => SetProperty(ref _cartItems, value);
        }

        // Thêm các ph??ng th?c khác ?? t??ng tác v?i gi? hàng (tùy ch?n)

        public async Task UpdateQuantity(int userId, int petId, int newQuantity)
        {
            try
            {
                var updatedCartItem = await _cartServices.UpdateQuantity(userId, petId, newQuantity); // Call CartServices method

                if (updatedCartItem != null) // Check if update was successful
                {
                    // Update the CartItems list with the updated cart item
                    var cartItemToUpdate = CartItems.FirstOrDefault(item => item.UserID == userId && item.PetID == petId);
                    if (cartItemToUpdate != null)
                    {
                        cartItemToUpdate.Quantity = updatedCartItem.Quantity;

                        // Notify UI about the change (optional)
                        // Implement a mechanism to notify the UI thread about the update
                        // This could involve events, messaging, or other techniques
                        // depending on your application architecture
                    }
                }
            }
            catch (Exception ex)
            {
                // Handle error
            }
        }
    }
}