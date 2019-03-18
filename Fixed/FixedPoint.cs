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

        Q32 WithInt(int a);
        Q32 WithRawInt(int a);


        //int GetFractionLength { get; }
    }

    public struct Fixed<T> where T : IArithmetics, new()
    {
        internal Q32 FixedPoint { get; }

        public override string ToString()
        {
            return ((double)this).ToString();
        }

        //private static int FractionLen;

        //static Fixed()
        //{
        //    //System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(T).TypeHandle);
        //    FractionLen = new T().GetFractionLength;
        //}

        private Fixed(Q32 fixedPoint)
        {
            FixedPoint = fixedPoint;
        }
        public Fixed(int number) : this(new T().WithInt(number)) { }
        public Fixed<T> Multiply(Fixed<T> other)
        {
            var newNumber = FixedPoint.Multilpy(other.FixedPoint);
            var a = new Fixed<T>(new T().WithRawInt(newNumber));
            //Console.WriteLine($"Fixed:  GetType: {this.GetType()} Fixed<{a}> typeof: {typeof(T)} ");
            return a;
        }

        public Fixed<T> Add(Fixed<T> other)
        {
            int number = FixedPoint.Add(other.FixedPoint);
            var a = new Fixed<T>(new T().WithRawInt(number));
            return a;
        }

        public Fixed<T> Subtract(Fixed<T> other)
        {
            int number = FixedPoint.Subtract(other.FixedPoint);
            var a = new Fixed<T>(new T().WithRawInt(number));
            return a;
        }

        public Fixed<T> Divide(Fixed<T> other)
        {
            int number = FixedPoint.Divide(other.FixedPoint);
            var a = new Fixed<T>(new T().WithRawInt(number));
            return a;
        }

        public static explicit operator double(Fixed<T> number)
        {
            return number.FixedPoint.DoubleValue;
        }
    }

    public abstract class Q32
    {
        public static int IntegerLength = 32;
        public static int FractionLength;

        internal long RawIntegerValue { get; }

        internal int IntIntegerValue
        {
            get
            {
                return (int)(RawIntegerValue >> FractionLength);
            }
        }
        internal int IntDecimalValue
        {
            get
            {
                return (int)(RawIntegerValue << (IntegerLength - FractionLength)) >> (IntegerLength - FractionLength);
            }
        }
        internal double DoubleDecimalValue
        {
            get
            {
                return IntDecimalValue / (double)(1 << FractionLength);
            }
        }
        public double DoubleValue
        {
            get
            {
                return IntIntegerValue + DoubleDecimalValue;
            }
        }
        protected Q32(int number)
        {
            RawIntegerValue = number;
        }

        public int Multilpy(Q32 a)
        {
            long num = (long)this.RawIntegerValue * (long)a.RawIntegerValue;
            //Console.WriteLine(this.GetType() + "Q32" +  typeof(Q32));                    
            return (int)(num >> FractionLength);
        }

        public int Divide(Q32 a)
        {
            long num = (this.RawIntegerValue << FractionLength + 1) / a.RawIntegerValue;
            //if (a.RawIntegerValue > this.RawIntegerValue)
            //{
            //    int positiveOne = 1 << FractionLength;
            //    num += positiveOne;
            //}                                
            return (int)num;
        }

        public int Add(Q32 a)
        {
            long num = this.RawIntegerValue + a.RawIntegerValue;                           
            return (int)num;
        }

        public int Subtract(Q32 a)
        {
            long num = this.RawIntegerValue - a.RawIntegerValue;                              
            return (int)num;
        }
    }

    public class Q24_8 : Q32, IArithmetics
    {
        static Q24_8()
        {
            FractionLength = 8;
        }

        private Q24_8(int number) : base(number) { }

        public Q24_8() : base(0) { }

        public Q32 WithInt(int number)
        {
            return new Q24_8(number << FractionLength);
        }

        public Q32 WithRawInt(int number)
        {
            return new Q24_8(number);
        }

        //public static implicit operator Q24_8(int number)
        //{
        //    return new Q24_8(number);
        //}

    }

    public class Q16_16 : Q32, IArithmetics
    {
        static Q16_16()
        {
            FractionLength = 16;
        }

        public Q16_16(int number) : base(number) { }

        public Q16_16() : base(0) { }

        public Q32 WithInt(int number)
        {
            return new Q16_16(number << FractionLength);
        }

        public Q32 WithRawInt(int number)
        {
            return new Q16_16(number);
        }
    }

    public class Q8_24 : Q32, IArithmetics
    {
        static Q8_24()
        {
            FractionLength = 24;
        }

        public Q8_24(int number) : base(number) { }

        public Q8_24() : base(0) { }

        public Q32 WithInt(int number)
        {
            return new Q8_24(number << FractionLength);
        }

        public Q32 WithRawInt(int number)
        {
            return new Q8_24(number);
        }
    }
}
    