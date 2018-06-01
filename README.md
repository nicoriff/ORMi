# ORMi (Object Relational Management Instrumentation)

ORMi is a quite simple Light-ORM to work with WMI. It handles WMI CRUD operations in a very easy way. ORMi does automatic mapping to model clases so you can easily work with WMI classes without having to worry about writing complex queries and handling WMI connections.

## Getting Started



## How to Use

ORMi is just to easy to work with. Let´s for example suppose we want to access 'root\CIMV2' namespace to get the some information about our computer proccesors. In that case, we´ll define the following class:

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

ORMi has some custom attributes to map the model clases to WMI classes. WMI classes usually have tricky or non conventional names that you will for sure not want to use on your class. For solvig that problem ORMi just maps the class property name to the WMI property name. If you do not want to use the WMI property name then you can just specify the ```WMIProperty``` attribute ant put the name of the WMI property. The same runs for class names. In that case you can make use of ```WMIClass```.

Then if we want to get the machine processors you just do:

```C#

    WMIHelper helper = new WMIHelper("root\\CimV2");

    List<Processor> processors = helper.Query<Processor>().ToList();

```