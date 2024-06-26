namespace PetsApp.Pages;

public partial class OrderDetails : ContentPage
{
    private List<OrderDetailsModel>? orderDetails;

    public OrderDetails()
    {
        InitializeComponent();
        UpdateOrderDetailsList();
        
    }

    public OrderDetails(List<OrderDetailsModel>? orderDetails)
    {
        InitializeComponent();
        this.orderDetails = orderDetails;
        UpdateOrderDetailsList();
        UpdateProgressBar();
    }
private void UpdateProgressBar()
{
    if (orderDetails != null && orderDetails.Any())
    {
        // L?y tr?ng thái c?a ??n hàng t? ??i t??ng ??u tiên trong danh sách (gi? s? m?i ??n hàng trong danh sách có cùng m?t tr?ng thái)
        int status = orderDetails.First().Order.Status;

        // Tính toán giá tr? c?a ProgressBar d?a trên tr?ng thái
        double progress = 0;

        // Ánh x? giá tr? status t? 0 ??n 3 sang giá tr? Progress t? 0.25 ??n 1
        switch (status)
        {
            case 0:
                progress = 0.25;
                break;
            case 1:
                progress = 0.5;
                break;
            case 2:
                progress = 0.75;
                break;
            case 3:
                progress = 1;
                break;
            default:
                progress = 0; // Tr??ng h?p không xác ??nh, b?n có th? ??t giá tr? m?c ??nh là 0
                break;
        }

        // C?p nh?t giá tr? c?a ProgressBar
        OrderStatusProgressBar.Progress = progress;
    }
}


    private void UpdateOrderDetailsList()
    {
        if (orderDetails != null)
        {
            OrderDetailsMM.ItemsSource = orderDetails;
            var recipientName = orderDetails.First().RecipientName;
            var recipientPhone = orderDetails.First().RecipientPhone;
            var address = orderDetails.First().Address;

            // Hiển thị thông tin người nhận lên giao diện
            RecipientNameLabel.Text = recipientName;
            RecipientPhoneLabel.Text = recipientPhone;
            AddressLabel.Text = address;
        }
    }

    private async void Goback(object sender, TappedEventArgs e)
    {
        await Navigation.PopAsync();
    }
}
