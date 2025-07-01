using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProductApi.Domain.Entities
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string NameProduct { get; set; }
        public string DescriptionProduct { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        //[ForeignKey("TypeProductId")]
        //public int TypeProductId { get; set; }
        //public virtual TypeProduct TypeProduct { get; set; }
        //public ICollection<ProductImages> ProductImages { get; set; }

        //public ICollection<OrderDetails> OrderDetails { get; set; }
    }
}
