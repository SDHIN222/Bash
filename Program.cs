using System;

public class Program
{
    public static void Main()
    {
        // Задание 1
        Console.WriteLine("Hello, world");

        // Задание 2
        try 
        {
            string num = Console.ReadLine();
            int numbers = Convert.ToInt32(num);

            if (numbers % 2 == 0)        
            {            
                Console.WriteLine($"Число {numbers} чётное");
            }        
            else       
            {            
                Console.WriteLine($"Число {numbers} нечётное"); 
            }       
        }
        catch (Exception ex)        
        {            
            Console.WriteLine("Что-то поломалось, возможно вы ввели буквы или не целое число");            
            Console.WriteLine(ex.Message);        
        }          

        {
            Console.WriteLine("Нажмите Enter для вывода таблицы умножения на 5");
            string quit = Console.ReadLine();
        }
        //усложнил себе задачу просто хотелось обработать всё
        // Задание 3
        int[] numbersmassiv = {1, 2, 3, 4, 5, 6, 7, 8, 9, 10};
        int number = 5;
        for (int i = 1; i <= numbersmassiv.Length; i++)
        {
            Console.WriteLine(number * i);
        }

        // Задание 4 
        void SayHello(string name)
        {
            Console.WriteLine($"Привет, {name}!");
        }
        Console.Write("Введите ваше имя: ");
        string userName = Console.ReadLine();
        SayHello(userName);

        // Задание 5
        Person person = new Person("Максим", 15);
        person.PrintInfo();
    }
}

public class Person
{    
    public string name;    
    public int age;    
    
    public Person(string name, int age)    
    {        
        this.name = name;       
        this.age = age;
    } 
    
    public void PrintInfo()    
    {        
        Console.WriteLine("Имя: " + name);    
        Console.WriteLine("Возраст: " + age);
    }
 
}
            