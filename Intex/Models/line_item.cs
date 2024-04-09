using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class Line_Item
{
    public int index { get; set; }

    public int transaction_ID { get; set; }

    public byte product_ID { get; set; }

    public byte qty { get; set; }

    public byte rating { get; set; }
}
