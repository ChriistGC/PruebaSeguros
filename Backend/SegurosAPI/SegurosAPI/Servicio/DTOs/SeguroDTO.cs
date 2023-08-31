namespace SegurosAPI.Servicio.DTOs
{
    public class SeguroDTO
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? codigo { get; set; }
        public decimal? suma { get; set; }
        public decimal? prima { get; set; }
    }
}
