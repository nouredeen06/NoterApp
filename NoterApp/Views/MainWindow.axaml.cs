using System;
using System.Collections.Generic;
using System.Threading;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Transformation;
using Avalonia.Styling;
using NoterApp.ViewModels;
using Svg;
using aSvg = Avalonia.Svg.Skia.Svg;

namespace NoterApp.Views;

public partial class MainWindow : Window
{
    public Button[] Buttons { get; set; }
    public TextBlock[] TextBlocks { get; set; }
    public aSvg[] Icons { get; set; }
    public string ClickedButton { get; set; }
    private bool _isListTabsOpen = false;

    public MainWindow()
    {
        InitializeComponent();

        int[] a = { 1, 2, 3 };
        Buttons = [btnDash, btnAllNotes, btnTrash, btnSettings];
        TextBlocks = [txtDash, txtAllNotes, txtTrash, txtSettings];
        Icons = [SvgDash, SvgAllNotes, SvgTrash, SvgSettings];
        ClickedButton = "btnDash";
    }


    private void Button_OnPointerEntered(object? sender, PointerEventArgs e)
    {
        for (int i = 0; i < 4; i++)
        {
            if (sender.Equals(Buttons[i]))
            {
                Icons[i].SetValue(aSvg.CssProperty, "path, circle { stroke: #E22727; }");
                TextBlocks[i].Foreground = Brush.Parse("#E22727");
            }
        }
    }

    private void Button_OnPointerExited(object? sender, PointerEventArgs e)
    {
        for (int i = 0; i < 4; i++)
        {
            if (sender.Equals(Buttons[i]) && Buttons[i].Name != ClickedButton)
            {
                Icons[i].SetValue(aSvg.CssProperty, "path, circle { stroke: #FFFFFF; }");
                TextBlocks[i].Foreground = Brush.Parse("#FFFFFF");
            }
        }
    }

    private void Button_OnClick(object? sender, RoutedEventArgs e)
    {
        for (int i = 0; i < 4; i++)
        {
            if (sender.Equals(Buttons[i]))
            {
                Icons[i].SetValue(aSvg.CssProperty, "path, circle { stroke: #E22727; }");
                TextBlocks[i].Foreground = Brush.Parse("#E22727");
                ClickedButton = Buttons[i].Name;
            }
            else
            {
                Icons[i].SetValue(aSvg.CssProperty, "path, circle { stroke: #FFFFFF; }");
                TextBlocks[i].Foreground = Brush.Parse("#FFFFFF");
            }
        }
    }

    private void BtnListTabs_OnClick(object? sender, RoutedEventArgs e)
    {
        if (_isListTabsOpen)
        {
            SvgListTabs.Path = "/Assets/Icons/chevron-down.svg";
            _isListTabsOpen = false;
        }
        else
        {
            SvgListTabs.Path = "/Assets/Icons/chevron-up.svg";
            _isListTabsOpen = true;
        }
    }
}