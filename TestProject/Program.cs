/*
 * This is a Test Project for me to test the library.
 */

using System;
using System.Collections.Generic;
using McModInfo;
using System.IO;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            McMod Mod = new McMod("G:\\Minecraft\\UDPPackV1122\\mods");
            string[] str = Mod.GetModIdList().ToArray();
            if (File.Exists("modlist.txt"))
            {
                File.Delete("modlist.txt");
            }
            File.Create("modlist.txt").Close();
            StreamWriter sw = new StreamWriter("modlist.txt");
            string temp;
            foreach(string s in str)
            {
                Dictionary<string, object> dic = Mod.GetInfo(s);
                temp = s + "   " + dic["Version"];
                Console.WriteLine(temp);
                sw.WriteLine(temp);
            }
            sw.Close();
            Console.ReadLine();
        }
    }
}
