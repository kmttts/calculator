using Calculator;

namespace CalculatorTest
{
    public class MainModelTest
    {
        private MainModel _model;

        public MainModelTest()
        {
            _model = new();
        }

        [Fact]
        public void DisplayValue()
        {
            _model = new();
            var actual = _model.DisplayValue;
            Assert.Equal("0", actual);
        }

        [Theory]
        [InlineData("0", '0')]
        [InlineData("1", '1')]
        public void DisplayValue_FirstInput(string expected, char param)
        {
            _model.InputChar(param);
            var actual = _model.DisplayValue;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("99999999999999999999", "99999999999999999999", '1')]
        [InlineData("999999999999999999.9", "999999999999999999.9", '1')]
        public void DisplayValue_MaxLengthInput(string expected, string maxLengthParam, char param)
        {
            foreach (var c in maxLengthParam)
            {
                _model.InputChar(c);
            }
            _model.InputChar(param);
            var actual = _model.DisplayValue;
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void DisplayValue_MaxLengthResult()
        {
            // 1 / 3 =
            _model.InputChar('1');
            _model.SetOperator(OperatorKind.Division);
            _model.InputChar('3');
            _model.Calculate();
            var actual = _model.DisplayValue;
            Assert.Equal("0.333333333333333333", actual);

            // 1 - 2 / 3 =
            _model = new();
            _model.InputChar('1');
            _model.SetOperator(OperatorKind.Subtraction);
            _model.InputChar('2');
            _model.SetOperator(OperatorKind.Division);
            _model.InputChar('3');
            _model.Calculate();
            actual = _model.DisplayValue;
            Assert.Equal("-0.33333333333333333", actual);

            // 11111111111111111111 + 1 =
            _model = new();
            foreach (var c in "11111111111111111111")
            {
                _model.InputChar(c);
            }
            _model.SetOperator(OperatorKind.Addition);
            _model.InputChar('1');
            _model.Calculate();
            actual = _model.DisplayValue;
            Assert.Equal("11111111111111111112", actual);

            // 99999999999999999999 + 1 =
            _model = new();
            foreach (var c in "99999999999999999999")
            {
                _model.InputChar(c);
            }
            _model.SetOperator(OperatorKind.Addition);
            _model.InputChar('1');
            Assert.Throws<OverflowException>(_model.Calculate);

            // 0.000000000000000001 + 1 =
            _model = new();
            foreach (var c in "0.000000000000000001")
            {
                _model.InputChar(c);
            }
            _model.SetOperator(OperatorKind.Addition);
            _model.InputChar('1');
            _model.Calculate();
            actual = _model.DisplayValue;
            Assert.Equal("1.000000000000000001", actual);

            // 0.000000000000000001 / 2 =
            _model = new();
            foreach (var c in "0.000000000000000001")
            {
                _model.InputChar(c);
            }
            _model.SetOperator(OperatorKind.Division);
            _model.InputChar('2');
            _model.Calculate();
            actual = _model.DisplayValue;
            Assert.Equal("0", actual);
        }

        [Fact]
        public void DisplayValue_DevideByZero()
        {
            _model.SetOperator(OperatorKind.Division);
            _model.InputChar('0');
            Assert.Throws<DivideByZeroException>(_model.Calculate);
        }

        [Theory]
        [InlineData("3", '1', OperatorKind.Addition, '2')]
        [InlineData("-1", '1', OperatorKind.Subtraction, '2')]
        [InlineData("6", '2', OperatorKind.Multiplication, '3')]
        [InlineData("0.5", '1', OperatorKind.Division, '2')]
        public void DisplayValue_Operation(string expected, char param1, OperatorKind param2, char param3)
        {
            _model.InputChar(param1);
            _model.SetOperator(param2);
            _model.InputChar(param3);
            _model.Calculate();
            var actual = _model.DisplayValue;
            Assert.Equal(expected, actual);
        }

        [Theory]
        [InlineData("3", "0", '1', OperatorKind.Addition, '2', OperatorKind.Subtraction, '3')]
        [InlineData("-1", "-3", '1', OperatorKind.Subtraction, '2', OperatorKind.Multiplication, '3')]
        [InlineData("2", "0.5", '1', OperatorKind.Multiplication, '2', OperatorKind.Division, '4')]
        [InlineData("0.5", "3.5", '1', OperatorKind.Division, '2', OperatorKind.Addition, '3')]

        public void DisplayValue_Operation2(string expected1, string expected2, char param1, OperatorKind param2, char param3, OperatorKind param4, char param5)
        {
            _model.InputChar(param1);
            _model.SetOperator(param2);
            _model.InputChar(param3);
            _model.SetOperator(param4);
            var actual = _model.DisplayValue;
            Assert.Equal(expected1, actual);
            _model.InputChar(param5);
            _model.Calculate();
            actual = _model.DisplayValue;
            Assert.Equal(expected2, actual);
        }

        [Fact]
        public void DisplayValue_ClearAll()
        {
            // 1
            _model.InputChar('1');
            _model.AllClear();
            var actual = _model.DisplayValue;
            Assert.Equal("0", actual);

            // 1 +
            _model.InputChar('1');
            _model.SetOperator(OperatorKind.Addition);
            _model.AllClear();
            actual = _model.DisplayValue;
            Assert.Equal("0", actual);

            // 1 + 1
            _model.InputChar('1');
            _model.SetOperator(OperatorKind.Addition);
            _model.InputChar('2');
            _model.AllClear();
            actual = _model.DisplayValue;
            Assert.Equal("0", actual);

            // 1 + 1 = 2
            _model.InputChar('1');
            _model.SetOperator(OperatorKind.Addition);
            _model.InputChar('2');
            _model.Calculate();
            _model.AllClear();
            actual = _model.DisplayValue;
            Assert.Equal("0", actual);
        }

        [Fact]
        public void DisplayValue_Clear()
        {
            // 1 C
            _model.InputChar('1');
            _model.Clear();
            var actual = _model.DisplayValue;
            Assert.Equal("0", actual);

            // 1 + C 2 = 3
            _model.InputChar('1');
            _model.SetOperator(OperatorKind.Addition);
            _model.Clear();
            actual = _model.DisplayValue;
            Assert.Equal("0", actual);
            _model.InputChar('2');
            _model.Calculate();
            actual = _model.DisplayValue;
            Assert.Equal("3", actual);

            // 1 + 2 C 2 = 3
            _model.InputChar('1');
            _model.SetOperator(OperatorKind.Addition);
            _model.InputChar('2');
            _model.Clear();
            actual = _model.DisplayValue;
            Assert.Equal("0", actual);
            _model.InputChar('2');
            _model.Calculate();
            actual = _model.DisplayValue;
            Assert.Equal("3", actual);

            // 1 + 2 = C
            _model.InputChar('1');
            _model.SetOperator(OperatorKind.Addition);
            _model.InputChar('2');
            _model.Calculate();
            _model.Clear();
            actual = _model.DisplayValue;
            Assert.Equal("0", actual);
            _model.InputChar('2');
            actual = _model.DisplayValue;
            Assert.Equal("2", actual);
        }
    }
}