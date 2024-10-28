using System.ComponentModel.DataAnnotations;

namespace MenuOnlineUdemy.Entities
{
    public class Image
    {
        public int Id { get; set; }
        [StringLength(1000)]
        public string? Description { get; set; }
        [StringLength(150)]
        public string? File {  get; set; }
    }
}
