using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;

namespace Intex.Models;

public class Cart
{

    public List<CartLine> Lines { get; set; } = new List<CartLine>();

    public void AddItem(Product pro, int quantity)
    {
        CartLine? line = Lines
            .Where(x => x.name.product_id == pro.product_id)
            .FirstOrDefault();

        if (line == null)
            Lines.Add(new CartLine
            {
                name = pro,
                Quantity = quantity
            });
        else
            line.Quantity += quantity;
    }

    public void RemoveLine(Product pro) => Lines.RemoveAll(x => x.name.product_id == pro.product_id);



    public void Clear() => Lines.Clear();

    public decimal CalculateTotal() => Lines.Sum(x => x.name.price * x.Quantity);


    public void UpdateItem(int productId, int quantity)
    {
        var line = Lines.FirstOrDefault(l => l.name.product_id == productId);
        if (line != null)
        {
            line.Quantity = quantity;
        }
    }
    

    public class CartLine
    {
        public int CartLineID { get; set; }
        public Product name { get; set; } = new();
        public int Quantity { get; set; }
    }
}