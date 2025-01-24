// See https://aka.ms/new-console-template for more information

using PySharpCompiler.Classes;
using PySharpCompiler.Components;
using System.Runtime.InteropServices;
using System.Text;

internal static class Program
{
    static void RunCommandLine(string code, bool showTree)
    {
        byte[] codeBytes = Encoding.UTF8.GetBytes(code);
        using (var memoryStream = new MemoryStream(codeBytes))
        using (var streamReader = new StreamReader(memoryStream))
        {
            CodeReader codeReader = new CodeReader(streamReader);
            Lexer lexer = new Lexer(codeReader);
            Parser parser = new Parser(lexer);

            try
            {
                var program = parser.ParseProgram();

                if (showTree)
                {
                    VisualizeVisitor printer = new VisualizeVisitor();
                    program.Visit(printer);
                }

                Interpreter interpreter = new Interpreter();
                var result = program.Visit(interpreter);

                if (result != null)
                {
                    Console.WriteLine(result.GetValue());
                }
            } catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.Message}");
                Console.WriteLine("The Interpreter has encountered an error in the code, try running it again after fixing it.");
            }
        }
    }

    static void RunFile(string path, bool showTree)
    {

        if (path.Substring(Math.Max(0, path.Length - 5)) != ".pycs")
        {
            Console.WriteLine("\nInvalid file type! Provide a .pycs file.\n");
            return;
        }

        using (var streamReader = new StreamReader(path))
        {
            CodeReader codeReader = new CodeReader(streamReader);
            Lexer lexer = new Lexer(codeReader);
            Parser parser = new Parser(lexer);

            try
            {
                var program = parser.ParseProgram();

                if (showTree)
                {
                    VisualizeVisitor printer = new VisualizeVisitor();
                    program.Visit(printer);
                }

                Interpreter interpreter = new Interpreter();
                var result = program.Visit(interpreter);

                if (result != null)
                {
                    Console.WriteLine(result.GetValue());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\n{ex.Message}");
                Console.WriteLine("The Interpreter has encountered an error in the code, try running it again after fixing it.");
            }
        }
    }

    static void Main(string[] args)
    {
        string usage = "\nUsage: pycs [options]\r\n\r\nOptions:\r\n\r\n--f [file path]\t\tRun code from a .pycs file.\r\n    [code]\t\tRun code wrapped in \"\".\r\n--tree\t\t\tPrint syntax tree after parsing.\n";
        if (args.Length == 0)
        {
            Console.WriteLine(usage);
            return;
        }

        if (args.Length == 1) {
            RunCommandLine(args[0], false);
            return;
        }

        if (args.Length == 2)
        {
            if (args[0] == "--tree")
            {
                RunCommandLine(args[1], true);
            } else if (args[0] == "--f")
            {
                RunFile(args[1], false);
            } else
            {
                Console.WriteLine("Invalid arguments");
                Console.WriteLine(usage);
            }

            return;
        }

        if (args.Length == 3)
        {
            if (args.Contains("--f") && args.Contains("--tree"))
            {
                RunFile(args[2], true);
            } else
            {
                Console.WriteLine("Invalid arguments");
                Console.WriteLine(usage);
            }
            return;
        } else
        {
            Console.WriteLine("Invalid arguments");
            Console.WriteLine(usage);
        }
    }
}