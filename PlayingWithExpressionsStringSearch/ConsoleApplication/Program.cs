using ConsoleApplication.Entities;
using Microsoft.EntityFrameworkCore;
using OnlineShop.MMA.Data.OnlineShopDbContext;
using QueryableExtensions.Extensions;

namespace ConsoleApplication
{
    internal class Program
    {
        private static Random random = new Random();

        public static User GetRandomUser(List<User> users)
        {
            int randomIndex = random.Next(0, users.Count);
            return users[randomIndex];
        }
        public static Product GetRandomProducts(List<Product> products)
        {
            int randomIndex = random.Next(0, products.Count);
            return products[randomIndex];
        }


        static async Task Main(string[] args)
        {
            var orderDbContext = new OrderDbContext();

            var users = new List<User>();
            for (int userCounter = 1; userCounter < 6; userCounter++)
            {
                users.Add(new User
                {
                    UserName = $"user{userCounter}",
                    Email =$"email{userCounter}@gmail.com",
                    FirstName= $"firstName{userCounter}",
                    LastName= $"lastName{userCounter}",                    
                });
            }
            await orderDbContext.Users.AddRangeAsync(users);
            await orderDbContext.SaveChangesAsync();

            var products = new List<Product>();
            for (int productCounter = 1; productCounter < 6; productCounter++)
            {
                products.Add(new Product
                {
                    Name = $"name{productCounter}",
                    Price = productCounter,
                    Description = $"description{productCounter}"
                });
            }
            await orderDbContext.Products.AddRangeAsync(products);
            await orderDbContext.SaveChangesAsync();

            var orders = new List<Order>();
            for (int orderCounter = 1; orderCounter < 11; orderCounter++)
            {
                var orderItems = new List<OrderItem>();
                for (int orderItemCounter = 1; orderItemCounter < 6; orderItemCounter++)
                {
                    orderItems.Add(new OrderItem
                    {
                        Quantity=orderCounter,
                        Price=orderCounter,                            
                        Product= GetRandomProducts(products)
                    });
                }


                var randomUser = GetRandomUser(users);
                orders.Add(new Order
                {
                    DateTime = DateTime.Now.AddDays(orderCounter),
                    Status = orderCounter,
                    User = randomUser,
                    Payment = new Payment
                    {
                        BankAccountNumber = $"bankAccountNumber{orderCounter}@gmail.com",
                        DateTime = DateTime.Now.AddDays(orderCounter),
                        Value = orderCounter,
                    },
                    OrderItems = orderItems
                });
            }
            await orderDbContext.Orders.AddRangeAsync(orders);
            await orderDbContext.SaveChangesAsync();

            var queryableOrders = orderDbContext.Orders
                .Include(order => order.User)
                .Include(order => order.Payment)
                .Search(order => new
                { 
                    order.User.UserName,
                    OrderDateTime= order.DateTime,
                    order.Status,
                    order.Payment.Value,
                    PaymentDateTime=order.Payment.DateTime
                },"2")
                .Select(order => new OrderModel
                {
                    IdOrder = order.IdOrder,
                    BuyerUserName = order.User.UserName!,
                    OrderDateTime = order.DateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    OrderStatus = order.Status,
                    PaymentDateTime = order.Payment.DateTime.ToString("yyyy/MM/dd HH:mm:ss"),
                    PaymentValue = order.Payment.Value
                })
                .ToListAsync();                           
        }
    }

    public class OrderModel
    {
        public int IdOrder { get; set; }
        public string BuyerUserName { get; set; } = string.Empty;
        public string OrderDateTime { get; set; } = string.Empty;
        public int OrderStatus { get; set; }

        public string BankAccountNumber { get; set; } = null!;
        public string PaymentDateTime { get; set; } = string.Empty;
        public decimal PaymentValue { get; set; }
    }
}
