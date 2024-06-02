using System.Windows.Input;

namespace Calculator
{
    /// <summary>
    /// コマンド実装クラス
    /// </summary>
    internal class DelegateCommand : ICommand
    {
        private readonly Action _execute;

#pragma warning disable 0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        public DelegateCommand(Action execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            _execute?.Invoke();
        }
    }


    /// <summary>
    /// パラメータありコマンド実装クラス
    /// </summary>
    /// <typeparam name="T">パラメータの型</typeparam>
    internal class DelegateCommand<T> : ICommand
    {
        private readonly Action<T> _execute;

#pragma warning disable 0067
        public event EventHandler? CanExecuteChanged;
#pragma warning restore 0067

        public DelegateCommand(Action<T> execute)
        {
            _execute = execute;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            if (parameter is T typeParam)
            {
                _execute?.Invoke(typeParam);
            }
        }
    }
}
