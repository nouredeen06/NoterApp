using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    private bool _isCollapsed = true;

    public MainWindow()
    {
        InitializeComponent();

        int[] a = { 1, 2, 3 };
        Buttons = [btnDash, btnAllNotes, btnTrash, btnSettings];
        TextBlocks = [txtDash, txtAllNotes, txtTrash, txtSettings];
        Icons = [SvgDash, SvgAllNotes, SvgTrash, SvgSettings];
        ClickedButton = "btnDash";
    }

    protected override void OnDataContextChanged(EventArgs e)
    {
        base.OnDataContextChanged(e);

        if (DataContext is MainWindowViewModel oldVm)
        {
            oldVm.PropertyChanged -= OnViewModelPropertyChanged;
        }

        if (DataContext is MainWindowViewModel newVm)
        {
            newVm.PropertyChanged += OnViewModelPropertyChanged;
        }
    }

    private void OnViewModelPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(MainWindowViewModel.IsCollapsed))
        {
            if (DataContext is MainWindowViewModel vm)
            {
                _isCollapsed = vm.IsCollapsed;

                collapseSidebar();
            }
        }
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
            ScrollTabs.IsVisible = false;
            SvgListTabs.Path = "/Assets/Icons/chevron-down.svg";
            _isListTabsOpen = false;
        }
        else
        {
            ScrollTabs.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            ScrollTabs.MaxHeight = 132;
            ScrollTabs.IsVisible = true;
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
            ScrollTabs.IsVisible = true;
            SvgListTabs.Path = "/Assets/Icons/chevron-up.svg";
            _isListTabsOpen = true;
        }
    }

    private async void collapseSidebar()
    {
        double desiredOpacity;

        if (_isCollapsed)
        {
            SvgSidebar.Path = "/Assets/Icons/panel-left-open.svg";
            desiredOpacity = 0;
            txtDash.Opacity = desiredOpacity;
            txtAllNotes.Opacity = desiredOpacity;
            txtNewNote.Opacity = desiredOpacity;
            txtSettings.Opacity = desiredOpacity;
            txtTrash.Opacity = desiredOpacity;
            ScrollTabs.Opacity = desiredOpacity;
            btnListTabs.Opacity = desiredOpacity;
            
            borderSidebar.Width = 56;

            await Task.Delay(300);


            txtDash.IsVisible = false;
            txtAllNotes.IsVisible = false;
            txtNewNote.IsVisible = false;
            txtSettings.IsVisible = false;
            txtTrash.IsVisible = false;
            ScrollTabs.IsVisible = false;
            btnListTabs.IsVisible = false;
        }
        else
        {
            SvgSidebar.Path = "/Assets/Icons/panel-left-close.svg";
            txtDash.IsVisible = true;
            txtAllNotes.IsVisible = true;
            txtNewNote.IsVisible = true;
            txtSettings.IsVisible = true;
            txtTrash.IsVisible = true;
            ScrollTabs.IsVisible = true;
            btnListTabs.IsVisible = true;
            
            borderSidebar.Width = 1801;

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