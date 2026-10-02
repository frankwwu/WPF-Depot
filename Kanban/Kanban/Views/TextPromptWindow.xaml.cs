using System.Windows;

namespace Kanban.Views;

/// <summary>A small modal "enter some text" dialog.</summary>
public partial class TextPromptWindow : Window
{
    public TextPromptWindow(string title, string prompt, string initialText)
    {
        InitializeComponent();

        Title = title;
        PromptText.Text = prompt;
        InputBox.Text = initialText;

        Loaded += (_, _) =>
        {
            InputBox.Focus();
            InputBox.SelectAll();
        };
    }

    public string Text => InputBox.Text;

    private void Ok_Click(object sender, RoutedEventArgs e) => DialogResult = true;
}
