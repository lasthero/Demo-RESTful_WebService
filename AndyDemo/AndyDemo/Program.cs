using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AndyDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
            IBase obj = new ConcreteA(1, 2, 3);
            Console.Write(obj.AnB());
            obj = new ConcreteB();
            Console.Write(obj.AnB());*/

            AConcrete a = new AConcrete();
            //((ABase)a).Call();
            a.Call();
            ABase b = a as ABase;
            b.Call();
          

        }
    }

    public interface IBase
    {
        int A { get; set; }
        int B { get; set; }
        int C { get; set; }
        bool AnB();
        bool BnC();
        bool AnC();
    }

    public class ABase 
    {
        protected int a = 2;

        public virtual void Call()
        {
            Console.WriteLine("ABase.Call()");
        }

        public virtual void Call2()
        {
            Console.WriteLine("ABase.Call2()");
        }
    }

    public class AConcrete: ABase
    {
        public override void Call2()
        {
            Console.WriteLine(string.Format("AConcrete.Call2(); a={0}", base.a));
        }

        public override void Call()
        {
            Console.WriteLine(string.Format("AConcrete.Call(); a={0}", base.a));
        }
    }

    public class ConcreteA : IBase
    {
        private int a, b, c;
        public ConcreteA(int a, int b, int c)
        {
            this.a = a;
            this.b = b;
            this.c = c;
        }

        public int A { get { return this.a; } set { this.a = value; } }
        public int B { get { return this.b; } set { this.b = value; } }
        public int C { get { return this.c; } set { this.c = value; } }

        public bool AnB()
        {
            return A > B;
        }

        public bool AnC()
        {
            return A > C;
        }

        public bool BnC()
        {
            return  B > C;
        }
    }

    public class ConcreteB : IBase
    {
        private int a, b, c;
        public ConcreteB()
        {
            //this.a = this.b = this.c = 1;
        }

        public int A { get { return this.a; } set { this.a = value; } }
        public int B { get { return this.b; } set { this.b = value; } }
        public int C { get { return this.c; } set { this.c = value; } }

        public bool AnB()
        {
            return A > B;
        }

        public bool AnC()
        {
            return A > C;
        }

        public bool BnC()
        {
            return B > C;
        }
    }
}
