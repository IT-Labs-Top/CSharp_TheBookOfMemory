using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using UserControl = System.Windows.Controls.UserControl;

namespace TheBookOfMemory.Views.Controls;

public partial class DoubleSliderControl : UserControl
{

    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register("Minimum", typeof(double), typeof(DoubleSliderControl), new UIPropertyMetadata(0d));
    public double Minimum
    {
        get { return (double)GetValue(MinimumProperty); }
        set { SetValue(MinimumProperty, value); }
    }

    public static readonly DependencyProperty LowerValueProperty =
        DependencyProperty.Register("LowerValue", typeof(double), typeof(DoubleSliderControl), new UIPropertyMetadata(0d, LowerValueCoerceValueCallback));

    public double LowerValue
    {
        get { return (double)GetValue(LowerValueProperty); }
        set { SetValue(LowerValueProperty, value); }
    }

    public static readonly DependencyProperty UpperValueProperty =
        DependencyProperty.Register("UpperValue", typeof(double), typeof(DoubleSliderControl), new UIPropertyMetadata(1d, UpperValueCoerceValueCallback));

    public double UpperValue
    {
        get { return (double)GetValue(UpperValueProperty); }
        set { SetValue(UpperValueProperty, value); }
    }

    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register("Maximum", typeof(double), typeof(DoubleSliderControl), new UIPropertyMetadata(1d));
    public double Maximum
    {
        get { return (double)GetValue(MaximumProperty); }
        set { SetValue(MaximumProperty, value); }
    }

    public static readonly DependencyProperty IsSnapToTickEnabledProperty =
        DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(DoubleSliderControl), new UIPropertyMetadata(false));
    public bool IsSnapToTickEnabled
    {
        get { return (bool)GetValue(IsSnapToTickEnabledProperty); }
        set { SetValue(IsSnapToTickEnabledProperty, value); }
    }

    public static readonly DependencyProperty TickFrequencyProperty =
        DependencyProperty.Register("TickFrequency", typeof(double), typeof(DoubleSliderControl), new UIPropertyMetadata(0.1d));
    public double TickFrequency
    {
        get { return (double)GetValue(TickFrequencyProperty); }
        set { SetValue(TickFrequencyProperty, value); }
    }

    public static readonly DependencyProperty TickPlacementProperty =
        DependencyProperty.Register("TickPlacement", typeof(TickPlacement), typeof(DoubleSliderControl), new UIPropertyMetadata(TickPlacement.None));
    public TickPlacement TickPlacement
    {
        get { return (TickPlacement)GetValue(TickPlacementProperty); }
        set { SetValue(TickPlacementProperty, value); }
    }

    public static readonly DependencyProperty TicksProperty =
        DependencyProperty.Register("Ticks", typeof(DoubleCollection), typeof(DoubleSliderControl), new UIPropertyMetadata(null));

    public DoubleCollection Ticks
    {
        get { return (DoubleCollection)GetValue(TicksProperty); }
        set { SetValue(TicksProperty, value); }
    }

    public DoubleSliderControl()
    {
        InitializeComponent();
    }

    private static void UpperValueCoerceValueCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var targetSlider = (DoubleSliderControl)d;
        var value = (double)e.NewValue;
        targetSlider.UpperValue = Math.Max(value, targetSlider.LowerValue);
    }

    private static void LowerValueCoerceValueCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        var targetSlider = (DoubleSliderControl)d;
        var value = (double)e.NewValue;
        targetSlider.LowerValue = Math.Min(value, targetSlider.UpperValue);
    }
}