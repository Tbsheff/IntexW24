namespace Intex.Middleware
{
	public class CartItemCountMiddleware
	{
		private readonly RequestDelegate _next;

		public CartItemCountMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			// Assuming you have a method to get the cart item count. You might need to inject services if necessary.
			int cartItemCount = context.Session.GetInt32("CartItemCount") ?? 0;
			context.Items["CartItemCount"] = cartItemCount;

			// Call the next delegate/middleware in the pipeline
			await _next(context);
		}
	}

	// Extension method used to add the middleware to the HTTP request pipeline.
	public static class CartItemCountMiddlewareExtensions
	{
		public static IApplicationBuilder UseCartItemCount(this IApplicationBuilder builder)
		{
			return builder.UseMiddleware<CartItemCountMiddleware>();
		}
	}

}
