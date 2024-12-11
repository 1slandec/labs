using System.Collections.Generic;
using System;
using System.IO.IsolatedStorage;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Diagnostics.CodeAnalysis;
namespace lab56
{
    internal class Program
    {
        static int NumberInput(string msg)
        {
            bool isConvert;
            string buf;
            int finalNumber;
            do
            {
                Console.WriteLine(msg);
                buf = Console.ReadLine();
                isConvert = int.TryParse(buf, out finalNumber);
                if (!isConvert)
                {
                    Console.WriteLine("Вы ввели неподходящее значение");
                }
            } while (!isConvert);
            return finalNumber;
        }
        static void PrintArray(int[] lst)
        {
            for (int i = 0; i < lst.Length; i++)
            {
                Console.Write($"{lst[i],4}");
            }
            Console.WriteLine();
        }
        static void PrintArray(int[,] lst)
        {
            if (EmptyCheck(lst))
            {
                for (int i = 0; i < lst.GetLength(0); i++)
                {
                    for (int j = 0; j < lst.GetLength(1); j++)
                    {
                        Console.Write($"{lst[i, j],4}");
                    }
                    Console.WriteLine();
                }
            }
            
        }
        static void PrintArray(int[][] lst)
        {
            if (EmptyCheck(lst))
            {
                for (int i = 0; i < lst.GetLength(0); i++)
                {
                    PrintArray(lst[i]);
                }
            }

        }
        static bool EmptyCheck(string str)
        {
            if (str == null || str.Length==0)
            {
                Console.WriteLine("Строка пустая");
                return false;
            }
            return true;
        }
        static bool EmptyCheck(int[,] lst)
        {
            if (lst == null || lst.Length == 0)
            {
                Console.WriteLine("Массив пустой");
                return false;
            }
            else
            {
                return true;
            }
        }
        static bool EmptyCheck(int[][] lst)
        {
            if (lst == null || lst.Length == 0)
            {
                Console.WriteLine("Массив пустой");
                return false;
            }
            else
            {
                return true;
            }
        }
        static int[,] CreateArray(int taskNumber, int stringsNumber, int columnsNumber)
        {
            int[,] lst = new int[stringsNumber, columnsNumber];
            switch (taskNumber)
            {
                case 1:
                    
                    Random rand = new Random();
                    for (int i = 0; i < stringsNumber; i++)
                    {
                        for (int j = 0; j < columnsNumber; j++)
                        {
                            lst[i,j] = rand.Next(-99, 99);
                        }
                    }
                    Console.WriteLine("Массив сформирован");
                    break;
                case 2:
                    for (int i = 0; i < stringsNumber; i++)
                    {
                        for (int j = 0; j < columnsNumber; j++)
                        {
                            lst[i, j] = NumberInput("Введите целый элемент списка");
                        }
                        Console.WriteLine("Следующая строка");
                    }
                    Console.WriteLine("Массив сформирован");
                    break;
            }
            return lst;
        }
        static int[][] CreateArray(int taskNumber)
        {
            int stringsNumber, columnsNumber;
            Random rand = new Random();
            do
            {
                stringsNumber = NumberInput("Введите количество строк");
            } while (stringsNumber < 0);
            int[][] lst = new int[stringsNumber][];
            switch (taskNumber)
            {
                case 1:
                    {
                        for (int i = 0; i<stringsNumber; i++)
                        {
                            columnsNumber = rand.Next(1,5);
                            lst[i] = new int[columnsNumber];
                            for (int j = 0;j<columnsNumber; j++)
                            {
                                lst[i][j] = rand.Next(-99, 99);
                            }
                        }
                        break;
                    }
                case 2:
                    {
                        for (int i = 0; i < stringsNumber; i++)
                        {
                            do
                            {
                                columnsNumber = NumberInput("Введите количество элементов в строке");
                            } while (columnsNumber < 0);
                            lst[i] = new int[columnsNumber];
                            for (int j = 0; j<columnsNumber; j++)
                            {
                                lst[i][j] = NumberInput("Введите элемент строки");
                            }
                        }
                        break;
                    }
            }
            return lst;
        }
        static int[,] AddingRow(int[,] lst, int stringsNumber, int columnsNumber)
        {
            int[] newRow = new int[columnsNumber];
            int taskNumber1;
            do
            {
                taskNumber1 = NumberInput($"1. Ввести строку с длиной {columnsNumber} вручную\n" +
                    "2. Сгенерировать строку из рандомных чисел от -100 до 100\n" +
                    "Введите номер функции");
                if (taskNumber1 < 1 || taskNumber1 > 2)
                {
                    Console.WriteLine("Вы ввели неподходящее значение");
                }
            } while (taskNumber1 < 1 || taskNumber1 > 2);
            switch (taskNumber1)
            {
                case 1:
                    {
                        for (int i = 0; i < columnsNumber; i++)
                        {
                            newRow[i] = NumberInput("Введите целый элемент списка");
                        }
                        break;
                    }
                case 2:
                    {
                        Random rand = new Random();
                        for (int i = 0; i < columnsNumber; i++)
                        {
                            newRow[i] = rand.Next(-99, 99);
                        }
                        break;
                    }
            }
            int[,] newLst = new int[stringsNumber + 1, columnsNumber];
            for (int j = 0; j< columnsNumber; j++)
            {
                newLst[0, j] = newRow[j];
            }
            for (int i = 0; i<stringsNumber; i++)
            {
                for (int j = 0; j< columnsNumber; j++)
                {
                    newLst[i+1,j] = lst[i,j];
                }
            }
            PrintArray(newLst);
            return newLst;
        }
        static int[][] DeletingRows(int[][] lst)
        {
            int k, n, cnt=0;
            do
            {
                k = NumberInput("Введите К (количество удаляемых строк)");
                if (k < 1 || k > lst.Length)
                {
                    Console.WriteLine("Вы ввели неподходящее значение");
                }
            } while (k<1 || k>lst.Length);
            do
            {
                n = NumberInput("Введите N (номер первой удаляемой строки)");
                if (n < 1 || n-1 > lst.Length - k)
                {
                    Console.WriteLine("Вы ввели неподходящее значение");
                }
            } while (n<1 || n-1>lst.Length-k);
            int[][] newLst = new int[lst.Length-k][];
            if (k == lst.Length)
            {
                Console.WriteLine("Остался пустой массив");
                return null;
            }
            for (int i = 0; i < lst.Length; i++)
            {
                if (i < n-1 || i >= n + k - 1)
                {
                    newLst[cnt] = new int[lst[i].Length];
                    for (int j = 0; j < lst[i].Length; j++)
                    {
                        newLst[cnt][j] = lst[i][j];
                    }
                    cnt++;
                }
            }
            PrintArray(newLst);
            return newLst;
        }
        static string ChangingWords(string str)

        {
            str = str.Trim();
            string[] lst = str.Split();
            string firstWord = lst[0], lastWord = lst[lst.Length-1];
            string tmpLetter = Char.ToString(lastWord[0]);
            string lastChar = char.ToString(lastWord[lastWord.Length - 1]);
            string firstSign = null;
            if (char.ToString(firstWord[firstWord.Length-1])==","
                || char.ToString(firstWord[firstWord.Length - 1]) == ";"
                || char.ToString(firstWord[firstWord.Length - 1]) == ":")
            {
                firstSign = char.ToString(firstWord[firstWord.Length - 1]);
                firstWord = firstWord.Substring(0, firstWord.Length - 1);
            }
            lastWord = string.Concat(tmpLetter.ToUpper(), lastWord.Substring(1, lastWord.Length-2), firstSign);
            tmpLetter = char.ToString(firstWord[0]);
            firstWord = string.Concat(tmpLetter.ToLower(), firstWord.Substring(1, firstWord.Length-1), lastChar);
            lst[0] = lastWord;
            lst[lst.Length-1] = firstWord;
            string lstOut = string.Join(" ", lst);
            if (lstOut.Substring(0,lstOut.Length-1) == str)
            {
                Console.WriteLine("Строка не изменена. Скорее всего, на вход была подана строка, несоответствующая заданию.");
                return lstOut;
            }
            Console.WriteLine("Строка изменена");
            Console.WriteLine(lstOut);
            return lstOut;
        }
        static string RandomString()
        {
            string[] lst = ["В лесу родилась елочка.",
                "В лесу она росла.",
                "Зимой и летом стройная, зеленая была!",
                "Что, уже весна?",
                "Сколько я спал?"];
            int tmp;
            do
            {
                tmp = NumberInput("Выберите одну из следующих строк:\n" +
                "1. В лесу родилась елочка.\n" +
                "2. В лесу она росла.\n" +
                "3. Зимой и летом стройная, зеленая была!\n" +
                "4. Что, уже весна?\n" +
                "5. Сколько я спал?") - 1;
            } while (tmp < 0 || tmp > 4);
            return lst[tmp];
        }
        static void Main(string[] args)
        {
            string str = null;
            int[,] lst = null;
            int[][] lst2 = null;
            int stringsNumber = 0, columnsNumber = 0, taskNumber1 = 1, taskNumber2 = 1;
            while (taskNumber1 != 0) {
                do
                {
                    Console.WriteLine("1. Работа с двумерными массивами\n" +
                        "2. Работа с рваными массивами\n" +
                        "3. Работа со строками\n" +
                        "0. Закончить работу программы");
                    taskNumber1 = NumberInput("Введите номер функции");
                    if (taskNumber1 < 0 || taskNumber1 > 3)
                    {
                        Console.WriteLine("Вы ввели неподходящее значение");
                    }
                } while (taskNumber1 <0 || taskNumber1 > 3);
                switch (taskNumber1)
                { 
                    case 1:
                        {
                            while (taskNumber2 != 0)
                            {
                                do
                                {
                                    Console.WriteLine("1. Сгенерировать двумерный массив из рандомных чисел от -100 до 100\n" +
                                            "2. Ввести двумерный массив вручную\n" +
                                            $"3. Добавить строку с длиной {columnsNumber} в начало матрицы\n" +
                                            "0. Закончить работу с двумерными массивами");
                                    taskNumber2 = NumberInput("Введите номер функции");
                                    if (taskNumber2 < 0 || taskNumber2 > 3)
                                    {
                                        Console.WriteLine("Вы ввели неподходящее значение");
                                    }
                                } while (taskNumber2 <0 || taskNumber2 > 3);
                                switch (taskNumber2)
                                {
                                    case 1:
                                        {
                                            do
                                            {
                                                stringsNumber = NumberInput("Введите натуральное число (количество строк)");
                                            } while (stringsNumber <= 0);
                                            do
                                            {
                                                columnsNumber = NumberInput("Введите натуральное число (количество столбцов)");
                                            } while (columnsNumber <= 0);
                                            lst = CreateArray(taskNumber2, stringsNumber, columnsNumber);
                                            PrintArray(lst);
                                            break;
                                        }
                                    case 2:
                                        {
                                            do
                                            {
                                                stringsNumber = NumberInput("Введите натуральное число (количество строк)");
                                            } while (stringsNumber <= 0);
                                            do
                                            {
                                                columnsNumber = NumberInput("Введите натуральное число (количество столбцов)");
                                            } while (columnsNumber <= 0);
                                            lst = CreateArray(taskNumber2, stringsNumber, columnsNumber);
                                            PrintArray(lst);
                                            break;
                                        }
                                    case 3:
                                        {
                                            if (EmptyCheck(lst))
                                            {
                                                lst = AddingRow(lst, stringsNumber, columnsNumber);
                                                stringsNumber++;
                                            }
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case 2: 
                        {
                            while (taskNumber2 != 0)
                            {
                                do
                                {
                                    Console.WriteLine("1. Сгенерировать рваный массив из рандомных чисел от -100 до 100\n" +
                                            "2. Ввести рваный массив вручную\n" +
                                            "3. Удалить K строк, начиная с номера N\n" +
                                            "0. Закончить работу с рваными массивами");
                                    taskNumber2 = NumberInput("Введите номер функции");
                                    if (taskNumber2 < 0 || taskNumber2 > 3)
                                    {
                                        Console.WriteLine("Вы ввели неподходящее значение");
                                    }
                                } while (taskNumber2 < 0 || taskNumber2 >3);
                                switch (taskNumber2)
                                {
                                    case 1:
                                        {
                                            lst2 = CreateArray(taskNumber2);
                                            PrintArray(lst2);
                                            break;
                                        }
                                    case 2:
                                        {
                                            lst2 = CreateArray(taskNumber2);
                                            PrintArray(lst2);
                                            break;
                                        }
                                    case 3:
                                        {
                                            if (EmptyCheck(lst2))
                                            {
                                                lst2 = DeletingRows(lst2);
                                            }
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case 3:
                        {
                            while (taskNumber2 != 0)
                            {
                                do
                                {
                                    Console.WriteLine("1. Ввести строку вручную\n" +
                                            "2. Выбрать строку из готового набора\n" +
                                            "3. Поменять местами первое и последнее слова\n" +
                                            "0. Закончить работу со строками");
                                    taskNumber2 = NumberInput("Введите номер функции");
                                    if (taskNumber2 < 0 || taskNumber2 > 3)
                                    {
                                        Console.WriteLine("Вы ввели неподходящее значение");
                                    }
                                } while (taskNumber2 < 0 || taskNumber2 >3);
                                switch (taskNumber2)
                                {
                                    case 1:
                                        {
                                            Console.WriteLine("Введите строку");
                                            str = Console.ReadLine();
                                            break;
                                        }
                                    case 2:
                                        {
                                            str = RandomString();
                                            break;
                                        }
                                    case 3:
                                        {
                                            if (EmptyCheck(str))
                                            {
                                                str = ChangingWords(str);
                                            }
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                }
                taskNumber2 = 1;
            }
        }
    }
}
