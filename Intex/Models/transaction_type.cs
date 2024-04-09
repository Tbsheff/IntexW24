using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class transaction_type
{
    public byte transaction_type_id { get; set; }

    public string description { get; set; } = null!;
}
