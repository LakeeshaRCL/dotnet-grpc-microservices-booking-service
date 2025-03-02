using System.Diagnostics.CodeAnalysis;
using BookingService.Models;

namespace BookingService.DTOs;

public class BookingDTO
{
    public long flightId { get; set; }
    
    public long userId { get; set; }
    
    public int ticketCount { get; set; }
    
    public string bookingDate { get; set; }

    [SetsRequiredMembers]
    public BookingDTO(BookingModel booking)
    {
        this.flightId = booking.flightId;
        this.userId = booking.userId;
        this.ticketCount = booking.ticketCount;
        this.bookingDate = booking.createdAt.ToString("s");
    }
}