namespace SegurosAPI.Modelo
{
    public class Cliente
    {
        public int id { get; set; }
        public string cedula { get; set; }
        public string nombre { get; set; }
        public string telefono { get; set; }
        public int edad { get; set; }
        public ICollection<ClienteSeguro> segurosLink { get; set; }
    }
}
