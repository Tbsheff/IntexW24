using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class Card_Type
{
    public byte card_type_id { get; set; }

    public string description { get; set; } = null!;
}
