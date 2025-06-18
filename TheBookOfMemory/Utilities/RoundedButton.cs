using System.Windows;
using System.Windows.Controls.Primitives;

namespace TheBookOfMemory.Utilities;

public class RoundedRepeatButton : RepeatButton
{
    public static readonly DependencyProperty CornerRadiusProperty = DependencyProperty.Register(
        nameof(CornerRadius), typeof(CornerRadius), typeof(RoundedRepeatButton), new PropertyMetadata(default(CornerRadius)));

    public CornerRadius CornerRadius
    {
        get => (CornerRadius)GetValue(CornerRadiusProperty);
        set => SetValue(CornerRadiusProperty, value);
    }

    public static readonly DependencyProperty TextProperty = DependencyProperty.Register(
        nameof(Text), typeof(string), typeof(RoundedRepeatButton), new PropertyMetadata(default(string)));

    public string Text
    {
        get => (string)GetValue(TextProperty);
        set => SetValue(TextProperty, value);
    }

    static RoundedRepeatButton()
    {
        DefaultStyleKeyProperty.OverrideMetadata(typeof(RoundedRepeatButton),
            new FrameworkPropertyMetadata(typeof(RoundedRepeatButton)));
    }
}