using System.Windows;
using System.Windows.Input;

namespace WpfGlobalCommands
{
    public static class GlobalCommands 
    {
        public static ICommand ChangeSettingCommand = new DelegateCommand<string>(ChangeSetting, CanChangeSetting);

        public static void ChangeSetting(string param)
        {
            MessageBox.Show(param, "Global Command Demo");
        }

        private static bool CanChangeSetting(string param)
        {
            return true;
        }
    }
}
