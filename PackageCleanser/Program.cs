using System;
using System.IO;
using System.Collections.Generic;
namespace PackageCleanser
{
    class Program
    {
        static void Main(string[] args)
        {
            if(args.Length != 0)
            {
                //Console.WriteLine("Incorrect input! Drag and drop the root project folder onto the application to cleanse the packages");
                //return;
            }

            StreamReader reader = null;
            try
            {
                bool success = false;
                reader = OpenManifest(args[0]);
                List<string> newManifest = CleanseManifest(reader);
                reader.Close();
                reader = null;
                success = OverwriteManifest(args[0], newManifest);
                if (success)
                {
                    SwapManifests(args[0]);
                }
            }
            catch(Exception e)
            {
                //To-do:
                //Add indication messages
                Console.WriteLine(e.Message);
            }
            finally
            {
                if(reader != null)
                {
                    reader.Close();
                }
            }
        }

        static StreamReader OpenManifest(string path)
        {
            path += "/Packages/manifest.json";
            StreamReader reader = new StreamReader(path);
            return reader;
        }

        static List<string> CleanseManifest(StreamReader reader)
        {
            string line;
            List<string> newManifest = new List<string>();
            while ((line = reader.ReadLine()) != null)
            {
                if (!(line.ToLower().Contains("com") && !line.ToLower().Contains("modules")) && !line.Contains("androidjni"))
                {
                    newManifest.Add(line);
                }
            }
            return newManifest;
        }

        static bool OverwriteManifest(string path, List<String> newManifest)
        {
            path += "/Packages/tempmanifest.json";
            StreamWriter writer = null;
            try
            {
                writer = new StreamWriter(path);
                foreach (string s in newManifest)
                {
                    writer.WriteLine(s);
                }
            }
            catch(Exception e)
            {
                throw e;
            }
            finally
            {
                if(writer != null)
                {
                    writer.Close();
                }
            }
            return true;
        }

        static void SwapManifests(string path)
        {
            //To-do:
            //Add file deleting prevention
            try
            {
                File.Delete(path + "/Packages/manifest.json");
                File.Move(path + "/Packages/tempmanifest.json", path + "/Packages/manifest.json");
            }
            catch(Exception e)
            {
                throw e;
            }
        }
    }
}
