using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("BestLibrary.UnitTests")]
namespace Cuni.Arithmetics.FixedPoint
{
    public interface IArithmetics
    {
        int Multilpy(Q32 a);
        int Divide(Q32 a);
        int Add(Q32 a);
        int Subtract(Q32 a);
        double DoubleValue { get; }

        Q32 WithValue(int a);
        Q32 WithRawValue(int a);


        //int GetFractionLength { get; }
    }

    public struct Fixed<T> where T : IArithmetics, new()
    {
        internal Q32 FixedPoint { get; }

        public override string ToString()
        {
            return ((double)this).ToString();
        }

        internal Fixed(Q32 fixedPoint)
        {
            FixedPoint = fixedPoint;
        }
        public Fixed(int number) : this(new T().WithValue(number)) { }

        public Fixed<T> Multiply(Fixed<T> other)
        {
            var newNumber = FixedPoint.Multilpy(other.FixedPoint);
            var a = new Fixed<T>(new T().WithRawValue(newNumber));            
            return a;
        }

        public Fixed<T> Add(Fixed<T> other)
        {
            int number = FixedPoint.Add(other.FixedPoint);
            var a = new Fixed<T>(new T().WithRawValue(number));
            return a;
        }

        public Fixed<T> Subtract(Fixed<T> other)
        {
            int number = FixedPoint.Subtract(other.FixedPoint);
            var a = new Fixed<T>(new T().WithRawValue(number));
            return a;
        }

        public Fixed<T> Divide(Fixed<T> other)
        {
            int number = FixedPoint.Divide(other.FixedPoint);
            var a = new Fixed<T>(new T().WithRawValue(number));
            return a;
        }

        public static implicit operator double(Fixed<T> number)
        {
            return number.FixedPoint.DoubleValue;
        }
    }

    public abstract class Q32
    {
        public static int IntegerLength = 32;
        public static int FractionLength;

        /// <summary>
        /// To make it easy when using arithmetics
        /// </summary>
        internal long RawLongValue { get; }
   
        public double DoubleValue
        {
            get
            {
                return RawLongValue / (double)(1 << FractionLength);
            }
        }
        
        protected Q32(int number)
        {
            RawLongValue = number;
        }

        public int Multilpy(Q32 other)
        {
            bool isResultNegative = (this.RawLongValue < 0) ^ (other.RawLongValue < 0);
            long num = Math.Abs(RawLongValue) * Math.Abs(other.RawLongValue);
            int result = (int)(num >> FractionLength);
            if (isResultNegative)
            {
                result *= -1;
            }
            return result;            
        }

        public int Divide(Q32 other)
        {
            bool isResultNegative = (this.RawLongValue < 0) ^ (other.RawLongValue < 0);
            long num = (Math.Abs(RawLongValue) << FractionLength) / Math.Abs(other.RawLongValue);
            int result = (int)num;
            if (isResultNegative)
            {
                result *= -1;
            }
            return result;
        }

        public int Add(Q32 other)
        {
            long num = this.RawLongValue + other.RawLongValue;                           
            return (int)num;
        }

        public int Subtract(Q32 other)
        {
            long num = this.RawLongValue - other.RawLongValue;                              
            return (int)num;
        }
    }

    public sealed class Q24_8 : Q32, IArithmetics
    {
        static Q24_8()
        {
            FractionLength = 8;
        }

        private Q24_8(int number) : base(number) { }

        public Q24_8() : base(0) { }

        public Q32 WithValue(int number)
        {
            return new Q24_8(number << FractionLength);
        }

        public Q32 WithRawValue(int number)
        {
            return new Q24_8(number);
        }

        //public static implicit operator Q24_8(int number)
        //{
        //    return new Q24_8(number);
        //}

    }

    public sealed class Q16_16 : Q32, IArithmetics
    {
        static Q16_16()
        {
            FractionLength = 16;
        }

        public Q16_16(int number) : base(number) { }

        public Q16_16() : base(0) { }

        public Q32 WithValue(int number)
        {
            return new Q16_16(number << FractionLength);
        }
        public Q32 WithRawValue(int number)
        {
            return new Q16_16(number);
        }
    }

    public sealed class Q8_24 : Q32, IArithmetics
    {
        static Q8_24()
        {
            FractionLength = 24;
        }

        public Q8_24(int number) : base(number) { }

        public Q8_24() : base(0) { }

        public Q32 WithValue(int number)
        {
            return new Q8_24(number << FractionLength);
        }
        public Q32 WithRawValue(int number)
        {
            return new Q8_24(number);
        }
    }
}
    