using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Sys = Cosmos.System;
using System.Linq;

namespace ThingOS
{
    public class Kernel : Sys.Kernel
    {

        protected override void BeforeRun()
        {
            var fs = new Sys.FileSystem.CosmosVFS();
            Sys.FileSystem.VFS.VFSManager.RegisterVFS(fs);
            Console.WriteLine(@"
      _   __ 
     ( `^` ))   ThingOS
     |     ||   2022
     |     ||   by Josiah
     '-----'`
");
            Login();
            
        }

        void Login()
        {
            if (File.Exists(@"0:\thingSys\thing.info"))
            {
                var infoRaw = File.ReadAllText(@"0:\thingSys\thing.info");
                var info = infoRaw.Split("|");

                // 3 fields
                var pcName = info[0];
                var userName = info[1];
                var password = info[2];

                Console.WriteLine(@$"Hello {userName}! You're logging into {pcName}. Input your password here!");
                if (Console.ReadLine() != password)
                {
                    // hmm... yep nobody cares!
                }
            } else
            {
                Console.BackgroundColor = ConsoleColor.Blue;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("ThingOS Setup - Info");
                Console.WriteLine("---------------------");
                Console.WriteLine("Welcome to ThingOS! The setup will begin by pressing a key.");
                Console.ReadKey();
                Console.Clear();
                Console.WriteLine("What is the name of this PC? Will default to THINGPC followed by 3 numbers.");
                var pcName = Console.ReadLine();
                
            }
        }

        protected override void Run()
        {
            var drive = new DriveInfo("0");
            Console.Write("thing@you: ");
            var input = Console.ReadLine();


            // let's handle the args
            string[] args;
            args = input.Split(' ');




            // Handle all cmds
            if (input == "") { return; } else if (input == "dir") {
                try
                {
                    string[] filePaths = Directory.GetFiles(@"0:\");
                    drive = new DriveInfo("0");
                    Console.WriteLine("Volume in drive 0 is " + $"{drive.VolumeLabel}");
                    Console.WriteLine("Directory of " + @"0:\");
                    Console.WriteLine("\n");
                    for (int i = 0; i < filePaths.Length; ++i)
                    {
                        string path = filePaths[i];
                        Console.WriteLine(System.IO.Path.GetFileName(path));
                    }
                    foreach (var d in System.IO.Directory.GetDirectories(@"0:\"))
                    {
                        var dir = new DirectoryInfo(d);
                        var dirName = dir.Name;

                        Console.WriteLine(dirName + " <DIR>");
                    }
                    Console.WriteLine("\n");
                    Console.WriteLine("        " + $"{drive.TotalSize}" + " bytes");
                    Console.WriteLine("        " + $"{drive.AvailableFreeSpace}" + " bytes free");
                } catch (Exception e) {
                    Console.WriteLine(@"
                              _ ._  _ , _ ._            BOOM
                            (_ ' ( `  )_  .__)          ThingOS accidentally had a mental explosion in the proccess of " + $"{input}" + @".
                          ( (  (    )   `)  ) _)        We'll restart for you. We just halted so we can press a key and we're cool, aren't we?
                         (__ (_   (_ . _) _) ,__)
                             `~~`\ ' . /`~~`
                                  ;   ;
                                  /   \
                    _____________/_ __ \_____________
                ");
                    Console.WriteLine("error code: " + e.ToString());
                    Console.ReadLine();
                    Sys.Power.Reboot();
                }
            } else if (input == "nmf") {
                Console.WriteLine(@"
                              _ ._  _ , _ ._            BOOM
                            (_ ' ( `  )_  .__)          ThingOS accidentally had a mental explosion in the proccess of " + "ThingOS" + @".
                          ( (  (    )   `)  ) _)        We'll restart for you. We just halted so we can press a key and we're cool, aren't we?
                         (__ (_   (_ . _) _) ,__)
                             `~~`\ ' . /`~~`
                                  ;   ;
                                  /   \
                    _____________/_ __ \_____________
                ");
                Console.WriteLine("error code: sys.command.MyFault");
                Console.ReadLine();
                Sys.Power.Reboot();
            } else if (input == "reboot") {
                Console.WriteLine("rebooting your thing");
                Sys.Power.Reboot();
            } else if (args[0] == "rfile")
            {
                Console.WriteLine(File.ReadAllText("0:\\" + Directory.GetCurrentDirectory() + "\\" + args[1]));
            } else if (args[0] == "touch") {
                
                foreach (string i in args)
                {
                    if (i == "touch")
                    {
                        continue;
                    }
                    try
                    {
                        Sys.FileSystem.VFS.VFSManager.CreateFile(@"0:\" + i);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.ToString());
                    }
                    Console.WriteLine("Created " + i + ".");
                }
            }
            else
            {
                Console.WriteLine("This isn't a command, try something else.");
            }
        }
    }
}
