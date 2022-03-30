using System;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        const int valueComb = 11881376;
        static string[] passwords = new string[valueComb];

        static string storedHash1 = "1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad";
        static string storedHash2 = "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b";
        static string storedHash3 = "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f";

        static string funcHashDecrypt(int numOfThread, string storedHash)
        {
            int from = 0;
            int to = 0;
            string strHashDecrypt = "";
            int[] valueThread = new int[numOfThread];
            int ostatok = valueComb % numOfThread;

            for (int i = 0; i < numOfThread; i++)
            {
                valueThread[i] = valueComb / numOfThread;
                if (ostatok != 0)
                {
                    valueThread[i]++;
                    ostatok--;
                }
            }

            int mark = 0;
            Parallel.For(0, numOfThread, (mark, loopState) =>
            {
                if (mark != 0) from += valueThread[mark - 1];
                to = from + valueThread[mark];
                for (int j = from; j < to; j++)
                {
                        using (SHA256 sha256Hash = SHA256.Create())
                        {
                            byte[] sourceBytes = Encoding.UTF8.GetBytes(passwords[j]);
                            byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                            string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();
                            if (hash == storedHash)
                            {
                                strHashDecrypt = passwords[j];
                                loopState.Stop();
                            }
                        }
                }
            });
            return strHashDecrypt;
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
                                passwords[it] = new string(hashDecrypt);
                                it++;
                            }
                        }
                    }
                }
            }

            while (true)
            {
                Console.WriteLine("---------------------------------------------------------------");
                Console.WriteLine("Select mode: \n0. Exit \n1. Single threaded \n2. Multithreaded");
                Console.WriteLine("---------------------------------------------------------------\n");

                int answer = int.Parse(Console.ReadLine());
                int numThreads = 0;

                if (answer == 0)
                {
                    break;
                }
                if (answer == 1)
                {
                    numThreads = 1;
                    Stopwatch swatch = new Stopwatch();
                    swatch.Start();

                    Console.WriteLine("---------------------------------------------------------------");
                    Console.WriteLine("Decrypt of first hash is " + funcHashDecrypt(numThreads, storedHash1));
                    Console.WriteLine("Decrypt of second hash is " + funcHashDecrypt(numThreads, storedHash2));
                    Console.WriteLine("Decrypt of third hash is " + funcHashDecrypt(numThreads, storedHash3));

                    swatch.Stop();

                    Console.WriteLine("\nTime taken to execute: " + swatch.Elapsed);
                    Console.WriteLine("---------------------------------------------------------------\n");
                }
                if (answer == 2)
                {
                    Console.WriteLine("Enter the number of threads: ");
                    numThreads = int.Parse(Console.ReadLine());

                    Stopwatch swatch = new Stopwatch();
                    swatch.Start();

                    Console.WriteLine("---------------------------------------------------------------");
                    Console.WriteLine("Decrypt of first hash is " + funcHashDecrypt(numThreads, storedHash1));
                    Console.WriteLine("Decrypt of second hash is " + funcHashDecrypt(numThreads, storedHash2));
                    Console.WriteLine("Decrypt of third hash is " + funcHashDecrypt(numThreads, storedHash3));

                    swatch.Stop();

                    Console.WriteLine("\nTime taken to execute: " + swatch.Elapsed);
                    Console.WriteLine("---------------------------------------------------------------\n");
                }
            }
        }
    }
}
