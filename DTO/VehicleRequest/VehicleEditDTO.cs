namespace DTO.VehicleDTO
{
    public class VehicleEditDTO
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string ChassisNumber { get; set; } = string.Empty;
        public byte SeatCount { get; set; }
        public byte DoorCount { get; set; }
    }
}
