using System.Windows.Input;

namespace RDPShadow.Extensions
{
    public static class CommandExtensions
    {
        public static void Invoke(this ICommand command, object parameter = null)
        {
            if (command.CanExecute(null))
                command.Execute(null);
        }
    }
}