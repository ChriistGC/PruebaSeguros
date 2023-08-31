namespace SegurosAPI.Modelo
{
    public class Seguro
    {
        public int id { get; set; }
        public string? nombre { get; set; }
        public string? codigo { get; set; }
        public decimal? suma { get; set; }
        public decimal? prima { get; set; }
        public ICollection<ClienteSeguro> clienteLink { get; set; }
    }
}
