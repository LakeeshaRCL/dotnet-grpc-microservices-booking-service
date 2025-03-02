using BookingService.DTOs;
using BookingService.DTOs.Responses;
using FlightServiceProtoService;
using Grpc.Core;
using Grpc.Net.Client;

namespace BookingService.Grpc.Clients;

public class FlightServiceClient
{
    private readonly FlightServiceProtoService.FlightServiceProtoService.FlightServiceProtoServiceClient client;
    public FlightServiceClient(string gRpcHostUrl)
    {
        GrpcChannel channel = GrpcChannel.ForAddress(gRpcHostUrl, new GrpcChannelOptions
        {
            HttpHandler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator,
            }
        });
        client = new FlightServiceProtoService.FlightServiceProtoService.FlightServiceProtoServiceClient(channel);
    }
    
    public BaseResponse GetFlight(long id)
    {
        try
        {
            GetFlightGrpcResponse grpcResponse =  client.GetFlight(new GetFlightRequest{Id = id});
            FlightDTO? flightDTO = null;

            if (grpcResponse.Flight != null)
            {
                flightDTO = new FlightDTO
                {
                    airlineName = grpcResponse.Flight.AirlineName,
                    arrivalTime = grpcResponse.Flight.ArrivalTime,
                    availableSeats = grpcResponse.Flight.AvailableSeats,
                    departureTime = grpcResponse.Flight.DepartureTime,
                    destination = grpcResponse.Flight.Destination,
                    source = grpcResponse.Flight.Source,
                };
            }
            return new BaseResponse(grpcResponse.StatusCode, flightDTO);
        }
        catch (Exception e)
        {
            Console.Error.WriteLine(e);
            return new BaseResponse(StatusCodes.Status500InternalServerError, "gRPC client error");
        }
    }
}