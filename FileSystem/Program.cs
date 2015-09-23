using System;

namespace FileSystem
{
    class MainProgram
    {
        private enum Select { Close, Open, Add, Update, Remove, Search, List }
        static void Main(string[] args)
        {
            while (true)
            {
                Control data = new Control();
            
                System.Console.WriteLine(">> FileSystem TEST");
                System.Console.WriteLine();
                System.Console.WriteLine("1. Open / Create File");
                System.Console.WriteLine("2. Add Element");
                System.Console.WriteLine("3. Update Element");
                System.Console.WriteLine("4. Remove Element");
                System.Console.WriteLine("5. Search Element");
                System.Console.WriteLine("6. Print List");
                System.Console.WriteLine();
                System.Console.WriteLine("0. Close / Quit Program");
                System.Console.WriteLine();
                System.Console.Write("Select Number : ");
                
                // select menu
                Select? sel = null;
                try
                {
                    sel = (Select)int.Parse(System.Console.ReadLine());
                }
                catch (Exception e) { }
                System.Console.WriteLine();
                switch (sel)
                {
                    case Select.Open: data.Open(); break;
                    case Select.Add: data.Add(); break;
                    case Select.Update: data.Update();  break;
                    case Select.Remove: data.Remove(); break;
                    case Select.Search: data.Search(); break;
                    case Select.List: data.List(); break;
                    case Select.Close: return;
                    default:
                        System.Console.WriteLine(">> Plz input Number (0~6)\n");break;
                }
            }

        }
    }
}
