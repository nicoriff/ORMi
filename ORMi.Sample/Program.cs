using ORMi.Sample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ORMi.Sample
{
    class Program
    {
        static void Main(string[] args)
        {
            WMIHelper helper = new WMIHelper("root\\CimV2");

            var processors = helper.QueryAsync<Processor>();

            Person person = new Person
            {
                FirstName = "John",
                Lastname = "Doe",
                DocumentNumber = "9995",
                Segment = -1
            };

            helper.AddInstance(person);

            Person queryPersonSingle = helper.Query<Person>("SELECT * FROM Lnl_Cardholder WHERE LASTNAME = 'Doe'").SingleOrDefault();

            queryPersonSingle.Lastname = "Doe Modified";

            helper.UpdateInstance(queryPersonSingle);

            List<Person> queryPerson = helper.Query<Person>("SELECT * FROM Lnl_Cardholder WHERE LASTNAME = 'Lopez'").ToList();

            Console.ReadLine();
        }
    }
}
