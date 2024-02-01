using Microsoft.EntityFrameworkCore;
using MVCWebApplication.Entities;
using System.Reflection.Emit;

namespace MVCWebApplication.DbContexts
{   
    public static class DataProvider
    {
        private static DateTime DateTimeNow = new (2024, 1, 1,9,1,1);
        public static void Seed(this ModelBuilder modelBuilder)
        {
            var users = GetUsers();
            modelBuilder.Entity<User>().HasData(users);

            var products = GetProducts();
            modelBuilder.Entity<Product>().HasData(products);

            var orders= GetOrders();
            modelBuilder.Entity<Order>().HasData(orders);

            var orderItems = GetOrderItems(products);
            modelBuilder.Entity<OrderItem>().HasData(orderItems);

            var payments=GetPayments(orderItems);
            modelBuilder.Entity<Payment>().HasData(payments);
        }
        public static IEnumerable<Order> GetEnumerableOrders()
        {
            var products = GetProducts();
            var orderItems = GetOrderItems(products);

            foreach (var orderItem in orderItems)
            {
                orderItem.Product = products.FirstOrDefault(t => t.IdProduct == orderItem.ProductId)!;
            }

            var users = GetUsers();
            var orders = GetOrders();
            var payments = GetPayments(orderItems);

            foreach (var order in orders)
            {
                order.User = users.FirstOrDefault(t => t.IdUser == order.UserId)!;
                order.Payment = payments.FirstOrDefault(t => t.OrderId == order.IdOrder)!;
                order.OrderItems = orderItems.Where(t => t.OrderId == order.IdOrder).ToList();
            }

            return orders;
        }
        public static IEnumerable<OrderItem> GetEnumerableOrderItems(int orderId)
        {
            var products = GetProducts();
            var orderItems = GetOrderItems(products)
                .Where(t=>t.OrderId == orderId);

            foreach (var orderItem in orderItems)
            {
                orderItem.Product = products
                    .FirstOrDefault(t => t.IdProduct == orderItem.ProductId)!;
            }

            return orderItems;
        }

        private static List<User> GetUsers()
        {
            return new List<User>
            {
                new User{IdUser= 1,UserName="bsargant0",FirstName="Barnie",LastName="Sargant",Email="bsargant0@ycombinator.com"},
                new User{IdUser= 2,UserName="dmangenet1",FirstName="Delmar",LastName="Mangenet",Email="dmangenet1@hud.gov"},
                new User{IdUser= 3,UserName="bjoutapavicius2",FirstName="Bird",LastName="Joutapavicius",Email="bjoutapavicius2@freewebs.com"},
                new User{IdUser= 4,UserName="gbernat3",FirstName="Gualterio",LastName="Bernat",Email="gbernat3@amazon.com"},
                new User{IdUser= 5,UserName="jrennocks4",FirstName="Jaquenetta",LastName="Rennocks",Email="jrennocks4@tuttocitta.it"},
                new User{IdUser= 6,UserName="bspeechly5",FirstName="Bridget",LastName="Speechly",Email="bspeechly5@wordpress.com"},
                new User{IdUser = 7, UserName="kpfeiffer6",FirstName="Kristi",LastName="Pfeiffer",Email="kpfeiffer6@sphinn.com"},
                new User{IdUser= 8,UserName="mquinnell7",FirstName="Mandel",LastName="Quinnell",Email="mquinnell7@springer.com"},
                new User{IdUser = 9, UserName="jsantino8",FirstName="Justino",LastName="Santino",Email="jsantino8@goodreads.com"},
                new User{IdUser = 10, UserName="jclowes9",FirstName="Janot",LastName="Clowes",Email="jclowes9@blinklist.com"},
                new User{IdUser= 11,UserName="jspeechly66",FirstName="Janot",LastName="Speechly",Email="jspeechly66@blinklist.com"},
                new User{IdUser = 12, UserName="jBernat19",FirstName="Janot",LastName="Bernat",Email="jBernat19@gmail.com"},
            };             
        }
        private static List<Product> GetProducts()
        {
            return new List<Product>
            {
                new Product{IdProduct=1, Name="iPhone 13",Type="Smartphone",Price=699,Description="Cuttingedge Apple phone with A15 Bionic chip, dualcamera system, and stunning Super Retina XDR display."},
                new Product{IdProduct=2,Name="Dell XPS 13",Type="Laptop",Price=999,Description="Sleek ultrabook with powerful processors, InfinityEdge display, and premium build, ideal for productivity on the go."},
                new Product{IdProduct = 3, Name="iPad Air",Type="Tablet",Price=499,Description="Powerful and portable tablet featuring the A14 Bionic chip, 10.9inch Liquid Retina display, and support for the Apple Pencil."},
                new Product{IdProduct=4,Name="Canon EOS M50",Type="Digital Camera",Price=649,Description="Mirrorless camera with 4K video, Dual Pixel CMOS AF, and a variangle touchscreen for creative photography and videography."},
                new Product{IdProduct = 5, Name="Samsung Galaxy S21",Type="Smartphone",Price=799,Description="A flagship Android phone with a powerful Exynos/Snapdragon processor, impressive camera setup, and a vibrant Dynamic AMOLED display for an immersive mobile experience."},
                new Product{IdProduct=6,Name="Lenovo ThinkPad X1 Carbon",Type="Laptop",Price=1299,Description="Businessclass ultrabook known for its durability, excellent keyboard, and robust performance, providing professionals with a reliable computing solution."},
                new Product{IdProduct=7,Name="Microsoft Surface Pro 7",Type="Tablet",Price=799,Description="Versatile 2in1 tablet/laptop hybrid featuring a detachable keyboard, powerful processors, and a highresolution PixelSense display, ideal for work and creativity."},
                new Product{IdProduct=8,Name="Canon EOS Rebel T7i ",Type="Digital Camera",Price=699,Description="Entrylevel DSLR camera with a 24.2MP sensor, DIGIC 7 processor, and Dual Pixel CMOS AF for capturing highquality photos and Full HD videos, perfect for photography enthusiasts."},
                new Product{IdProduct=9,Name="Nokia 6",Type="Smartphone",Price=300,Description="The Nokia 6, a budgetfriendly Android smartphone, offers a sturdy build, a 5.5inch Full HD display, and efficient performance with its Snapdragon processor."},
            };
        }
        private static List<Order> GetOrders()
        {
            
            return new List<Order>
            {
                new Order {IdOrder = 1,DateTime = DateTimeNow.AddDays(1).AddMinutes(1),Status = 1,UserId = 1},
                new Order {IdOrder = 2,DateTime = DateTimeNow.AddDays(1).AddMinutes(2),Status = 1,UserId = 2},
                new Order {IdOrder = 3,DateTime = DateTimeNow.AddDays(1).AddMinutes(3),Status = 1,UserId = 3},
                new Order {IdOrder = 4,DateTime = DateTimeNow.AddDays(1).AddMinutes(4),Status = 1,UserId = 4},
                new Order {IdOrder = 5,DateTime = DateTimeNow.AddDays(1).AddMinutes(5),Status = 1,UserId = 5},
                new Order {IdOrder = 6,DateTime = DateTimeNow.AddDays(1).AddMinutes(6),Status = 1,UserId = 6},
                new Order {IdOrder = 7,DateTime = DateTimeNow.AddDays(1).AddMinutes(7),Status = 1,UserId = 7},
                new Order {IdOrder = 8,DateTime = DateTimeNow.AddDays(1).AddMinutes(8),Status = 1,UserId = 8},
                new Order {IdOrder = 9,DateTime = DateTimeNow.AddDays(1).AddMinutes(9),Status = 1,UserId = 9},
                new Order {IdOrder = 10,DateTime = DateTimeNow.AddDays(1).AddMinutes(10),Status = 1,UserId = 10},
                new Order {IdOrder = 11,DateTime = DateTimeNow.AddDays(2).AddMinutes(1),Status = 2,UserId = 11},
                new Order {IdOrder = 12,DateTime = DateTimeNow.AddDays(2).AddMinutes(2),Status = 2,UserId = 12},
                new Order {IdOrder = 13,DateTime = DateTimeNow.AddDays(2).AddMinutes(3),Status = 2,UserId = 1},
                new Order {IdOrder = 14,DateTime = DateTimeNow.AddDays(2).AddMinutes(4),Status = 2,UserId = 2},
                new Order {IdOrder = 15,DateTime = DateTimeNow.AddDays(2).AddMinutes(5),Status = 2,UserId = 3},
                new Order {IdOrder = 16,DateTime = DateTimeNow.AddDays(2).AddMinutes(6),Status = 2,UserId = 4},
                new Order {IdOrder = 17,DateTime = DateTimeNow.AddDays(2).AddMinutes(7),Status = 2,UserId = 5},
                new Order {IdOrder = 18,DateTime = DateTimeNow.AddDays(2).AddMinutes(8),Status = 2,UserId = 6},
                new Order {IdOrder = 19,DateTime = DateTimeNow.AddDays(2).AddMinutes(9),Status = 2,UserId = 7},
                new Order {IdOrder = 20,DateTime = DateTimeNow.AddDays(2).AddMinutes(10),Status = 2,UserId = 8},
            };
        }
        private static List<OrderItem> GetOrderItems(List<Product> products)
        {
            return new List<OrderItem>
            {
                new OrderItem { IdOrderItem=1,Quantity = 2,Price = products[0].Price,ProductId = products[0].IdProduct,OrderId=1},
                new OrderItem { IdOrderItem=2,Quantity = 1,Price = products[1].Price,ProductId = products[1].IdProduct,OrderId=1},
                new OrderItem { IdOrderItem=3,Quantity = 3,Price = products[2].Price,ProductId = products[2].IdProduct,OrderId=1},

                new OrderItem { IdOrderItem=4,Quantity = 2,Price = products[3].Price,ProductId = products[3].IdProduct,OrderId=2},
                new OrderItem { IdOrderItem=5,Quantity = 1,Price = products[4].Price,ProductId = products[4].IdProduct,OrderId=2},
                new OrderItem { IdOrderItem=6,Quantity = 3,Price = products[5].Price,ProductId = products[5].IdProduct,OrderId=2},

                new OrderItem { IdOrderItem=7,Quantity = 2,Price = products[6].Price,ProductId = products[6].IdProduct,OrderId=3},
                new OrderItem { IdOrderItem=8,Quantity = 1,Price = products[7].Price,ProductId = products[7].IdProduct,OrderId=3},
                new OrderItem { IdOrderItem=9,Quantity = 3,Price = products[8].Price,ProductId = products[8].IdProduct,OrderId=3},

                new OrderItem { IdOrderItem=10,Quantity = 2,Price = products[0].Price,ProductId = products[0].IdProduct,OrderId=4},
                new OrderItem { IdOrderItem=11,Quantity = 1,Price = products[1].Price,ProductId = products[1].IdProduct,OrderId=4},
                new OrderItem { IdOrderItem=12,Quantity = 3,Price = products[2].Price,ProductId = products[2].IdProduct,OrderId=4},

                new OrderItem { IdOrderItem=13,Quantity = 2,Price = products[3].Price,ProductId = products[3].IdProduct,OrderId=5},
                new OrderItem { IdOrderItem=14,Quantity = 1,Price = products[4].Price,ProductId = products[4].IdProduct,OrderId=5},
                new OrderItem { IdOrderItem=15,Quantity = 3,Price = products[5].Price,ProductId = products[5].IdProduct,OrderId=5},

                new OrderItem { IdOrderItem=16,Quantity = 2,Price = products[6].Price,ProductId = products[6].IdProduct,OrderId=6},
                new OrderItem { IdOrderItem=17,Quantity = 1,Price = products[7].Price,ProductId = products[7].IdProduct,OrderId=6},
                new OrderItem { IdOrderItem=18,Quantity = 3,Price = products[8].Price,ProductId = products[8].IdProduct,OrderId=6},

                new OrderItem { IdOrderItem=19,Quantity = 2,Price = products[0].Price,ProductId = products[0].IdProduct,OrderId=7},
                new OrderItem { IdOrderItem=20,Quantity = 1,Price = products[1].Price,ProductId = products[1].IdProduct,OrderId=7},
                new OrderItem { IdOrderItem=21,Quantity = 3,Price = products[2].Price,ProductId = products[2].IdProduct,OrderId=7},

                new OrderItem { IdOrderItem=22,Quantity = 2,Price = products[3].Price,ProductId = products[3].IdProduct,OrderId=8},
                new OrderItem { IdOrderItem=23,Quantity = 1,Price = products[4].Price,ProductId = products[4].IdProduct,OrderId=8},
                new OrderItem { IdOrderItem=24,Quantity = 3,Price = products[5].Price,ProductId = products[5].IdProduct,OrderId=8},

                new OrderItem { IdOrderItem=25,Quantity = 2,Price = products[6].Price,ProductId = products[6].IdProduct,OrderId=9},
                new OrderItem { IdOrderItem=26,Quantity = 1,Price = products[7].Price,ProductId = products[7].IdProduct,OrderId=9},
                new OrderItem { IdOrderItem=27,Quantity = 3,Price = products[8].Price,ProductId = products[8].IdProduct,OrderId=9},

                new OrderItem { IdOrderItem=28,Quantity = 2,Price = products[0].Price,ProductId = products[0].IdProduct,OrderId=10},
                new OrderItem { IdOrderItem=29,Quantity = 1,Price = products[1].Price,ProductId = products[1].IdProduct,OrderId=10},
                new OrderItem { IdOrderItem=30,Quantity = 3,Price = products[2].Price,ProductId = products[2].IdProduct,OrderId=10},

                new OrderItem { IdOrderItem=31,Quantity = 2,Price = products[3].Price,ProductId = products[3].IdProduct,OrderId=11},
                new OrderItem { IdOrderItem=32,Quantity = 1,Price = products[4].Price,ProductId = products[4].IdProduct,OrderId=11},
                new OrderItem { IdOrderItem=33,Quantity = 3,Price = products[5].Price,ProductId = products[5].IdProduct,OrderId=11},

                new OrderItem { IdOrderItem=34,Quantity = 2,Price = products[6].Price,ProductId = products[6].IdProduct,OrderId=12},
                new OrderItem { IdOrderItem=35,Quantity = 1,Price = products[7].Price,ProductId = products[7].IdProduct,OrderId=12},
                new OrderItem { IdOrderItem=36,Quantity = 3,Price = products[8].Price,ProductId = products[8].IdProduct,OrderId=12},

                new OrderItem { IdOrderItem=37,Quantity = 2,Price = products[0].Price,ProductId = products[0].IdProduct,OrderId=13},
                new OrderItem { IdOrderItem=38,Quantity = 1,Price = products[1].Price,ProductId = products[1].IdProduct,OrderId=13},
                new OrderItem { IdOrderItem=39,Quantity = 3,Price = products[2].Price,ProductId = products[2].IdProduct,OrderId=13},

                new OrderItem { IdOrderItem=40,Quantity = 2,Price = products[3].Price,ProductId = products[3].IdProduct,OrderId=14},
                new OrderItem { IdOrderItem=41,Quantity = 1,Price = products[4].Price,ProductId = products[4].IdProduct,OrderId=14},
                new OrderItem { IdOrderItem=42,Quantity = 3,Price = products[5].Price,ProductId = products[5].IdProduct,OrderId=14},

                new OrderItem { IdOrderItem=43,Quantity = 2,Price = products[6].Price,ProductId = products[6].IdProduct,OrderId=15},
                new OrderItem { IdOrderItem=44,Quantity = 1,Price = products[7].Price,ProductId = products[7].IdProduct,OrderId=15},
                new OrderItem { IdOrderItem=45,Quantity = 3,Price = products[8].Price,ProductId = products[8].IdProduct,OrderId=15},

                new OrderItem { IdOrderItem=46,Quantity = 2,Price = products[0].Price,ProductId = products[0].IdProduct,OrderId=16},
                new OrderItem { IdOrderItem=47,Quantity = 1,Price = products[1].Price,ProductId = products[1].IdProduct,OrderId=16},
                new OrderItem { IdOrderItem=48,Quantity = 3,Price = products[2].Price,ProductId = products[2].IdProduct,OrderId=16},

                new OrderItem { IdOrderItem=49,Quantity = 2,Price = products[3].Price,ProductId = products[3].IdProduct,OrderId=17},
                new OrderItem { IdOrderItem=50,Quantity = 1,Price = products[4].Price,ProductId = products[4].IdProduct,OrderId=17},
                new OrderItem { IdOrderItem=51,Quantity = 3,Price = products[5].Price,ProductId = products[5].IdProduct,OrderId=17},

                new OrderItem { IdOrderItem=52,Quantity = 2,Price = products[6].Price,ProductId = products[6].IdProduct,OrderId=18},
                new OrderItem { IdOrderItem=53,Quantity = 1,Price = products[7].Price,ProductId = products[7].IdProduct,OrderId=18},
                new OrderItem { IdOrderItem=54,Quantity = 3,Price = products[8].Price,ProductId = products[8].IdProduct,OrderId=18},

                new OrderItem { IdOrderItem=55,Quantity = 2,Price = products[0].Price,ProductId = products[0].IdProduct,OrderId=19},
                new OrderItem { IdOrderItem=56,Quantity = 1,Price = products[1].Price,ProductId = products[1].IdProduct,OrderId=19},
                new OrderItem { IdOrderItem=57,Quantity = 3,Price = products[2].Price,ProductId = products[2].IdProduct,OrderId=19},

                new OrderItem { IdOrderItem=58,Quantity = 2,Price = products[3].Price,ProductId = products[3].IdProduct,OrderId=20},
                new OrderItem { IdOrderItem=59,Quantity = 1,Price = products[4].Price,ProductId = products[4].IdProduct,OrderId=20},
                new OrderItem { IdOrderItem=60,Quantity = 3,Price = products[5].Price,ProductId = products[5].IdProduct,OrderId=20},
            };
        }
        private static List<Payment> GetPayments(List<OrderItem> orderItems)
        {
            return new List<Payment>
            {
                new Payment{IdPayment = 1,BankAccountNumber = "CZ81 5889 0000 4643 3473 5252",         DateTime = DateTimeNow.AddDays(1).AddHours(1).AddMinutes(1),Value = orderItems.Where(t=>t.OrderId==1).Sum(t => t.Price),OrderId = 1},
                new Payment{IdPayment = 2,BankAccountNumber = "HR24 0874 9806 5762 1181 6",            DateTime = DateTimeNow.AddDays(1).AddHours(1).AddMinutes(2),Value = orderItems.Where(t=>t.OrderId==2).Sum(t => t.Price),OrderId = 2},
                new Payment{IdPayment = 3,BankAccountNumber = "AL37 9846 8374 WUD8 JZGT LVMN JCOW",    DateTime = DateTimeNow.AddDays(1).AddHours(1).AddMinutes(3),Value = orderItems.Where(t=>t.OrderId==3).Sum(t => t.Price),OrderId = 3},
                new Payment{IdPayment = 4,BankAccountNumber = "DO40 72LI 0452 9323 7721 7730 7647",    DateTime = DateTimeNow.AddDays(1).AddHours(1).AddMinutes(4),Value = orderItems.Where(t=>t.OrderId==4).Sum(t => t.Price),OrderId = 4},
                new Payment{IdPayment = 5,BankAccountNumber = "SA94 83LU 2BLO NBCS WSVT LAZP",         DateTime = DateTimeNow.AddDays(1).AddHours(1).AddMinutes(5),Value = orderItems.Where(t=>t.OrderId==5).Sum(t => t.Price),OrderId = 5},
                new Payment{IdPayment = 6,BankAccountNumber = "IS07 9401 7783 5946 2967 7237 65",      DateTime = DateTimeNow.AddDays(1).AddHours(1).AddMinutes(6),Value = orderItems.Where(t=>t.OrderId==6).Sum(t => t.Price),OrderId = 6},
                new Payment{IdPayment = 7,BankAccountNumber = "IS10 1020 8272 3613 7283 5610 19",      DateTime = DateTimeNow.AddDays(1).AddHours(1).AddMinutes(7),Value = orderItems.Where(t=>t.OrderId==7).Sum(t => t.Price),OrderId = 7},
                new Payment{IdPayment = 8,BankAccountNumber = "FR62 8976 4864 50GL 8ABI JVWY L60",     DateTime = DateTimeNow.AddDays(1).AddHours(1).AddMinutes(8),Value = orderItems.Where(t=>t.OrderId==8).Sum(t => t.Price),OrderId = 8},
                new Payment{IdPayment = 9,BankAccountNumber = "BR93 5424 5107 5566 4290 2950 450F O",  DateTime = DateTimeNow.AddDays(1).AddHours(1).AddMinutes(9),Value = orderItems.Where(t=>t.OrderId==9).Sum(t => t.Price),OrderId = 9},
                new Payment{IdPayment = 10,BankAccountNumber = "MR88 5504 2625 3138 4613 3045 666",    DateTime = DateTimeNow.AddDays(1).AddHours(1).AddMinutes(10),Value = orderItems.Where(t=>t.OrderId==10).Sum(t => t.Price),OrderId = 10},
                new Payment{IdPayment = 11,BankAccountNumber = "KW69 RTQQ HVJI XCHJ YD5G SCZI 9M3F AT",DateTime = DateTimeNow.AddDays(2).AddHours(1).AddMinutes(1),Value = orderItems.Where(t=>t.OrderId==11).Sum(t => t.Price),OrderId = 11},
                new Payment{IdPayment = 12,BankAccountNumber = "FR04 1021 2432 41T2 YTFH 3ITC J99",    DateTime = DateTimeNow.AddDays(2).AddHours(1).AddMinutes(2),Value = orderItems.Where(t=>t.OrderId==12).Sum(t => t.Price),OrderId = 12},
                new Payment{IdPayment = 13,BankAccountNumber = "CZ81 5889 0000 4643 3473 5252",        DateTime = DateTimeNow.AddDays(2).AddHours(1).AddMinutes(3),Value = orderItems.Where(t=>t.OrderId==13).Sum(t => t.Price),OrderId = 13},
                new Payment{IdPayment = 14,BankAccountNumber = "HR24 0874 9806 5762 1181 6",           DateTime = DateTimeNow.AddDays(2).AddHours(1).AddMinutes(4),Value = orderItems.Where(t=>t.OrderId==14).Sum(t => t.Price),OrderId = 14},
                new Payment{IdPayment = 15,BankAccountNumber = "AL37 9846 8374 WUD8 JZGT LVMN JCOW",   DateTime = DateTimeNow.AddDays(2).AddHours(1).AddMinutes(5),Value = orderItems.Where(t=>t.OrderId==15).Sum(t => t.Price),OrderId = 15},
                new Payment{IdPayment = 16,BankAccountNumber = "DO40 72LI 0452 9323 7721 7730 7647",   DateTime = DateTimeNow.AddDays(2).AddHours(1).AddMinutes(6),Value = orderItems.Where(t=>t.OrderId==16).Sum(t => t.Price),OrderId = 16},
                new Payment{IdPayment = 17,BankAccountNumber = "SA94 83LU 2BLO NBCS WSVT LAZP",        DateTime = DateTimeNow.AddDays(2).AddHours(1).AddMinutes(7),Value = orderItems.Where(t=>t.OrderId==17).Sum(t => t.Price),OrderId = 17},
                new Payment{IdPayment = 18,BankAccountNumber = "IS07 9401 7783 5946 2967 7237 65",     DateTime = DateTimeNow.AddDays(2).AddHours(1).AddMinutes(8),Value = orderItems.Where(t=>t.OrderId==18).Sum(t => t.Price),OrderId = 18},
                new Payment{IdPayment = 19,BankAccountNumber = "IS10 1020 8272 3613 7283 5610 19",     DateTime = DateTimeNow.AddDays(2).AddHours(1).AddMinutes(9),Value = orderItems.Where(t=>t.OrderId==19).Sum(t => t.Price),OrderId = 19},
                new Payment{IdPayment = 20,BankAccountNumber = "FR62 8976 4864 50GL 8ABI JVWY L60",    DateTime = DateTimeNow.AddDays(2).AddHours(1).AddMinutes(10),Value = orderItems.Where(t=>t.OrderId==20).Sum(t => t.Price),OrderId = 20}
            };
        }
    }
}
