using System;
using System.Linq;
using System.Collections.Generic;

namespace _2048
{
    class Program
    {
        public static int[,] gameArray =  new int[4,4];
        public static Random rand = new Random();
        public static List<int> GetColumn(int columnNumber)
        {
            List<int> val = Enumerable.Range(0, gameArray.GetLength(0))
                    .Select(x => gameArray[x, columnNumber])
                    .ToList();
            val.RemoveAll(f => f == 0);
            return val;
        }

        public static List<int> GetRow(int rowNumber)
        {
            List<int> val = Enumerable.Range(0, gameArray.GetLength(1))
                    .Select(x => gameArray[rowNumber, x])
                    .ToList();
            val.RemoveAll(f => f == 0);
            return val;
        }
        public static int CountEmptyCell()
        {
            int count = 0;
            for(int i = 0; i < 4; i++)
            {
                for(int j = 0; j < 4; j++)
                {
                    if(gameArray[i,j] == 0)
                        count++;
                }
            }
            return count;
        }
        public static void DisplayGameArray()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------");
            for (int i = 0; i < 4; i++)
            {
                for (int l = 0; l < 4; l++)
                {
                    if(gameArray[i,l] != 0)
                    {
                        Console.Write("|");
                        Console.ForegroundColor =  ConsoleColor.Blue;
                        Console.Write(String.Format("{0,5}", gameArray[i,l]) +" ");
                        Console.ForegroundColor =  ConsoleColor.White;
                    }
                    else 
                        Console.Write("|      ");
                    if(l == 3)
                        Console.WriteLine("|\n-----------------------------");
                }
            }
        }
        static public void InitGameArray()
        {
            for (int i = 0; i < 4; i++)
            {
                for (int l = 0; l < 4; l++)
                {
                    gameArray[i,l] = 0;
                }
            }
        }
        static public bool CheckIfLost()
        {
            if(CountEmptyCell() > 0)
                return false;
            for(int i = 0; i < 4; i++)
            {
                List<int> row = GetRow(i);
                if(CanMoveLine(row)) 
                    return false;

                List<int> column = GetColumn(i);
                if(CanMoveLine(column)) 
                    return false;
            }
            return true;
        }
        static public bool CanMoveLine(List<int> value)
        {
            if(value.Count() != 4)
                return true;

            return value[0] == value[1] || value[1] == value[2] || value[2] == value[3]; 
        }
        public static void GenerateCell()
        {
            while(CountEmptyCell() > 0)
            {
                int y = rand.Next(0, 4);
                int x = rand.Next(0,4);
                if(gameArray[y,x] == 0)
                {  
                    gameArray[y,x]  = rand.Next(1,3) * 2;
                    break;
                }
            }
        }
        public static List<int> CalculateALine(List<int> list, bool rightToLeft)
        {   
            List<int> outta = new List<int>();
            int len = list.Count;
            if(list.Count > 1)
            {
                if(rightToLeft)
                    list.Reverse(); 
                if(len >= 2)
                {
                    if(list[0] == list[1])
                    {
                        outta.Add(list[0] * 2);
                        if(len == 3)
                            outta.Add(list[2]); 
                        else if(len == 4)
                        {
                            if(list[2] == list[3])
                                outta.Add(list[2] * 2); 
                            else 
                            {
                                outta.Add(list[2]);
                                outta.Add(list[3]);
                            }
                        }
                        
                    }
                        
                    else
                    {
                        outta.Add(list[0]);
                        if(len == 2)
                            outta.Add(list[1]);
                        else 
                        {
                            if(list[1] == list[2])
                            {
                                outta.Add(list[1] * 2);
                                if(len == 4) 
                                    outta.Add(list[3]);
                            }
                            else
                            {
                                outta.Add(list[1]);
                                if(len == 3)
                                    outta.Add(list[2]);
                                else 
                                {
                                    if(list[2] == list[3])
                                        outta.Add(list[2] * 2);
                                    else 
                                    {
                                        outta.Add(list[2]);
                                        outta.Add(list[3]);
                                    }
                                }

                            }
                        }
                    }   
                }
            }
            else if(list.Count == 1)
                outta = list;
            while(outta.Count != 4)
            {
                outta.Add(0);
            }
            if(rightToLeft)
                outta.Reverse();
            return outta;
        }
        public static bool MakeAMove(ConsoleKeyInfo direction) 
        {
            if(direction.Key is ConsoleKey.RightArrow or ConsoleKey.LeftArrow)
            {
                for (int i = 0; i < 4; i++)
                {
                    List<int> row = GetRow(i);
                    List<int> rowCalculed = CalculateALine(row, direction.Key == ConsoleKey.RightArrow);
                    for (int n = 0; n < 4; n++)
                    {
                        gameArray[i,n] = rowCalculed[n];
                    } 
                }
            }
            else if(direction.Key is ConsoleKey.UpArrow or ConsoleKey.DownArrow)
            {
                for (int i = 0; i < 4; i++)
                {
                    List<int> row = GetColumn(i);
                    List<int> rowCalculated = CalculateALine(row, direction.Key == ConsoleKey.DownArrow);
                    for (int n = 0; n < 4; n++)
                    {
                        gameArray[n,i] = rowCalculated[n];
                    } 
                }
            }
            else 
                return false;
            return true;
        }
        static void Main(string[] args)
        {
            InitGameArray();
            DisplayGameArray();
            while(!CheckIfLost())
            {
                GenerateCell();
                DisplayGameArray();
                while(true)
                {
                    if(MakeAMove(Console.ReadKey())) 
                        break;
                }
            }
            DisplayGameArray();
            Console.Beep(440, 1000);
            Console.WriteLine("You lost!");
        }
   }
}
