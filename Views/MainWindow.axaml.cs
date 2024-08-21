using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;

namespace ToDoAval.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        var TasksFrame = this.FindControl<ItemsControl>("TasksFrame");
    }

    public void OnButtonClick(object sender, RoutedEventArgs args)
    {
        AddTask();
    }

    private void AddTask()
    {

        var grid = new Grid
        {
            ColumnDefinitions = new ColumnDefinitions("Auto,*,Auto")
        };

        var checkbox = new CheckBox
        {
            Margin = new Thickness(15,10),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            CornerRadius = new CornerRadius(50)
        };

        var textbox = new TextBox
        {
            Watermark = "New Task",
            Margin = new Thickness(10,10),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            CornerRadius = new CornerRadius(5)
        };

        var button = new Button
        {
            Margin = new Thickness(15,10),
            VerticalAlignment = Avalonia.Layout.VerticalAlignment.Center,
            CornerRadius = new CornerRadius(5),
            Width = 30,
            Height = 30,
            
            Content = new Image
            {
                Width = 30,
                Height = 30,
                Source = new Avalonia.Media.Imaging.Bitmap("Assets/Images/Icons/delete.png")
            }
        };

        button.Click += (sender, e) => 
        {
            TasksFrame.Items.Remove(grid);
        };

        Grid.SetColumn(checkbox, 0);
        Grid.SetColumn(textbox, 1);
        Grid.SetColumn(button, 2);

        grid.Children.Add(checkbox);
        grid.Children.Add(textbox);
        grid.Children.Add(button);

        TasksFrame.Items.Add(grid);
    }
}