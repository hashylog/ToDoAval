using Avalonia;
using Avalonia.Input;
using Avalonia.Controls;
using Avalonia.Interactivity;
using System.Collections.Generic;

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


    private void AddTask()
    {
        
        // --
        // This Grid acts like a TaskItem. 
        //
        // It contains the following components:
        // 
        // - TextBox: Main field for displaying the task's name and description.
        // - CheckBox: Indicates whether the task has been completed or not.
        // - Button: Used to delete the task.
        // 
        // The Grid, acting as a TaskItem, is inserted into the TaskFrame, which is an ItemControl.
        // To add a new task item to the TaskFrame, simply add the Grid to it.
        // --

        Grid grid = new()
        {
            Background = Avalonia.Media.Brushes.Transparent,
            ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto")
        };

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
            Background = Avalonia.Media.Brushes.Transparent,
            Content = new Image
            {
                Width = 30,
                Height = 30,
                Source = new Avalonia.Media.Imaging.Bitmap("Assets/Images/Icons/delete.png")
            }
        };        

        // Feature: Add Drag Drop
        // ToDo (bruh lol): Add More Features.
        grid.AddHandler(PointerPressedEvent, (sender, e) => { if (sender != null && e != null) OnPointerPressedHandler(sender, e); });
        grid.AddHandler(PointerReleasedEvent, (sender, e) => { if (sender != null && e != null) OnPointerReleaseHandler(sender, e); });

        // Remove Task Button
        button.Click += (sender, e) => { Tasks.Remove(grid); TasksFrame.Items.Remove(grid); };

        // If OnKeyDown Enter or Esc, Lose Focus
        textbox.KeyDown += (sender, e) => { if ((e.Key == Key.Enter || e.Key == Key.Escape) && FocusManager != null) { FocusManager.ClearFocus(); } };

        // Styling the Grid (TaskItem)
        Grid.SetColumn(checkbox, 0);
        Grid.SetColumn(textbox, 1);
        Grid.SetColumn(button, 2);

        // Adding the Components to the Grid (TaskItem)
        grid.Children.Add(checkbox);
        grid.Children.Add(textbox);
        grid.Children.Add(button);

        // Add the Grid to the List<Grid>, making the DragDrop/Sort feature works.
        Tasks.Add(grid);

        // Add the Grid (TaskItem) to the TaskFrame (Main Window).
        TasksFrame.Items.Add(grid);
    }

    Grid? GridClicked;

    public void OnPointerPressedHandler(object sender, PointerPressedEventArgs args)
    {
        System.Console.WriteLine("Pointer Clicked");
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
        System.Console.WriteLine("Pointer Released");

        if (GridClicked == null) return;

        Grid? HitRelease = Avalonia.VisualTree.VisualExtensions.GetVisualAt(this, args.GetPosition(this)) as Grid;
        
        if (GridClicked == HitRelease) return;

        // First Index Snap (If you DragDrop above the limit, it will Snap).
        if (args.GetPosition(Tasks[0]).Y < -5)
        {
            System.Console.WriteLine("Above the Limit, Snapping.");
            SwapItemInList(Tasks, Tasks.FindIndex(item => item == GridClicked), 0);
            RebuildTasks();
            return;
        }

        // Last Index Snap (If you DragDrop below the limit, it will Snap).
        if (args.GetPosition(Tasks[^1]).Y > 50)
        {
            System.Console.WriteLine("Below the Limit, Snapping.");
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

    // Swap Two Items in the List.
    static void SwapItemInList<T>(List<T> list, int firstindex, int secondindex)
    {
        (list[secondindex], list[firstindex]) = (list[firstindex], list[secondindex]);
    }
}