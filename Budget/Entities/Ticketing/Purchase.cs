namespace Budget.Entities.Ticketing;

public class Purchase
{
    public DateTimeOffset PurchaseDate { get; set; }
    public int TicketId { get; set; }
    public string BuyerId { get; set; }
    public int Quantity { get; set; }
}