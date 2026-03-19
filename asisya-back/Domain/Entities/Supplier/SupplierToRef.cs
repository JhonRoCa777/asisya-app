namespace Domain.Entities.Supplier
{
    public class SupplierToRef
    {
        public Guid SupplierId { get; set; }

        public string CompanyName { get; set; } = string.Empty;
    }
}
