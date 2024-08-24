using Avalonia;
using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Avalonia.Media;
using Avalonia.VisualTree;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoAval.Views;

public partial class MainWindow : Window
{

    List<Grid> Tasks = [];

    public MainWindow()
    {
        InitializeComponent();
    }

    public void OnButtonClick(object sender, RoutedEventArgs args)
    {
        AddTask();
    }

    int i = 0;
    private void AddTask()
    {

        Grid grid = new()
        {
            Name = "GridName"+i.ToString(),
            Background = Brushes.Transparent,
            ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto")
        }; i++;

        CheckBox checkbox = new()
        {
            Margin = new Thickness(15,10),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            CornerRadius = new CornerRadius(50)
        };

        TextBox textbox = new()
        {
            Watermark = "New Task",
            Margin = new Thickness(10,10),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            CornerRadius = new CornerRadius(5)
        };

        Button button = new()
        {
            Margin = new Thickness(15,10),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            CornerRadius = new CornerRadius(5),
            Width = 30,
            Height = 30,
            Background = Brushes.Transparent,
            Content = new Image
            {
                Width = 30,
                Height = 30,
                Source = new Avalonia.Media.Imaging.Bitmap("Assets/Images/Icons/delete.png")
            }
        };        

        // Feature: Add Drag Drop
        grid.AddHandler(PointerPressedEvent, (sender, e) => { if (sender != null && e != null) OnPointerPressedHandler(sender, e); });
        grid.AddHandler(PointerReleasedEvent, (sender, e) => { if (sender != null && e != null) OnPointerReleaseHandler(sender, e); });

        // Remove Task Button
        button.Click += (sender, e) => { Tasks.Remove(grid); TasksFrame.Items.Remove(grid); };

        // If OnKeyDown Enter or Esc, Lose Focus;
        textbox.KeyDown += (sender, e) => { if ((e.Key == Key.Enter || e.Key == Key.Escape) && FocusManager != null) { FocusManager.ClearFocus(); } };

        Grid.SetColumn(checkbox, 0);
        Grid.SetColumn(textbox, 1);
        Grid.SetColumn(button, 2);

        grid.Children.Add(checkbox);
        grid.Children.Add(textbox);
        grid.Children.Add(button);

        Tasks.Add(grid);
        TasksFrame.Items.Add(grid);
    }




    Grid? GridClicked;

    public void OnPointerPressedHandler(object sender, PointerPressedEventArgs args)
    {
        //System.Console.WriteLine("Pointer Clicked");
        GridClicked = args.Source as Grid;
    }

    void RebuildTasks()
    {
        TasksFrame.Items.Clear();
        for (int i = 0; i < Tasks.Count; i++)
        {
            TasksFrame.Items.Add(Tasks[i]);
        }
        GridClicked = null;
    }

    public void OnPointerReleaseHandler(object sender, PointerReleasedEventArgs args)
    {
        //System.Console.WriteLine("Pointer Released");

        if (GridClicked == null) return;

        Grid? HitRelease = this.GetVisualAt(args.GetPosition(this)) as Grid;
        
        if (GridClicked == HitRelease) return;

        // Last Index Snap
        if (args.GetPosition(Tasks.Last()).Y < -5)
        {
            SwapItemInList(Tasks, Tasks.FindIndex(item => item == GridClicked), 0);
            RebuildTasks();
            return;
        }

        // Last Index Snap
        if (args.GetPosition(Tasks.Last()).Y > 50)
        {
            SwapItemInList(Tasks, Tasks.FindIndex(item => item == GridClicked), Tasks.Count-1);
            RebuildTasks();
            return;
        }

        if (HitRelease != null && GridClicked != null)
        {
            SwapItemInList(Tasks, Tasks.FindIndex(item => item == GridClicked), Tasks.FindIndex(item => item == HitRelease));
            RebuildTasks();
        }
        
        GridClicked = null;

    }

    static void SwapItemInList<T>(List<T> list, int firstindex, int secondindex)
    {
        (list[secondindex], list[firstindex]) = (list[firstindex], list[secondindex]);
    }
}