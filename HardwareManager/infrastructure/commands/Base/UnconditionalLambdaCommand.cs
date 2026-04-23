using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HardwareManager.infrastructure.commands.Base
{
    internal class UnconditionalLambdaCommand : Command
    {
        private readonly Action<object> execute;

        public UnconditionalLambdaCommand(Action<object> execute)
        {
            if (execute == null) { throw new ArgumentNullException(nameof(execute)); }
            this.execute = execute;
        }

        public override bool CanExecute(object parameter) => true;

        public override void Execute(object parameter) => execute(parameter);
    }
}
