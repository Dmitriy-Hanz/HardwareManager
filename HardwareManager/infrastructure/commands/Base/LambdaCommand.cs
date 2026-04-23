using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.infrastructure.commands.Base
{
    internal class LambdaCommand : Command
    {
        private readonly Action<object> execute;
        private readonly Func<object, bool> canExecute;

        public LambdaCommand(Action<object> execute, Func<object, bool> canExecute = null)
        {
            if (execute == null) { throw new ArgumentNullException(nameof(execute)); }
            this.execute = execute;
            this.canExecute = canExecute;
        }

        public override bool CanExecute(object parameter) => (bool)(canExecute?.Invoke(parameter));

        public override void Execute(object parameter) => execute(parameter);
    }
}
