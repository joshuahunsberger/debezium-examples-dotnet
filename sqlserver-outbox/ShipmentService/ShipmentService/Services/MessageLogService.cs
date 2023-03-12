using Microsoft.EntityFrameworkCore;
using ShipmentService.Data;

namespace ShipmentService.Services;

public interface IMessageLogService
{
    Task LogProcessedMessage(Guid messageKey);
}

public class MessageLogService : IMessageLogService
{
    private readonly ShipmentsContext _context;

    public MessageLogService(ShipmentsContext context)
    {
        _context = context;
    }

    public async Task LogProcessedMessage(Guid messageKey)
    {
        await _context.Database.ExecuteSqlAsync($"INSERT INTO [Inventory].[ConsumedMessages] (EventId, TimeOfReceiving) VALUES ( {messageKey}, SYSDATETIMEOFFSET() )");
    }
}

