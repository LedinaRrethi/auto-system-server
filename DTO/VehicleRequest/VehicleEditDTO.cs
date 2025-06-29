namespace DTO.VehicleDTO
{
    public class VehicleEditDTO
    {
        public string PlateNumber { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
        public string ChassisNumber { get; set; } = string.Empty;
        public int SeatCount { get; set; }
        public int DoorCount { get; set; }
    }
}
