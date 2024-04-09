using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class Transaction_Type
{
    public byte transaction_type_id { get; set; }

    public string description { get; set; } = null!;
}
