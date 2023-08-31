namespace SegurosAPI.Modelo
{
    public class ClienteSeguro
    {
        public int clienteid { get; set; }
        public int seguroid { get; set; }
        public Cliente cliente { get; set; }
        public Seguro seguro { get; set; }
    }
}
