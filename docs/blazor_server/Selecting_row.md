## Blazor server-side

# Selecting row

[Index](Documentation.md)

The **GridClient** object has a method called **Selectable** to configure if a row can be selected. 
It's value can be **true** and **false**. 
Since the version 1.1.0 of the GridBlazor nuget package the default value of the **Selectable** feature is **false** (it was **true** for earlier versions).

You can enable it as follows:
```c#
    var client = new GridClient<Order>(q => orderService.GetOrdersGridRows(columns, q), query, false, "ordersGrid", columns)
        .Selectable(true);
```

You have to add the **OnRowClicked** attribute on the component. For example, the razor page can contain the following line:
```razor
    <GridComponent T="Order" Grid="@_grid" OnRowClicked="@OrderDetails"></GridComponent>
```
Then you have to add the function called by the event. In this example its name is **OrderDetails**:
```c#
    protected void OrderDetails(object item)
    {
        Order order = null;
        if (item.GetType() == typeof(Order))
        {
            order = (Order)item;
        }
        Console.WriteLine("Order Id: " + (order == null ? "NULL" : order.OrderID.ToString()));
    }
```

In this sample a line of text with de selected row id is writen on the console log.

In the GridComponent.Demo project you will find another example where the order details are shown on a component when a row is selected.

[<- Grouping](Grouping.md) | [Searching ->](Searching.md)