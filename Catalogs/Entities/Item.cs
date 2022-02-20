namespace Catalogs.Entities
{
    public record Item
    {
        //immutable property for set ie init (init - only)
        public Guid Id { get; init; }
        public string Name { get; init; } 
        public decimal Price { get; init; }
        public DateTimeOffset  CreatedDate { get; init; }

    }
}
