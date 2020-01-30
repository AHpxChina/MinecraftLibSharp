# MinecraftLibSharp

## About
These are some tools that you can use in your development of some Minecraft related apps.

This Lib contains many different packages which have different functions. Some of theme are under develop and some of them are published.

## Category and Plan
* McModInfo - Released, support Forge Mod only. May support fabric and liteloader in the future.
* Account - Maybe will be released before Feb 2020.

## Install
The package has been published on nuget.com.

You can install it with nuget package manager in Visual Studio or use dotnet CLI or nuget command line tools. You can find the install command [HERE](https://www.nuget.org/packages/LiamSho.MinecraftLibSharp.McModInfo/).

## Use Guild
### McModInfo
This Lib is used to read the **mcmod.info** file in every Minecraft Forge Mod Jar file. It contains many infomation about this mod include modid, version, mcversion, etc. This Lib can help you to get the mod info more easy.

Use Case : I am developing a Mod Sync Tool for my Minecraft Server with Mods. It can help the players of my server update there mods easier.

1. Install this package and add to your project.
2. Add the namespace to your source code file.
```C#
using McModInfo;
```
3. New an object, the parameter is the full path of you mod folder.
```C#
McMod Mod = new McMod("Full Path Of Your *Mods* Folder");
```
4. You can get the modid array by using ***GetModIdList()*** function. It returns a **string array**.
5. You can get the ModInfo by using ***GetInfo("modid")*** function. The parameter is the modid. It returns a **Dictionary<string,object>** in ver1.0.1. In ver 1.0.2, it returns a **McMod.Mod** structure. The structure Keys and Values are below.

The Dictionary<string,object> you get from ***GetInfo("modid")*** function contains these keys.

Visit Forge Doc to get a better understand of these attributes.

|Key|ValueType|Key|ValueType|
| :---- | :---- | :---- | :---- |
| ModId | string | Name | string |
| Description | string | Version | string |
| Credits | string | McVersion | string |
| Parent | string | AuthorList | string[] |
| Dependencies | string[] | RequiredMods | string[] |
| Dependants | string[] | Filename | string |

### Account
Unpublished Yet...
