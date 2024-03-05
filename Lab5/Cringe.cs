using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab5 {
  public class MyDictionary {
    public StreamReader ReaderOfFile;
    string[] directory;
    string stringFromFile;
    static string replacePhoneNumber = "[()-]";
    string target = " ";
    string fixedNumber = " 012  345 67 89";
    Regex phoneNumberSeeker = new Regex(replacePhoneNumber);
    public FileStream TextFileStream;

    //Проверка наличия "Ошибочных" слов в файле, их замена, а также замена номера телефона
    public void Check(List <string> inputBadList, string goodWord, string path) {
      directory = Directory.GetFiles(path);
      
      //Проверяем для каждого файла в директории (я не знаю, как сделать это без foreach)
      foreach (string filename in directory) {
        TextFileStream = new FileStream(filename, FileMode.OpenOrCreate);
        byte[] temporary = new byte[TextFileStream.Length];

        //Получаем текст в строку из файла
        TextFileStream.Read(temporary, 0, temporary.Length);
        stringFromFile = Encoding.Default.GetString(temporary);


        //Заменяем номер телефона другим
        stringFromFile = phoneNumberSeeker.Replace(stringFromFile, target);
        if (stringFromFile.Contains(fixedNumber)) {
          stringFromFile = stringFromFile.Replace(fixedNumber, "+380" + fixedNumber.Remove(1,1));
          Console.WriteLine("Number fixed");
        }

        // Непосредственно заменяем "ошибочные" слова из списка
        for (int checkIndex = 0; checkIndex < inputBadList.Count; ++checkIndex) {         
          if (stringFromFile.Contains(inputBadList[checkIndex])) {
            stringFromFile = stringFromFile.Replace(inputBadList[checkIndex], goodWord);
          } else {
            Console.WriteLine("No bad words in " + filename);
          }
        }

        //Закрываем файл
        TextFileStream.Close();

        //Чтобы снова открыть его, записать отформатированные данные и закрыть
        StreamWriter newText = new StreamWriter(filename);
        newText.Write(stringFromFile);
        newText.Flush();
        newText.Close();
      }
      
      
    }
  }
  internal class Program {
    static void Main(string[] args) {
      string path = "C:\\Lab4\\Lab";
      MyDictionary workspace = new MyDictionary();
      List<string> listOfMistakenWords = new List<string>();
      bool isRunning = true;
      bool isNeedToFillList = true;
      string word;
      string goodWord;
      char choice;

      //Заполняем лист с "ошибочными" словами
      while(isNeedToFillList) {
        Console.WriteLine("Add bad word in list");
        word = Console.ReadLine();

        listOfMistakenWords.Add(word);

        Console.WriteLine("Enough? y/n ");
        choice = Convert.ToChar(Console.ReadLine());

        if (choice == 'y') {
          isNeedToFillList = false;
        } else if (choice == 'n') {
          continue;
        }
      }

      //Добавляем правильный вариант
      Console.WriteLine("Add only one good word here: ");
      goodWord = Console.ReadLine();

      //Основной цикл работы программы.
      while (isRunning) {
        Console.WriteLine("Add path");
        path = Console.ReadLine();

        workspace.Check(listOfMistakenWords, goodWord, path);

        Console.WriteLine("Enough? y/n ");
        choice = Convert.ToChar(Console.ReadLine());

        if (choice == 'y') {
          isRunning = false;
        } else if (choice == 'n') {
          continue;
        }
      }
      
      Console.ReadKey();
    }
  }
}
