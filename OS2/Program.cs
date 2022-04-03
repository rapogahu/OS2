using System;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace OS2
{
    class Program
    {
        const int valueComb = 11881376;
        static string[] passwords = new string[valueComb];

        static string storedHash1 = "1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad";
        static string storedHash2 = "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b";
        static string storedHash3 = "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f";

        static bool marker = false;

        static string bruteForce(string storedHash, int from, int to)
        {
            string luckyPass = "";
            for (int j = from; j < to; j++)
            {
                if (marker == true) break;
                using (SHA256 sha256Hash = SHA256.Create())
                {
                    byte[] sourceBytes = Encoding.UTF8.GetBytes(passwords[j]);
                    byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                    string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();
                    if (hash == storedHash)
                    {
                        luckyPass = passwords[j];
                        marker = true;
                        break;
                    }
                }
            }
            return luckyPass;
        }

        static string threadFunc(int numOfThread, string storedHash)
        {
            int[] valueThread = new int[numOfThread];
            int ostatok = valueComb % numOfThread;

            Thread[] tasks = new Thread[numOfThread];

            string[] answers = new string[numOfThread];

            for (int i = 0; i < numOfThread; i++)
            {
                valueThread[i] = valueComb / numOfThread;
                if (ostatok != 0)
                {
                    valueThread[i]++;
                    ostatok--;
                }
            }

            string strHashDecrypt1 = System.String.Empty;

            int from = 0;
            int to = 0;

            for (int i = 0; i < numOfThread; i++)
            {
                if (i != 0) from = to;
                to = from + valueThread[i];

                int start = from;
                int finish = to;
                tasks[i] = new Thread(() =>
                {
                    string strHashDecrypt = System.String.Empty;
                    strHashDecrypt = bruteForce(storedHash, start, finish);
                    if (!String.IsNullOrEmpty(strHashDecrypt))
                    {
                        strHashDecrypt1 = strHashDecrypt;
                    }
                });
                tasks[i].Start();
            }

            for (int i = 0; i < numOfThread; i++)
            {
                tasks[i].Join();
                tasks[i].Interrupt(); 
            }

            marker = false;
            return strHashDecrypt1;
        }

        static void Main(string[] args)
        {
            const string alphabet = "abcdefghijklmnopqrstuvwxyz";
            char[] hashDecrypt = new char[5];

            int it = 0;

            for (int i = 0; i < alphabet.Length; i++)
            {
                hashDecrypt[0] = alphabet[i];
                for (int j = 0; j < alphabet.Length; j++)
                {
                    hashDecrypt[1] = alphabet[j];
                    for (int k = 0; k < alphabet.Length; k++)
                    {
                        hashDecrypt[2] = alphabet[k];
                        for (int l = 0; l < alphabet.Length; l++)
                        {
                            hashDecrypt[3] = alphabet[l];
                            for (int m = 0; m < alphabet.Length; m++)
                            {
                                hashDecrypt[4] = alphabet[m];
                                passwords[it++] = new string(hashDecrypt);
                            }
                        }
                    }
                }
            }

            while (true)
            {
                Console.WriteLine("---------------------------------------------------------------------");
                Console.WriteLine("Choose hash to bruteforce: " +
                    "\n0. Exit " +
                    "\n1. 1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad " +
                    "\n2. 3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b" +
                    "\n3. 74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f");
                Console.WriteLine("---------------------------------------------------------------------\n");

                int answer = int.Parse(Console.ReadLine());
                int numThreads = 0;

                if (answer == 0)
                {
                    break;
                }
                if (answer == 1)
                {
                    Console.WriteLine("Enter the number of threads: ");
                    numThreads = int.Parse(Console.ReadLine());
                    Stopwatch swatch = new Stopwatch();
                    swatch.Start();

                    Console.WriteLine("---------------------------------------------------------------------\n");
                    Console.WriteLine("Decrypt of first hash is " + threadFunc(numThreads, storedHash1));

                    swatch.Stop();

                    Console.WriteLine("\nTime taken to execute: " + swatch.Elapsed);
                    Console.WriteLine("---------------------------------------------------------------------\n");
                }
                if (answer == 2)
                {
                    Console.WriteLine("Enter the number of threads: ");
                    numThreads = int.Parse(Console.ReadLine());
                    Stopwatch swatch = new Stopwatch();
                    swatch.Start();

                    Console.WriteLine("---------------------------------------------------------------------\n");
                    Console.WriteLine("Decrypt of second hash is " + threadFunc(numThreads, storedHash2));

                    swatch.Stop();

                    Console.WriteLine("\nTime taken to execute: " + swatch.Elapsed);
                    Console.WriteLine("---------------------------------------------------------------------\n");
                }
                if (answer == 3)
                {
                    Console.WriteLine("Enter the number of threads: ");
                    numThreads = int.Parse(Console.ReadLine());

                    Stopwatch swatch = new Stopwatch();
                    swatch.Start();

                    Console.WriteLine("---------------------------------------------------------------------\n");
                    Console.WriteLine("Decrypt of third hash is " + threadFunc(numThreads, storedHash3));

                    swatch.Stop();

                    Console.WriteLine("\nTime taken to execute: " + swatch.Elapsed);
                    Console.WriteLine("---------------------------------------------------------------------\n");
                }
            }
        }
    }
}
