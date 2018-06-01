using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace EncryptProject
{
    //Classe de usuario
    public class User
    {
        public string user { get; set; }
        public string password { get; set; }
    }

    //Classe com funções para decriptar
    public static class Decrypt
    {
        public static string CifraCesar(string entrada)
        {
            try
            {
                string retorno = "";

                foreach(char ch in entrada)
                {
                    if (Char.IsNumber(ch))
                    {
                        retorno += ch;
                    }
                    else
                    {
                        retorno += (char)((int)ch - 12);
                    }
                }

                return retorno;
            }
            catch (Exception e)
            {
                return "Erro ao converter Cifra de Cesar";
            }            
        }
    }

    //Classe com funções para encriptar
    public static class Converters
    {
        public static string CifraCesar(string entrada)
        {
            try
            {
                string retorno = "";

                foreach (char ch in entrada)
                {
                    if (Char.IsNumber(ch))
                    {
                        retorno += ch;
                    }
                    else
                    {
                        retorno += (char)(ch + 12);
                    }
                }

                return retorno;
            }
            catch (Exception e)
            {
                return "Erro ao converter Cifra de Cesar";
            }
        }

        public static List<User> MD5_(List<User> users, bool salted = false)
        {
            try
            {
                MD5 md5Hash = MD5.Create();
                byte[] temp_password;

                foreach (User usr in users)
                {
                    if(salted)
                        temp_password = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(usr.user + usr.password));
                    else
                        temp_password = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(usr.password));

                    usr.password = BitConverter.ToString(temp_password).Replace("-", string.Empty);
                }

                return users;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<User> SHA1_(List<User> users, bool salted = false)
        {
            try
            {
                SHA1 sha1hash = SHA1.Create();
                byte[] temp_password;

                foreach (User usr in users)
                {
                    if(salted)
                        temp_password = sha1hash.ComputeHash(Encoding.UTF8.GetBytes(usr.user + usr.password));
                    else
                        temp_password = sha1hash.ComputeHash(Encoding.UTF8.GetBytes(usr.password));

                    usr.password = BitConverter.ToString(temp_password).Replace("-", string.Empty);
                }

                return users;
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public static List<User> SHA256_(List<User> users, bool salted = false)
        {
            try
            {
                SHA256 sha256hash = SHA256.Create();
                byte[] temp_password;

                foreach (User usr in users)
                {
                    if(salted)
                        temp_password = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(usr.password + usr.user));
                    else
                        temp_password = sha256hash.ComputeHash(Encoding.UTF8.GetBytes(usr.password));

                    usr.password = BitConverter.ToString(temp_password).Replace("-", string.Empty);
                }

                return users;
            }
            catch (Exception e)
            {
                return null;
            }
        }
    }

    //Validações
    public static class Validate
    {
        public static bool MD5_(List<User> users, User usr, bool salted = false)
        {
            try
            {
                MD5 md5Hash = MD5.Create();
                byte[] temp_password;

                if (salted)
                    temp_password = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(usr.user + usr.password));
                else
                    temp_password = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(usr.password));

                string passConvertido = BitConverter.ToString(temp_password).Replace("-", string.Empty);

                var temp = (from us in users
                            where us.user == usr.user
                            select us).FirstOrDefault();

                if (temp.user == usr.user && temp.password == passConvertido)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool SHA1_(List<User> users, User usr, bool salted = false)
        {
            try
            {
                SHA1 SHA1_hash = SHA1.Create();
                byte[] temp_password;

                if (salted)
                    temp_password = SHA1_hash.ComputeHash(Encoding.UTF8.GetBytes(usr.user + usr.password));
                else
                    temp_password = SHA1_hash.ComputeHash(Encoding.UTF8.GetBytes(usr.password));

                string passConvertido = BitConverter.ToString(temp_password).Replace("-", string.Empty);

                var temp = (from us in users
                            where us.user == usr.user
                            select us).FirstOrDefault();

                if (temp.user == usr.user && temp.password == passConvertido)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public static bool SHA256_(List<User> users, User usr, bool salted = false)
        {
            try
            {
                SHA256 SHA256_hash = SHA256.Create();
                byte[] temp_password;

                if (salted)
                    temp_password = SHA256_hash.ComputeHash(Encoding.UTF8.GetBytes(usr.user + usr.password));
                else
                    temp_password = SHA256_hash.ComputeHash(Encoding.UTF8.GetBytes(usr.password));

                string passConvertido = BitConverter.ToString(temp_password).Replace("-", string.Empty);

                var temp = (from us in users
                            where us.user == usr.user
                            select us).FirstOrDefault();

                if (temp.user == usr.user && temp.password == passConvertido)
                    return true;
                else
                    return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }

    //Interação com o usuario
    public static class Interact
    {
        static string baseAtual = "Não Convetida";
        static List<User> users = new List<User>();
        static string caminhoBase = "";

        public static void iniciaLista()
        {

            bool mostrarMensagens = false;
            if (caminhoBase == "")
                mostrarMensagens = true;

            if (mostrarMensagens)
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("|           Digite o caminho da Base.                |");
                Console.WriteLine("------------------------------------------------------");

                caminhoBase = Console.ReadLine();
            }

            users = new List<User>();
            using (StreamReader sr = new StreamReader(caminhoBase))
            {
                while (sr.Peek() >= 0)
                {
                    User usr = new User() {
                        user = sr.ReadLine().Split('|')[0],
                        password = Decrypt.CifraCesar(sr.ReadLine().Split('|')[1])
                    };

                    users.Add(usr);
                }
            }

            if (mostrarMensagens)
            {
                Console.WriteLine("|                                                    |");
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("|           Base carregada com sucesso.              |");
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("|                                                    |");
            }
        
        }

        public static void exportBase()
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("|           Digite o caminho da Base.                |");
            Console.WriteLine("------------------------------------------------------");

            string caminho = Console.ReadLine();

            if (!File.Exists(caminho))
                File.Create(caminho).Dispose();

            using (TextWriter tw = new StreamWriter(caminho))
            {
                foreach(var usr in users)
                {
                    tw.WriteLine(usr.user + "|" + usr.password);
                }

                tw.Close();
            }
        }

        public static void imprimirBase()
        {
            foreach (var usr in users)
            {
                Console.WriteLine("Login: " + usr.user);
                Console.WriteLine("Senha: " + usr.password);
                Console.WriteLine("                                                    ");
            }
        }

        public static void printTimeConvert(double time)
        {
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("  Tempo para conversão: " + time + " Milisegundos.             ");
            Console.WriteLine("------------------------------------------------------");
        }

        public static void printTimeValidate(double time, bool valido)
        {
            Console.WriteLine("------------------------------------------------------");
            if(valido)
                Console.WriteLine(" Usuário correto, validado em " + time + " Milisegundos    ");
            else
                Console.WriteLine(" Usuário incorreto, validado em " + time + " Milisegundos    ");
            Console.WriteLine("------------------------------------------------------");
        }

        public static void userText(bool confirmacao)
        {
            if (!confirmacao)
            {
                Console.WriteLine("------------------------------------------------------");
                Console.WriteLine("|           Aperte Enter para continuar.             |");
                Console.WriteLine("------------------------------------------------------");
                Console.ReadLine();
                Console.Clear();
            }

            Console.WriteLine("      Base atual convertida em: " + baseAtual);
            Console.WriteLine("|                                                    |");
            Console.WriteLine("------------------------------------------------------");
            Console.WriteLine("|              O que deseja fazer ?                  |");
            Console.WriteLine("|                                                    |");
            Console.WriteLine("|           1 - Converter para MD5                   |");
            Console.WriteLine("|           2 - Converter para SHA1                  |");
            Console.WriteLine("|           3 - Converter para SHA256                |");
            Console.WriteLine("|                                                    |");
            Console.WriteLine("|           4 - Converter para MD5 (Salted)          |");
            Console.WriteLine("|           5 - Converter para SHA1 (Salted)         |");
            Console.WriteLine("|           6 - Converter para SHA256 (Salted)       |");
            Console.WriteLine("|                                                    |");
            Console.WriteLine("|           7 - Validar conversão MD5                |");
            Console.WriteLine("|           8 - Validar conversão SHA1               |");
            Console.WriteLine("|           9 - Validar conversão SHA256             |");
            Console.WriteLine("|                                                    |");
            Console.WriteLine("|           10 - Validar conversão MD5 (Salted)      |");
            Console.WriteLine("|           11 - Validar conversão SHA1 (Salted)     |");
            Console.WriteLine("|           12 - Validar conversão SHA256 (Salted)   |");
            Console.WriteLine("|                                                    |");
            Console.WriteLine("|           13 - Mostrar base atual                  |");
            Console.WriteLine("|           14 - Exportar base atual                 |");
            Console.WriteLine("|           0 - Sair                                 |");
            Console.WriteLine("------------------------------------------------------");

        }

        public static User readUser()
        {
            var usr = new User();

            Console.WriteLine("Usuario:");
            usr.user = Console.ReadLine();

            Console.WriteLine("Senha:");
            usr.password = Console.ReadLine();

            return usr;
        }

        public static void execute()
        {
            var exeute_ = true;
            var primeiro = true;

            iniciaLista();

            while (exeute_)
            {
                
                userText(primeiro);
                primeiro = false;
                bool valido;
                Stopwatch watch;
                User usr = null;

                switch (Console.ReadLine())
                {
                    case "1":
                        iniciaLista();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        Converters.MD5_(users);
                        watch.Stop();
                        baseAtual = "MD5";
                        printTimeConvert(watch.Elapsed.TotalMilliseconds);
                        break;

                    case "2":
                        iniciaLista();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        Converters.SHA1_(users);
                        watch.Stop();
                        baseAtual = "SHA1";
                        printTimeConvert(watch.Elapsed.TotalMilliseconds);
                        break;

                    case "3":
                        iniciaLista();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        Converters.SHA256_(users);
                        watch.Stop();
                        baseAtual = "SHA256";
                        printTimeConvert(watch.Elapsed.TotalMilliseconds);
                        break;

                    case "4":
                        iniciaLista();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        Converters.MD5_(users, true);
                        watch.Stop();
                        baseAtual = "MD5 (Salted)";
                        printTimeConvert(watch.Elapsed.TotalMilliseconds);
                        break;

                    case "5":
                        iniciaLista();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        Converters.SHA1_(users, true);
                        watch.Stop();
                        baseAtual = "SHA1 (Salted)";
                        printTimeConvert(watch.Elapsed.TotalMilliseconds);
                        break;

                    case "6":
                        iniciaLista();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        Converters.SHA256_(users, true);
                        watch.Stop();
                        baseAtual = "SHA256 (Salted)";
                        printTimeConvert(watch.Elapsed.TotalMilliseconds);
                        break;

                    case "7":
                        usr = readUser();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        valido = Validate.MD5_(users, usr);
                        watch.Stop();                        
                        printTimeValidate(watch.Elapsed.TotalMilliseconds, valido);
                        break;

                    case "8":
                        usr = readUser();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        valido = Validate.SHA1_(users, usr);
                        watch.Stop();
                        printTimeValidate(watch.Elapsed.TotalMilliseconds, valido);
                        break;

                    case "9":
                        usr = readUser();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        valido = Validate.SHA256_(users, usr);
                        watch.Stop();
                        printTimeValidate(watch.Elapsed.TotalMilliseconds, valido);
                        break;

                    case "10":
                        usr = readUser();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        valido = Validate.MD5_(users, usr, true);
                        watch.Stop();
                        printTimeValidate(watch.Elapsed.TotalMilliseconds, valido);
                        break;

                    case "11":
                        usr = readUser();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        valido = Validate.SHA1_(users, usr, true);
                        watch.Stop();
                        printTimeValidate(watch.Elapsed.TotalMilliseconds, valido);
                        break;

                    case "12":
                        usr = readUser();
                        watch = System.Diagnostics.Stopwatch.StartNew();
                        valido = Validate.SHA256_(users, usr, true);
                        watch.Stop();
                        printTimeValidate(watch.Elapsed.TotalMilliseconds, valido);
                        break;

                    case "13":
                        imprimirBase();
                        break;

                    case "14":
                        exportBase();
                        break;
                    case "0":
                        exeute_ = false;
                        break;

                    default:
                        Console.WriteLine("------------------------------------------------------");
                        Console.WriteLine("|           Insira uma opção valida                  |");
                        break;
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Interact.execute();
        }
    }
}
