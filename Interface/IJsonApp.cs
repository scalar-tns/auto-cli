﻿namespace autocli.Interface
{
    public class IJsonApp
    {
        #region Properties

        private readonly IProperty properties;

        public IProperty GetProperties() => properties;

        public static IProperty ConstructProperties(Dictionary<string, dynamic> dict)
        {
            const string name = "Properties";
            Log.Verbose("Extracting {entity}", name);
            Log.Debug("Properties built.");
            return dict[name].ToObject<List<IProperty>>()[0];
        }

        #endregion Properties

        #region Packages

        private readonly List<IPackage> packages;

        public List<IPackage> GetPackages() => packages;

        public static List<IPackage> ConstructPackages(Dictionary<string, dynamic> dict)
        {
            const string name = "Packages";
            Log.Verbose("Extracting {entity}", name);
            Log.Debug("Packages built.");
            return dict[name].ToObject<List<IPackage>>();
        }

        #endregion Packages

        #region Commands

        private readonly List<Command> commands;

        public List<Command> GetCommands() => commands;

        public RootCommand GetRootCommand() => (RootCommand)this.GetCommands()[0];

        public Command GetCommand(string name)
        {
            foreach (Command item in GetCommands())
                if (item!.Name == name)
                {
                    Log.Verbose("Accessing {C}.", $"{item}");
                    return item;
                }
            Log.Error("No command of name: {name} found.", name);
            return default!;
        }

        public List<Command> ConstructCommands(Dictionary<string, dynamic> dict)
        {
            #region Extracting Commands' attributes from json

            const string name = "Commands";
            Log.Verbose("Extracting {entity}", name);
            var ListCommands = dict[name].ToObject<List<ICommand>>();

            #endregion Extracting Commands' attributes from json

            #region Build loop for the Commands

            var Commands = new List<Command>()
            {
                this.GetProperties().BuildRoot()
            };
            foreach (ICommand cmd in ListCommands)
            {
                Commands.Add(cmd.BuildCommand(Commands.Find(el => el.Name.Equals(cmd.Parent))!));
            }
            Log.Debug("Commands built.");

            #endregion Build loop for the Commands

            return Commands;
        }

        #endregion Commands

        #region Options

        private readonly List<Option> options;

        public List<Option> GetOptions() => options;

        public Option GetOption(string name)
        {
            foreach (Option item in GetOptions())
                if (item!.Name == name)
                {
                    Log.Verbose("Accessing {C}.", $"{item}");
                    return item;
                }
            Log.Error("No option of name: {name} found.", name);
            return default!;
        }

        public List<Option> ConstructOptions(Dictionary<string, dynamic> dict)
        {
            #region Extracting the Options' attributes from json

            const string name = "Options";
            Log.Verbose("Extracting {entity}", name);
            var ListOptions = dict[name].ToObject<List<IOption>>();

            #endregion Extracting the Options' attributes from json

            #region Build loop for the Options

            var Options = new List<Option>();
            foreach (IOption option in ListOptions)
            {
                Options.Add(option.BuildOption(GetCommands().Find(el => el.Name.Equals(option.Command))!));
            }

            /// <summary>
            /// Add verbosity global option
            /// </summary>
            Option<string> verbose = new Option<string>(
                new[] { "--verbose", "-v" }, "Verbosity level of the output : m[inimal]; d[ebug]; v[erbose].")
                .FromAmong("m", "d", "v");
            verbose.SetDefaultValue("m");
            this.GetCommands()[0].AddGlobalOption(verbose);

            Log.Debug("Options built.");

            #endregion Build loop for the Options

            return Options;
        }

        #endregion Options

        #region Arguments

        private readonly List<Argument> arguments;

        public List<Argument> GetArguments() => arguments;

        public Argument GetArgument(string name)
        {
            foreach (Argument item in GetArguments())
                if (item!.Name == name)
                {
                    Log.Verbose("Accessing {C}.", $"{item}");
                    return item;
                }
            Log.Error("No argument of name: {name} found.", name);
            return default!;
        }

        public List<Argument> ConstructArguments(Dictionary<string, dynamic> dict)
        {
            #region Extracting Arguments' attributes from json

            const string name = "Arguments";
            Log.Verbose("Extracting {entity}", name);
            var ListArguments = dict[name].ToObject<List<IArgument>>();

            #endregion Extracting Arguments' attributes from json

            #region Build loop for the Arguments

            var Arguments = new List<Argument>();
            foreach (IArgument arg in ListArguments)
            {
                Arguments.Add(arg.BuildArgument(GetCommands().Find(el => el.Name.Equals(arg.Command))!));
            }
            Log.Debug("Arguments built.");

            #endregion Build loop for the Arguments

            return Arguments;
        }

        #endregion Arguments

        /// <summary>
        /// Class Constructor.
        /// </summary>
        /// <param name="Configuration">Path to configuration file for deserialization.</param>
        public IJsonApp(string Configuration)
        {
            Dictionary<string, dynamic> dict = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(File.ReadAllText(Configuration))!;
            properties = ConstructProperties(dict);
            packages = ConstructPackages(dict);
            commands = ConstructCommands(dict);
            options = ConstructOptions(dict);
            arguments = ConstructArguments(dict);
        }
    }
}