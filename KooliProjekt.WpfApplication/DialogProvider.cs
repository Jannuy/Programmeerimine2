using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace KooliProjekt.WpfApplication
{
    public class DialogProvider : IDialogProvider
    {
        public bool Confirm(string message)
        {
            var result = MessageBox.Show(message, "Confirm", MessageBoxButton.YesNo);
            return  result == MessageBoxResult.Yes;
        }

        public void ShowError(string message)
        {
            MessageBox.Show(message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
