using System;
using System.Collections.Generic;

namespace Intex.Models;

public partial class order
{
    public int transaction_ID { get; set; }

    public int customer_ID { get; set; }

    public DateOnly date { get; set; }

    public string day_of_week { get; set; } = null!;

    public byte hour { get; set; }

    public byte entry_mode_id { get; set; }

    public short amount { get; set; }

    public byte transaction_type_id { get; set; }

    public string country_of_transaction { get; set; } = null!;

    public string shipping_address { get; set; } = null!;

    public byte bank_id { get; set; }

    public byte card_type_id { get; set; }

    public bool fraud { get; set; }
}
