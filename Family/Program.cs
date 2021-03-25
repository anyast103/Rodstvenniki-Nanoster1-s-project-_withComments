using System;
using System.Collections.Generic;
using System.IO;

namespace Family
{
    class Program
    {
        static string[] info = File.ReadAllLines("family.txt"); // Считываем информацию из файла.
        static List<Person> people = new List<Person>();        // Каждый элемент этого списка будет содержать  
                                                                // ВСЕ объявленные поля класса Person.
        static void Main(string[] args)
        {
            Person.GetInfo(info, ref people);
            string first = "";
            string second = "";

            Greetings(ref first, ref second);
            Person.Finding(first, second, people);
        }
        static void Greetings(ref string first, ref string second)
        {
            Console.WriteLine("Введите данные первого человека: ");
            first = Console.ReadLine();
            Console.WriteLine("Введите данные второго человека: ");
            second = Console.ReadLine();
        }
    }
    class Person // Класс Person определяет поля, соответствующие колонкам в файле.
    {
        private int Id;
        private string BirthDate;
        private string FirstName;
        private string LastName;

        private bool Parent = false;    // Булевая переменная, которая пригодится для того, чтобы определять, является ли человек
                                        // родителем другого человека.

        public static void GetInfo(string[] info, ref List<Person> people)
        {
            int numString = 0; //Отслеживаем номера строк
            string[] columns = info[0].Split(";"); // Разделяем на колонки Id, LastName, FirstName, BirthDate.
            

            for (int i = 1; !string.IsNullOrWhiteSpace(info[i]); i++) // Цикл идёт с единицы, т.к мы собираем всю информацию, не
                // считая колонки в нулевом элементе массива (т.е первой строки списка, что есть - колонки.
            {
                string[] inform = info[i].Split(";"); // Собираем в этот массив все полученные данные из перебранного участка файла.
                // Из-за строкового метода IsNullOrWhiteSpace мы рассматриваем лишь информацию о людях, не включая их родственные связи.

                Person person = new Person(); // Вызываем класс Person, чтобы каждому полю присвоить нужное значение (будет заметно
                                              // в следующем операторе ветвления switch() case.

                for (int k = 0; k < 4; k++)     // Промежуток k<4 указан не спроста. Колонок всего 4, как и полей, соответственно, 
                                                // рассматриваем только их. Других элементов у нас нет.
                {
                    switch(columns[k]) // Мы сравниваем именно колонку с её значением, чтобы корректно определить поля класса Person.
                    {
                        case ("Id"):
                            person.Id = Convert.ToInt32(inform[k]); // Полю Id класса Person мы присваиваем, соответственно, 
                            // значение из массива, который содержит в себе все перебранные значения, начинающиеся с первого
                            // элемента массива всех строк файла. Дальше - аналогично.
                            break;
                        case ("BirthDate"):
                            person.BirthDate = inform[k];
                            break;
                        case ("FirstName"):
                            person.FirstName = inform[k];
                            break;
                        case ("LastName"):
                            person.LastName = inform[k];
                            break;
                    }
                }
                people.Add(person); // В список добавляем уже инициализированные поля класса Person.
                numString = i + 2;
            }
            for (int i = numString; info.Length > i; i++) // В этом цикле мы перебираем уже вторую часть файла. 
            {
                string[] inform = info[i].Split("="); // В этот массив строка файла делится на две части, до знака равенства, и после,
                // где, соответственно, первая часть (элемент 0) - это не тип отношений, а вторая часть (элмент 1) - соответственно,
                // тип отношений.
                string[] inform2 = inform[0].Split("<->"); // В этот массив собираются id людей.
                int id = int.Parse(inform2[0]); // Нулевой элемент массива - первый человек, первый - второй, соответственно.
                int id2 = int.Parse(inform2[1]);
                string role = inform[1]; // Эта переменная определяет тип отношений, в дальнейшем - сравнивается.
                switch (role)
                {
                    case ("parent"):
                        people[id - 1].Parent = true; // Человек является родителем, если только у него поле Parent равно True.
                        break;
                    case ("spouse"):
                        people[id - 1].Parent = true; // Если у обоих человек поле Parent равно True, то они - супруги, что логично.
                        people[id2 - 1].Parent = true;
                        break;
                }
            }
        }
        public static void Finding(string first, string second, List<Person> people)
        {
            int firstId = FoundPerson(first.Split(" "), people); // Получаем данные первого человека.
            int secondId = FoundPerson(second.Split(" "), people); // Получаем данные второго человека.
            if (people[firstId].Parent && people[secondId].Parent) // Если человек с первым id - родитель, и второй тоже - они супруги.
                Console.WriteLine("Spouse");
            else if (people[firstId].Parent && !people[secondId].Parent) // Если человек с первым id родитель, а второй нет - то первый - родитель второго.
                Console.WriteLine("Parent");
            else if (!people[firstId].Parent && !people[secondId].Parent) // Если ни тот, ни другой родителем не является - то они брат/сестра.
                Console.WriteLine("Sibling");
        }
        private static int FoundPerson(string[] first, List<Person> people)
        {
            for (int i = 0; i < people.Count; i++) // Цикл перебирает все элементы списка people (выше описано, что он содержит).
            {
                if (people[i].FirstName == first[1] && people[i].LastName == first[0]) // Ищем человека по имени, фамилии, возвращаем его
                    // данные.
                    return i;
            }
            return 0;
        }
    }
}
