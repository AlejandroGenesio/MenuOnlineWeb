using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.DTOs
{
    public class VariantDTO:IVariantDTO
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal price { get; set; }
        public int stock { get; set; }
        public int ProductId { get; set; }       
    }


    public interface IVariantDTO
    {
         int Id { get; set; }
         string? Name { get; set; }
         string? Description { get; set; }
         decimal price { get; set; }
         int stock { get; set; }      
    }



}
