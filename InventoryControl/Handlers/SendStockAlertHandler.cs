using InventoryControl.Mails;
using InventoryControl.Models;

namespace InventoryControl.Handlers;

public class SendStockAlertHandler
{
    public static async Task handleAsync(string to, Product product, int quantity)
    {
        var mail = new StockAlertMail();
        mail.sendAsync(to, product, quantity);
    }
}