using System.ComponentModel;

namespace BookingService.DTOs.Requests;

public class FilterBookingsRequest
{
    public long? flightId { get; set; } 
    
    public long? userId { get; set; }
}