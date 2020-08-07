## GridBlazor for ASP.NET Core MVC

# Export to Excel

[Index](Documentation.md)

Grids can be exported to an Excel file. You have to use the ```SetExcelExport``` method of the ```GridClient``` object:
 
```c#
    var client = new GridClient<Order>(q => orderService.GetOrdersGridRows(columns, q),
         query, false, "ordersGrid", columns, locale)
        .SetExcelExport(true, false, "Orders"));
```

The ```SetExcelExport``` method has 3 parameters, one of them is required:

Parameter | Description
--------- | -----------
enabled | bool to enable Excel export (required)
allRows | bool to enable export of all grid rows (optional). The default value is false and only visible rows are exported
fileName | string to define the file name (optional). If no value is defined, the grid internal name is used as file name

This is an example of a grid with an export to Excel button:

![](../images/Excel.png)


[<- Embedded components on the grid](Embedded_components.md)