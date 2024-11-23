using System.Collections.Generic;
using System;
using System.IO.IsolatedStorage;
using System.Reflection.Metadata;
using System.Security.Principal;
using System.Diagnostics.CodeAnalysis;
namespace lab4
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
            string lstOut = string.Join(", ", lst);
            Console.WriteLine(lstOut);
        }
        static bool EmptyCheck(int[] lst)
        {
            if (lst == null || lst.Length ==0)
            {
                Console.WriteLine("Массив пустой");
                return false;
            }
            else
            {
                return true;
            }
        }
        static int[] CreateArray(string numberTask)
        {
            int lenList, elemList;
            string buf;
            bool isConvert;
            int[] lst = null;
            switch (numberTask)
            {
                case "1":
                    do
                    {
                        lenList = NumberInput("Введите натуральное число (длина списка)");
                    } while (lenList <= 0);
                    Random rand = new Random();
                    lst = new int[lenList];
                    for (int i = 0; i < lenList; i++)
                    {
                        lst[i] = rand.Next(-1000, 1000);
                    }
                    Console.WriteLine("Массив сформирован");
                    break;
                case "2":
                    do
                    {
                        lenList = NumberInput("Введите натуральное число (длина списка)");
                    } while (lenList <= 0);
                    lst = new int[lenList];
                    for (int i = 0; i < lenList; i++)
                    {
                        lst[i] = NumberInput("Введите целый элемент списка");
                    }
                    Console.WriteLine("Массив сформирован");
                    break;
            }
            return lst;
        }
        static int[] DeletingElements(int[] lst)
        {
            int j = 0, cnt = 0;
            for (int i = 0; i < lst.Length; i++)
            {
                if (lst[i] % 2 != 0)
                {
                    cnt++;
                }
            }
            int[] tmpLst1 = new int[cnt];
            for (int i = 0; i < lst.Length; i++)
            {
                if (lst[i] % 2 != 0)
                {
                    tmpLst1[j] = lst[i];
                    j++;
                }
            }
            Console.WriteLine("Четные элементы удалены");
            return tmpLst1;
        }
        static int[] AddingElements(int[] lst)  
        {
            int k = NumberInput("Введите натуральное k");
            int[] tmpLst2 = new int[lst.Length + k];
            int j = 0;
            for (int i = 0; i < k + lst.Length; i++)
            {
                if (i > k-1)
                {
                    tmpLst2[i] = lst[i-k];
                }
                else
                {
                    tmpLst2[i] = NumberInput("Введите целый элемент списка");
                }
            }
            Console.WriteLine("Новые элементы добавлены");
            return tmpLst2;
        }
        static int[] PermutationElements(int[] lst)
        {
            int[] tmpLst3 = new int[lst.Length];
            int cnt = 0;
            for (int i = 0; i < lst.Length; i++)
            {
                if (lst[i] % 2 == 0)
                {
                    tmpLst3[cnt] = lst[i];
                    cnt++;
                }
            }
            for (int i = 0; i < lst.Length; i++)
            {
                if (lst[i] % 2 != 0)
                {
                    tmpLst3[cnt] = lst[i];
                    cnt++;
                }
            }
            Console.WriteLine("Элементы переставлены");
            return tmpLst3;
        }
        static void CommonSearch(int[] lst)
        {
            int tmp_cnt = 0;    
            for (int i = 0; i < lst.Length; i++)
            {
                if (lst[i] % 2 == 0)
                {
                    Console.WriteLine($"Элемент - {lst[i]}, номер - {i + 1}, количество сравнений - {tmp_cnt}");
                    tmp_cnt++;
                    break;
                }
            }
            if (tmp_cnt == 0)
            {
                Console.WriteLine("В списке нет четных элементов");
            }
        }
        static int[] SortingArray(int[] lst)
        {
            int min, n_min;
            for (int i = 0; i < lst.Length - 1; i++)
            {
                min = lst[i]; n_min = i;
                for (int j = i + 1; j < lst.Length; j++)
                {

                    if (lst[j] < min)
                    {
                        min = lst[j];
                        n_min = j;
                    }
                }
                lst[n_min] = lst[i];
                lst[i] = min;
            }
            Console.WriteLine("Массив отсортирован");
            return lst;
        }
        static void BinarySearch(int[] lst, bool isSorted)
        {
            if (!isSorted)
            {
                Console.WriteLine("Массив не отсортирован. Отсортировать - 1, не сортировать и не выполнять бинарный поиск - 2");
                string buf = Console.ReadLine();
                if (buf == "1")
                {
                    lst = SortingArray(lst);
                    isSorted = true;
                }
            }
            if (isSorted)
            {
                int finalElem = NumberInput("Введите искомый элемент"), numIterations = 0, left = 0, right = lst.Length - 1, sred;
                do
                {
                    sred = (left + right) / 2;
                    if (finalElem > lst[sred])
                    {
                        left = sred + 1;
                    }
                    else
                    {
                        right = sred;
                    }
                    numIterations++;
                } while (left != right);
                if (lst[left] == finalElem)
                {
                    Console.WriteLine($"Элемент - {finalElem}, номер - {left + 1}, итераций для поиска - {numIterations}");
                }
                else
                {
                    Console.WriteLine("Элемент не найден");
                }
            }
        }
        static void Main(string[] args)
        {
            int lenList = 0, elemList;
            bool isConvert = true, isSorted = false;
            string buf, numberTask = null;
            int[] lst = null;
            while (numberTask != "0")
            {
                Console.WriteLine("Введите номер функции программы, которую хотите выполнить\n" +
                    "1. Сгенерировать список из рандомных чисел от -1000 до 1000\n" +
                    "2. Ввести список вручную\n" +
                    "3. Вывести список на экран\n" +
                    "4. Удалить все четные элементы из списка\n" +
                    "5. Добавить k элементов в начало списка\n" +
                    "6. Четные элементы переставить в начало списка, нечетные в конец\n" +
                    "7. Найти первый четный элемент перебором списка\n" +
                    "8. Отсортировать список методом простого выбора\n" +
                    "9. Найти введенный элемент бинарным поиском в отсортированном списке\n" +
                    "0. Закончить работу программы и вывести список");
                numberTask = Console.ReadLine();
                switch (numberTask)
                {
                    case "1":
                        lst = CreateArray("1");
                        break;
                    case "2":
                        lst = CreateArray("2");
                        break;
                    case "3":
                        if (EmptyCheck(lst))
                        {
                            PrintArray(lst);
                        }
                        break;
                    case "4":
                        if (EmptyCheck(lst))
                        {
                            lst = DeletingElements(lst);
                            isSorted = false;
                        }
                        break;
                    case "5":
                        if (EmptyCheck(lst))
                        {
                            lst = AddingElements(lst);
                            isSorted = false;
                        }
                        break;
                    case "6":
                        if (EmptyCheck(lst))
                        {   
                            lst = PermutationElements(lst);
                            isSorted = false;
                        }
                        break;
                    case "7":
                        if (EmptyCheck(lst))
                        {
                            CommonSearch(lst);
                        }
                        break;
                    case "8":
                        if (EmptyCheck(lst))
                        {
                            lst = SortingArray(lst);
                            isSorted = true;
                        }
                        break;
                    case "9":
                        if (EmptyCheck(lst))
                        {
                            BinarySearch(lst, isSorted);
                        }
                        break;
                }
            }
            if (EmptyCheck(lst))
            {
                PrintArray(lst);
            }
        }
    }
}
