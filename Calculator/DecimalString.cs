using System.Windows.Automation.Provider;

namespace Calculator
{
    /// <summary>
    /// 浮動小数点数値型を保持する文字列クラス。初期値"0"
    /// </summary>
    internal class DecimalString
    {
        public const int MaxLength = 20;

        private const string DefaultValue = "0";

        private string _value = DefaultValue;

        public string Value => _value;

        public static DecimalString operator +(DecimalString a, DecimalString b)
        {
            return new(a.ToDecimal() + b.ToDecimal());
        }

        public static DecimalString operator -(DecimalString a, DecimalString b)
        {
            return new(a.ToDecimal() - b.ToDecimal());
        }

        public static DecimalString operator *(DecimalString a, DecimalString b)
        {
            return new(a.ToDecimal() * b.ToDecimal());
        }

        public static DecimalString operator /(DecimalString a, DecimalString b)
        {
            return new(a.ToDecimal() / b.ToDecimal());
        }

        public DecimalString()
        {
        }

        private DecimalString(decimal value)
        {
            var strValue = value.ToString();
            if (strValue.Length <= MaxLength)
            {
                _value = strValue;
            }
            else
            {
                decimal remainder = Math.Abs(value % 1);
                // 小数の場合、右端を削る
                if (0 < remainder && remainder < 1)
                {
                    _value = strValue.Substring(0, MaxLength);
                    Omit();
                }
                else
                {
                    throw new OverflowException("Over max length.");
                }
            }
        }

        public void Clear()
        {
            _value = DefaultValue;
        }

        public string Append(char value)
        {
            if (!CanAppend())
            {
                return _value;
            }
            if (value == '.')
            {
                if (_value.Contains('.'))
                {
                    return _value;
                }
                _value += '.';
            }
            else
            {
                if (value < '0' || '9' < value)
                {
                    return _value;
                }

                if (_value.Equals("0"))
                {
                    _value = string.Empty;
                }
                _value += value.ToString();
            }

            return _value;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null)
            {
                return false;
            }
            if (obj is DecimalString ds)
            {
                return _value.Equals(ds._value);
            }
            if (obj is string str)
            {
                return _value.Equals(str);
            }
            return false;
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        public DecimalString Clone()
        {
            return (DecimalString)MemberwiseClone();
        }

        private bool CanAppend()
        {
            return _value.Length < MaxLength;
        }

        private decimal ToDecimal()
        {
            Omit();
            return decimal.Parse(_value);
        }

        private void Omit()
        {
            // 小数部が'.', '0'で終わらないようにする
            if (_value.Contains('.'))
            {
                while (_value.EndsWith('0'))
                {
                    _value = _value.TrimEnd('0');
                }
                if (_value.EndsWith('.'))
                {
                    _value = _value.TrimEnd('.');
                }
            }
        }
    }
}
