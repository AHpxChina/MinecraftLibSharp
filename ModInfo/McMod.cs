using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using Newtonsoft.Json;

namespace ModInfo
{
    class McMod
    {
        // mcmod.info File
        public struct Mod
        {
            public string ModId;
            public string Name;
            public string Description;
            public string Version;
            public string AuthorList;
            public string Credits;
            public string McVersion;
            public string Dependencies;
            public string Parent;
            public string RequiredMods;
            public string Dependants;
        };
        
        // a list of mod info
        private List<Mod> Mods;

        // get info as a dictionary by modid
        public Dictionary<string, string> GetInfo(string TargetModId)
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
            Dictionary<string, string> ModInfoDictionary = new Dictionary<string, string>();
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
        public string Init(string ModsFolderPath)
        {
            Mods = new List<Mod>();
            FileInfo[] ModsJars;
            try
            {
                ModsJars = new DirectoryInfo(ModsFolderPath).GetFiles();
            }
            catch(Exception e)
            {
                return "ERROR! " + e.Message;
            }
            Directory.CreateDirectory(ModsFolderPath + "\\ModsInfoTemp");
            foreach(FileInfo ModsJar in ModsJars)
            {
                ZipFile zf = new ZipFile(ModsJar.FullName);
                foreach(ZipEntry ze in zf)
                {
                    if(ze.Name == "mcmod.info")
                    {
                        FileStream fileStream = new FileStream(ze.Name, FileMode.Create);
                        Stream inputStream = zf.GetInputStream(ze);
                        byte[] buffer = new byte[10 * 1024];
                        int length;
                        while ((length = inputStream.Read(buffer, 0, 10 * 1024)) > 0)
                        {
                            fileStream.Write(buffer, 0, length);
                        }
                        fileStream.Close();
                        inputStream.Close();
                        AddToList(ModsJar.FullName + "\\" + ze.Name);
                    }
                }
            }
            return "OK!";
        }

        // add info to list by mcmod.info file
        private void AddToList(string Path)
        {
            StreamReader sr = new StreamReader(Path);
            string JsonString = sr.ReadToEnd();
            sr.Close();
            Dictionary<string, string> ModInfo = JsonConvert.DeserializeObject<Dictionary<string, string>>(JsonString);
            Mod newMod;
            // add to mods list
            {
                //modid
                newMod.ModId = ModInfo["modid"];
                //name
                newMod.Name = ModInfo["name"];
                //description
                if (ModInfo.ContainsKey("description"))
                {
                    newMod.Description = ModInfo["description"];
                }
                else
                {
                    newMod.Description = "NOINFO";
                }
                //version
                if (ModInfo.ContainsKey("version"))
                {
                    newMod.Version = ModInfo["version"];
                }
                else
                {
                    newMod.Version = "NOINFO";
                }
                //authorList
                if (ModInfo.ContainsKey("authorList"))
                {
                    newMod.AuthorList = ModInfo["authorList"];
                }
                else
                {
                    newMod.AuthorList = "NOINFO";
                }
                //Credits
                if (ModInfo.ContainsKey("credits"))
                {
                    newMod.Credits = ModInfo["credits"];
                }
                else
                {
                    newMod.Credits = "NOINFO";
                }
                //mcversion
                if (ModInfo.ContainsKey("mcversion"))
                {
                    newMod.McVersion = ModInfo["mcversion"];
                }
                else
                {
                    newMod.McVersion = "NOINFO";
                }
                //dependencies
                if (ModInfo.ContainsKey("dependencies"))
                {
                    newMod.Dependencies = ModInfo["dependencies"];
                }
                else
                {
                    newMod.Dependencies = "NOINFO";
                }
                //parent
                if (ModInfo.ContainsKey("parent"))
                {
                    newMod.Parent = ModInfo["parent"];
                }
                else
                {
                    newMod.Parent = "NOINFO";
                }
                //requiredMods
                if (ModInfo.ContainsKey("requiredMods"))
                {
                    newMod.RequiredMods = ModInfo["requiredMods"];
                }
                else
                {
                    newMod.RequiredMods = "NOINFO";
                }
                //depandants
                if (ModInfo.ContainsKey("dependants"))
                {
                    newMod.Dependants = ModInfo["dependants"];
                }
                else
                {
                    newMod.Dependants = "NOINFO";
                }

            }
            Mods.Add(newMod);
        }
    }
}
