using BookingService.DTOs.Requests;
using BookingService.DTOs.Responses;

namespace BookingService.Services;

public interface IBookingService
{
    BaseResponse MakeBooking(MakeBookingRequest request);
    
    BaseResponse GetBookings();

    BaseResponse FilterBookings(FilterBookingsRequest request);
}