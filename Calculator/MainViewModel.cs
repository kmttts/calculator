using System.Windows.Input;

namespace Calculator
{
    internal class MainViewModel : BindableBase
    {
        private const string ErrorStr = "ERROR";

        private string _displayValue = string.Empty;

        public string DisplayValue
        {
            get => _displayValue;
            set
            {
                SetProperty(ref _displayValue, value);
            }
        }

        public ICommand NumberCommand { get; set; }

        public ICommand OperatorCommand { get; set; }

        public ICommand EqualCommand { get; set; }

        public ICommand ClearCommand { get; set; }

        public ICommand AllClearCommand { get; set; }

        public MainViewModel()
        {
            MainModel model = new();
            DisplayValue = model.DisplayValue;

            NumberCommand = new DelegateCommand<string>(
                param =>
                {
                    foreach (char value in param.ToCharArray())
                    {
                        model.InputChar(value);
                    }
                    DisplayValue = model.DisplayValue;
                });

            OperatorCommand = new DelegateCommand<OperatorKind>(
                param =>
                {
                    try
                    {
                        model.SetOperator(param);
                        DisplayValue = model.DisplayValue;
                    }
                    catch (DivideByZeroException)
                    {
                        DisplayValue = ErrorStr;
                    }
                    catch (OverflowException)
                    {
                        DisplayValue = ErrorStr;
                    }
                });

            EqualCommand = new DelegateCommand(
                () =>
                {
                    try
                    {
                        model.Calculate();
                        DisplayValue = model.DisplayValue;
                    }
                                        catch (DivideByZeroException)
                    {
                        DisplayValue = ErrorStr;
                    }
                    catch (OverflowException)
                    {
                        DisplayValue = ErrorStr;
                    }
                });

            ClearCommand = new DelegateCommand(
                () =>
                {
                    model.Clear();
                    DisplayValue = model.DisplayValue;
                });

            AllClearCommand = new DelegateCommand(
                () =>
                {
                    model.AllClear();
                    DisplayValue = model.DisplayValue;
                });
        }
    }
}