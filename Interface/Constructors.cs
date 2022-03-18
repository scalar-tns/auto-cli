﻿// This file must be pasted in ther interface folder

using autocli.Functionnals;

namespace autocli.Interface
{
    public static class Constructors
    {
        public static RootCommand MakeRootCommand(
            string title,
            string description,
            bool setverbosity)
        {
            RootCommand ROOTCOMMAND = new()
            {
                Description = Utils.Boxed(title) + description + "\n"
            };
            ROOTCOMMAND.SetHandler(() => ROOTCOMMAND.Invoke("-h"));
            Log.Debug("RootCommand built.");
            if (setverbosity) SetVerbosity(ROOTCOMMAND);
            return ROOTCOMMAND;
        }

        public static Command MakeCommand(
            Command command,
            string symbol,
            string description,
            bool setverbosity)
        {
            Command cmd = new(symbol);
            try
            {
                cmd.Description = description;
                command.AddCommand(cmd);
                if (setverbosity) SetVerbosity(cmd);
                string upco = $"{command}";
                Log.Debug("Command {symbol} built and added to {upco}.", symbol, upco);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return cmd;
        }

        public static Argument<T> MakeArgument<T>(
            Command command,
            string symbol,
            string? defaultvalue,
            string description)
        {
            Argument<T> argument = new(symbol);
            if (defaultvalue is not null) argument.SetDefaultValue(defaultvalue);
            try
            {
                argument.Description = description;
                command.AddArgument(argument);
                string upco = $"{command}";
                Log.Debug("Argument {symbol} built and added to {upco}.", symbol, upco);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return argument;
        }

        public static Option<T> MakeOption<T>(
            Command command,
            bool required,
            string symbol,
            string? alias,
            string? defaultvalue,
            string description)
        {
            Option<T> option = new(symbol);
            if (alias is not null) option.AddAlias(alias);
            if (defaultvalue is not null) option.SetDefaultValue(defaultvalue);
            try
            {
                option.IsRequired = required;
                option.Description = description;
                command.AddOption(option);
                string upco = $"{command}";
                Log.Debug("Option {symbol} built and added to {upco}.", symbol, upco);
            }
            catch (Exception ex)
            {
                Log.Error(ex, ex.Message, ex.ToString);
            }
            return option;
        }

        // Implement verbosity option
        public static Option<string> SetVerbosity(Command command)
        {
            return MakeOption<string>(
                command: command,
                required: false,
                symbol: "--verbosity",
                alias: "-v",
                defaultvalue: "m",
                description: "Choix de verbosité de sortie : q[uiet]; m[inimal]; diag[nostic].");
        }
    }
}