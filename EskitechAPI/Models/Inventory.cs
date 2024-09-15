using System.ComponentModel.DataAnnotations.Schema;

public class Inventory
{
    public int Id { get; set; }

    // Many-to-One: One product can have many inventories
    [ForeignKey("Product")] 
    public int ProductId { get; set; }
    public int Quantity { get; set; }
}