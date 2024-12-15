using IWshRuntimeLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Codex_Ipsa.Installer
{
    internal class Program
    {
        public static String thePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ".codexipsa");

        static void Main(string[] args)
        {
            Console.Title = "Codex-Ipsa Installer v1.0";

            WebClient client = new WebClient();
            Directory.CreateDirectory(thePath);

            if (System.IO.File.Exists(thePath + "\\MCLauncher.exe"))
            {
                Console.WriteLine("Seems like you already have the launcher installed.");
                Console.WriteLine("Do you wish to repair it? [y/n]");
                Console.Write("> ");
                ConsoleKeyInfo yn = Console.ReadKey();

                if (yn.KeyChar == 'y')
                {
                    System.IO.File.Delete($"{thePath}\\MCLauncher.exe");

                    if (System.IO.File.Exists($"{thePath}\\DiscordRPC.dll"))
                        System.IO.File.Delete($"{thePath}\\DiscordRPC.dll");

                    if (System.IO.File.Exists($"{thePath}\\Newtonsoft.Json.dll"))
                        System.IO.File.Delete($"{thePath}\\Newtonsoft.Json.dll");
                }
                else
                {
                    return;
                }
            }

            //get versions info
            String verJson = client.DownloadString("http://files.codex-ipsa.cz/update/manifest.json");
            List<UpdateJson> uj = JsonConvert.DeserializeObject<List<UpdateJson>>(verJson);

            foreach (UpdateJson v in uj)
            {
                //download the stable release
                if (v.id == "stable")
                {
                    //download launcher zip
                    Console.WriteLine("Downloading the launcher...");
                    client.DownloadFile(v.url, $"{thePath}\\launcher.zip");

                    //extract launcher zip
                    ZipFile.ExtractToDirectory($"{thePath}\\launcher.zip", thePath);
                    System.IO.File.Delete($"{thePath}\\launcher.zip");

                    //create link on desktop
                    CreateShortcut();

                    Console.WriteLine("Installed!");
                    break;
                }
            }
        }

        static void CreateShortcut()
        {
            object shDesktop = (object)"Desktop";
            WshShell shell = new WshShell();
            string shortcutAddress = (string)shell.SpecialFolders.Item(ref shDesktop) + @"\Codex-Ipsa Launcher.lnk";
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(shortcutAddress);
            shortcut.TargetPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\.codexipsa\MCLauncher.exe";
            shortcut.Save();
        }
    }
}
