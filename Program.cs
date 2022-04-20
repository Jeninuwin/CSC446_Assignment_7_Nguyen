/// <summary>
/// Name: Jenny Nguyen 
/// Assignment: 6
/// Description: Add the following grammar rules to your parser. Add the appropriate actions to your parser to check for undeclared 
/// variables used in an assignment statement
/// </summary>
/// </summary>
using System;

namespace CSC446_Assignment_7_Nguyen
{
    /// <summary>
    /// Defines the <see cref="Program" />.
    /// </summary>
    public static class Program
    {
        public static void Main(string[] args)
        {
            //start:
            Lexie.LexicalAnalyzer(args);
            Console.WriteLine("Lexical Analyzer completed. Commencing Parser.\n");
            SymbolTable.symList();
            Parser.Parse();
            Console.WriteLine("\nParser completed completed.");


            //string continueProgram;

            //cp:
            //    Console.WriteLine("\nDo you want to enter another file? Enter Y for yes and N for to exit the program");
            //    continueProgram = Console.ReadLine();

            //    if (continueProgram.ToLower() == "n")
            //    {
            //        System.Environment.Exit(0);
            //    }
            //    else if (continueProgram.ToLower() == "y")
            //    {
            //        Console.Clear();
            //        Lexie.MatchTokens.Clear();
            //        Lexie.Token.Equals(null);
            //        Lexie.Lexeme.Equals(null);

            //        Lexie.counting = 0;
            //        Parser.increments = 0;
            //        SymbolTable.symboltable = null;
            //        goto start;
            //    }
            //    else
            //    {
            //        Console.WriteLine("Invalid Response.");
            //        goto cp;
            //    }


        }
    }
}