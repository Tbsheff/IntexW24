using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class category
{
    public byte category_id { get; set; }

    public string description { get; set; } = null!;
}
