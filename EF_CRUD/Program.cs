namespace EF_CRUD
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.Core.EntityClient;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("請選擇CRUD功能:");
            Console.WriteLine("C => Create");
            Console.WriteLine("LR => Lazy_Read");
            Console.WriteLine("ER => Eager_Read");
            Console.WriteLine("U => Update");
            Console.WriteLine("D => Delete");

            string function = Console.ReadLine();

            if (!string.IsNullOrEmpty(function))
            {
                try
                {
                    EF_CRUD_Entities ef_crud_entities = new EF_CRUD_Entities();
                    ef_crud_entities.Database.Log = Console.Write;
                    switch (function)
                    {
                        case "C":
                            Create(ef_crud_entities);
                            break;
                        case "LR":
                            LazyRead(ef_crud_entities);
                            break;
                        case "ER":
                            EagerRead(ef_crud_entities);
                            break;
                        case "U":
                            Update(ef_crud_entities);
                            break;
                        case "D":
                            Delete(ef_crud_entities);
                            break;
                        default:
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    Console.WriteLine("press any key");
                    Console.ReadKey();
                }
            }
        }

        private static void Create(EF_CRUD_Entities ef_crud_entities)
        {
            Customer larry = new Customer { CustName = "LarrySU", Created = System.DateTime.Now, Modified = System.DateTime.Now };
            ef_crud_entities.Customer.Add(larry);
            Order order = new Order { CustId = larry.Id, IsExpress = true, Created = System.DateTime.Now, Modified = System.DateTime.Now };
            ef_crud_entities.Order.Add(order);
            OrderDetail order_detail = new OrderDetail { OrderId = order.Id, ItemName = "蒜頭", Count = 2, Created = System.DateTime.Now, Modified = System.DateTime.Now };
            ef_crud_entities.OrderDetail.Add(order_detail);
            
            ef_crud_entities.SaveChanges();
        }

        private static void LazyRead(EF_CRUD_Entities ef_crud_entities)
        {
            int n = 0;
            Customer jim = ef_crud_entities.Customer.FirstOrDefault(c => c.CustName.Equals("JimLin"));
            n += 1;
            foreach (Order o in jim.Order)
            {
                n += 1;
                foreach (OrderDetail od in o.OrderDetail)
                {
                    Console.WriteLine(od.ItemName);
                }
            }

            Console.WriteLine("n+1 problem   " + n);
        }

        private static void EagerRead(EF_CRUD_Entities ef_crud_entities)
        {
            Customer jim = ef_crud_entities.Customer.Include("Order").Include("Order.OrderDetail").Where(c => c.CustName.Equals("JimLin")).SingleOrDefault();
            if (jim != null)
            {
                foreach (Order o in jim.Order)
                {
                    foreach (OrderDetail od in o.OrderDetail)
                    {
                        Console.WriteLine(od.ItemName);
                    }
                }
            }
        }

        private static void Update(EF_CRUD_Entities ef_crud_entities)
        {
            Customer larry = ef_crud_entities.Customer.FirstOrDefault(c => c.CustName.Equals("LarrySU"));
            if (larry != null)
            {
                larry.CustName = "LarrySu";
                ef_crud_entities.SaveChanges();
            }
        }

        private static void Delete(EF_CRUD_Entities ef_crud_entities)
        {
            Customer larry = ef_crud_entities.Customer.FirstOrDefault(c => c.CustName.Equals("LarrySu"));
            if (larry != null)
            {
                // already set Cascade
                ef_crud_entities.Customer.Remove(larry);
                ef_crud_entities.SaveChanges();
            }
        }
    }
}
