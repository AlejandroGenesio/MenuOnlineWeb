using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class VariantDTO:IVariantDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; } = 0;
        public int Stock { get; set; } = 0;
        public int ProductId { get; set; }
    }


    public interface IVariantDTO
    {
         int Id { get; set; }
         string? Name { get; set; }
         string? Description { get; set; }
         decimal Price { get; set; }
         int Stock { get; set; }      
    }



}
