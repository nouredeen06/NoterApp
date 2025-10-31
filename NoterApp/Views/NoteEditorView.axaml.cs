using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Media;

namespace NoterApp.Views;

public partial class NoteEditorView : UserControl
{
    private bool _editing = true;
    private string _textAlignment = "left";

    public NoteEditorView()
    {
        InitializeComponent();
    }

    private void BtnMode_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_editing)
        {
            SvgMode.Path = "/Assets/Icons/pencil.svg";
            TxtTitle.IsReadOnly = true;
            TxtBody.IsReadOnly = true;
            BtnModeTipLine1.Text = "Current: Reading Mode";
            BtnModeTipLine2.Text = "Next: Editing Mode";
            _editing = false;
        }
        else
        {
            SvgMode.Path = "/Assets/Icons/book-open-text.svg";
            TxtTitle.IsReadOnly = false;
            TxtBody.IsReadOnly = false;
            BtnModeTipLine1.Text = "Current: Editing Mode";
            BtnModeTipLine2.Text = "Next: Reading Mode";
            _editing = true;
        }
    }

    private void BtnAlignment_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_textAlignment == "left")
        {
            _textAlignment = "center";
            TxtBody.TextAlignment = TextAlignment.Center;
            SvgAlignment.Path = "/Assets/Icons/text-align-center.svg";
            BtnAlignmentTipLine1.Text = "Current: Center Align";
            BtnAlignmentTipLine2.Text = "Next: Right Align";
        } else if (_textAlignment == "center")
        {
            _textAlignment = "right";
            TxtBody.TextAlignment = TextAlignment.Right;
            SvgAlignment.Path = "/Assets/Icons/text-align-end.svg";
            BtnAlignmentTipLine1.Text = "Current: Center Align";
            BtnAlignmentTipLine2.Text = "Next: Right Align";
        }
        else if (_textAlignment == "right")
        {
            _textAlignment = "left";
            TxtBody.TextAlignment = TextAlignment.Left;
            SvgAlignment.Path = "/Assets/Icons/text-align-start.svg";
            BtnAlignmentTipLine1.Text = "Current: Left Align";
            BtnAlignmentTipLine2.Text = "Next: Center Align";
        }
    }
}