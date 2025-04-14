using InventoryControl.Models;

namespace InventoryControl.Mails;

public class StockAlertMail
{
    public async Task sendAsync(string to, Product product, int quantity)
    {
        Console.WriteLine("Sending StockAlertMail");
    }
}