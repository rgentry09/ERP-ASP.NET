## Blazor client-side

# Keyboard navigation

[Index](Documentation.md)

Users can navigate between pages using the mouse or using the keyboard.

These are the keys to be used:

- [Ctrl] + [Left] and [Ctrl] + [Right] arrows navigate between pages
- [Ctrl] + [Home] key goes to the first page
- [Ctrl] + [End] key goes to the last page
- [Ctrl] + [Up] and [Ctrl] + [Down] arrows navigate from one row to another for grids where rows are selectable. It doesn´t work when multiselectable is enabled. 
- [Tab] key navigates among elements of a filter widget when it is visible
- [Esc] key minimises a filter widget when it is visible
- [Ctrl] +[Backspace] clear all filters

It's possible to change the modifier key used for keyboard navigation using the ```SetModifierKey``` method of the ```GridClient``` object:

```c#
    var client = new GridClient<Order>(HttpClient, url, query, false, "ordersGrid", Columns).SetModifierKey(ModifierKey.ShiftKey);
```

The parameter options of the ```SetModifierKey``` method are:
- ```ModifierKey.CtrlKey``` (default value)
- ```ModifierKey.ShiftKey```
- ```ModifierKey.AltKey```
- ```ModifierKey.MetaKey```

Keep in mnf that the last 2 options can collide with the modifier keys of the browser. The recomended options are ```ModifierKey.CtrlKey``` and ```ModifierKey.ShiftKey```.

[<- GridBlazor configuration](GridBlazor_configuration.md) | [Paging ->](Paging.md)