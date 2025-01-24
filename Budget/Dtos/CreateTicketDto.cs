using Budget.Entities.Ticketing;

namespace Budget.Dtos;

public class CreateTicketDto
{
    public TicketCategory Category { get; set; }
    public int EventId { get; set; }
    public DateTimeOffset Date { get; set; }
    public long Amount { get; set; }
}