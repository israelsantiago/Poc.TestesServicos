namespace PoC.TestesServicos.Data.Models
{
    public class Customer
    {
        public int Code { get; set; }
        public string Name { get; set; }
        public string Cep { get; set; }
        public CepModel CepDetails { get; set; }
    }        
}