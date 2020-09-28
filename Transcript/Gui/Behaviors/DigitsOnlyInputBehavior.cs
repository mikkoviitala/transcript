using System;
using System.Windows.Input;
using System.Windows.Controls;
using Microsoft.Xaml.Behaviors;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace Gui.Behaviors
{
    public class DigitsOnlyInputBehavior : Behavior<TextBox>
    {
        protected override void OnAttached()
        {
            base.OnAttached();
            AssociatedObject.PreviewTextInput += AssociatedObject_TextInput;
        }

        private void AssociatedObject_TextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, "^[0-9]"))
            {
                e.Handled = true;
            }
        }

        protected override void OnDetaching()
        {
            base.OnDetaching();
            //AssociatedObject.PreviewTextInput -= AssociatedObjectPreviewTextInput;
            //AssociatedObject.PreviewKeyDown -= AssociatedObjectPreviewKeyDown;

           // DataObject.RemovePastingHandler(AssociatedObject, Pasting);
        }       
    }
}
