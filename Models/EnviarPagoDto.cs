namespace AppMercadoPago_Youtube.Models
{
    public class EnviarPagoDto
    {
        public required string Email { get; set; }
        public required string PhoneNumber { get; set; }
        public required string PersonType { get; set; }
        public string IdentificationType { get; set; }
        public required string IdentificationNumber { get; set; }
        public required string BanksList { get; set; }
        public required string TransactionAmount { get; set; }
        public required string Description { get; set; }
        public required string ZipCode { get; set; }
        public required string StreetName { get; set; }
        public required string StreetNumber { get; set; }
        public required string Neighborhood { get; set; }
        public required string City { get; set; }
        public required string FederalUnit { get; set; }
        public required string PhoneAreaCode { get; set; }
    }
}
