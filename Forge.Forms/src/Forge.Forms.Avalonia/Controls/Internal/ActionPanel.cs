﻿using System;
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

    public static Position GetPosition(Control element)
    {
        return element.GetValue(PositionProperty);
    }

    public static void SetPosition(Control element, Position value)
    {
        element.SetValue(PositionProperty, value);
    }

    protected override Size MeasureOverride(Size availableSize)
    {
        var newAvailableSize = availableSize.WithHeight(double.PositiveInfinity);
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

        // Test for h h.
        if (leftWidth + rightWidth <= finalSize.Width)
        {
            StackHorizontally(leftChildren, 0d, 0d, finalSize.Height);
            fillChild?.Arrange(new Rect(leftWidth, 0d, finalSize.Width - leftWidth - rightWidth, finalSize.Height));
            StackHorizontally(rightChildren, finalSize.Width - rightWidth, 0d, finalSize.Height);
            return finalSize;
        }

        if (leftWidth <= finalSize.Width)
        {
            // Test for h / h
            if (rightWidth <= finalSize.Width)
            {
                StackHorizontally(leftChildren, 0d, 0d, leftMaxHeight);
                fillChild?.Arrange(new Rect(0d, leftMaxHeight, finalSize.Width, fillHeight));
                StackHorizontally(rightChildren, finalSize.Width - rightWidth, leftMaxHeight + fillHeight,
                    rightMaxHeight);
                return finalSize;
            }

            // Return h / v
            StackHorizontally(leftChildren, 0d, 0d, leftMaxHeight);
            fillChild?.Arrange(new Rect(0d, leftMaxHeight, finalSize.Width, fillHeight));
            StackVertically(Enumerable.Reverse(rightChildren), finalSize.Width - rightMaxWidth,
                leftMaxHeight + fillHeight, rightMaxWidth);
            return finalSize;
        }

        // Test for v / h
        if (rightWidth <= finalSize.Width)
        {
            StackVertically(leftChildren, 0d, 0d, leftMaxWidth);
            fillChild?.Arrange(new Rect(0d, leftHeight, finalSize.Width, fillHeight));
            StackHorizontally(rightChildren, finalSize.Width - rightWidth, leftHeight + fillHeight, rightMaxHeight);
            return finalSize;
        }

        // Return v / v
        StackVertically(leftChildren, 0d, 0d, leftMaxWidth);
        fillChild?.Arrange(new Rect(0d, leftHeight, finalSize.Width, fillHeight));
        StackVertically(Enumerable.Reverse(rightChildren), finalSize.Width - rightMaxWidth, leftHeight + fillHeight,
            rightMaxWidth);
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