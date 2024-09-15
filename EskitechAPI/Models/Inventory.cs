using System.ComponentModel.DataAnnotations.Schema;

public class Inventory
{
    public int Id { get; set; }

    // Many-to-One: One product can have many inventories
    public int ProductId { get; set; }
    public int Quantity { get; set; }

    [ForeignKey("Product")]
    public virtual Product Product { get; set; }
}