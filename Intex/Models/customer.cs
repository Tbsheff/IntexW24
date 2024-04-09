using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class customer
{
    public short customer_ID { get; set; }

    public string first_name { get; set; } = null!;

    public string last_name { get; set; } = null!;

    public DateOnly birth_date { get; set; }

    public string country_of_residence { get; set; } = null!;

    public string gender { get; set; } = null!;

    public byte age { get; set; }
}
