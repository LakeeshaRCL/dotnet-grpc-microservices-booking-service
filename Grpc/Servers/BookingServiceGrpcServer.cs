using BookingService.DTOs;
using BookingService.DTOs.Responses;
using BookingService.Services;
using BookingServiceProtoService;
using Grpc.Core;

namespace BookingService.Grpc.Servers;

public class BookingServiceGrpcServer : BookingServiceProtoService.BookingServiceProtoService.BookingServiceProtoServiceBase
{
    private readonly IBookingService bookingService;

    public BookingServiceGrpcServer(IBookingService bookingService)
    {
        this.bookingService = bookingService;  
    }


    public override Task<FilterBookingsGrpcResponse> FilterBookings(FilterBookingsRequest request, ServerCallContext context)
    {
        
        BaseResponse baseResponse = this.bookingService
            .FilterBookings(new DTOs.Requests.FilterBookingsRequest{ flightId = request.FlightId });
        FilterBookingsGrpcResponse grpcResponse = new FilterBookingsGrpcResponse();
        grpcResponse.StatusCode = baseResponse.statusCode;
        
        
        if (baseResponse.statusCode == StatusCodes.Status200OK)
        {
            List<BookingDTO>? bookingDTOs = baseResponse.data as List<BookingDTO> ?? new List<BookingDTO>();
            grpcResponse.Bookings.AddRange(bookingDTOs.Select(b => new Booking
            {
                FlightId = b.flightId,
                UserId = b.userId,
                TicketCount = b.ticketCount,
                BookingDate = b.bookingDate,
            }).ToList());
        }
        
        return Task.FromResult(grpcResponse);
    }
}