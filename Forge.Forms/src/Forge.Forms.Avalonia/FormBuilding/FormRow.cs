using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using Avalonia.Layout;
using Forge.Forms.AvaloniaUI.Controls.Internal;

namespace Forge.Forms.AvaloniaUI.FormBuilding;

public class FormRow
{
    public FormRow()
        : this(true, 1)
    {
    }

    public FormRow(bool startsNewRow, int rowSpan)
    {
        StartsNewRow = startsNewRow;
        RowSpan = rowSpan;
        Elements = new List<FormElementContainer>();
    }

    public bool StartsNewRow { get; }

    public int RowSpan { get; }

    public List<FormElementContainer> Elements { get; }
}

public class FormElementContainer
{
    public FormElementContainer(int column, int columnSpan, FormElement element)
        : this(column, columnSpan, new List<FormElement> { element })
    {
    }

    public FormElementContainer(int column, int columnSpan, List<FormElement> elements)
    {
        Column = column;
        ColumnSpan = columnSpan;
        Elements = elements;
    }

    internal FormElementContainer(int column, int columnSpan, ILayout layout)
    {
        Column = column;
        ColumnSpan = columnSpan;
        Elements = layout.GetElements().ToList();
        Layout = layout;
    }

    public int Column { get; }

    public int ColumnSpan { get; }

    public List<FormElement> Elements { get; }

    // This is not ready for public API
    internal ILayout Layout { get; }
}

/// <summary>
///     Supports custom layout of form elements.
/// </summary>
internal interface ILayout
{
    IEnumerable<FormElement> GetElements();

    Control Build(Func<FormElement, Control> elementBuilder);
}

internal class GridLayout : ILayout
{
    public GridLayout(IEnumerable<GridColumnLayout> columns, double top, double bottom)
    {
        Columns = columns?.ToList() ?? new List<GridColumnLayout>(0);
        Top = top;
        Bottom = bottom;
    }

    public List<GridColumnLayout> Columns { get; }

    public double Top { get; }

    public double Bottom { get; }

    public IEnumerable<FormElement> GetElements()
    {
        return Columns.SelectMany(c => c.GetElements());
    }

    public Control Build(Func<FormElement, Control> elementBuilder)
    {
        ColumnDefinition GetDefinition(double size)
        {
            if (size > 0d)
                return new ColumnDefinition
                {
                    Width = new GridLength(size, GridUnitType.Star)
                };

            if (size < 0d)
                return new ColumnDefinition
                {
                    Width = new GridLength(-size, GridUnitType.Pixel)
                };

            return null;
        }

        var grid = new Grid
        {
            Margin = new Thickness(0d, Top, 0d, Bottom)
        };

        var colnum = 0;
        foreach (var column in Columns)
        {
            if (column.Width == 0d) continue;

            var gridColumn = GetDefinition(column.Left);
            if (gridColumn != null)
            {
                grid.ColumnDefinitions.Add(gridColumn);
                colnum++;
            }

            grid.ColumnDefinitions.Add(GetDefinition(column.Width));
            var child = column.Build(elementBuilder);
            Grid.SetColumn(child, colnum);
            grid.Children.Add(child);
            colnum++;
            gridColumn = GetDefinition(column.Right);
            if (gridColumn != null)
            {
                grid.ColumnDefinitions.Add(gridColumn);
                colnum++;
            }
        }

        return grid;
    }
}

internal class Layout : ILayout
{
    public Layout(IEnumerable<ILayout> children)
        : this(children, new Thickness(), VerticalAlignment.Stretch, HorizontalAlignment.Stretch, null, null)
    {
    }

    public Layout(
        IEnumerable<ILayout> children,
        Thickness margin,
        VerticalAlignment verticalAlignment,
        HorizontalAlignment horizontalAlignment,
        double? minHeight,
        double? maxHeight)
    {
        Children = children?.ToList() ?? new List<ILayout>(0);
        Margin = margin;
        VerticalAlignment = verticalAlignment;
        HorizontalAlignment = horizontalAlignment;
        MinHeight = minHeight;
        MaxHeight = maxHeight;
    }

    public List<ILayout> Children { get; }

    public Thickness Margin { get; set; }

    public VerticalAlignment VerticalAlignment { get; }

    public HorizontalAlignment HorizontalAlignment { get; }

    public double? MinHeight { get; }

    public double? MaxHeight { get; }

    public IEnumerable<FormElement> GetElements()
    {
        return Children.SelectMany(c => c.GetElements());
    }

    public Control Build(Func<FormElement, Control> elementBuilder)
    {
        var stackPanel = new StackPanel();
        foreach (var child in Children) stackPanel.Children.Add(child.Build(elementBuilder));

        var scrollViewer = new ScrollViewer
        {
            Content = stackPanel,
            Margin = Margin,
            VerticalAlignment = VerticalAlignment,
            HorizontalAlignment = HorizontalAlignment,
            HorizontalScrollBarVisibility = ScrollBarVisibility.Disabled,
            VerticalScrollBarVisibility = ScrollBarVisibility.Auto
        };

        if (MinHeight != null) scrollViewer.MinHeight = MinHeight.Value;

        if (MaxHeight != null) scrollViewer.MaxHeight = MaxHeight.Value;

        return scrollViewer;
    }
}

internal class TabLayout : ILayout
{
   // private static ThicknessConverter thicknessConverter = new ThicknessConverter();
    //private static GridLengthConverter gridLengthConverter = new GridLengthConverter();

    public TabLayout(
        IEnumerable<TabItemLayout> tabItems,
        Dock tabStripPlacement,
        double? minHeight,
        double? maxHeight,
        string tabHeaderMargin,
        HorizontalAlignment tabHeaderHorizontalAlignment,
        double? tabHeaderFontSize)
    {
        TabItems = tabItems?.ToList() ?? new List<TabItemLayout>(0);
        TabStripPlacement = tabStripPlacement;
        MinHeight = minHeight;
        MaxHeight = maxHeight;
        TabHeaderMargin = tabHeaderMargin;
        TabHeaderHorizontalAlignment = tabHeaderHorizontalAlignment;
        TabHeaderFontSize = tabHeaderFontSize;
    }

    public List<TabItemLayout> TabItems { get; set; }
    public Dock TabStripPlacement { get; set; }
    public double? MinHeight { get; set; }
    public double? MaxHeight { get; set; }
    public string TabHeaderMargin { get; set; }
    public HorizontalAlignment TabHeaderHorizontalAlignment { get; set; }
    public double? TabHeaderFontSize { get; set; }

    public IEnumerable<FormElement> GetElements()
    {
        return TabItems.SelectMany(c => c.GetElements());
    }

    public Control Build(Func<FormElement, Control> elementBuilder)
    {
        var tabControl = new TabControl();
        tabControl.TabStripPlacement = TabStripPlacement;
        tabControl.MinHeight = MinHeight.HasValue ? MinHeight.Value : tabControl.MinHeight;
        tabControl.MaxHeight = MaxHeight.HasValue ? MaxHeight.Value : tabControl.MaxHeight;
        if (string.IsNullOrWhiteSpace(TabHeaderMargin) == false)
        {
           // var margin = (Thickness)thicknessConverter.ConvertFromString(TabHeaderMargin);
           // Headered.SetTabHeaderMargin(tabControl, margin);
        }

        if (TabHeaderFontSize != null) tabControl.FontSize = TabHeaderFontSize.Value;

        // TODO: Might be wrong with tabcontrolassist missing and setting header margin, fontsize, etc.
        tabControl.HorizontalAlignment = TabHeaderHorizontalAlignment;

        foreach (var tabItem in TabItems)
        {
            var tab = new TabItem
            {
                Header = tabItem.Header,
                Content = tabItem.Build(elementBuilder)
            };

            tabControl.Items.Add(tab);
        }

        return tabControl;
    }
}

internal class TabItemLayout : ILayout
{
    public TabItemLayout(string header, ILayout child)
    {
        Header = header;
        Child = child;
    }

    public string Header { get; set; }

    public ILayout Child { get; }

    public IEnumerable<FormElement> GetElements()
    {
        return Child.GetElements();
    }

    public Control Build(Func<FormElement, Control> elementBuilder)
    {
        return Child.Build(elementBuilder);
    }
}

internal class GridColumnLayout : ILayout
{
    public GridColumnLayout(ILayout child, double width, double left, double right)
    {
        Child = child;
        Width = width;
        Left = left;
        Right = right;
    }

    public ILayout Child { get; }

    public double Width { get; }

    public double Left { get; }

    public double Right { get; }

    public IEnumerable<FormElement> GetElements()
    {
        return Child.GetElements();
    }

    public Control Build(Func<FormElement, Control> elementBuilder)
    {
        return Child.Build(elementBuilder);
    }
}

internal class FormElementLayout : ILayout
{
    public FormElementLayout(FormElement element)
    {
        Element = element;
    }

    public FormElement Element { get; }

    public IEnumerable<FormElement> GetElements()
    {
        return new[] { Element };
    }

    public Control Build(Func<FormElement, Control> elementBuilder)
    {
        return elementBuilder(Element);
    }
}

internal class InlineLayout : ILayout
{
    public InlineLayout(IEnumerable<ILayout> elements, double top, double bottom)
    {
        Elements = elements?.ToList() ?? new List<ILayout>(0);
        Top = top;
        Bottom = bottom;
    }

    public List<ILayout> Elements { get; }

    public double Top { get; }

    public double Bottom { get; }

    public IEnumerable<FormElement> GetElements()
    {
        return Elements.SelectMany(e => e.GetElements());
    }

    public Control Build(Func<FormElement, Control> elementBuilder)
    {
        var panel = new ActionPanel
        {
            Margin = new Thickness(0d, Top, 0d, Bottom)
        };

        foreach (var element in Elements)
            if (element is FormElementLayout formElementLayout)
            {
                var contentPresenter = elementBuilder(formElementLayout.Element);
                ActionPanel.SetPosition(contentPresenter, formElementLayout.Element.LinePosition);
                panel.Children.Add(contentPresenter);
            }
            else
            {
                panel.Children.Add(element.Build(elementBuilder));
            }

        return panel;
    }
}