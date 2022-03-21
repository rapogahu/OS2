using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

const string alphabet = "abcdefghijklmnopqrstuvwxyz";
char[] hashDecrypt = new char[5];

string funcHashDecrypt(string storedHash)
{
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

                        using (SHA256 sha256Hash = SHA256.Create())
                        {
                            byte[] sourceBytes = Encoding.UTF8.GetBytes(hashDecrypt);
                            byte[] hashBytes = sha256Hash.ComputeHash(sourceBytes);
                            string hash = BitConverter.ToString(hashBytes).Replace("-", String.Empty).ToLower();

                            if (hash == storedHash)
                            {
                                goto Found;
                            }
                        }
                    }
                }
            }
        }
    }

Found:
    string strHashDecrypt = new string(hashDecrypt);
    return strHashDecrypt;
}

string storedHash1 = "1115dd800feaacefdf481f1f9070374a2a81e27880f187396db67958b207cbad";
string storedHash2 = "3a7bd3e2360a3d29eea436fcfb7e44c735d117c42d1c1835420b6b9942dd4f1b";
string storedHash3 = "74e1bb62f8dabb8125a58852b63bdf6eaef667cb56ac7f7cdba6d7305c50a22f";

while (true)
{
    Console.WriteLine("---------------------------------------------------------------");
    Console.WriteLine("Select mode: \n0. Exit \n1. Single threaded \n2. Multithreaded");
    Console.WriteLine("---------------------------------------------------------------\n");

    int answer = int.Parse(Console.ReadLine());

    if (answer == 0)
    {
        break;
    }
    if (answer == 1)
    {
        Stopwatch swatch = new Stopwatch();
        swatch.Start();

        Console.WriteLine("---------------------------------------------------------------");
        Console.WriteLine("Decrypt of first hash is " + funcHashDecrypt(storedHash1));
        Console.WriteLine("Decrypt of second hash is " + funcHashDecrypt(storedHash2));
        Console.WriteLine("Decrypt of third hash is " + funcHashDecrypt(storedHash3));

        swatch.Stop();

        Console.WriteLine("\nTime taken to execute: " + swatch.Elapsed);
        Console.WriteLine("---------------------------------------------------------------\n");
    }
    if (answer == 2)
    {
        Console.WriteLine("Enter the number of threads: ");
        int numThreads = int.Parse(Console.ReadLine());

        if (numThreads == 1)
        {
            Stopwatch swatch1 = new Stopwatch();
            swatch1.Start();

            Console.WriteLine("---------------------------------------------------------------");
            Console.WriteLine("Decrypt of first hash is " + funcHashDecrypt(storedHash1));
            Console.WriteLine("Decrypt of second hash is " + funcHashDecrypt(storedHash2));
            Console.WriteLine("Decrypt of third hash is " + funcHashDecrypt(storedHash3));

            swatch1.Stop();

            Console.WriteLine("\nTime taken to execute: " + swatch1.Elapsed);
            Console.WriteLine("---------------------------------------------------------------\n");
        }
        else if (numThreads == 2)
        {
            Stopwatch swatch = new Stopwatch();

            Console.WriteLine("---------------------------------------------------------------");

            Task t1 = new Task(() =>
            {
                Console.WriteLine("Decrypt of first hash is " + funcHashDecrypt(storedHash1));
                Console.WriteLine("Decrypt of second hash is " + funcHashDecrypt(storedHash2));
            });
            Task t2 = new Task(() =>
            {
                Console.WriteLine("Decrypt of third hash is " + funcHashDecrypt(storedHash3));
            });

            swatch.Start();

            t1.RunSynchronously();
            t2.RunSynchronously();

            swatch.Stop();
            Console.WriteLine("\nTime taken to execute: " + swatch.Elapsed);

            Console.WriteLine("---------------------------------------------------------------\n");

            t1.Wait();
            t2.Wait();
        }
        else
        {
            Stopwatch swatch = new Stopwatch();

            Console.WriteLine("---------------------------------------------------------------");

            Task t1 = new Task(() =>
            {
                Console.WriteLine("Decrypt of first hash is " + funcHashDecrypt(storedHash1));
            });
            Task t2 = new Task(() =>
            {
                Console.WriteLine("Decrypt of second hash is " + funcHashDecrypt(storedHash2));
            });
            Task t3 = new Task(() =>
            {
                Console.WriteLine("Decrypt of third hash is " + funcHashDecrypt(storedHash3));
            });

            swatch.Start();

            t1.RunSynchronously();
            t2.RunSynchronously();
            t3.RunSynchronously();

            swatch.Stop();
            Console.WriteLine("\nTime taken to execute: " + swatch.Elapsed);

            Console.WriteLine("---------------------------------------------------------------\n");

            t1.Wait();
            t2.Wait();
            t3.Wait();
        }
    }
}
