using DBApp;

namespace DbApp_Test
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using(UserContext db = new UserContext()) 
            {
                Subscriber sub1 = new Subscriber { Name = "Mike", SecondName = "Brown" };
                Subscriber sub2 = new Subscriber { Name = "Mike", SecondName = "Brown" };

                db.Subscribers.Add(sub1);
                db.Subscribers.Add(sub2);
                db.SaveChanges();
                Console.WriteLine("Successful save!");

                var subs = db.Subscribers;
                foreach(Subscriber sub in subs) 
                {
                    Console.WriteLine("{0}.{1},  {2}", sub.Id, sub.Name, sub.SecondName);
                }
            }
            Console.Read();
        }
    }
}