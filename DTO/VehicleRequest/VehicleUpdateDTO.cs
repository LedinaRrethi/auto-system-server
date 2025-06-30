namespace DTO.VehicleRequest
{
    public class VehicleUpdateDTO
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public byte SeatCount { get; set; }
        public byte DoorCount { get; set; }
        public string ChassisNumber { get; set; } = string.Empty;
    }
}
