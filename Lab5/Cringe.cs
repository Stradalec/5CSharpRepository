using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Lab5 {
  public class WordReplacer {
    private string[] targetDirectory;
    private string stringFromFile;
    private static string s_phoneNumberReplacementTemplate = "[()-]";
    private string placeholder = " ";
    private string secondIterationOfReplacePhoneNumber = " 012  345 67 89";
    private Regex phoneNumberSeeker = new Regex(s_phoneNumberReplacementTemplate);
    public FileStream incomingTXTFileStream;

    //Проверка наличия "Ошибочных" слов в файле, их замена, а также замена номера телефона
    public void Check(List <string> inputBadList, string inputGoodWord, string path) {
      targetDirectory = Directory.GetFiles(path);
      
      //Проверяем для каждого файла в директории (я не знаю, как сделать это без foreach. UPD: пробовал через Iterator, не вышло)
      foreach (string filename in targetDirectory) {
        incomingTXTFileStream = new FileStream(filename, FileMode.OpenOrCreate);
        byte[] temporary = new byte[incomingTXTFileStream.Length];

        //Получаем текст в строку из файла
        incomingTXTFileStream.Read(temporary, 0, temporary.Length);
        stringFromFile = Encoding.Default.GetString(temporary);


        //Заменяем номер телефона другим
        stringFromFile = phoneNumberSeeker.Replace(stringFromFile, placeholder);
        if (stringFromFile.Contains(secondIterationOfReplacePhoneNumber)) {
          stringFromFile = stringFromFile.Replace(secondIterationOfReplacePhoneNumber, "+380" + secondIterationOfReplacePhoneNumber.Remove(1,1));
          Console.WriteLine("Number fixed");
        }

        // Непосредственно заменяем "ошибочные" слова из списка
        for (int checkIndex = 0; checkIndex < inputBadList.Count; ++checkIndex) {         
          if (stringFromFile.Contains(inputBadList[checkIndex])) {
            stringFromFile = stringFromFile.Replace(inputBadList[checkIndex], inputGoodWord);
            Console.WriteLine("Fixed bad inputBadWord in " + filename);
          } else {
            Console.WriteLine("No bad words in " + filename);
          }
        }

        //Закрываем файл
        incomingTXTFileStream.Close();

        //Чтобы снова открыть его, записать отформатированные данные и закрыть
        StreamWriter saveTextInFile = new StreamWriter(filename);
        saveTextInFile.Write(stringFromFile);
        saveTextInFile.Flush();
        saveTextInFile.Close();
      }
      
      
    }
  }
  internal class Program {
    static void Main(string[] args) {
      string path = "C:\\Lab4\\Lab";
      WordReplacer replacerOfBadWords = new WordReplacer();
      List<string> listOfMistakenWords = new List<string>();
      bool isRunning = true;
      bool isNeedToFillList = true;
      string inputBadWord;
      string inputGoodWord;
      char userChoice;

      //Заполняем лист с "ошибочными" словами
      while(isNeedToFillList) {
        Console.WriteLine("Add bad inputBadWord in list");
        inputBadWord = Console.ReadLine();

        listOfMistakenWords.Add(inputBadWord);

        Console.WriteLine("Enough? y/n ");
        userChoice = Convert.ToChar(Console.ReadLine());

        if (userChoice == 'y') {
          isNeedToFillList = false;
        } else if (userChoice == 'n') {
          continue;
        }
      }

      //Добавляем правильный вариант
      Console.WriteLine("Add only one good inputBadWord here: ");
      inputGoodWord = Console.ReadLine();

      //Основной цикл работы программы.
      while (isRunning) {
        Console.WriteLine("Add path");
        path = Console.ReadLine();

        replacerOfBadWords.Check(listOfMistakenWords, inputGoodWord, path);

        Console.WriteLine("Enough? y/n ");
        userChoice = Convert.ToChar(Console.ReadLine());

        if (userChoice == 'y') {
          isRunning = false;
        } else if (userChoice == 'n') {
          continue;
        }
      }
      
      Console.ReadKey();
    }
  }
}
