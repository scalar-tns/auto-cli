﻿global using System.CommandLine;
global using System.CommandLine.NamingConventionBinder;
global using System.CommandLine.Parsing;

namespace autocli
{
    public static class Parser
    {
        public static async Task Main(string[] args)
        {
            // ===========================================ROOTCOMMAND===========================================

            RootCommand ROOTCOMMAND = new();
            ROOTCOMMAND.Description = Utils.Boxed("AUTOCLI : automation for CLI applications interface creation\n");

            ROOTCOMMAND.Description += $"Author : scalar-tns.\nHost name : {Environment.MachineName}\nOS : {Environment.OSVersion}\nHost version : .NET {Environment.Version}\n\n";

            ROOTCOMMAND.Description += "[autocli] aims to automate .NET 6.0.* CLI applications development based on an input architecture stored in a .json file.\nThe configuration file stores the architecture for the project's commands, subcommands, options, arguments and properties.\n";

            // ===========================================COMMANDS===========================================

            Command? creation = Commands._creation(ROOTCOMMAND);
            Command? generation = Commands._generation(ROOTCOMMAND);

            // ===========================================OPTIONS===========================================

            Option<string>? pushing = Options._pushing(generation);

            // ===========================================ARGUMENTS===========================================

            Argument<string>? file_name = Arguments._file_name(creation);
            Argument<string>? file_path = Arguments._file_path(generation);

            // ===========================================HANDLERS===========================================

            creation.Handler = CommandHandler.Create<string>(Building.Create);
            generation.Handler = CommandHandler.Create<string>(Building.Generation);

            // ===========================================INVOKE===========================================

            // Parse the incoming args and invoke the handler
            await ROOTCOMMAND.InvokeAsync(args);
        }
    }
}