using System;
using System.IO;

namespace _9pLab3._1
{

    class OutsideTheSetException : Exception
    {
        public OutsideTheSetException(string str) : base(str) {

        }
    }


    abstract class Set
    {
        protected int maxSize;

        abstract public void Insert(int x);
        abstract public void Remove(int x);
        abstract public bool Find(int x);

        public void MakeSet(string str) {
            string[] mas = str.Split(' ', ',');
            for (int i = 0; i < mas.Length; i++) {
                int x = Convert.ToInt32(mas[i]);
                if (x <= 0 || x > maxSize) {
                    throw new OutsideTheSetException($"Не удалось вставить элемент со значением {x}");
                }
                else {
                    Insert(x);
                }
            }
        }

        public void MakeSet(int[] mas) {
            for (int i = 0; i < mas.Length; i++) {
                if (mas[i] <= 0 || mas[i] > maxSize) {
                    throw new OutsideTheSetException($"Не удалось вставить элемент со значением {mas[i]}");
                }
                else {
                    Insert(mas[i]);
                }
            }
        }

        public void Print() {
            bool flag = false;
            for (int i = 1; i <= maxSize; i++) {
                if (Find(i)) {
                    Console.Write($"{i} ");
                    flag = true;
                }
            }
            if (!flag) Console.Write("Пустое множество");
            Console.WriteLine();
        }

    }

    class SimpleSet : Set
    {
        private bool[] mas;

        public SimpleSet(int maxSize) {
            this.maxSize = maxSize;
            mas = new bool[maxSize];
        }

        override public void Insert(int x) {
            if (x <= 0 || x > maxSize) {
                throw new OutsideTheSetException($"Не удалось вставить элемент со значением {x}");
            }
            else {
                mas[x - 1] = true;
            }
        }

        override public void Remove(int x) {
            mas[x - 1] = false;
        }

        override public bool Find(int x) {
            if (mas[x - 1] == true) {
                return true;
            }
            else {
                return false;
            }
        }

        public static SimpleSet operator +(SimpleSet first, SimpleSet second) {
            int n = Math.Min(first.maxSize, second.maxSize);
            if (n < first.maxSize || first.maxSize == second.maxSize) {
                return Union(first, second);
            }
            else {
                return Union(second, first);
            }

            SimpleSet Union(SimpleSet max, SimpleSet min) {
                SimpleSet newSet = new SimpleSet(max.maxSize);
                for (int i = 1; i <= max.maxSize; i++) {
                    if (max.Find(i)) {
                        newSet.Insert(i);
                    }
                }

                for (int i = 1; i <= n; i++) {
                    if (newSet.Find(i) || min.Find(i))
                        newSet.Insert(i);
                }

                return newSet;
            }
        }

        public static SimpleSet operator *(SimpleSet first, SimpleSet second) {
            int n = Math.Min(first.maxSize, second.maxSize);
            if (n < first.maxSize || first.maxSize == second.maxSize) {
                return Intersection(first, second);
            }
            else {
                return Intersection(second, first);
            }

            SimpleSet Intersection(SimpleSet min, SimpleSet max) {
                SimpleSet newSet = new SimpleSet(n);
                for (int i = 1; i <= n; i++) {
                    if (min.Find(i)) {
                        newSet.Insert(i);
                    }
                }

                for (int i = 1; i <= n; i++) {
                    if (max.Find(i) && newSet.Find(i))
                        newSet.Insert(i);
                    else {
                        newSet.Remove(i);
                    }
                }

                return newSet;
            }
        }
    }

    class BitSet : Set
    {
        private const int INT = 32;
        private int[] mas;

        public BitSet(int maxSize) {
            this.maxSize = maxSize;
            mas = new int[maxSize / INT + 1];
        }

        override public void Insert(int x) {
            if (x <= 0 || x > maxSize) {
                throw new OutsideTheSetException($"Не удалось вставить элемент со значением {x}");
            }
            else {
                int i = x / 32;
                mas[i] = mas[i] | (1 << x % INT - 1);
            }
        }

        override public void Remove(int x) {
            int i = x / 32;
            mas[i] = mas[i] & ~(1 << x % INT - 1);
        }

        override public bool Find(int x) {
            int i = x / 32;
            return (mas[i] & (1 << x % INT - 1)) != 0;
        }

        public static BitSet operator +(BitSet first, BitSet second) {
            int n = Math.Min(first.maxSize, second.maxSize);
            if (n < first.maxSize || first.maxSize == second.maxSize) {
                return Union(first, second);
            }
            else {
                return Union(second, first);
            }

            BitSet Union(BitSet max, BitSet min) {
                BitSet newSet = new BitSet(max.maxSize);
                for (int i = 1; i <= max.maxSize; i++) {
                    if (max.Find(i)) {
                        newSet.Insert(i);
                    }
                }

                for (int i = 1; i <= n; i++) {
                    if (newSet.Find(i) || min.Find(i))
                        newSet.Insert(i);
                }

                return newSet;
            }
        }

        public static BitSet operator *(BitSet first, BitSet second) {
            int n = Math.Min(first.maxSize, second.maxSize);
            if (n < first.maxSize || first.maxSize == second.maxSize) {
                return Intersection(first, second);
            }
            else {
                return Intersection(second, first);
            }

            BitSet Intersection(BitSet min, BitSet max) {
                BitSet newSet = new BitSet(n);
                for (int i = 1; i <= n; i++) {
                    if (min.Find(i)) {
                        newSet.Insert(i);
                    }
                }

                for (int i = 1; i <= n; i++) {
                    if (max.Find(i) && newSet.Find(i))
                        newSet.Insert(i);
                    else {
                        newSet.Remove(i);
                    }
                }

                return newSet;
            }
        }
    }

    class MultiSet : Set
    {
        private int[] mas;

        public MultiSet(int maxSize) {
            this.maxSize = maxSize;
            mas = new int[maxSize];
        }

        override public void Insert(int x) {
            if (x <= 0 || x > maxSize) {
                throw new OutsideTheSetException($"Не удалось вставить элемент со значением {x}");
            }
            else {
                mas[x - 1]++;
            }
        }

        override public void Remove(int x) {
            mas[x - 1]--;
        }

        override public bool Find(int x) {
            if (mas[x - 1] > 0) {
                return true;
            }
            else {
                return false;
            }
        }
    }

    internal class Program
    {
        static void Main(string[] args) {
            Console.Write("Какого типа множество? 1 - логический, 2 - битовый, 3 - мультимножество: ");
            int choice = Convert.ToInt32(Console.ReadLine());
            Console.Write("Введите размерность множества: ");
            int size = Convert.ToInt32(Console.ReadLine());
            Set set;
            switch (choice) {
                case 1:
                set = new SimpleSet(size);
                break;

                case 2:
                set = new BitSet(size);
                break;

                case 3:
                set = new MultiSet(size);
                break;

                default:
                Console.WriteLine("error");
                return;
            }

            Console.Write("Как заполнить множество? 1 - с клавиатуры, 2 - из файла: ");
            choice = Convert.ToInt32(Console.ReadLine());
            switch (choice) {
                case 1:
                Console.Write("Введите множество через пробел или запятую: ");
                string str = Console.ReadLine();
                try {
                    set.MakeSet(str);
                }
                catch (OutsideTheSetException ex) {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Прекращение работы.....");
                    return;
                }
                break;

                case 2:
                string[] drop = File.ReadAllLines(@"YourWay"); //по одному элементу в строке
                int[] mas = new int[drop.Length];
                for (int i = 0; i < mas.Length; i++)
                    mas[i] = Convert.ToInt32(drop[i]);
                try {
                    set.MakeSet(mas);
                }
                catch (OutsideTheSetException ex) {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Прекращение работы.....");
                    return;
                }
                break;
            }

            while (true) {
                Console.Write("Полученное Множество: ");
                set.Print();
                Console.Write("Выберите операцию над множеством 1 - добавить, 2 - исключить, 3 - проверка на наличие, 4 - завершение программы: ");
                choice = Convert.ToInt32(Console.ReadLine());
                switch (choice) {
                    case 1:
                    Console.Write("Введите число: ");
                    try {
                        set.Insert(Convert.ToInt32(Console.ReadLine()));
                    }
                    catch (OutsideTheSetException ex) {
                        Console.WriteLine(ex.Message);
                    }
                    break;

                    case 2:
                    Console.Write("Введите число: ");
                    set.Remove(Convert.ToInt32(Console.ReadLine()));
                    break;

                    case 3:
                    Console.Write("Введите число: ");
                    Console.WriteLine(set.Find(Convert.ToInt32(Console.ReadLine())));
                    break;

                    case 4:
                    Console.WriteLine("Прекращение работы.....");
                    return;

                    default:
                    Console.WriteLine("error");
                    return;
                }
            }
            //Console.Write("Ввдите множество А: ");
            //string a = Console.ReadLine();
            //Console.Write("Ввдите множество B: ");
            //string b = Console.ReadLine();
            //SimpleSet A = new SimpleSet(500);
            //SimpleSet B = new SimpleSet(1000);
            //A.MakeSet(a);
            //B.MakeSet(b);
            //SimpleSet C = A * B;
            //SimpleSet D = A + B;
            //Console.Write("A * B = C: ");
            //C.Print();
            //Console.Write("A + B = D: ");
            //D.Print();
        }
    }
}