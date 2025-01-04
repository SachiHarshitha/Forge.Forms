using System;
using System.Collections.Generic;
using System.Linq;
using Avalonia;
using Avalonia.Controls;
using Forge.Forms.AvaloniaUI.Annotations;

namespace Forge.Forms.AvaloniaUI.Controls.Internal;

internal class ActionPanel : Panel
{
    public static readonly AttachedProperty<Position> PositionProperty =
        AvaloniaProperty.RegisterAttached<ActionPanel, Panel, Position>(
            "Position",
            Position.Right,
            true);

    public static readonly AvaloniaProperty<Thickness> PanelMarginProperty =
        AvaloniaProperty.Register<ActionPanel, Thickness>(
            nameof(PanelMargin),
            new Thickness(8d, 0d));

    public Thickness PanelMargin
    {
        get => (Thickness)GetValue(PanelMarginProperty);
        set => SetValue(PanelMarginProperty, value);
    }

    public static Position GetPosition(AvaloniaObject element)
    {
        return element.GetValue(PositionProperty);
    }

    public static void SetPosition(AvaloniaObject element, Position value)
    {
        element.SetValue(PositionProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var margin = PanelMargin;
        var adjustedSize = availableSize.Deflate(margin);

        // Existing logic, but use `adjustedSize` instead of `availableSize`
        var newAvailableSize = adjustedSize.WithHeight(double.PositiveInfinity);
        var children = Children;
        var leftWidth = 0d;
        var rightWidth = 0d;
        var leftHeight = 0d;
        var rightHeight = 0d;
        var leftMaxHeight = 0d;
        var rightMaxHeight = 0d;
        var leftMaxWidth = 0d;
        var rightMaxWidth = 0d;
        Control fillChild = null;
        var fillHeight = 0d;
        for (var i = 0; i < children.Count; i++)
        {
            var child = children[i];
            if (child == null) continue;

            var pos = GetPosition(child);
            if (fillChild == null && pos == (Position)(-1))
            {
                fillChild = child;
                fillChild.Measure(newAvailableSize);
                fillHeight = fillChild.DesiredSize.Height;
                continue;
            }

            child.Measure(newAvailableSize);
            if (pos == Position.Right)
            {
                var width = child.DesiredSize.Width;
                var height = child.DesiredSize.Height;
                rightWidth += width;
                rightHeight += height;
                rightMaxWidth = Math.Max(rightMaxWidth, width);
                rightMaxHeight = Math.Max(rightMaxHeight, height);
            }
            else
            {
                var width = child.DesiredSize.Width;
                var height = child.DesiredSize.Height;
                leftWidth += width;
                leftHeight += height;
                leftMaxWidth = Math.Max(leftMaxWidth, width);
                leftMaxHeight = Math.Max(leftMaxHeight, height);
            }
        }

        // Test for h h.
        if (leftWidth + rightWidth <= newAvailableSize.Width)
            return new Size(leftWidth + rightWidth, Math.Max(Math.Max(leftMaxHeight, rightMaxHeight), fillHeight));

        if (leftWidth <= newAvailableSize.Width)
        {
            // Test for h / h
            if (rightWidth <= newAvailableSize.Width)
                return new Size(Math.Max(leftWidth, rightWidth), leftMaxHeight + rightMaxHeight + fillHeight);

            // Return h / v
            return new Size(Math.Max(leftWidth, rightMaxWidth), leftMaxHeight + rightHeight + fillHeight);
        }

        // Test for v / h
        if (rightWidth <= newAvailableSize.Width)
            return new Size(Math.Max(leftMaxWidth, rightWidth), leftHeight + rightMaxHeight + fillHeight);

        // Return v / v
        return new Size(Math.Max(leftMaxWidth, rightMaxWidth), leftHeight + rightHeight + fillHeight);
    }

    protected override Size ArrangeOverride(Size finalSize)
    {
        // Get the PanelMargin
        var margin = PanelMargin;

        // Adjust the bounds by the PanelMargin
        var adjustedBounds = new Rect(
            margin.Left,
            margin.Top,
            finalSize.Width - margin.Left - margin.Right,
            finalSize.Height - margin.Top - margin.Bottom);

        var children = Children;
        var leftWidth = 0d;
        var rightWidth = 0d;
        var leftHeight = 0d;
        var rightHeight = 0d;
        var leftMaxHeight = 0d;
        var rightMaxHeight = 0d;
        var leftMaxWidth = 0d;
        var rightMaxWidth = 0d;
        var leftChildren = new List<Control>();
        var rightChildren = new List<Control>();
        Control fillChild = null;
        var fillHeight = 0d;

        // Classify children into left, right, or fill categories
        for (var i = 0; i < children.Count; i++)
        {
            var child = children[i];
            if (child == null) continue;

            var pos = GetPosition(child);
            if (fillChild == null && pos == (Position)(-1))
            {
                fillChild = child;
                fillHeight = fillChild.DesiredSize.Height;
                continue;
            }

            if (pos == Position.Right)
            {
                var width = child.DesiredSize.Width;
                var height = child.DesiredSize.Height;
                rightWidth += width;
                rightHeight += height;
                rightMaxWidth = Math.Max(rightMaxWidth, width);
                rightMaxHeight = Math.Max(rightMaxHeight, height);
                rightChildren.Add(child);
            }
            else
            {
                var width = child.DesiredSize.Width;
                var height = child.DesiredSize.Height;
                leftWidth += width;
                leftHeight += height;
                leftMaxWidth = Math.Max(leftMaxWidth, width);
                leftMaxHeight = Math.Max(leftMaxHeight, height);
                leftChildren.Add(child);
            }
        }

        // Handle different layout configurations
        if (leftWidth + rightWidth <= adjustedBounds.Width)
        {
            StackHorizontally(leftChildren, adjustedBounds.Left, adjustedBounds.Top, adjustedBounds.Height);
            fillChild?.Arrange(new Rect(
                adjustedBounds.Left + leftWidth,
                adjustedBounds.Top,
                adjustedBounds.Width - leftWidth - rightWidth,
                adjustedBounds.Height));
            StackHorizontally(rightChildren, adjustedBounds.Right - rightWidth, adjustedBounds.Top,
                adjustedBounds.Height);
            return finalSize;
        }

        if (leftWidth <= adjustedBounds.Width)
        {
            if (rightWidth <= adjustedBounds.Width)
            {
                StackHorizontally(leftChildren, adjustedBounds.Left, adjustedBounds.Top, leftMaxHeight);
                fillChild?.Arrange(new Rect(
                    adjustedBounds.Left,
                    adjustedBounds.Top + leftMaxHeight,
                    adjustedBounds.Width,
                    fillHeight));
                StackHorizontally(rightChildren, adjustedBounds.Right - rightWidth,
                    adjustedBounds.Top + leftMaxHeight + fillHeight, rightMaxHeight);
                return finalSize;
            }

            StackHorizontally(leftChildren, adjustedBounds.Left, adjustedBounds.Top, leftMaxHeight);
            fillChild?.Arrange(new Rect(
                adjustedBounds.Left,
                adjustedBounds.Top + leftMaxHeight,
                adjustedBounds.Width,
                fillHeight));
            StackVertically(Enumerable.Reverse(rightChildren), adjustedBounds.Right - rightMaxWidth,
                adjustedBounds.Top + leftMaxHeight + fillHeight, rightMaxWidth);
            return finalSize;
        }

        if (rightWidth <= adjustedBounds.Width)
        {
            StackVertically(leftChildren, adjustedBounds.Left, adjustedBounds.Top, leftMaxWidth);
            fillChild?.Arrange(new Rect(
                adjustedBounds.Left,
                adjustedBounds.Top + leftHeight,
                adjustedBounds.Width,
                fillHeight));
            StackHorizontally(rightChildren, adjustedBounds.Right - rightWidth,
                adjustedBounds.Top + leftHeight + fillHeight, rightMaxHeight);
            return finalSize;
        }

        StackVertically(leftChildren, adjustedBounds.Left, adjustedBounds.Top, leftMaxWidth);
        fillChild?.Arrange(new Rect(
            adjustedBounds.Left,
            adjustedBounds.Top + leftHeight,
            adjustedBounds.Width,
            fillHeight));
        StackVertically(Enumerable.Reverse(rightChildren), adjustedBounds.Right - rightMaxWidth,
            adjustedBounds.Top + leftHeight + fillHeight, rightMaxWidth);
        return finalSize;
    }

    private static void StackVertically(IEnumerable<Control> children, double x, double y, double width)
    {
        var offset = 0d;
        foreach (var child in children)
        {
            var height = child.DesiredSize.Height;
            child.Arrange(new Rect(x, y + offset, width, height));
            offset += height;
        }
    }

    private static void StackHorizontally(List<Control> children, double x, double y, double height)
    {
        var offset = 0d;
        foreach (var child in children)
        {
            var width = child.DesiredSize.Width;
            child.Arrange(new Rect(x + offset, y, width, height));
            offset += width;
        }
    }
}