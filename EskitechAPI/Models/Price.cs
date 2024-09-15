using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Price
{
    public int Id { get; set; }
    
    // One-to-One: Each product has exactly one price
    [ForeignKey("Product")] 
    public int ProductId { get; set; }
    public decimal Amount { get; set; }
}