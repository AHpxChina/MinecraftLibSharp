/*
 * This is a Test Project for me to test the library.
 */

using System;
using System.Collections.Generic;
using McModInfo;

namespace TestProject
{
    class Program
    {
        static void Main()
        {
            McMod Mod = new McMod("G:\\Minecraft\\Test");
            string[] str = Mod.GetModIdList().ToArray();
            foreach(string s in str)
            {
                Dictionary<string, object> dic = Mod.GetInfo(s);
                Console.WriteLine(s + "  " + dic["Version"]);
            }
            Console.ReadLine();
        }
    }
}
