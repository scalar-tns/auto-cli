# Automated Command Line Interface Creation

The auto-CLI creation tool aims to automate .NET 6.0.\* CLI applications development based on an input architecture of the project's commands, subcommands, options, arguments and properties.
The choice was made to store said architecture in a .json configuration file, whose architecture is described in the following sections.

## Configuration file

The configuration file stores the architecture for the project's commands, subcommands, options, arguments and properties in a .json format, deserializable by the [JSON.NET](https://www.newtonsoft.com) library.

### _Packages_

The package property, stores in a json array the name and version of each NuGet package required by the project. The array must at least contain the following packages in order for the auto-generated interface to work and to implement logging.

<details>
<summary><b><u>
Packages Json Array
</u></b></summary>

```json
"Packages": [
    {
      "Name": "System.CommandLine",
      "Version": "--prerelease"
    },
    {
      "Name": "Newtonsoft.Json",
      "Version": "--prerelease"
    },
    {
      "Name": "Serilog",
      "Version": "--prerelease"
    },
    {
      "Name": "Serilog.Sinks.Console",
      "Version": "--prerelease"
    },
    {
      "Name": "Serilog.Sinks.File",
      "Version": "--prerelease"
    }
  ]
```

</details>

Every CLI application built using this tool will rely on the [System.CommandLine API](https://github.com/dotnet/command-line-api) for command line parsing, on [Newtonsoft.Json](https://www.newtonsoft.com/json) to deserialize and build the interface and on [Serilog](https://serilog.net/) for logging.

### _Commands_

The commands array stores in a json array the alias, description, the verbosity option setting and parent of each command of the interface. The alias is the command let to call on the CLI in order to invoke the command and parse its arguments. The description is the text displayed in the help menu of the CLI application. The verbosity option setting is the boolean used to implement the option that will set the verbosity level of the logger. The parent is the parent of the command (e.g. : the root command).

<details>
<summary><b><u>
Commands Json Array
</u></b></summary>

```json
  "Commands": [
    {
      "Alias": "alias",
      "Parent": "parent",
      "Verbosity": "bool",
      "Description": "description"
    }
  ]
```

</details>

### _Arguments_

The arguments array stores in a json array the alias, description, the type of the argument and the parent of each argument of the interface. The alias is the argument to call on the CLI in order to invoke the argument. The description is the text displayed in the help menu of the CLI application. The type is the type of the argument (e.g. : string, int, bool, etc.). The parent is the parent of the argument (e.g. : the command).

<details>
<summary><b><u>
Arguments Json Array
</u></b></summary>

```json
  "Arguments": [
    {
      "Alias": "<name>",
      "Type": "Type",
      "Command": "command-alias",
      "Defaultvalue": null,
      "Description": "description"
    }
  ]
```

</details>

### _Options_

The options array stores in a json array the alias, description, the type of the option and the parent of each option of the interface. The alias is the option to call on the CLI in order to invoke the option. The description is the text displayed in the help menu of the CLI application. The type is the type of the option (e.g. : string, int, bool, etc.). The parent is the parent of the option (e.g. : the command).

<details>
<summary><b><u>
Options Json Array
</u></b></summary>

```json
  "Options": [
    {
      "Aliases": ["--option", "-o"],
      "Type": "Type",
      "Command": "command-alias",
      "Required": "bool",
      "Defaultvalue": "string",
      "Description": "description"
    }
  ]
```

</details>
