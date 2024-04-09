using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class product
{
    public byte product_id { get; set; }

    public string name { get; set; } = null!;

    public short year { get; set; }

    public short num_parts { get; set; }

    public short price { get; set; }

    public string img_link { get; set; } = null!;

    public string primary_color { get; set; } = null!;

    public string secondary_color { get; set; } = null!;

    public string description { get; set; } = null!;

    public byte category_id { get; set; }
}
