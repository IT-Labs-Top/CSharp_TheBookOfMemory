using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace TheBookOfMemory.Views.Controls;

public partial class DoubleSliderControl : UserControl
{
    public static readonly DependencyProperty MinimumProperty =
        DependencyProperty.Register("Minimum", typeof(double), typeof(DoubleSliderControl), new UIPropertyMetadata(0d));
    public static readonly DependencyProperty LowerValueProperty =
        DependencyProperty.Register("LowerValue", typeof(double), typeof(DoubleSliderControl), new UIPropertyMetadata(0d, null, LowerValueCoerceValueCallback));
    public static readonly DependencyProperty UpperValueProperty =
        DependencyProperty.Register("UpperValue", typeof(double), typeof(DoubleSliderControl), new UIPropertyMetadata(1d, null, UpperValueCoerceValueCallback));
    public static readonly DependencyProperty MaximumProperty =
        DependencyProperty.Register("Maximum", typeof(double), typeof(DoubleSliderControl), new UIPropertyMetadata(1d));
    public static readonly DependencyProperty IsSnapToTickEnabledProperty =
        DependencyProperty.Register("IsSnapToTickEnabled", typeof(bool), typeof(DoubleSliderControl), new UIPropertyMetadata(false));
    public static readonly DependencyProperty TickFrequencyProperty =
        DependencyProperty.Register("TickFrequency", typeof(double), typeof(DoubleSliderControl), new UIPropertyMetadata(0.1d));
    public static readonly DependencyProperty TickPlacementProperty =
        DependencyProperty.Register("TickPlacement", typeof(TickPlacement), typeof(DoubleSliderControl), new UIPropertyMetadata(TickPlacement.None));
    public static readonly DependencyProperty TicksProperty =
        DependencyProperty.Register("Ticks", typeof(DoubleCollection), typeof(DoubleSliderControl), new UIPropertyMetadata(null));

    public double Minimum
    {
        get { return (double)GetValue(MinimumProperty); }
        set { SetValue(MinimumProperty, value); }
    }

    public double LowerValue
    {
        get { return (double)GetValue(LowerValueProperty); }
        set { SetValue(LowerValueProperty, value); }
    }

    public double UpperValue
    {
        get { return (double)GetValue(UpperValueProperty); }
        set { SetValue(UpperValueProperty, value); }
    }

    public double Maximum
    {
        get { return (double)GetValue(MaximumProperty); }
        set { SetValue(MaximumProperty, value); }
    }

    public bool IsSnapToTickEnabled
    {
        get { return (bool)GetValue(IsSnapToTickEnabledProperty); }
        set { SetValue(IsSnapToTickEnabledProperty, value); }
    }

    public double TickFrequency
    {
        get { return (double)GetValue(TickFrequencyProperty); }
        set { SetValue(TickFrequencyProperty, value); }
    }

    public TickPlacement TickPlacement
    {
        get { return (TickPlacement)GetValue(TickPlacementProperty); }
        set { SetValue(TickPlacementProperty, value); }
    }

    public DoubleCollection Ticks
    {
        get { return (DoubleCollection)GetValue(TicksProperty); }
        set { SetValue(TicksProperty, value); }
    }

    public DoubleSliderControl()
    {
        InitializeComponent();
    }


    private static object LowerValueCoerceValueCallback(DependencyObject target, object valueObject)
    {
        var targetSlider = (DoubleSliderControl)target;
        var value = (double)valueObject;
        return Math.Min(value, targetSlider.UpperValue);
    }

    private static object UpperValueCoerceValueCallback(DependencyObject target, object valueObject)
    {
        var targetSlider = (DoubleSliderControl)target;
        var value = (double)valueObject;
        return Math.Max(value, targetSlider.LowerValue);
    }
}