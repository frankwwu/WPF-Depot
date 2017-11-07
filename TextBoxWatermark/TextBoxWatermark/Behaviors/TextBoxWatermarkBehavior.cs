using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interactivity;
using System.Windows.Media;

namespace TextBoxWatermark.Behaviors
{
    public class TextBoxWatermarkBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.GotFocus += OnGotFocus;
            AssociatedObject.LostFocus += OnLostFocus;
            AssociatedObject.TextChanged += OnTextChanged;

            OnLostFocus(null, null);
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            AssociatedObject.GotFocus -= OnGotFocus;
            AssociatedObject.LostFocus -= OnLostFocus;
            AssociatedObject.TextChanged -= OnTextChanged;
        }

        private void OnGotFocus(object sender, RoutedEventArgs e)
        {
            if (IsWatermarked)
            {
                ChangeText(string.Empty);
            }
        }

        private void OnLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(AssociatedObject.Text) || AssociatedObject.Text.Equals(Text))
            {
                if (EnableWatermark)
                {
                    ChangeText(this.Text);
                }
                else
                {
                    ChangeText(string.Empty);
                }
            }
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            if (!_isChangingText && !AssociatedObject.IsFocused)
            {
                OnLostFocus(null, null);
            }
        }

        public bool EnableWatermark
        {
            get { return (bool)GetValue(EnableWatermarkProperty); }
            set { SetValue(EnableWatermarkProperty, value); }
        }

        public static readonly DependencyProperty EnableWatermarkProperty =
            DependencyProperty.Register("EnableWatermark", typeof(bool), typeof(TextBoxWatermarkBehavior), new UIPropertyMetadata(OnEnableWatermarkChanged));

        private static void OnEnableWatermarkChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBoxWatermarkBehavior beh = sender as TextBoxWatermarkBehavior;
            if (beh != null)
            {
                beh.OnLostFocus(null, null);
            }
        }

        private bool IsWatermarked
        {
            get
            {
                return !string.IsNullOrEmpty(AssociatedObject.Text) && AssociatedObject.Text.Equals(Text);
            }
        }

        [Category("Appearance")]
        public string Text
        {
            get { return (string)base.GetValue(TextProperty); }
            set { base.SetValue(TextProperty, value); }
        }

        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(TextBoxWatermarkBehavior),
                                        new FrameworkPropertyMetadata(string.Empty));

        private Brush _foreground;

        private bool _isChangingText;

        private void ChangeText(string newText)
        {
            if (!string.IsNullOrEmpty(newText) && newText.Equals(Text))
            {
                _foreground = AssociatedObject.Foreground;
                AssociatedObject.Foreground = Brushes.Gray;
            }
            else
            {
                if (_foreground != null)
                {
                    AssociatedObject.Foreground = _foreground;
                }
            }

            _isChangingText = true;
            AssociatedObject.Text = newText;
            _isChangingText = false;
        }
    }
}

