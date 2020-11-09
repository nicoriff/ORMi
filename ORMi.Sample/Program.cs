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
            IWMIHelper helper = new WMIHelper("root\\CimV2");

            Process p = new Process();
            p.Create("C:/Windows/notepad.exe", null, null);

            //List<NetworkAdapterConfiguration> interfaces = helper.Query<NetworkAdapterConfiguration>().ToList();

            //Printer printer = helper.QueryFirstOrDefault<Printer>();

            //List<Printer> printers = helper.Query<Printer>().ToList();

            //foreach (Printer p in printers)
            //{
            //    p.RenamePrinter("Newly renamed printer");
            //}

            //Output outp = new Output
            //{
            //    PanelID = 10,
            //    ReaderID = 1,
            //    Hostname = "ONGUARD01",
            //    Name = "ONGUARD01"
            //};

            //outp.Activate();

            //List<Process> processes = helper.Query<Process>().ToList();

            //foreach (Process p in processes)
            //{
            //    //dynamic d = p.GetOwnerSid();
            //    ProcessOwner po = p.GetOwner();
            //    //int res = p.AttachDebugger();
            //}

            //var dynDevices = helper.Query("SELECT * FROM Win32_PnPEntity");

            //var processors = helper.Query<Processor>();

            //List<Processor> procesors = helper.Query<Processor>().ToList();

            //List<Device> devices = helper.Query<Device>().ToList()
            //    .Where(p => (p.Name ?? "")
            //    .Contains("Intel")).ToList();

            //foreach (Device d in devices)
            //{
            //    Console.WriteLine(d.Name);
            //}

            //Person person = new Person
            //{
            //    FirstName = "John",
            //    DocumentNumber = "9995",
            //};

            //helper.AddInstance(person);

            //Person queryPersonSingle = helper.Query<Person>("SELECT * FROM Lnl_Cardholder WHERE LASTNAME = 'Doe Modified'").SingleOrDefault();

            //queryPersonSingle.Lastname = "Doe Modified";

            //helper.UpdateInstance(queryPersonSingle);

            //List<Person> queryPerson = helper.Query<Person>("SELECT * FROM Lnl_Cardholder WHERE LASTNAME = 'Lopez'").ToList();

            //WMIWatcher watcher = new WMIWatcher("root\\CimV2", "SELECT * FROM Win32_ProcessStartTrace", typeof(Process));
            //WMIWatcher watcher = new WMIWatcher("root\\CimV2", "SELECT * FROM Win32_ProcessStartTrace");
            //watcher.WMIEventArrived += Watcher_WMIEventArrived;

            Console.ReadLine();
        }

        private static void Watcher_WMIEventArrived(object sender, WMIEventArgs e)
        {
            //Process process = (Process)e.Object;

            dynamic process = e.Object;

            Console.WriteLine("New Process: {0} (Pid: {1})", process.ProcessName, process.ProcessID.ToString());
        }
    }
}
