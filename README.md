# ORMi (Object Relational Management Instrumentation)

ORMi is a quite simple Light-ORM to work with WMI (Windows Management Instrumentation). It handles WMI CRUD operations in a very easy way. ORMi does automatic mapping to model clases so you can easily work with WMI classes without having to worry about writing complex queries and handling WMI connections.

## Getting Started

ORMi is available via [NuGet](https://www.nuget.org/packages/ORMi/). Also, you can always download the latest release on https://github.com/nicoriff/ORMi/releases/.

## How to Use

ORMi is just too easy to work with. Let's for example suppose we want to access ```root\CIMV2``` namespace to get the some information about our computer proccesors. 
First of all, we need to use the library:

```C# 
using ORMi;
```

Then, we'll define the following class:

```C#
    [WMIClass("Win32_Processor")]
    public class Processor : WMIInstance
    {
        public string Name { get; set; }

        [WMIProperty("NumberOfCores")]
        public int Cores { get; set; }

        public string Description { get; set; }
    }
```

ORMi has some custom attributes to map the model clases to WMI classes. WMI classes usually have tricky or non conventional names that you will for sure not want to use on your class. For solving that problem ORMi just maps the class property name to the WMI property name. If you do not want to use the WMI property name then you can just specify the ```WMIProperty``` attribute and match it to the name of the WMI property. The same run for class names. In that case you can make use of ```WMIClass``` attribute.
Note that the class inherits from ``WMIInstance`` class. This is an optional practice from version 2.0. Even if you can not use it, it is recommended as it will be strictly neccesary if you work with WMI methods. If you don't use methods, just don't add the inheritance.

Then, the first thing you got to do is create the ```WMIHelper``` that is the class that you'll use to interact with WMI. You can either create the instance for local use or to use with a remote client. In that case you have to specify credentials or make sure that the user have the corresponding privileges:

```C#
    WMIHelper helper = new WMIHelper("root\\CimV2");
```
Or specifiying client machine and credentials:

```C#
    WMIHelper helper = new WMIHelper("root\\CimV2", "W2012SRV-WRK", "Administrator", "Password01");
```

Then you simple query for the data:

```
    List<Processor> processors = helper.Query<Processor>().ToList();
```
This can also be done in async fashion:

```C#
    List<Processor> processors = await helper.QueryAsync<Processor>().ToList();
```

If you don't want to define your model classes the you can also get the result in a `List<dynamic>`

```C#
var devices = helper.Query("SELECT * FROM Win32_PnPEntity");
```
You can also search for single instances:

```C#
Printer printer = helper.QueryFirstOrDefault<Printer>();
```

## Create, Update and Delete:

For adding, updating and deleting instances we are going to use a custom namespace and classes. In this example the namespace is `root\EmployeesSystem`. And we have the following class defined:

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

**NOTE:** From version 1.5 ORMi supports multiple ``SearchKey`` attributes set. This is due to WMI classes that have composite keys. If there is no ``SearchKey``  set, then a exception will be thrown.

All above operations can also be done asynchronously. For example:

 **Add Instance asynchronously:**
 
```C#
	 Person person = new Person
	 {
	     FirstName = "John",
	     Lastname = "Doe",
	     DocumentNumber = "9995",
	     Segment = -1
	 };

	 await helper.AddInstanceAsync(person);
```

**Creating an WMI Event Watcher:**

Creating a watcher is one of the simplest tasks in ORMi. Just declare the watcher specifying scope, query and the desired output type and that's it!. Start receiving events!.
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
Or you can just not define any Type to return and ORMi will return a dynamic object containing all the WMI properties for the WMI instance:

```C#
WMIWatcher watcher = new WMIWatcher("root\\CimV2", "SELECT * FROM Win32_ProcessStartTrace");
watcher.WMIEventArrived += Watcher_WMIEventArrived;
```

## Methods

Since version 1.3.0 ORMi supports method working in a quite simple way. WMI defines two types of methods: Instance methods and Static methods.

Static methods are the ones in which you don't need any instance of a class to run the method. You just call the method and that's about it. What you will have to do on with ORMi to call the methods you want, is to define them on your model class. This will make a more readable and understandable code, and you will not have to mess with the complexity of having to do all the method calling coding by yourself.

IMPORTANT: From version 2.0 all classes that have methods declared must inherit from ``WMIInstance``. This is due to the need to use the scope of the instance. If there is no inheritance, the methods will be executed using the current user, and will probably throw an exception if it runs against a remote computer.

**Static Methods:**

Let's suppose we have a model class that represents an output on a smart card reader. The class will look like this:
```C#
    [WMIClass("Lnl_ReaderOutput1")]
    public class Output : WMIInstance
    {
        public int PanelID { get; set; }
        public int ReaderID { get; set; }
        public string Hostname { get; set; }
        public string Name { get; set; }
    }
```

We want to call a method that `Lnl_ReaderOutput1` has defined as `Activate` with no parameters in it. First, we are going to change a little bit the `WMIClass` attribute that the class has set. We are going to define the default namespace for the class:
```C#
[WMIClass(Name = "Lnl_ReaderOutput1", Namespace = "root\\OnGuard")]
```
Then, we have to add the method:
```C#
	public dynamic Activate()
	{
	    return WMIMethod.ExecuteMethod(this);
	}
```
There is two things to note on the above code. The first one is that the method name **MUST** be the same than the method defined on the WMI class that we want to work with. The second one is that the method implementation will always be the same

What `WMIMethod.ExecuteMethod(this)` does is to call the static `WMIMethod` class and pass the actual instance so the WMI methods can be called. If the method require parameters being sent, so we can pass them using a anonymous object:

```C#
	public dynamic Activate(int panelID, int readerID)
	{
	    return WMIMethod.ExecuteMethod(this, new { PanelID = panelID, ReaderID = readerID });
	}
```
So, finally, on your code you will have a much more cleaner implementation of method working:

```C#
	List<Output> outputList = helper.Query<Output>().ToList();

	foreach (Output o in outputList)
	{
	    dynamic d = o.Activate(1, 5);
	}
```

**Instance Methods:**

Instance methods are a little bit more complicated than static ones. Instance methods (as you imagine) requiere that you call the method on an actual instance of a class. So you will first have to retrieve the instance on where you want to run the method. In this example we are going to work with `Win32_Printer` class and we are going to rename one printer.

Firstly, we are going to define our class:
```C#
	[WMIClass(Name = "Win32_Printer", Namespace = "root\\CimV2")]
	public class Printer : WMIInstance
	{
	    public string DeviceID { get; set; }
	    public string Name { get; set; }
	    public string Caption { get; set; }

	    public void RenamePrinter(string newName)
	    {
	        WMIMethod.ExecuteMethod(this, new { NewPrinterName = newName });
	    }
	}
   ```

Note that we have a `DeviceID` property. This property must always be set on this case because in this case, `DeviceID` is a `CIM_Key` and `Unique` and it is the only unique identifier between instances. If you do not set this property then you will get an exception.
Also you can note that the parameters are sent using an anonymous object with properties that match the instance method ones. If this properties cannot be mapped the you'll receive an exception.

Then finally we'll use it this way:

```C#
	List<Printer> printers = helper.Query<Printer>().ToList();

	foreach (Printer p in printers)
	{
	    p.RenamePrinter("Newly renamed printer");
	}
```
The above code will rename all printers to "Newly renamed printer" (be careful! :D)

ORMi can also return and object containing the method execution result. For example let´s take `Win32_Process` class:

We´ll define it as the following:

```C#
    [WMIClass("Win32_Process")]
    public class Process : WMIInstance
    {
        public int Handle { get; set; }
        public string Name { get; set; }
        public int ProcessID { get; set; }

        public dynamic GetOwnerSid()
        {
            return WMIMethod.ExecuteMethod(this);
        }

        public ProcessOwner GetOwner()
        {
            return WMIMethod.ExecuteMethod<ProcessOwner>(this);
        }

        public int AttachDebugger()
        {
            return WMIMethod.ExecuteMethod<int>(this);
        }
    }
```

Note that `GetOwner()` implementation specifies a `ProcesOwner` type.   If you check WMI docs on `Win32_Processor` class for `GetOwner` method, you´ll see the following definition:

    uint32 GetOwner(
      [out] string User,
      [out] string Domain
    );

So you will have to define the following structure to retrieve the result in a beautiful way:

```C#
    public class ProcessOwner
    {
        public string Domain { get; set; }
        public int ReturnValue { get; set; }
        public string User { get; set; }
    }
```

So, finally, you would do the following:

```C#
    List<Process> processes = helper.Query<Process>().ToList();

    foreach (Process p in processes)
    {
        ProcessOwner po = p.GetOwner();
    }
```
