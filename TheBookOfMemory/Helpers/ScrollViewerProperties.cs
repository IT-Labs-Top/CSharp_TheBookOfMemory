using System.Windows;

namespace TheBookOfMemory.Helpers;

public static class ScrollViewerProperties
{
    public static readonly System.Windows.DependencyProperty VerticalScrollBarMarginProperty =
        System.Windows.DependencyProperty.RegisterAttached("VerticalScrollBarMargin", typeof(Thickness), typeof(ScrollViewerProperties), new PropertyMetadata(new Thickness(0)));

    public static readonly System.Windows.DependencyProperty HorizontalScrollBarMarginProperty =
        System.Windows.DependencyProperty.RegisterAttached("HorizontalScrollBarMargin", typeof(Thickness), typeof(ScrollViewerProperties), new PropertyMetadata(new Thickness(0)));

    public static void SetVerticalScrollBarMargin(UIElement element, Thickness value)
    {
        element.SetValue(VerticalScrollBarMarginProperty, value);
    }

    public static Thickness GetVerticalScrollBarMargin(UIElement element)
    {
        return (Thickness)element.GetValue(VerticalScrollBarMarginProperty);
    }

    public static void SetHorizontalScrollBarMargin(UIElement element, Thickness value)
    {
        element.SetValue(HorizontalScrollBarMarginProperty, value);
    }

    public static Thickness GetHorizontalScrollBarMargin(UIElement element)
    {
        return (Thickness)element.GetValue(HorizontalScrollBarMarginProperty);
    }

    public static readonly System.Windows.DependencyProperty ScrollBarStyleProperty =
        System.Windows.DependencyProperty.RegisterAttached("ScrollBarStyle", typeof(Style), typeof(ScrollViewerProperties), new PropertyMetadata(null));

    public static void SetScrollBarStyle(DependencyObject element, Style value)
    {
        element.SetValue(ScrollBarStyleProperty, value);
    }

    public static Style GetScrollBarStyle(DependencyObject element)
    {
        return (Style)element.GetValue(ScrollBarStyleProperty);
    }
}