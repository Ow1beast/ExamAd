using System;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using static System.Console;
using System.Linq;
using System.Data.Entity;

namespace Exam
{
    class Program
    {
        static void Main(string[] args)
        {
            Menu();
            ReadLine();
        }
        public static void Menu()
        {
            WriteLine("1. Регистрация");
            WriteLine("2. Восстановление");

            var key = ReadLine();

            int menuNumber;
            Int32.TryParse(key, out menuNumber);

            switch (menuNumber)
            {
                case 1:
                    Clear();
                    WriteLine("Введите номер телефона (+77ХХ ХХХ ХХ ХХ): +77");
                    var number = ReadLine();
                    var user = new User { PhoneNumber = "+77" + number, IsConfirmed = false };
                    using (ExamContext context = new ExamContext())
                    {
                        context.Users.Add(user);
                        context.SaveChanges();

                        Random random = new Random();
                        int temp = random.Next(1000);
                        try
                        {
                            PhoneVerification(user.PhoneNumber, temp);
                        }
                        catch
                        {
                            Write("Неправильный номер");
                            break;
                        }
                        Write("Verification Number: ");
                        string inputString = ReadLine();
                        int inputCode;

                        Int32.TryParse(inputString, out inputCode);

                        if (inputCode == temp)
                        {
                            var userChange = context.Users.Where(u => u.PhoneNumber == user.PhoneNumber).FirstOrDefault();

                            context.Entry(userChange).State = EntityState.Modified;
                            userChange.IsConfirmed = true;
                            context.SaveChanges();

                            Clear();
                            WriteLine("Регистрация пройдена успешно! \n");
                            Menu();
                        }
                        else
                        {
                            WriteLine("Неверный код подтверждения!");
                            Menu();
                        }
                    }
                    break;
                case 2:
                    Random random2 = new Random();
                    int temp2 = random2.Next(1000);
                    Write("Введите номер телефона (+77ХХ ХХХ ХХ ХХ): +77");
                    var number2 = ReadLine();
                    var user2 = new User { PhoneNumber = "+77" + number2 };

                    try
                    {
                        PhoneVerification(user2.PhoneNumber, temp2);
                    }
                    catch
                    {
                        WriteLine("Неправильный номер");
                        break;
                    }

                    Write("Verification Number: ");
                    string inputString2 = ReadLine();
                    int inputCode2;

                    Int32.TryParse(inputString2, out inputCode2);

                    if (inputCode2 == temp2)
                    {
                        using (ExamContext context = new ExamContext())
                        {
                            var userChange = context.Users.Where(u => u.PhoneNumber == user2.PhoneNumber).FirstOrDefault();
                            if (userChange.Id > 0)
                            {
                                if (userChange.IsConfirmed == false)
                                {
                                    context.Entry(userChange).State = EntityState.Modified;
                                    userChange.IsConfirmed = true;
                                    context.SaveChanges();
                                }
                            }
                            else
                            {
                                Write("Неправильный номер");
                            }
                            Clear();
                            WriteLine("Добро пожаловать! \n");
                            Menu();
                        }
                    }
                    else
                    {
                        WriteLine("Неверный код подтверждения!");
                        Menu();
                    }
                    break;
            }
        }

        public static void PhoneVerification(string number, int code)
        {
            string stringCode = code.ToString();
            const string accountSid = "AC077c00839970a717c5e527238f9b8cd8";
            const string authToken = "1a3c7809c9c368969275841f4f46ebe1";

            TwilioClient.Init(accountSid, authToken);

            var message = MessageResource.Create(
                body: $"Код подтверждения: {stringCode}",
                from: new Twilio.Types.PhoneNumber("+18125779858"),
                to: new Twilio.Types.PhoneNumber(number)
            );

            WriteLine(message.Sid);
        }
    }
}
