using System;
using System.Collections.Generic;
using System.Linq;
usign System.IO;


public class CommandInfo
{
    //            arguments, flags
    public Action<string[], string[]> function;
    public string[] possibleFlags = new string[0];
    public bool anyFlag = false;

    public CommandInfo(Action<string[], string[]> function)
    {
        this.function = function;
    }
    public CommandInfo(Action<string[], string[]> function, string[] possibleFlags)
    {
        this.function = function;
        this.possibleFlags = possibleFlags;
    }

    public CommandInfo(Action<string[], string[]> function, bool anyFlag)
    {
        this.function = function;
        this.anyFlag = anyFlag;
    }
}

public class CommandProcessor
{
    public Dictionary<string, CommandInfo> commands = new Dictionary<string, CommandInfo>();

    public string commandPrefix = "/";
    public string flagPrefix = "-";
    public string errorInputMessage = "Not correct input";
    public string wrongFlagMessage = "flag is not allowed";
    public CommandProcessor()
    {

    }

    public CommandProcessor(string commandPrefix)
    {
        this.commandPrefix = commandPrefix;
    }

    public void Process(string? cmdInput)
    {
        if (cmdInput == null)
        {
            Console.WriteLine(errorInputMessage);
            return;
        }

        var words = cmdInput.Split();
        if (words.Length < 1)
        {
            Console.WriteLine(errorInputMessage);
            return;
        }

        string commandName = "";
        if (commandPrefix != "")
            commandName = words[0].Replace(commandPrefix, "");
        else
            commandName = words[0];

        if (!commands.ContainsKey(commandName))
        {
            Console.WriteLine(errorInputMessage);
            return;
        }

        List<string> arguments = new List<string>();
        List<string> flags = new List<string>();

        for (int i = 1; i < words.Length; i++)
        {
            string w = words[i];
            if (w.StartsWith(flagPrefix))
            {
                string flag = "";
                if (flagPrefix != "")
                    flag = w.Replace(flagPrefix, "");
                else
                    flag = w;
                if (commands[commandName].anyFlag)
                {
                    flags.Add(flag);
                }
                else if (commands[commandName].possibleFlags.Contains(flag))
                {
                    flags.Add(flag);
                }
                else
                {
                    Console.WriteLine($"'{flag}' " + wrongFlagMessage);
                }
            }
            else
                arguments.Add(w);
        }

        commands[commandName].function.Invoke(arguments.ToArray(), flags.ToArray());
    }

    public void AppendCommand(string name, Action<string[], string[]> function, bool anyFlag = true)
    {
        if (commands.ContainsKey(name)) throw new Exception("This command is appended already");

        commands.Add(name, new CommandInfo(function, anyFlag));
    }

    public void AppendCommand(string name, Action<string[], string[]> function, string[] possibleFlags)
    {
        if (commands.ContainsKey(name)) throw new Exception("This command is appended already");

        commands.Add(name, new CommandInfo(function, possibleFlags));
    }
}

