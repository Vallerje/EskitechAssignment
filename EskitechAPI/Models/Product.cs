using System.ComponentModel.DataAnnotations;

public class Product
{
    public int Id { get; set; }

    [Required] 
    public string Name { get; set; }

    public string Description { get; set; }

    // One-to-Many: A product can have many inventories
    public virtual ICollection<Inventory> Inventories { get; set; }

    // One-to-One: A product can only have one price
    public virtual Price Price { get; set; }
}