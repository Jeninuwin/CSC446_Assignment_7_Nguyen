/// <summary>
/// Name: Jenny Nguyen 
/// Assignment: 6
/// Description: Add the following grammar rules to your parser. Add the appropriate actions to your parser to check for undeclared 
/// variables used in an assignment statement
/// </summary>
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CSC446_Assignment_7_Nguyen
{
    /// <summary>
    /// Defines the <see cref="Parser" />.
    /// </summary>
    public class Parser
    {
        public static int currentOffset = 0;
        public static int charOffset = 1;
        public static int floatOffset = 4;
        public static int intOffset = 2;
        public static int totalOffset;
        public static int depth = 0;
        public static int tempies;
        public static string varString = "_BP-";
        public static string prevBP = "";
        public static int previousCounter;
        public static int varCounter = 2;
        public static string functionTemp;
        public static string tempReturn = "";
        public static string tempinfo = "";
        public static StreamWriter writerfile;
        public static string returnString = "_AX";
        public static string mulopTemp = "";
        public static string addopTemp = "";
        public static string tempFile = Lexie.tempFileName;
        public static SymbolTable.entryTable val = new SymbolTable.entryTable();

        public SymbolTable sym = new SymbolTable();
        /// <summary>
        /// Defines the increments  while setting it to 0.
        /// </summary>
        public static int increments = 0;
        public static string temp = "";
        /// <summary>
        /// The Parse will grab the MatchToken list and will call Prog to start the parsing
        /// </summary>
        public static void Parse()
        {
            //Console.WriteLine(temp.PadRight(22, ' ') + "Tokens");
            //Console.WriteLine(temp.PadRight(14, ' ') + "________________________");
            //for (int i = 0; i < Lexie.counting; i++)
            //{
            //    Console.WriteLine(Lexie.MatchTokens[i].PadLeft(28, ' '));
            //}

            int index = tempFile.IndexOf(".");
            if(index >= 0)
            {
                tempFile = tempFile.Substring(0, index);
            }

            writerfile = new StreamWriter(tempFile + ".tac");

            Prog();
            val = SymbolTable.lookUp("main");
            if (val.lexeme != "main")
            {
                Console.WriteLine("Error: Main was not found int the function.");
                writerfile.Close();
                Environment.Exit(1);
            }

            writerfile.Close();
        }

        /// <summary>
        /// The Prog sees if the the current MatchToken equals either int, float, or char. If it doesn't equal any of those go to eofft
        /// PROG -> TYPE idt REST PROG |
        //const idt = num; PROG |
        public static void Prog()
        {
            bool done = false;

            while (done == false) //if done is true it will exit the loop and prog will be completed
            {
                switch (Lexie.MatchTokens[increments])
                {
                    case "intt":
                    case "floatt":
                    case "chart":
                        {
                            //offset increase base on type 
                            if (Lexie.MatchTokens[increments] == "intt")
                            {
                                currentOffset = intOffset;
                            }
                            else if (Lexie.MatchTokens[increments] == "floatt")
                            {
                                currentOffset = floatOffset;
                            }
                            else
                            {
                                currentOffset = charOffset;
                            }

                            totalOffset += currentOffset;

                            increments++;

                            switch (Lexie.MatchTokens[increments]) //this will see if the MatchTokens will match "idt" if it does call Rest, else error
                            {
                                case "idt":
                                    {
                                        if (Lexie.MatchTokens[increments] == "commat")
                                        {
                                            //store after the value into a temp                                       
                                            //lookUp list for dups 
                                            val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                                            if (val.lexeme != Lexie.LexemeString[increments])
                                            {
                                                //insert after storing the value
                                                SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Variable);
                                            }

                                            else if (val.lexeme == Lexie.LexemeString[increments] && val.depth != depth)
                                            {
                                                SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Variable);
                                            }


                                            else if (val.lexeme == Lexie.LexemeString[increments] && val.depth == depth)
                                            {
                                                Console.WriteLine("Error: " + val.lexeme + " was found when searching for duplicates. The depth found at:" + depth);
                                                Environment.Exit(1);
                                                break;

                                            }
                                        }
                                        else
                                        {
                                            //store after the value into a temp                                       
                                            //lookUp list for dups 
                                            val = SymbolTable.lookUp(Lexie.LexemeString[increments]);

                                            if (val.lexeme != Lexie.LexemeString[increments])
                                            {
                                                increments++; //looks ahead to determine if function or variable
                                                if (Lexie.MatchTokens[increments] == "commat" || Lexie.MatchTokens[increments] == "semit")
                                                {
                                                    increments--;
                                                    SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Variable);
                                                }
                                                else
                                                {
                                                    increments--;
                                                    //insert after storing the value
                                                    SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Function);
                                                }


                                            }

                                            else if (val.lexeme == Lexie.LexemeString[increments] && val.depth != depth)
                                            {
                                                increments++; //looks ahead to determine if function or variable
                                                if (Lexie.MatchTokens[increments] == "commat" || Lexie.MatchTokens[increments] == "semit")
                                                {
                                                    increments--;
                                                    SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Variable);
                                                }
                                                else
                                                {
                                                    increments--;
                                                    //insert after storing the value
                                                    SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Function);
                                                }
                                            }


                                            else if (val.lexeme == Lexie.LexemeString[increments] && val.depth == depth)
                                            {
                                                Console.WriteLine("Error: " + val.lexeme + " was found when searching for duplicates. The depth found at:" + depth);
                                                Environment.Exit(1);
                                                break;

                                            }

                                        }
                                        increments++;
                                        Rest();
                                        break;
                                    }
                                case "constt":
                                    {
                                        //store after the value into a temp                                       
                                        //lookup list for dups 
                                        val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                                        if (val.lexeme != Lexie.LexemeString[increments])
                                        {
                                            //insert after storing the value
                                            SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Constant);
                                        }

                                        else if (val.lexeme == Lexie.LexemeString[increments] && val.depth != depth)
                                        {
                                            SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Constant);
                                        }


                                        else if (val.lexeme == Lexie.LexemeString[increments] && val.depth == depth)
                                        {
                                            Console.WriteLine("Error: " + val.lexeme + " was found when searching for duplicates. The depth found at:" + depth);
                                            Environment.Exit(1);
                                            break;

                                        }
                                        increments++;
                                        Prog();
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                                        Environment.Exit(1);
                                        break;
                                    }
                            }
                            break;
                        }
                    case "constt":
                        increments++;
                        switch (Lexie.MatchTokens[increments])
                        {
                            case "idt":
                                {
                                    //store after the value into a temp                                       
                                    //lookup list for dups 
                                    val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                                    if (val.lexeme != Lexie.LexemeString[increments])
                                    {
                                        //insert after storing the value
                                        SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Constant);
                                    }

                                    else if (val.lexeme == Lexie.LexemeString[increments] && val.depth != depth)
                                    {
                                        SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Constant);
                                    }


                                    else if (val.lexeme == Lexie.LexemeString[increments] && val.depth == depth)
                                    {
                                        Console.WriteLine("Error: " + val.lexeme + " was found when searching for duplicates. The depth found at:" + depth);
                                        Environment.Exit(1);
                                        break;

                                    }
                                    increments++;
                                    switch (Lexie.MatchTokens[increments])
                                    {
                                        case "assignopt":
                                            {
                                                increments++;
                                                switch (Lexie.MatchTokens[increments])
                                                {
                                                    case "numt":
                                                        {
                                                            //check if float or int then increase offset 
                                                            if (Lexie.MatchTokens[increments] == "intt")
                                                            {
                                                                totalOffset += intOffset;
                                                            }
                                                            else if (Lexie.MatchTokens[increments] == "floatt")
                                                            {
                                                                totalOffset += floatOffset;
                                                            }
                                                            increments++;
                                                            switch (Lexie.MatchTokens[increments])
                                                            {
                                                                case "semit":
                                                                    {
                                                                        increments++;
                                                                        Prog();
                                                                        break;
                                                                    }
                                                                case "eoftt":
                                                                    done = true;
                                                                    break;
                                                                default:
                                                                    {
                                                                        done = true;
                                                                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'semit'. Not correct Grammar.");
                                                                        Environment.Exit(1);
                                                                        break;
                                                                    }
                                                            }
                                                            break;
                                                        }
                                                    case "eoftt":
                                                        done = true;
                                                        break;
                                                    default:
                                                        {
                                                            done = true;
                                                            Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'numt'. Not correct Grammar.");
                                                            Environment.Exit(1);
                                                            break;
                                                        }
                                                }
                                                break;
                                            }
                                        case "eoftt":
                                            done = true;
                                            break;
                                        default:
                                            {
                                                done = true;
                                                Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'assignopt'. Not correct Grammar.");
                                                Environment.Exit(1);
                                                break;
                                            }
                                    }
                                    break;
                                }
                            case "eoftt":
                                done = true;
                                break;
                            default:
                                {
                                    done = true;
                                    Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                                    Environment.Exit(1);
                                    break;
                                }
                        }
                        break;
                    case "eoftt":
                        done = true;
                        break;
                    default:
                        {
                            done = true;
                            Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'intt', 'floatt', 'constt' or 'chart'. Not correct Grammar.");
                            Environment.Exit(1);
                            break;
                        }
                }

            }
        }

        /// <summary>
        /// The Rest will see if the MatchTokens fit either lparent, commat, or semit.
        /// </summary>
        public static void Rest()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "lparent":
                    {
                        writerfile.WriteLine("PROC " + Lexie.LexemeString[increments - 1]);
                        functionTemp = Lexie.LexemeString[increments - 1];
                        prevBP = "";
                        mulopTemp = "";
                        addopTemp = "";
                        tempinfo = "";
                        varCounter = 2;
                        //increase depth
                        depth++;
                        increments++;
                        Paramlist();

                        if (Lexie.MatchTokens[increments] == "rparent")
                        {
                            increments++;
                            Compound();
                        }
                        break;
                    }
                case "semit":
                case "commat":
                    {
                        IDTail();
                        if (Lexie.MatchTokens[increments] == "semit")
                        {
                            increments++;
                        }

                        else
                        {
                            Console.WriteLine("Error:  " + Lexie.MatchTokens[increments] + " was found when searching for 'semit'. Not correct Grammar.");
                            Environment.Exit(1);
                        }
                        break;
                    }
                //case "rparent":
                //    break;
                default:
                    {
                        Console.WriteLine("Error:  " + Lexie.MatchTokens[increments] + " was found when searching for 'semit', 'commat', or 'lparent'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        /// <summary>
        /// The Paramlist will see if the MatchTokens fit either int, float, char, or rparent
        /// </summary>
        public static void Paramlist()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "intt":
                case "floatt":
                case "chart":
                    {
                        //increase offset 
                        if (Lexie.MatchTokens[increments] == "intt")
                        {
                            currentOffset = intOffset;
                        }
                        else if (Lexie.MatchTokens[increments] == "floatt")
                        {
                            currentOffset = floatOffset;
                        }
                        else
                        {
                            currentOffset = charOffset;
                        }

                        totalOffset += currentOffset;

                        increments++;
                        if (Lexie.MatchTokens[increments] == "idt")
                        {
                            val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                            if (val.lexeme != Lexie.LexemeString[increments])
                            {
                                //insert after storing the value
                                SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Variable);
                            }

                            else if (val.lexeme == Lexie.LexemeString[increments] && val.depth != depth)
                            {
                                SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Variable);
                            }


                            else if (val.lexeme == Lexie.LexemeString[increments] && val.depth == depth)
                            {
                                Console.WriteLine("Error: " + val.lexeme + " was found when searching for duplicates. The depth found at:" + depth);
                                Environment.Exit(1);
                                break;

                            }
                            increments++;
                            ParamTail();
                        }

                        else
                        {
                            Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                            Environment.Exit(1);
                        }
                        break;
                    }
                case "rparent":
                    break;
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'rparent'. Not correct Grammar."); Environment.Exit(1);
                        break;
                    }
            }
        }


        /// <summary>
        /// The ParamTail will see if the MatchTokens fit either int, float, char, or rparent
        /// </summary>
        public static void ParamTail()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "commat":
                    {
                        increments++;

                        if (Lexie.MatchTokens[increments] == "intt" | Lexie.MatchTokens[increments] == "floatt" | Lexie.MatchTokens[increments] == "chart")
                        {
                            if (Lexie.MatchTokens[increments] == "intt")
                            {
                                currentOffset = intOffset;
                            }
                            else if (Lexie.MatchTokens[increments] == "floatt")
                            {
                                currentOffset = floatOffset;
                            }
                            else
                            {
                                currentOffset = charOffset;
                            }

                            totalOffset += currentOffset;

                            increments++;

                            switch (Lexie.MatchTokens[increments]) //this will see if the MatchTokens will match "idt" if it does call Rest, else error
                            {
                                case "idt":
                                    {
                                        //lookup for dups and insert
                                        val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                                        if (val.lexeme != Lexie.LexemeString[increments])
                                        {
                                            //insert after storing the value
                                            SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Variable);
                                        }

                                        else if (val.lexeme == Lexie.LexemeString[increments] && val.depth != depth)
                                        {
                                            SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Variable);
                                        }


                                        else if (val.lexeme == Lexie.LexemeString[increments] && val.depth == depth)
                                        {
                                            Console.WriteLine("Error: " + val.lexeme + " was found when searching for duplicates. The depth found at:" + depth);
                                            Environment.Exit(1);
                                            break;

                                        }
                                        increments++;
                                        ParamTail();
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                                        Environment.Exit(1);
                                        break;
                                    }
                            }
                        }

                        else
                        {
                            Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'intt', 'floatt', or 'chart'. Not correct Grammar.");
                            Environment.Exit(1);

                        }
                        break;
                    }
                case "rparent":
                    break;
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'rparent'. Not correct Grammar."); Environment.Exit(1);
                        Environment.Exit(1);
                        break;
                    }

            }
        }

        /// <summary>
        /// The Compound will see if the MatchTokens start with a '{' if not then it's an error 
        /// </summary>
        public static void Compound()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "openCurlyParent":
                    {
                        increments++;
                        Decl();
                        StatList();
                        Ret_Stat();

                        if (Lexie.MatchTokens[increments] == "closeCurlyParent")
                        {
                            writerfile.WriteLine("ENDP " + functionTemp);
                            writerfile.WriteLine("\n");
                            //decrease depth 
                            if (depth > 0)
                            {
                                SymbolTable.writeTable(depth);
                            }

                            SymbolTable.deleteDepth(depth);

                            depth--;
                            SymbolTable.writeTable(depth);

                            increments++;
                            Prog();
                            if (Lexie.MatchTokens[increments] == "eoftt")
                            {
                                break;
                            }
                        }

                        else
                        {
                            Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'closeCurlyParent'. Not correct Grammar.");
                            Environment.Exit(1);

                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'openCurlyParent'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        /// <summary>
        /// The Decl will see if the MatchToken matches either int, float, char, or '}'. If it doesn't then it will throw an error
        /// DECL -> TYPE IDLIST |
        //const idt = num; DECL |
        /// </summary>
        public static void Decl()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "intt":
                case "floatt":
                case "chart":
                    {
                        //offsets increase 
                        if (Lexie.MatchTokens[increments] == "intt")
                        {
                            currentOffset = intOffset;
                        }
                        else if (Lexie.MatchTokens[increments] == "floatt")
                        {
                            currentOffset = floatOffset;
                        }
                        else
                        {
                            currentOffset = charOffset;
                        }

                        totalOffset += currentOffset;

                        increments++;
                        IDList();
                        break;
                    }
                case "closeCurlyParent":
                    {
                        ////decrease depth 
                        //if (depth > 0)
                        //{
                        //    SymbolTable.writeTable(depth);
                        //}
                        //SymbolTable.deleteDepth(depth);
                        //depth--;
                        //SymbolTable.writeTable(depth);
                        break;
                    }
                case "idt":
                    {

                        break;
                    }
                case "constt":
                    {
                        increments++;
                        switch (Lexie.MatchTokens[increments])
                        {
                            case "idt":
                                {
                                    //lookup for dups and insert
                                    val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                                    if (val.lexeme != Lexie.LexemeString[increments])
                                    {
                                        //insert after storing the value
                                        SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Constant);
                                    }

                                    else if (val.lexeme == Lexie.LexemeString[increments] && val.depth != depth)
                                    {
                                        SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Constant);
                                    }

                                    else if (val.lexeme == Lexie.LexemeString[increments] && val.depth == depth)
                                    {
                                        Console.WriteLine("Error: " + val.lexeme + " was found when searching for duplicates. The depth found at:" + depth);
                                        Environment.Exit(1);
                                        break;

                                    }
                                    increments++;
                                    switch (Lexie.MatchTokens[increments])
                                    {
                                        case "assignopt":
                                            {
                                                increments++;
                                                switch (Lexie.MatchTokens[increments])
                                                {
                                                    case "numt":
                                                        {
                                                            //check if float or int then increase offset 
                                                            if (Lexie.MatchTokens[increments] == "intt")
                                                            {
                                                                totalOffset += intOffset;
                                                            }
                                                            else if (Lexie.MatchTokens[increments] == "floatt")
                                                            {
                                                                totalOffset += floatOffset;
                                                            }

                                                            increments++;
                                                            switch (Lexie.MatchTokens[increments])
                                                            {
                                                                case "semit":
                                                                    {
                                                                        increments++;
                                                                        Decl();
                                                                        break;
                                                                    }
                                                                case "eoftt":
                                                                    break;
                                                                default:
                                                                    {
                                                                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'semit'. Not correct Grammar.");
                                                                        Environment.Exit(1);
                                                                        break;
                                                                    }
                                                            }
                                                            break;
                                                        }
                                                    case "eoftt":
                                                        break;
                                                    default:
                                                        {
                                                            Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'numt'. Not correct Grammar.");
                                                            Environment.Exit(1);
                                                            break;
                                                        }
                                                }
                                                break;
                                            }
                                        case "eoftt":
                                            break;
                                        default:
                                            {
                                                Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'assignopt'. Not correct Grammar.");
                                                Environment.Exit(1);
                                                break;
                                            }
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'intt', 'floatt', 'const' or 'chart'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }

        }

        /// <summary>
        /// The IDList will see if the MatchToken Matches with idt and semit after. if not then an error has occurred
        /// </summary>
        public static void IDList()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "idt":
                    {
                        //lookup for dups and insert
                        val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                        if (val.lexeme != Lexie.LexemeString[increments])
                        {
                            //insert after storing the value
                            SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Variable);
                        }

                        else if (val.lexeme == Lexie.LexemeString[increments] && val.depth != depth)
                        {
                            SymbolTable.insert(Lexie.LexemeString[increments], Lexie.MatchTokens[increments - 1], depth, SymbolTable.RecordEnum.Variable);
                        }


                        else if (val.lexeme == Lexie.LexemeString[increments] && val.depth == depth)
                        {
                            Console.WriteLine("Error: " + val.lexeme + " was found when searching for duplicates. The depth found at:" + depth);
                            Environment.Exit(1);
                            break;

                        }
                        increments++;
                        IDTail();

                        switch (Lexie.MatchTokens[increments])
                        {
                            case "semit":
                                {
                                    increments++;
                                    Decl();
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'semit'. Not correct Grammar.");
                                    Environment.Exit(1);
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        /// <summary>
        /// The IDTail will see if the MatchToken matches with Commat or Semmit. If it doesn't then it's an error
        /// </summary>
        public static void IDTail()
        {
            string position;
            switch (Lexie.MatchTokens[increments])
            {
                case "commat":
                    {
                        if (currentOffset == intOffset)
                        {
                            position = "intt";
                        }
                        else if (currentOffset == charOffset)
                        {
                            position = "chart";
                        }
                        else
                        {
                            position = "floatt";
                        }

                        //increase sym tab offset += local current offset
                        totalOffset += currentOffset;



                        increments++;
                        switch (Lexie.MatchTokens[increments])
                        {
                            case "idt":
                                {
                                    //lookup for dups and insert
                                    val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                                    if (val.lexeme != Lexie.LexemeString[increments])
                                    {
                                        //insert after storing the value
                                        SymbolTable.insert(Lexie.LexemeString[increments], position, depth, SymbolTable.RecordEnum.Variable);
                                    }

                                    else if (val.lexeme == Lexie.LexemeString[increments] && val.depth != depth)
                                    {
                                        SymbolTable.insert(Lexie.LexemeString[increments], position, depth, SymbolTable.RecordEnum.Variable);
                                    }


                                    else if (val.lexeme == Lexie.LexemeString[increments] && val.depth == depth)
                                    {
                                        Console.WriteLine("Error: " + val.lexeme + " was found when searching for duplicates. The depth found at:" + depth);
                                        Environment.Exit(1);
                                        break;

                                    }

                                    increments++;
                                    IDTail();
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                                    Environment.Exit(1);
                                    break;
                                }
                        }
                        break;
                    }
                case "semit":
                    {
                        break;

                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'semit'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        /// <summary>
        /// StatList -> Statement ; StatList|
        /// </summary>
        public static void StatList()
        {
            if (Lexie.MatchTokens[increments] == "idt")
            {
                Statement();

                switch (Lexie.MatchTokens[increments])
                {
                    case "semit":
                        {
                            increments++;
                            StatList();
                            break;
                        }
                    case "closeCurlyParent":
                        {
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'semit'. Not correct Grammar.");
                            Environment.Exit(1);
                            break;
                        }
                }

            }
            else if (Lexie.MatchTokens[increments] == "closeCurlyParent" || Lexie.MatchTokens[increments] == "returnt")
            {
                //do nothing || exits program
            }
            else
            {
                Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt' or 'closeCurlyParent'. Not correct Grammar.");
                Environment.Exit(1);
            }


        }

        /// <summary>
        /// Statement -> AssignStat | IOStat
        /// </summary>
        public static void Statement()
        {

            switch (Lexie.MatchTokens[increments])
            {
                case "idt":
                    {
                        AssignStat();
                        break;
                    }
                case "eoftt":
                    {
                        break;
                    }
                case "IO": //this is a placement till we get more information
                    {
                        IOStat();
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt', 'eoftt', or 'IO'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        /// <summary>
        /// AssignStat -> idt = Expr
        /// </summary>
        public static void AssignStat()
        {

            switch (Lexie.MatchTokens[increments])
            {

                case "idt":
                    {
                        //lookup for dups and insert
                        val = SymbolTable.lookUp(Lexie.LexemeString[increments]);
                        //if (val.lexeme != Lexie.LexemeString[increments])
                        //{
                        //    //insert after storing the value
                        //    SymbolTable.insert(Lexie.LexemeString[increments], position, depth, SymbolTable.RecordEnum.Variable);
                        //}

                        //else if (val.lexeme == Lexie.LexemeString[increments] && val.depth != depth)
                        //{
                        //    SymbolTable.insert(Lexie.LexemeString[increments], position, depth, SymbolTable.RecordEnum.Variable);
                        //}


                        if (val.lexeme != Lexie.LexemeString[increments])
                        {
                            Console.WriteLine("Error: " + Lexie.LexemeString[increments] + " was not found when searching for declaration. The depth found at:" + depth);
                            Environment.Exit(1);
                            break;

                        }
                        else
                        {

                            increments++;


                            switch (Lexie.MatchTokens[increments])
                            {
                                case "assignopt":
                                    {
                                        tempinfo += Lexie.LexemeString[increments];
                                        addopTemp += Lexie.LexemeString[increments];

                                        increments += 2;

                                        if (Lexie.MatchTokens[increments] == "lparent")
                                        {
                                            increments-=2;
                                            FuncCall();
                                        }
                                        else
                                        {
                                            increments -= 2;
                                            Expr();
                                        }
                                        break;
                                    }
                                default:
                                    {
                                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'assignopt'. Not correct Grammar.");
                                        Environment.Exit(1);
                                        break;
                                    }
                            }
                            break;
                        }

                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        public static void FuncCall()
        {
            increments++;
            switch (Lexie.MatchTokens[increments])
            {
                case "idt":
                    {
                        writerfile.WriteLine();
                        writerfile.WriteLine("Call " + Lexie.LexemeString[increments]);
                        increments++;
                        switch (Lexie.MatchTokens[increments])
                        {
                            case "lparent":
                                {
                                    increments++;
                                    writerfile.WriteLine("Push " + Lexie.LexemeString[increments]);
                                    Params();
                                    writerfile.WriteLine("Push " + Lexie.LexemeString[increments - 1]);
                                    if (Lexie.MatchTokens[increments] == "rparent")
                                    {
                                        increments++;
                                    }
                                    break;
                                }
                        }
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        public static void Params()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "idt":
                    {
                        increments++;
                        ParamsTail();

                        break;
                    }
                case "numt":
                    {
                        ParamsTail();
                        break;
                    }
                case "eofft":
                    {
                        break;
                    }
                default:
                    {
                        break;
                    }
            }
        }
        /// <summary>
        /// IOStat -> 
        /// </summary>
        public static void IOStat()
        {
            if (Lexie.MatchTokens[increments] == "eoftt")
            {
                //do nothing for now
            }
        }

        /// <summary>
        /// Expr -> Realtion
        /// </summary>
        public static void Expr()
        {
            Relation();
        }

        /// <summary>
        /// Realtion -> SimpleExpr
        /// </summary>
        public static void Relation()
        {
            SimpleExpr();
        }

        /// <summary>
        /// SimpleExpr -> SignOp Term MoreTerm
        /// </summary>
        public static void SimpleExpr()
        {
            SignOp();
            Term();
            MoreTerm();
        }

        /// <summary>
        /// MoreTerm -> Addop Term MoreTerm 
        /// </summary>
        public static void MoreTerm()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "semit":
                case "mulopt":
                    {
                        break;
                    }
                case "rparent":
                    {
                        increments++;
                        break;
                    }
                default:
                    {
                        Addop();
                        Term();
                        MoreTerm();
                        break;
                    }
            }

            if (Lexie.MatchTokens[increments] == "eoftt")
            {
                //do nothing for now
            }
        }

        public static void ParamsTail()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "commat":
                    {
                        increments++;

                        switch (Lexie.MatchTokens[increments]) //this will see if the MatchTokens will match "idt" if it does call Rest, else error
                        {
                            case "idt":
                            case "numt":
                                {
                                    increments++;
                                    ParamTail();
                                    break;
                                }
                            case "rparent":
                                {
                                    increments++;
                                    break;
                                }
                            default:
                                {
                                    Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'idt'. Not correct Grammar.");
                                    Environment.Exit(1);
                                    break;
                                }
                        }
                    }
                    break;
                case "rparent":
                    break;
            default:
                    {
                Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'rparent'. Not correct Grammar."); Environment.Exit(1);
                Environment.Exit(1);
                break;
            }

        }
          
        }
        public static void Ret_Stat()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "returnt":
                    {
                        Expr();
                        if (Lexie.MatchTokens[increments] == "semit")
                        {
                            increments++;
                            break;
                        }
                        break;
                    }
            }
        }
        /// <summary>
        /// Term -> Factor MoreFactor
        /// </summary>
        public static void Term()
        {
            Factor();
            MoreFactor();
        }

        /// <summary>
        /// MoreFactor -> Mulop Factor MoreFactor 
        /// </summary>
        public static void MoreFactor()
        {

            switch (Lexie.MatchTokens[increments])
            {
                case "semit":
                    {
                        writerfile.WriteLine();

                        if (Lexie.LexemeString[increments -2] == "return")
                        {
                            writerfile.WriteLine(returnString + "=" + Lexie.LexemeString[increments - (tempReturn.Length)]);
                        }
                        else
                        {
                            writerfile.WriteLine(varString + (varCounter) + tempinfo);
                            writerfile.WriteLine(Lexie.LexemeString[increments - (tempinfo.Length + 1)] + "=" + varString + varCounter);
                            //writerfile.WriteLine(Lexie.LexemeString[increments - (tempinfo.Length + 1)] + "=" + mulopTemp);

                        }

                        writerfile.WriteLine();
                        tempinfo = "";
                        mulopTemp = "";
                        addopTemp = "";
                        tempReturn = "";
                        varCounter+=2;
                        break;
                    }
                case "addopt":
                    {
                        break;
                    }
                case "rparent":
                    {
                        if(tempinfo.Length >= 6)
                        {
                            if(tempinfo.Contains("+") || tempinfo.Contains("-"))
                            {
                                addopTemp = "";
                                addopTemp += tempinfo;
                                prevBP = varString + varCounter;

                                increments--;
                                string[] temptest = addopTemp.Split("+");

                                temptest[0] = temptest[0].Replace("=", "");
                                temptest[1] = prevBP;
                                addopTemp = "="+ temptest[1] + "-" +temptest[0];
                                // addopTemp = String.Concat(temptest);
                                //increment to bp_6
                                varCounter += 2;
                                writerfile.WriteLine(varString + (varCounter) + addopTemp);
                                writerfile.WriteLine();
                                prevBP = varString + varCounter;
                                tempinfo = temptest[0] + "+" + prevBP;
                                varCounter -= 2;


                            }
                        }
                        increments++;
                        break;
                    }
                default:
                    {
                        Mulop();
                        Factor();
                        MoreFactor();
                        break;
                    }
            }

        }

        /// <summary>
        /// Factor -> id | num | ( Expr )
        /// </summary>
        public static void Factor()
        {

            switch (Lexie.MatchTokens[increments])
            {
                case "idt":
                    {
                        //lookup for dups and insert
                        val = SymbolTable.lookUp(Lexie.LexemeString[increments]);

                        if (val.lexeme != Lexie.LexemeString[increments])
                        {
                            Console.WriteLine("Error: " + Lexie.LexemeString[increments] + " was not found when searching for declaration. The depth found at:" + depth);
                            Environment.Exit(1);
                            break;

                        }
                        else
                        {
                            if (Lexie.LexemeString[increments - 1] == "return")
                            {
                                tempReturn += Lexie.LexemeString[increments];
                            }
                            else
                            {
                                tempinfo += Lexie.LexemeString[increments];
                                addopTemp += Lexie.LexemeString[increments];

                            }
                            increments++;
                        }
                        break;
                    }
                case "numt":
                case "rparent":
                    {
                      
                        if(Lexie.LexemeString[increments-1] == "return")
                        {
                            tempReturn += Lexie.LexemeString[increments];
                        }
                        else
                        {
                            tempinfo += Lexie.LexemeString[increments];

                        }

                        increments++;
                        break;
                    }
                case "lparent":
                    {
                        //tempinfo += Lexie.LexemeString[increments];
                        increments++;
                        tempinfo += Lexie.LexemeString[increments];
                        Expr();
                        break;
                    }
            }
        }


        /// <summary>
        /// Addop -> + | - | '||'
        /// </summary>
        public static void Addop()
        {

            switch (Lexie.MatchTokens[increments])
            {
                case "addopt":
                    {
                        if (Lexie.LexemeString[increments] == "+")
                        {
                            //tempinfo += Lexie.LexemeString[increments];
                            addopTemp += Lexie.LexemeString[increments];
                            tempinfo += Lexie.LexemeString[increments];
                            increments++;
                        }
                        else if (Lexie.LexemeString[increments] == "-")
                        {
                            tempinfo += Lexie.LexemeString[increments];
                            increments++;
                        }
                        else if (Lexie.LexemeString[increments] == "||")
                        {
                            tempinfo += Lexie.LexemeString[increments];
                            increments++;
                        }
                        //writerfile.WriteLine(varString + (varCounter) + addopTemp);
                        //writerfile.WriteLine(Lexie.LexemeString[increments - (addopTemp.Length + 1)] + "=" + addopTemp);
                        //writerfile.WriteLine();
                        //tempinfo = "";
                        //previousCounter += varCounter;
                        //prevBP = varString + previousCounter;
                        //varCounter += 2;
                        break;
                    }
                case "semit":
                case "numt":
                case "idt":
                    {
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'addopt'(+,-), 'semit', 'numt', 'idt', or 'relopt'(||). Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }


        /// <summary>
        /// Mulop -> * | / | &&
        /// </summary>
        public static void Mulop()
        {
            switch (Lexie.MatchTokens[increments])
            {
                case "mulopt":
                    {
                        if (Lexie.LexemeString[increments] == "*" || Lexie.LexemeString[increments] == "/" || Lexie.LexemeString[increments] == "&" || Lexie.LexemeString[increments] == "&")
                        {
                            //tempinfo += Lexie.LexemeString[increments];
                            if(Lexie.LexemeString[increments] == "*")
                            {
                                //if(addopTemp == Lexie.LexemeString[increments - 1])
                                //{
                                //    addopTemp = addopTemp.TrimEnd() + prevBP;
                                //}
                                increments--;
                                mulopTemp += Lexie.LexemeString[increments];
                                increments++;
                                mulopTemp += Lexie.LexemeString[increments];
                                increments++;
                                mulopTemp += Lexie.LexemeString[increments];
                                increments++;
                            }
                            else if(Lexie.LexemeString[increments] == "/")
                            {
                                increments--;
                                mulopTemp += Lexie.LexemeString[increments];
                                increments++;
                                mulopTemp += Lexie.LexemeString[increments];
                                increments++;
                                mulopTemp += Lexie.LexemeString[increments];
                                increments++;
                            }
                            else
                            {

                            }
                            writerfile.WriteLine(varString + (varCounter) + "=" + mulopTemp);
                            writerfile.WriteLine();
                            previousCounter += varCounter;
                            tempinfo = tempinfo.Remove(tempinfo.Length - 1, 1);
                            prevBP = varString + previousCounter;
                            tempinfo = tempinfo + prevBP;
                            varCounter += 2;
                            // increments++;
                        }
                        else
                        {

                        }
                        break;
                    }
                case "semit":
                case "numt":
                case "idt":
                    {
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'mulopt'(*,/), 'semit', 'numt', 'idt' or 'relopt'(&&). Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        /// <summary>
        /// SignOp -> ! | - | 
        /// </summary>
        public static void SignOp()
        {
            increments++;
            switch (Lexie.MatchTokens[increments])
            {
                case "addopt":
                    {
                        if (Lexie.LexemeString[increments] == "-")
                        {
                            tempinfo += Lexie.LexemeString[increments];
                            increments++;
                        }
                        break;
                    }
                case "signopt":
                    {
                        if (Lexie.LexemeString[increments] == "!")
                        {
                            tempinfo += Lexie.LexemeString[increments];
                            increments++;
                        }
                        break;
                    }
                case "numt":
                case "idt":
                case "lparent":
                    {
                        break;
                    }
                case "eoftt":
                    {
                        break;
                    }
                default:
                    {
                        Console.WriteLine("Error: " + Lexie.MatchTokens[increments] + " was found when searching for 'addopt'(+), 'eoftt' or 'signopt(!) Not correct Grammar.");
                        Environment.Exit(1);
                        break;
                    }
            }
        }

        //public static void buildThreeAddressCode(string tempString)
        //{
        //    //FileWriter.WriteLine(tempString);
        //    List<string> Statement = new List<string>();
        //    string tempNum = "";
        //    bool negation = false; //attached negative to the value or variable
        //    string firstVariable = "";
        //    int start = 0;
        //    int end = 0;
        //    int indexMult = 0;
        //    int indexDiv = 0;
        //    int indexAdd = 0;
        //    int startcounter = 0;

        //    foreach (char c in tempString)
        //    {

        //        if (char.IsNumber(c))
        //        {
        //            tempNum += c;
        //        }
        //        else if (c == '-')
        //        {
        //            negation = true;
        //        }
        //        else
        //        {
        //            if (tempNum != "")
        //            {
        //                if (negation)
        //                {
        //                    if (startcounter > 4) //3 because you dont want to add a + at the start of a statement
        //                    {
        //                        Statement.Add("+");
        //                    }
        //                    Statement.Add("-" + tempNum);
        //                    Statement.Add("-" + c.ToString());
        //                    tempNum = "";
        //                    negation = false;
        //                }
        //                else
        //                {
        //                    Statement.Add(tempNum);
        //                    Statement.Add(c.ToString());
        //                    tempNum = "";
        //                }

        //            }
        //            else
        //            {
        //                if (negation)
        //                {
        //                    if (startcounter > 4) //3 because you dont want to add a + at the start of a statement
        //                    {
        //                        Statement.Add("+");
        //                    }
        //                    Statement.Add("-" + c.ToString());
        //                    negation = false;
        //                }
        //                else
        //                {
        //                    Statement.Add(c.ToString());
        //                }
        //            }
        //            if (c != ' ')
        //            { startcounter++; }
        //        }
        //    }
        //    if (tempNum != "")
        //    {
        //        if (negation)
        //        {
        //            if (startcounter > 4) //2 because you dont want to add a + at the start of a statement
        //            {
        //                Statement.Add("+");
        //            }
        //            Statement.Add("-" + tempNum);
        //        }
        //        else
        //        {
        //            Statement.Add(tempNum);
        //        }
        //    }
        //    firstVariable = Statement[0];
        //    Statement.RemoveAt(0);
        //    Statement.RemoveAt(0);
        //    //Statement.ForEach(p => FileWriter.WriteLine(p));

        //    bool LastStatement = false;
        //    while (LastStatement == false)
        //    {
        //        if (Statement.Contains("("))
        //        {
        //            start = Statement.IndexOf("(");
        //            end = Statement.IndexOf(")");
        //            if (end - start == 4)//(a addop b) or (a mulop b)
        //            {
        //                if (Statement[start + 1].StartsWith('-'))
        //                {
        //                    temp = SymbolTable.lookUp(Statement[start + 1].Substring(Statement[start + 1].Length - 1));
        //                }
        //                else
        //                {
        //                    temp = SymbolTable.lookUp(Statement[start + 1]);
        //                }
        //                if (Statement[end - 1].StartsWith('-'))
        //                {
        //                    temp2 = SymbolTable.lookUp(Statement[end - 1].Substring(Statement[end - 1].Length - 1));
        //                }
        //                else
        //                {
        //                    temp2 = SymbolTable.lookUp(Statement[end - 1]);
        //                }

        //                if (temp.depth == 1 && temp2.depth == 1)
        //                {
        //                    writerfile.WriteLine(varString + varCounter + " = " + varString + "+" + temp.offset + " " + Statement[start + 2] + " " + varString + " + " + temp.offset);
        //                    //writerfile.WriteLine(firstVariable + " = " + varString + varCounter);
        //                }
        //                else if (temp.depth == 1 && temp2.depth == 0)
        //                {
        //                    writerfile.WriteLine(varString + varCounter + " = " + varString + "+" + temp.offset + " " + Statement[start + 2] + " " + Statement[start + 3]);
        //                    //writerfile.WriteLine(firstVariable + " = " + varString + varCounter);
        //                }
        //                else if (temp.depth == 0 && temp2.depth == 1)
        //                {
        //                    writerfile.WriteLine(varString + varCounter + " = " + Statement[start + 1] + " " + Statement[start + 2] + " " + varString + " + " + temp.offset);
        //                    //writerfile.WriteLine(firstVariable + " = " + varString + varCounter);
        //                }
        //                else
        //                {
        //                    writerfile.WriteLine(varString + varCounter + " = " + Statement[start + 1] + " " + Statement[start + 2] + " " + Statement[start + 3]);

        //                }
        //                writerfile.WriteLine();
        //                tempbp = varString + varCounter;
        //                varCounter -= 2;
        //            }
        //            else if (end == 2) //just negation
        //            {
        //                writerfile.WriteLine(varString + varCounter + " = " + Statement[end - 1]);
        //                tempbp = varString + varCounter;
        //                varCounter -= 2;
        //                temp = SymbolTable.lookUp(Statement[1].Substring(Statement[end - 1].Length - 1));
        //                if (temp.depth == 1)
        //                {
        //                    writerfile.WriteLine(varString + "+" + temp.offset + " = " + tempbp);
        //                }
        //                else
        //                {
        //                    writerfile.WriteLine(firstVariable + " = " + tempbp);
        //                }
        //                writerfile.WriteLine();
        //            }

        //            for (int p = 0; p <= end - start; p++) //removes the part we just evaluated ( to )
        //            {
        //                Statement.RemoveAt(start);
        //            }
        //            Statement.Insert(start, tempbp);
        //            if (Statement.Count == 1) // if its just the temp value we inserted then the evaluation is done
        //            {
        //                writerfile.WriteLine(firstVariable + " = " + tempbp);
        //                writerfile.WriteLine();
        //                LastStatement = true;
        //            }
        //            //Statement.ForEach(p => writerfile.WriteLine(p));
        //        }
        //        else if (Statement.Contains("*") || Statement.Contains("/"))
        //        {
        //            int index = 0;
        //            indexMult = Statement.IndexOf("*");
        //            indexDiv = Statement.IndexOf("/");
        //            if (indexMult < indexDiv && indexMult != -1)
        //            {
        //                index = indexMult;
        //            }
        //            else if (indexDiv == -1)
        //            {
        //                index = indexMult;
        //            }
        //            else
        //            {
        //                index = indexDiv;
        //            }
        //            //remove - for lookUp
        //            if (Statement[index + 1].StartsWith('-'))
        //            {
        //                temp = SymbolTable.lookUp(Statement[index + 1].Substring(Statement[index + 1].Length - 1));
        //            }
        //            else
        //            {
        //                temp = SymbolTable.lookUp(Statement[index + 1]);
        //            }
        //            if (Statement[index - 1].StartsWith('-'))
        //            {
        //                temp2 = SymbolTable.lookUp(Statement[index - 1].Substring(Statement[index - 1].Length - 1));
        //            }
        //            else
        //            {
        //                temp2 = SymbolTable.lookUp(Statement[index - 1]);
        //            }

        //            if (temp.depth == 1 && temp2.depth == 1)
        //            {
        //                writerfile.WriteLine(varString + varCounter + " = " + varString + "+" + temp.offset + " " + Statement[index] + " " + varString + " + " + temp.offset);
        //                //writerfile.WriteLine(firstVariable + " = " + varString + varCounter);
        //            }
        //            else if (temp.depth == 1 && temp2.depth == 0)
        //            {
        //                writerfile.WriteLine(varString + varCounter + " = " + varString + "+" + temp.offset + " " + Statement[index] + " " + Statement[index + 1]);
        //                //writerfile.WriteLine(firstVariable + " = " + varString + varCounter);
        //            }
        //            else if (temp.depth == 0 && temp2.depth == 1)
        //            {
        //                writerfile.WriteLine(varString + varCounter + " = " + Statement[index - 1] + " " + Statement[index] + " " + varString + " + " + temp.offset);
        //                //writerfile.WriteLine(firstVariable + " = " + varString + varCounter);
        //            }
        //            else
        //            {
        //                writerfile.WriteLine(varString + varCounter + " = " + Statement[index - 1] + " " + Statement[index] + " " + Statement[index + 1]);
        //                //writerfile.WriteLine(firstVariable + " = " + varString + varCounter);
        //            }
        //            writerfile.WriteLine();
        //            tempbp = varString + varCounter;
        //            varCounter -= 2;
        //            for (int p = 0; p < 3; p++)
        //            {
        //                Statement.RemoveAt(index - 1);
        //            }
        //            Statement.Insert(index - 1, tempbp);
        //            if (Statement.Count == 1)
        //            {
        //                writerfile.WriteLine(firstVariable + " = " + tempbp);
        //                writerfile.WriteLine();
        //                LastStatement = true;
        //            }
        //            //Statement.ForEach(p => writerfile.WriteLine(p));
        //        }
        //        else if (Statement.Contains("+") || Statement.Contains("-"))
        //        {
        //            indexAdd = Statement.IndexOf("+");
        //            if (Statement[indexAdd + 1].StartsWith('-'))
        //            {
        //                temp = SymbolTable.lookUp(Statement[indexAdd + 1].Substring(Statement[indexAdd + 1].Length - 1));
        //            }
        //            else
        //            {
        //                temp = SymbolTable.lookUp(Statement[indexAdd + 1]);
        //            }
        //            if (Statement[indexAdd - 1].StartsWith('-'))
        //            {
        //                temp2 = SymbolTable.lookUp(Statement[indexAdd - 1].Substring(Statement[indexAdd - 1].Length - 1));
        //            }
        //            else
        //            {
        //                temp2 = SymbolTable.lookUp(Statement[indexAdd - 1]);
        //            }
        //            if (temp.depth == 1 && temp2.depth == 1)
        //            {
        //                writerfile.WriteLine(varString + varCounter + " = " + varString + "+" + temp.offset + " " + Statement[indexAdd] + " " + varString + " + " + temp.offset);
        //                //writerfile.WriteLine(firstVariable + " = " + varString + varCounter);
        //            }
        //            else if (temp.depth == 1 && temp2.depth == 0)
        //            {
        //                writerfile.WriteLine(varString + varCounter + " = " + varString + "+" + temp.offset + " " + Statement[indexAdd] + " " + Statement[indexAdd + 1]);
        //                //writerfile.WriteLine(firstVariable + " = " + varString + varCounter);
        //            }
        //            else if (temp.depth == 0 && temp2.depth == 1)
        //            {
        //                writerfile.WriteLine(varString + varCounter + " = " + Statement[indexAdd - 1] + " " + Statement[indexAdd] + " " + varString + " + " + temp.offset);
        //                //writerfile.WriteLine(firstVariable + " = " + varString + varCounter);
        //            }
        //            else
        //            {
        //                writerfile.WriteLine(varString + varCounter + " = " + Statement[indexAdd - 1] + " " + Statement[indexAdd] + " " + Statement[indexAdd + 1]);
        //                //writerfile.WriteLine(firstVariable + " = " + varString + varCounter);
        //            }
        //            writerfile.WriteLine();
        //            tempbp = varString + varCounter;
        //            varCounter -= 2;
        //            //Statement.Insert(indexAdd - 1 ,tempbp);
        //            for (int p = 0; p < 3; p++)
        //            {
        //                Statement.RemoveAt(indexAdd - 1);
        //            }
        //            Statement.Insert(indexAdd - 1, tempbp);
        //            if (Statement.Count == 1)
        //            {
        //                writerfile.WriteLine(firstVariable + " = " + tempbp);
        //                writerfile.WriteLine();
        //                LastStatement = true;
        //            }
        //            //Statement.ForEach(p => writerfile.WriteLine(p));
        //        }
        //        else
        //        {
        //            writerfile.WriteLine(varString + varCounter + " = " + Statement[0]);
        //            tempbp = varString + varCounter;
        //            varCounter -= 2;
        //            if (Statement[0].StartsWith('-'))
        //            {
        //                temp = temp = SymbolTable.lookUp(Statement[0].Substring(Statement[0].Length - 1));
        //            }
        //            else
        //            {
        //                temp = SymbolTable.lookUp(Statement[0]);
        //            }
        //            if (temp.depth == 1)
        //            {
        //                writerfile.WriteLine(varString + "+" + temp.offset + " = " + tempbp);
        //            }
        //            else
        //            {
        //                writerfile.WriteLine(firstVariable + " = " + tempbp);
        //            }
        //            writerfile.WriteLine();
        //            LastStatement = true;

        //        }
        //    }
        //}
    }
}