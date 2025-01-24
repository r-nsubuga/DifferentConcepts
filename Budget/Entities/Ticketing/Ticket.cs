namespace Budget.Entities.Ticketing;

public class Ticket
{
    public Guid Id { get; set; }
    public int EventId { get; set; }
    public long Quantity { get; set; }
    public TicketCategory Category { get; set; }
    public long Price { get; set; }
    public DateTimeOffset Date { get; set; }
    public ICollection<Purchase> Purchases { get; set; }
}