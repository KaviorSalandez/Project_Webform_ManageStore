using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication2.Models;

public partial class Brand
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Image { get; set; }

    public virtual ICollection<Product> Products { get; } = new List<Product>();

    [NotMapped]
    public IFormFile imageFile1 { get; set; }
}
