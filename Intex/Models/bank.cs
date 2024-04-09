using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class Bank
{
    public byte bank_id { get; set; }

    public string name { get; set; } = null!;
}
