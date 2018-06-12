# ORMi (Object Relational Management Instrumentation)

ORMi is a quite simple Light-ORM to work with WMI (Windows Management Instrumentation). It handles WMI CRUD operations in a very easy way. ORMi does automatic mapping to model clases so you can easily work with WMI classes without having to worry about writing complex queries and handling WMI connections.

## Getting Started

Momentarily ORMi is not available via NuGet (It will be soon). You can download the latest release on https://github.com/nicoriff/ORMi/releases/.

Once downloaded, you need to add the reference to the library on your project. 


## How to Use

ORMi is just too easy to work with. Let´s for example suppose we want to access 'root\CIMV2' namespace to get the some information about our computer proccesors. 
First of all, we need to use the library:

```C# 
using ORMi;
```

Then, we´ll define the following class:

```C#
    [WMIClass("Win32_Processor")]
    public class Processor
    {
        public string Name { get; set; }

        [WMIProperty("NumberOfCores")]
        public int Cores { get; set; }

        public string Description { get; set; }
    }
```

ORMi has some custom attributes to map the model clases to WMI classes. WMI classes usually have tricky or non conventional names that you will for sure not want to use on your class. For solvig that problem ORMi just maps the class property name to the WMI property name. If you do not want to use the WMI property name then you can just specify the ```WMIProperty``` attribute ant put the name of the WMI property. The same run for class names. In that case you can make use of ```WMIClass```.

Then if we want to get the machine processors you just do:

```C#

    WMIHelper helper = new WMIHelper("root\\CimV2");

    List<Processor> processors = helper.Query<Processor>().ToList();
```

For adding, updating and deleting instances we are going to use a custom namespace and classes. In this example the namespace is 'root\EmployeesSystem'. And we have the following class defined:

```C#
    [WMIClass("Lnl_Person")]
    public class Person
    {
        public string Lastname { get; set; }
        public string FirstName { get; set; }

        [WMIProperty( Name = "SSNO", SearchKey = true)]
        public string DocumentNumber { get; set; }

        [WMIProperty("PRIMARYSEGMENTID")]
        public int Segment { get; set; }
    }
 ```
   
Examples:

 **Add Instance:**
 
```C#
	 Person person = new Person
	 {
	     FirstName = "John",
	     Lastname = "Doe",
	     DocumentNumber = "9995",
	     Segment = -1
	 };

	 helper.AddInstance(person);
```

**Update Instance:**

For the Update operation, the class must have the ```WmiProperty``` attribute declared with the ```SearchKey``` property properly set to true. This will use the property with the ```SearchKey``` to get the instance that is going to be updated. For example:

```C#
      Person person= helper.Query<Person>("SELECT * FROM Lnl_Cardholder WHERE LASTNAME = 'Doe'").SingleOrDefault();

      person.Lastname = "Doe Modified";

      helper.UpdateInstance(person);
```
	
In the above example, ORMi is going to look for the person with SSNO = 9995 and update that instance with the properties set on ```person``` instance.

**Remove Instance:**

As in the update operation, the removal works with the ```SearchKey``` property set or manually specifying a query. In both cases, the result will be removed:

```C#
	Person p = helper.Query<Person>("SELECT * FROM Lnl_Cardholder WHERE LASTNAME = 'Doe'").SingleOrDefault();
	helper.RemoveInstance(p);
```

**Creating an WMI Event Watcher:**

Creating a watcher is one of the simplest tasks in ORMi. Just declare the watcher specifying scope, query and the desired output type and that´s it!. Start receiving events!.
In this example we are going to watch for new processes created on the system:

First, we define the class:

```C#
    [WMIClass("Win32_ProcessStartTrace")]
    public class Process
    {
        public string ProcessName { get; set; }
        public int ProcessID { get; set; }
    }
```
Then subscribe for events...

```C#
	WMIWatcher watcher = new WMIWatcher("root\\CimV2", "SELECT * FROM Win32_ProcessStartTrace", typeof(Process));
	watcher.WMIEventArrived += Watcher_WMIEventArrived;
```
Or if you have WMIClass attribute set:

```C#
	WMIWatcher watcher = new WMIWatcher("root\\CimV2", typeof(Process));
	watcher.WMIEventArrived += Watcher_WMIEventArrived;
```
And then, just handle the events...

```C#
    private static void Watcher_WMIEventArrived(object sender, WMIEventArgs e)
    {
        Process process = (Process)e.Object;

        Console.WriteLine("New Process: {0} (Pid: {1})", process.ProcessName, process.ProcessID.ToString());
    }
```