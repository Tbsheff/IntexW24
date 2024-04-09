using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class card_type
{
    public byte card_type_id { get; set; }

    public string description { get; set; } = null!;
}
