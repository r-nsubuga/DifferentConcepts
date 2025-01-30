using Budget.Dtos;
using Budget.Entities.Ticketing;

namespace Budget.Services.Tickets;

public interface ITicketService
{
    Task CreateTicketAsync(CreateTicketDto ticket);
    Task DeleteTicketAsync(Guid id);
    Task BuyTicketAsync();
    Task SellTicketAsync();
}