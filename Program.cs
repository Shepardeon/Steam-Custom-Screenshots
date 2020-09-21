using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Drawing.Imaging;
using System.IO;

namespace VRChat_Image_Converter
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length == 0)
                Console.WriteLine("Aucune image n'a été glissée-déposée sur l'exécutable.");
            else
                Convert(args);

            Console.WriteLine("\nAppuyer sur ENTRER pour quitter l'application...");
            Console.ReadLine();
        }

        static void Convert(string[] files)
        {
            bool blaireau = false;
            string basePath;
            Dictionary<string, Image> images = new Dictionary<string, Image>();

            do
            {
                if (blaireau)
                    Console.WriteLine("T'es un blaireau! Le chemin rensigné n'existe pas sur ta machine.");
                Console.WriteLine("Entrez le chemin où vous voulez que vos images soient sauvegardés et appuyez sur ENTRER");
                Console.WriteLine("(et si t'es pas trop con tu feras un glisser-déposer du dossier et tu feras pas chier)");
                basePath = Console.ReadLine().Trim('"');

                if (!Directory.Exists(basePath))
                    blaireau = true;
                else
                    blaireau = false;

                if (String.IsNullOrEmpty(basePath))
                    Console.WriteLine("Les fichiers seront créés au même endroit");
                else
                {
                    basePath = basePath + "\\";
                    Console.WriteLine(basePath + " ?");
                }

                Console.WriteLine("Tu valides ? (O/N)");

            } while (Console.ReadLine().ToUpper() != "O" || blaireau);

            foreach(string file in files)
            {
                Console.WriteLine("Chargement de l'image: " + file);
                Image img = LoadImage(file);
                if (img != null)
                    images.Add(basePath + GetImageName(file), img);
            }

            foreach (KeyValuePair<string, Image> elem in images)
            {
                WriteImage(elem.Key + ".jpg", elem.Value);
                WriteImage(elem.Key + "_vr.jpg", elem.Value);
            }
        }

        static void WriteImage(string path, Image data)
        {
            Console.WriteLine("Création de l'image: " + path);
            data.Save(path, ImageFormat.Jpeg);
        }

        static Image LoadImage(string path)
        {
            try
            {
                return Image.FromFile(path);
            }
            catch (Exception e)
            {
                switch (e.GetType().ToString())
                {
                    case "System.OutOfMemoryException":
                        Console.WriteLine("Le fichier " + path + " n'est pas un fichier image => ignoré");
                        break;
                    case "System.FileNotFoundException":
                        Console.WriteLine("Le fichier " + path + " n'existe pas => ignoré");
                        break;
                    case "System.ArgumentException":
                        Console.WriteLine(path + " est une URI => ignoré");
                        break;
                    default:
                        Console.WriteLine("WTF, tu fout quoi là?");
                        break;
                }
            }

            return null;
        }

        static string GetImageName(string file)
        {
            string name = file.Split('\\').Last();
            name = name.Split('_')[2] + name.Split('_')[3];
            name = name.Split('.')[0].Replace("-", "") + "_1";

            return name;
        }
    }
}
