using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class Entry_Mode
{
    public byte entry_mode_id { get; set; }

    public string description { get; set; } = null!;
}
