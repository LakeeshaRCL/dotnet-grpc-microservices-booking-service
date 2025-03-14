using System.Diagnostics.CodeAnalysis;

namespace BookingService.DTOs;

public class MessageDTO
{
    public required string message { get; set; }

    [SetsRequiredMembers]
    public MessageDTO(string message)
    {
        this.message = message;
    }
}