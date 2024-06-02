namespace Calculator
{
    internal class MainModel
    {
        public const int MaxLength = DecimalString.MaxLength;

        private DecimalString _first = new();
        private DecimalString _second = new();
        private OperatorKind? _operator;
        private State _state;

        public string DisplayValue { get; private set; }

        public MainModel()
        {
            _state = new FirstState(this);
            DisplayValue = _first.Value;
        }

        public void InputChar(char value)
        {
            _state = _state.InputChar(value);
        }

        public void SetOperator(OperatorKind value)
        {

            _state = _state.SetOperator(value);
        }

        public void Calculate()
        {
            _state = _state.Calculate();
        }

        public void Clear()
        {
            _state = _state.Clear();
        }

        public void AllClear()
        {
            _state = _state.ClearAll();
        }

        private void Reset()
        {
            _first.Clear();
            _second.Clear();
            _operator = null;
            DisplayValue = _first.Value;
        }

        #region private abstract class State
        private abstract class State
        {
            private readonly MainModel _model;

            protected MainModel Model => _model;

            public State(MainModel model)
            {
                _model = model;
            }

            public abstract State InputChar(char value);

            public abstract State SetOperator(OperatorKind value);

            public abstract State Calculate();

            public abstract State Clear();

            public abstract State ClearAll();

            protected DecimalString Operate(OperatorKind @operator)
            {
                return @operator switch
                {
                    OperatorKind.Addition => _model._first + _model._second,
                    OperatorKind.Subtraction => _model._first - _model._second,
                    OperatorKind.Multiplication => _model._first * _model._second,
                    OperatorKind.Division => _model._first / _model._second,
                    _ => throw new ArgumentOutOfRangeException(nameof(@operator))
                };
            }
        }

        private class FirstState : State
        {
            public FirstState(MainModel model)
                : base(model)
            {
            }

            public override State InputChar(char value)
            {
                Model._first.Append(value);
                Model.DisplayValue = Model._first.Value;
                return new FirstState(Model);
            }

            public override State SetOperator(OperatorKind value)
            {
                Model._operator = value;
                return new OperatorState(Model);
            }

            public override State Calculate()
            {
                return this;
            }

            public override State Clear()
            {
                return ClearAll();
            }

            public override State ClearAll()
            {
                Model.Reset();
                return this;
            }
        }

        private class OperatorState : State
        {
            public OperatorState(MainModel model)
                : base(model)
            {
            }

            public override State InputChar(char value)
            {
                Model._second.Append(value);
                Model.DisplayValue = Model._second.Value;
                return new SecondState(Model);
            }

            public override State SetOperator(OperatorKind value)
            {
                Model._operator = value;
                return new OperatorState(Model);
            }

            public override State Calculate()
            {
                if (Model._operator == null)
                {
                    return this;
                }

                Model._second = Model._first.Clone();
                Model._first = Operate(Model._operator.Value);
                Model.DisplayValue = Model._first.Value;
                return new ResultState(Model);
            }

            public override State Clear()
            {
                Model._second.Clear();
                Model.DisplayValue = Model._second.Value;
                return new SecondState(Model);
            }

            public override State ClearAll()
            {
                Model.Reset();
                return new FirstState(Model);
            }
        }

        private class SecondState : State
        {
            public SecondState(MainModel model)
                : base(model)
            {
            }

            public override State InputChar(char value)
            {
                Model._second.Append(value);
                Model.DisplayValue = Model._second.Value;
                return new SecondState(Model);
            }

            public override State SetOperator(OperatorKind value)
            {
                if (Model._operator == null)
                {
                    return this;
                }

                Model._first = Operate(Model._operator.Value);
                Model._second.Clear();
                Model._operator = value;
                Model.DisplayValue = Model._first.Value;
                return new OperatorState(Model);
            }

            public override State Calculate()
            {
                if (Model._operator == null)
                {
                    return this;
                }

                Model._first = Operate(Model._operator.Value);
                Model.DisplayValue = Model._first.Value;
                return new ResultState(Model);
            }

            public override State Clear()
            {
                Model._second.Clear();
                Model.DisplayValue = Model._second.Value;
                return this;
            }

            public override State ClearAll()
            {
                Model.Reset();
                return new FirstState(Model);
            }
        }

        private class ResultState : State
        {
            public ResultState(MainModel model)
                : base(model)
            {
            }

            public override State InputChar(char value)
            {
                Model._first.Clear();
                Model._first.Append(value);
                Model._second.Clear();
                Model.DisplayValue = Model._first.Value;
                return new FirstState(Model);
            }

            public override State SetOperator(OperatorKind value)
            {
                Model._operator = value;
                Model._second.Clear();
                return new OperatorState(Model);
            }

            public override State Calculate()
            {
                if (Model._operator == null)
                {
                    return this;
                }

                Model._first = Operate(Model._operator.Value);
                Model.DisplayValue = Model._first.Value;
                return new ResultState(Model);
            }

            public override State Clear()
            {
                return ClearAll();
            }

            public override State ClearAll()
            {
                Model.Reset();
                return new FirstState(Model);
            }
        }
        #endregion
    }
}