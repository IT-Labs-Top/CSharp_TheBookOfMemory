using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using MainComponents.Components;
using TheBookOfMemory.Helpers;
using TheBookOfMemory.Utilities;
using UserControl = System.Windows.Controls.UserControl;

namespace TheBookOfMemory.Views.Controls;

public enum LanguageType
{
    Ru,
    Eng
}
public partial class CustomKeyboard : UserControl
{
    public static readonly DependencyProperty SymbolsProperty = DependencyProperty.Register(
        nameof(Symbols), typeof(List<List<char>>), typeof(CustomKeyboard), new PropertyMetadata(new List<List<char>>()));

    public List<List<char>> Symbols
    {
        get => (List<List<char>>)GetValue(SymbolsProperty);
        set => SetValue(SymbolsProperty, value);
    }

    public static readonly DependencyProperty LanguageProperty = DependencyProperty.Register(
        nameof(Language), typeof(LanguageType), typeof(CustomKeyboard), new PropertyMetadata(default(LanguageType),PropertyChangedCallback));

    private static void PropertyChangedCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if(d is not CustomKeyboard keyboard) return;
        if (keyboard.IsNumbers)
        {
            keyboard.Symbols = keyboard.GetNumberSymbols();
        }
        else
        {
            keyboard.Symbols = keyboard.Language == LanguageType.Ru ? keyboard.GetRuSymbols() : keyboard.GetEngSymbols();

        }
    }

    public new LanguageType Language
    {
        get => (LanguageType)GetValue(LanguageProperty);
        set => SetValue(LanguageProperty, value);
    }

    public static readonly DependencyProperty KeyBaseWidthProperty = DependencyProperty.Register(
        nameof(KeyBaseWidth), typeof(double), typeof(CustomKeyboard), new PropertyMetadata(0.0));

    public double KeyBaseWidth
    {
        get => (double)GetValue(KeyBaseWidthProperty);
        set => SetValue(KeyBaseWidthProperty, value);
    }

    public static readonly DependencyProperty KeyBaseHeightProperty = DependencyProperty.Register(
        nameof(KeyBaseHeight), typeof(double), typeof(CustomKeyboard), new PropertyMetadata(0.0));

    public double KeyBaseHeight
    {
        get => (double)GetValue(KeyBaseHeightProperty);
        set => SetValue(KeyBaseHeightProperty, value);
    }

    public static readonly DependencyProperty ButtonBaseMarginProperty = DependencyProperty.Register(
        nameof(ButtonBaseMargin), typeof(Thickness), typeof(CustomKeyboard), new PropertyMetadata(default(Thickness)));

    public Thickness ButtonBaseMargin
    {
        get => (Thickness)GetValue(ButtonBaseMarginProperty);
        set => SetValue(ButtonBaseMarginProperty, value);
    }

    public static readonly DependencyProperty ShiftIsPressedProperty = DependencyProperty.Register(
        nameof(ShiftIsPressed), typeof(bool), typeof(CustomKeyboard), new PropertyMetadata(false));

    public bool ShiftIsPressed
    {
        get => (bool)GetValue(ShiftIsPressedProperty);
        set => SetValue(ShiftIsPressedProperty, value);
    }

    public static readonly DependencyProperty IsNumbersProperty = DependencyProperty.Register(
        nameof(IsNumbers), typeof(bool), typeof(CustomKeyboard), new PropertyMetadata(default(bool),IsNumberPropertyCallback));

    private static void IsNumberPropertyCallback(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not CustomKeyboard keyboard) return;
        if (keyboard.IsNumbers)
        {
            keyboard.Symbols = keyboard.GetNumberSymbols();
        }
        else
        {
            keyboard.Symbols = keyboard.Language == LanguageType.Ru ? keyboard.GetRuSymbols() : keyboard.GetEngSymbols();

        }
    }

    public bool IsNumbers
    {
        get => (bool)GetValue(IsNumbersProperty);
        set => SetValue(IsNumbersProperty, value);
    }

    public CustomKeyboard()
    {
        InitializeComponent();
    }

    private List<List<char>> GetRuSymbols()
    {
        List<List<char>> symbols =
        [
            GetSymbols(ShiftIsPressed ? "йцукенгшщзхъ".ToUpper() : "йцукенгшщзхъ").ToList(),
            GetSymbols(ShiftIsPressed ? "фывапролджэ".ToUpper() : "фывапролджэ").ToList(),
            GetSymbols(ShiftIsPressed ? "ячсмитьбю".ToUpper() : "ячсмитьбю").ToList()
        ];
       
        return symbols;
    }
    private List<List<char>> GetEngSymbols()
    {
        List<List<char>> symbols =
        [
            GetSymbols(ShiftIsPressed ? "qwertyuiop".ToUpper() : "qwertyuiop").ToList(),
            GetSymbols(ShiftIsPressed ? "asdfghjkl".ToUpper() : "asdfghjkl").ToList(),
            GetSymbols(ShiftIsPressed ? "zxcvbnm".ToUpper() : "zxcvbnm").ToList()
        ];
        
        return symbols;
    }
    private List<List<char>> GetNumberSymbols()
    {
        List<List<char>> symbols =
        [
            GetSymbols("1234567890" ).ToList(),
            GetSymbols("@#%_&-+()/").ToList(),
            GetSymbols("*\"':;!?").ToList()
        ];
        
        return symbols;
    }

    private List<char> GetSymbols(string symbols)
    {
        List<char> symbolsTemp = [];
        symbolsTemp.AddRange(symbols);
        return symbolsTemp;
    }

    private void KeyPressed(object sender, RoutedEventArgs e)
    {
        var symbol = (sender as RoundedRepeatButton)?.Text == "Пробел" ? ' ' : ((sender as RoundedRepeatButton)?.Text ?? string.Empty).First();
        InputLanguage.CurrentInputLanguage = Language == LanguageType.Ru 
            ? InputLanguage.FromCulture(new CultureInfo("ru-RU"))
            : InputLanguage.FromCulture(new CultureInfo("en-US"));
        KeyboardInputHelper.SendKey(symbol, ShiftIsPressed);
    }
    private void DeletePressed(object sender, RoutedEventArgs e)
    {
        KeyboardInputHelper.SendKey(0x08, false);
    }

    private void ShiftPress(object sender, RoutedEventArgs e)
    {
        ShiftIsPressed = !ShiftIsPressed;
        IsNumbers = false;
        Symbols = Language == LanguageType.Ru ? GetRuSymbols() : GetEngSymbols();
    }
    private void NumberPress(object sender, RoutedEventArgs e)
    {
        IsNumbers = !IsNumbers;
           
           
    }

    private void ChangeLanguage(object sender, RoutedEventArgs e)
    {
        Language = Language == LanguageType.Ru ? LanguageType.Eng : LanguageType.Ru;
        IsNumbers = false;
    }


    private void FrameworkElement_OnLoaded(object sender, RoutedEventArgs e)
    {
        Symbols = Language == LanguageType.Ru ? GetRuSymbols() : GetEngSymbols();
    }
}