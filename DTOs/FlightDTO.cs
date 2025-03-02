namespace BookingService.DTOs;

public class FlightDTO
{
    public required string airlineName { get; set; }
    
    public required string source { get; set; }
    
    public required string destination { get; set; }
    
    public int availableSeats { get; set; }

    public string departureTime { get; set; }
    
    public string arrivalTime { get; set; }

}