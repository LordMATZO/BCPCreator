using System;
using System.Text;
using System.IO;

namespace BCPCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            string result = @"@echo OFF
                        :set ServerName=
                        set /p ServerName=^> Input server IP: %ServerName%
                        :set DBName=
                        set /p DBName=^> Input database name: %DBName%
                        :set DBLogin=
                        set  /p DBLogin=^> Input login: %DBName%
                        :set UserPass=
                        set /p UserPass=^> Input password for %DBLogin%: %UserPass%
                         ";
            Console.Write("Input what do you want to do, upload(in) or unload(out): ");
            string outORin = Console.ReadLine();
            if (outORin != "out" && outORin != "in")
            {
                Console.WriteLine("Wrong operation");
                Console.ReadKey();
                return;
            }
            if (!File.Exists(".\\tableList.txt"))
            {
                Console.WriteLine("File tableList.txt doesn't exist");
                Console.ReadKey();
                return;
            }
            if (!File.Exists(".\\resultBCP.bat"))
            {
                File.Create(".\\resultBCP.bat").Close();

            }
            else
            {
                File.Delete(".\\resultBCP.bat");
                File.Create(".\\resultBCP.bat").Close();

            }
            var tableListFile = new StreamReader(".\\tableList.txt", Encoding.ASCII);
            string tableName;
            result += "\n";
            using (var resBCP = new StreamWriter(".\\resultBCP.bat"))
            {
                while ((tableName = tableListFile.ReadLine()) != null)
                {
                    result += $"bcp {tableName} {outORin} {tableName}.bcp -S %ServerName% -d %DBName% -b 5000 -h \"TABLOCK\" -m 1 -c -e {tableName}_error.log -U %DBLogin% -P %UserPass% -o {tableName}.log\n";

                }
                result += @"echo ^>Success
                        pause";
                resBCP.WriteLine(result, Encoding.ASCII);
                resBCP.Close();
            }

            tableListFile.Close();
            Console.WriteLine("Finished");
            Console.ReadKey();
        }
    }
}
