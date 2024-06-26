using Newtonsoft.Json;
using System.Text;


namespace PetsApp.Pages.Admin;

public partial class OrdersAdmin : ContentPage
{
    HttpClient client = new HttpClient();
    public OrdersAdmin()
	{
		InitializeComponent();
        LoadOrders();
        ordersListView.ItemSelected += OnItemSelected;
    }
    private string GetStatusText(int status)
    {
        switch (status)
        {
            case 0:
                return "Ordered";
            case 1:
                return "Order confirmed";
            case 2:
                return "Delivering";
            case 3:
                return "Successful delivery";
            default:
                return "Cancelled";
        }
    }

    private async void LoadOrders()
    {
        try
        {
            var httpClient = new HttpClient();
            var apiUrl = Config.ApiConfig.BaseUrl + "/Orders";
            var response = await httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var orders = JsonConvert.DeserializeObject<List<OrderModel>>(content);

                // Chuyển đổi giá trị status thành văn bản tương ứng
                foreach (var order in orders)
                {
                    order.statusText = GetStatusText(order.Status);
                }

                // Gán danh sách đơn hàng đã được cập nhật vào ItemsSource của ListView
                ordersListView.ItemsSource = orders;
            }
            else
            {
                await DisplayAlert("Error", "Failed to load orders. Please try again later.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
        }
    }

    
    async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
    {
        if (e.SelectedItem == null)
            return;

        var selectedOrder = e.SelectedItem as OrderModel; // Thay YourOrderModel bằng tên lớp chứa dữ liệu đơn hàng của bạn
        var orderId = selectedOrder.Id;

        // Gọi API để lấy chi tiết đơn hàng từ máy chủ
        string apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/OrderDetails/{0}", orderId);
        var response = await client.GetAsync(apiUrl);

        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var orderDetails = JsonConvert.DeserializeObject<List<OrderDetailsModel>>(content); // Thay List<OrderDetail> bằng lớp chứa dữ liệu chi tiết đơn hàng của bạn
            await Navigation.PushAsync(new OrderDetailsAdmin(orderDetails));
        }
        else
        {
            // Xử lý trường hợp không thành công
        }

            // Chọn lại mục đầu tiên để cho phép người dùng nhấn lại
            ((ListView)sender).SelectedItem = null;
    }

    private async void StatusPickerSelectedIndexChanged(object sender, EventArgs e)
    {
        var picker = sender as Picker;
        var selectedOrder = picker.BindingContext as OrderModel;
        if (selectedOrder != null)
        {
            var selectedIndex = picker.SelectedIndex;

            // Kiểm tra xem chỉ số đã được chọn hợp lệ không
            if (selectedIndex >= 0 && selectedIndex <= 4)
            {
                int newStatus = selectedIndex; // Gán chỉ số trực tiếp vào newStatus

                try
                {
                    var httpClient = new HttpClient();
                    /*var apiUrl = $"https://localhost:7104/api/Orders/{selectedOrder.Id}/{newStatus}";*/ // Thêm newStatus vào URL
                    string apiUrl = string.Format(Config.ApiConfig.BaseUrl + "/Orders/{0}/{1}", selectedOrder.Id, newStatus);
                    var response = await httpClient.PutAsync(apiUrl, null);

                    if (response.IsSuccessStatusCode)
                    {
                        // Cập nhật trạng thái của đơn hàng trong danh sách hiện tại
                        selectedOrder.Status = newStatus; // Không cần lấy giá trị từ int? vì không có giá trị null
                        selectedOrder.statusText = GetStatusText(newStatus);
                        await DisplayAlert("Success", "Order status updated successfully.", "OK");
                        LoadOrders();
                    }
                    else
                    {
                        await DisplayAlert("Error", "Failed to update order status. Please try again later.", "OK");
                    }
                }
                catch (Exception ex)
                {
                    await DisplayAlert("Error", $"An error occurred: {ex.Message}", "OK");
                }
            }
            else
            {
                await DisplayAlert("Error", "Invalid status selected.", "OK");
            }
        }
    }


    private async void Goback(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}

