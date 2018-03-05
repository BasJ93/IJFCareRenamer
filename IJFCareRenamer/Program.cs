using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace IJFCareRenamer
{
    class Program
    {
        //Define Epoch
        private static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        //Convert Timestap in Unix millisecons to DateTime object
        public static DateTime DateTimeFromUnixTimestampMillis(long millis)
        {
            return UnixEpoch.AddMilliseconds(millis);
        }

        public static void Rename(string IJFFolder, string OuputFolder, bool move)
        {
            if (Directory.Exists(IJFFolder))
            {
                DirectoryInfo IJFMetaDir = new DirectoryInfo(IJFFolder + "/metadata");
                FileInfo[] files = IJFMetaDir.GetFiles("*.json");
                foreach (FileInfo file in files)
                {
                    using (StreamReader r = new StreamReader(file.FullName))
                    {
                        string json = r.ReadToEnd();
                        dynamic array = JsonConvert.DeserializeObject(json);
                        DateTime MatchDateTime = DateTimeFromUnixTimestampMillis(long.Parse(array.data.timestamp_float.ToString()));
                        string newFilename = MatchDateTime.ToShortDateString() + " " + array.data.Category + " " + array.data.ContestID + " " + array.data.NameWhiteLong + " " + array.data.NameBlueLong + ".flv";
                        newFilename = newFilename.Replace(" ", "_");
                        if (!File.Exists(IJFFolder + "/" + array.filename))
                        {
                            Console.WriteLine("For some reason, the videofile referenced by the metadata does not exist.");
                        }
                        else
                        {
                            string targetLocation = OuputFolder + "/" + newFilename;
                            if(move)
                            {
                                File.Move(IJFFolder + "/" + array.filename, targetLocation);
                            }
                            else
                            {
                                File.Copy(IJFFolder + "/" + array.filename, targetLocation, true);
                            }
                            Console.WriteLine("{0} -> {1}", array.filename, newFilename);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("The specified folder does not exist.");
            }
            Console.WriteLine("Finished copying files to new names.");
            Console.ReadKey();
        }

        public static void RenameDemo(string IJFFolder, string OuputFolder)
        {
            if (Directory.Exists(IJFFolder))
            {
                DirectoryInfo IJFMetaDir = new DirectoryInfo(IJFFolder + "/metadata");
                FileInfo[] files = IJFMetaDir.GetFiles("*.json");
                foreach (FileInfo file in files)
                {
                    using (StreamReader r = new StreamReader(file.FullName))
                    {
                        string json = r.ReadToEnd();
                        dynamic array = JsonConvert.DeserializeObject(json);
                        DateTime MatchDateTime = DateTimeFromUnixTimestampMillis(long.Parse(array.data.timestamp_float.ToString()));
                        string newFilename = MatchDateTime.ToShortDateString() + " " + array.data.Category + " " + array.data.ContestID + " " + array.data.NameWhiteLong + " " + array.data.NameBlueLong + ".flv";
                        newFilename = newFilename.Replace(" ", "_");
                        Console.WriteLine(array);
                        if (!File.Exists(IJFFolder + "/" + array.filename))
                        {
                            Console.WriteLine("For some reason, the videofile referenced by the metadata does not exist.");
                        }
                        else
                        {
                            string targetLocation = OuputFolder + "/" + newFilename;
                            Console.WriteLine("{0} -> {1}", array.filename, newFilename);
                        }
                    }
                }
            }
            else
            {
                Console.WriteLine("The specified folder does not exist.");
            }
            Console.WriteLine("Finished filename demo.");
            Console.ReadKey();
        }

        static void GUI()
        {
            Application.EnableVisualStyles();
            Application.Run(new Form1());
        }

        [STAThread]
        static void Main(string[] args)
        {
            //Make multiple arguments available. Not just input folder, but also output folder, and copy/replace.
            //Let's also add an argument to show a GUI instead of the terminal.
            string IJFFolder;
            if (args.Length == 0)
            {
                Console.WriteLine("Not enough arguments provided. Usage: IJFCareRenamer.exe gui or IJFCareRenamer inputfolder outputfolder copy/move.");
                Console.ReadKey();
            }
            else
            {
                if(args[0] == "gui")
                {
                    GUI();
                }
                else
                {
                    if(args.Length == 3)
                    {
                        if(args[2] == "copy")
                            Rename(args[0], args[1], false);
                        if(args[2] == "replace")
                            Rename(args[0], args[1], true);
                        if (args[2] == "demo")
                            RenameDemo(args[0], args[1]);
                    }
                    else
                    {
                        Rename(Directory.GetCurrentDirectory(), Directory.GetCurrentDirectory(), false);
                    }
                }
            }
        }
    }
}
