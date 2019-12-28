using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ModInfo
{
    public class McMod
    {
        // mcmod.info File
        public struct Mod
        {
            public string ModId;
            public string Name;
            public string Description;
            public string Version;
            public string[] AuthorList;
            public string Credits;
            public string McVersion;
            public string[] Dependencies;
            public string Parent;
            public string[] RequiredMods;
            public string[] Dependants;
        };
        
        // a list of mod info
        private readonly List<Mod> Mods;

        // get info as a dictionary by modid
        public Dictionary<string, object> GetInfo(string TargetModId)
        {
            int keyIndex = 0;
            foreach (Mod mod in Mods)
            {
                if(mod.ModId== TargetModId)
                {
                    break;
                }
                keyIndex++;
            }
            Dictionary<string, object> ModInfoDictionary = new Dictionary<string, object>();
            {
                ModInfoDictionary.Add("ModId", Mods[keyIndex].ModId);
                ModInfoDictionary.Add("Name", Mods[keyIndex].Name);
                ModInfoDictionary.Add("Description", Mods[keyIndex].Description);
                ModInfoDictionary.Add("Version", Mods[keyIndex].Version);
                ModInfoDictionary.Add("AuthorList", Mods[keyIndex].AuthorList);
                ModInfoDictionary.Add("Credits", Mods[keyIndex].Credits);
                ModInfoDictionary.Add("McVersion", Mods[keyIndex].McVersion);
                ModInfoDictionary.Add("Dependencies", Mods[keyIndex].Dependencies);
                ModInfoDictionary.Add("Parent", Mods[keyIndex].Parent);
                ModInfoDictionary.Add("RequiredMods", Mods[keyIndex].RequiredMods);
                ModInfoDictionary.Add("Dependants", Mods[keyIndex].Dependants);
            }
            return ModInfoDictionary;
            
        }

        // get the modid list
        public List<string> GetModIdList()
        {
            List<string> ID = new List<string>();
            foreach(Mod mod in Mods)
            {
                ID.Add(mod.ModId);
            }
            return ID;
        }
        
        // get mcmod.info file 
        public McMod(string ModsFolderPath)
        {
            Mods = new List<Mod>();
            Directory.CreateDirectory(ModsFolderPath + "\\ModInfoTemp");
            FileInfo[] ModsJars = null;
            try
            {
                ModsJars = new DirectoryInfo(ModsFolderPath).GetFiles();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            foreach(FileInfo ModsJar in ModsJars)
            {
                ZipFile zf = new ZipFile(ModsJar.FullName);
                foreach(ZipEntry ze in zf)
                {
                    if(ze.Name == "mcmod.info")
                    {
                        FileStream fileStream = File.Create(ModsJar.DirectoryName + "\\mcmod.info");
                        Stream inputStream = zf.GetInputStream(ze);
                        byte[] buffer = new byte[10 * 1024];
                        int length;
                        while ((length = inputStream.Read(buffer, 0, 10 * 1024)) > 0)
                        {
                            fileStream.Write(buffer, 0, length);
                        }
                        fileStream.Close();
                        inputStream.Close();
                        if(File.Exists(ModsFolderPath + "\\ModInfoTemp\\mcmod.info"))
                        {
                            File.Delete(ModsFolderPath + "\\ModInfoTemp\\mcmod.info");
                        }
                        File.Move(ModsFolderPath + "\\mcmod.info", ModsFolderPath + "\\ModInfoTemp\\mcmod.info");
                        AddToList(ModsFolderPath + "\\ModInfoTemp\\mcmod.info");
                    }
                }
            }
        }

        // add info to list by mcmod.info file
        private void AddToList(string Path)
        {
            StreamReader sr = new StreamReader(Path);
            string[] JsonString = FormatJsonString(sr.ReadToEnd());
            /*
             * Debug
             */
                /*
             for(int debug = 0; debug < JsonString.Length; debug++)
            {
                Console.WriteLine(JsonString[debug]);
            }
                */
            /*
             * Debug
             */
            sr.Close();
            for(int i = 0; i < JsonString.Length; i++)
            {
                Dictionary<string, object> ModInfo = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonString[i]);
                Mod newMod = new Mod();
                // add to mods list
                {
                    string[] str = { "NOINFO" };
                    //modid
                    newMod.ModId = Convert.ToString(ModInfo["modid"]);
                    //name
                    newMod.Name = Convert.ToString(ModInfo["name"]);
                    //description
                    if (ModInfo.ContainsKey("description"))
                    {
                        newMod.Description = Convert.ToString(ModInfo["description"]);
                    }
                    else
                    {
                        newMod.Description = "NOINFO";
                    }
                    //version
                    if (ModInfo.ContainsKey("version"))
                    {
                        newMod.Version = Convert.ToString(ModInfo["version"]);
                    }
                    else
                    {
                        newMod.Version = "NOINFO";
                    }
                    //authorList
                    if (ModInfo.ContainsKey("authorList"))
                    {
                        newMod.AuthorList = FormatJsonArrayInAttributes(ModInfo["authorList"].ToString());
                    }
                    else
                    {
                        newMod.AuthorList = str;
                    }
                    //Credits
                    if (ModInfo.ContainsKey("credits"))
                    {
                        newMod.Credits = Convert.ToString(ModInfo["credits"]);
                    }
                    else
                    {
                        newMod.Credits = "NOINFO";
                    }
                    //mcversion
                    if (ModInfo.ContainsKey("mcversion"))
                    {
                        newMod.McVersion = Convert.ToString(ModInfo["mcversion"]);
                    }
                    else
                    {
                        newMod.McVersion = "NOINFO";
                    }
                    //dependencies
                    if (ModInfo.ContainsKey("dependencies"))
                    {
                        newMod.Dependencies = FormatJsonArrayInAttributes(ModInfo["dependencies"].ToString());
                    }
                    else
                    {
                        newMod.Dependencies = str;
                    }
                    //parent
                    if (ModInfo.ContainsKey("parent"))
                    {
                        newMod.Parent = Convert.ToString(ModInfo["parent"]);
                    }
                    else
                    {
                        newMod.Parent = "NOINFO";
                    }
                    //requiredMods
                    if (ModInfo.ContainsKey("requiredMods"))
                    {
                        newMod.RequiredMods = FormatJsonArrayInAttributes(ModInfo["requiredMods"].ToString());
                    }
                    else
                    {
                        newMod.RequiredMods = str;
                    }
                    //depandants
                    if (ModInfo.ContainsKey("dependants"))
                    {
                        newMod.Dependants = FormatJsonArrayInAttributes(ModInfo["dependants"].ToString());
                    }
                    else
                    {
                        newMod.Dependants = str;
                    }

                }
                Mods.Add(newMod);
            }
            
        }

        // divide every modinfo json string in a single file
        private string[] FormatJsonString(string MixedString)
        {
            List<string> Result = new List<string>();
            char[] CharString = MixedString.ToCharArray();
            List<char> Temp = new List<char>();
            bool flag = false;
            for(int i = 0; i < CharString.Length; i++)
            {
                if (CharString[i] == '{')
                {
                    flag = true;
                }
                if (CharString[i] == '}')
                {
                    Temp.Add(CharString[i]);
                    flag = false;
                    Result.Add(new string(Temp.ToArray()));
                    Temp.Clear();
                }
                if (flag)
                {
                    Temp.Add(CharString[i]);
                }
            }
            return Result.ToArray();
        }

        // array in attributes
        private string[] FormatJsonArrayInAttributes(string JsonArrayString)
        {
            List<string> Result = new List<string>();
            char[] CharString = JsonArrayString.ToCharArray();
            List<char> Temp = new List<char>();
            bool flag = false;
            for(int i = 1; i < CharString.Length; i++)
            {
                if (CharString[i-1] == '\"')
                {
                    flag = true;
                }
                if (CharString[i] == '\"')
                {
                    flag = false;
                    Result.Add(new string(Temp.ToArray()));
                    Temp.Clear();
                }
                if (flag)
                {
                    Temp.Add(CharString[i]);
                }
            }
            return Result.ToArray();
        }
    }
}
