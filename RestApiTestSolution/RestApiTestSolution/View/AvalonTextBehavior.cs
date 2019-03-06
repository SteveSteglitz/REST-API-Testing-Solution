using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interactivity;
using ICSharpCode.AvalonEdit;

namespace RestApiTestSolution.View
{
    public sealed class AvalonEditBehaviour : Behavior<TextEditor>
    {
        public static readonly DependencyProperty GiveMeTheTextProperty =
            DependencyProperty.Register("GiveMeTheText", typeof(string), typeof(AvalonEditBehaviour),
                new FrameworkPropertyMetadata(default(string), FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, PropertyChangedCallback));

        public string GiveMeTheText
        {
            get => (string)GetValue(GiveMeTheTextProperty);
            set => SetValue(GiveMeTheTextProperty, value);
        }

        protected override void OnAttached()
        {
            base.OnAttached();
            if (AssociatedObject != null)
                AssociatedObject.TextChanged += AssociatedObjectOnTextChanged;
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            if (AssociatedObject != null)
                AssociatedObject.TextChanged -= AssociatedObjectOnTextChanged;
        }

        private void AssociatedObjectOnTextChanged(object sender, EventArgs eventArgs)
        {
            if (sender is TextEditor textEditor)
            {
                if (textEditor.Document != null)
                    GiveMeTheText = textEditor.Document.Text;
            }
        }

        private static void PropertyChangedCallback(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            if(dependencyPropertyChangedEventArgs.NewValue == null) return;
            
            var behavior = dependencyObject as AvalonEditBehaviour;
            if (behavior?.AssociatedObject != null)
            {
                var editor = behavior.AssociatedObject;
                if (editor.Document != null)
                {
                    var caretOffset = editor.CaretOffset;
                    editor.Document.Text = dependencyPropertyChangedEventArgs.NewValue.ToString();
                    try
                    {
                        editor.CaretOffset = caretOffset;
                    }
                    catch (System.ArgumentOutOfRangeException)
                    {
                        editor.CaretOffset = 0;
                    }
                }
            }
        }
    }
}
