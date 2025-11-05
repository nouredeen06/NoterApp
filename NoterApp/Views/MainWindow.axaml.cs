using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Animation;
using Avalonia.Animation.Easings;
using Avalonia.Controls.Primitives;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.Media.Transformation;
using Avalonia.Styling;
using Avalonia.Threading;
using NoterApp.ViewModels;
using Svg;
using Svg.Transforms;
using aSvg = Avalonia.Svg.Skia.Svg;

namespace NoterApp.Views;

public partial class MainWindow : Window
{
    public Button[] Buttons { get; set; }
    public TextBlock[] TextBlocks { get; set; }
    public aSvg[] Icons { get; set; }
    public string ClickedButton { get; set; }
    private bool _isListTabsOpen = false;
    private bool _isSidebarOpen = true;

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
            ScrollTabs.VerticalScrollBarVisibility = ScrollBarVisibility.Disabled;
            ScrollTabs.MaxHeight = 0;
            SvgListTabs.Path = "/Assets/Icons/chevron-down.svg";
            _isListTabsOpen = false;
        }
        else
        {
            ScrollTabs.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScrollTabs.MaxHeight = 132;

            SvgListTabs.Path = "/Assets/Icons/chevron-up.svg";
            _isListTabsOpen = true;
        }
    }

    public void BtnNewNote_OnClick(object? sender, RoutedEventArgs e)
    {
        if (!_isListTabsOpen)
        {
            ScrollTabs.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScrollTabs.MaxHeight = 132;
            SvgListTabs.Path = "/Assets/Icons/chevron-up.svg";
            _isListTabsOpen = true;
        }
    }

    private void BtnMin_OnClick(object? sender, RoutedEventArgs e)
    {
        minimizeSidebar();
    }


    private async void minimizeSidebar()
    {
        double desiredOpacity;

        if (_isSidebarOpen)
        {
            _isSidebarOpen = false;
            svgSidebar.Path = "/Assets/Icons/chevron-last.svg";
            desiredOpacity = 0;
            txtDash.Opacity = desiredOpacity;
            txtAllNotes.Opacity = desiredOpacity;
            txtNewNote.Opacity = desiredOpacity;
            txtSettings.Opacity = desiredOpacity;
            txtTrash.Opacity = desiredOpacity;
            ScrollTabs.Opacity = desiredOpacity;
            btnListTabs.Opacity = desiredOpacity;

            borderMargin.Width = 5;
            borderSidebar.Width = 56;

            await Task.Delay(300);

            if (!_isSidebarOpen)
            {
                txtDash.IsVisible = false;
                txtAllNotes.IsVisible = false;
                txtNewNote.IsVisible = false;
                txtSettings.IsVisible = false;
                txtTrash.IsVisible = false;
                ScrollTabs.IsVisible = false;
                btnListTabs.IsVisible = false;
            }
        }
        else
        {
            _isSidebarOpen = true;
            svgSidebar.Path = "/Assets/Icons/chevron-first.svg";
            txtDash.IsVisible = true;
            txtAllNotes.IsVisible = true;
            txtNewNote.IsVisible = true;
            txtSettings.IsVisible = true;
            txtTrash.IsVisible = true;
            ScrollTabs.IsVisible = true;
            btnListTabs.IsVisible = true;

            borderMargin.Width = 30;
            borderSidebar.Width = 227;

            desiredOpacity = 1;
            txtDash.Opacity = desiredOpacity;
            txtAllNotes.Opacity = desiredOpacity;
            txtNewNote.Opacity = desiredOpacity;
            txtSettings.Opacity = desiredOpacity;
            txtTrash.Opacity = desiredOpacity;
            
            await Task.Delay(600);
            ScrollTabs.Opacity = desiredOpacity;
            btnListTabs.Opacity = desiredOpacity;
        }
    }
}