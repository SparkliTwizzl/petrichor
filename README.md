<div align="center"><image width="200" src="./branding/logo.png"></div>
<h1 align="center">Petrichor</h1>
<h3 align="center">Version 0.10</h3>
<h4 align="center">/ˈpɛtrɪˌkɔːr/ (noun) The scent of damp earth, particularly after rain.</h4>

<div align="center">Part of the <a href="https://github.com/SparkliTwizzl/trioxichor">Trioxichor project</a>.</div>

---

[Source code and releases](https://github.com/SparkliTwizzl/petrichor)

Petrichor is licensed under the [Anti-Exploitation License Noncommercial Attribution v1.0 (AEL-NC-AT 1.0)](https://github.com/SparkliTwizzl/anti-exploitation-license). By using this project or its source material, you agree to abide by the terms of this license.

---

# 1 - What is it?

This tool is a command-line app with miscellaneous utilities. Currently wi're the only maintainer, so currently it only has stuff that wi feel the need to add to it.

---

# 2 - What does it do?

Petrichor consists of modules for performing various utilities.

Modules:

- [Text Shortcut Script Generation](#21---text-shortcut-script-generation-module)

---

## 2.1 - Text Shortcut Script Generation module

This module generates text shortcut scripts to replace short sequences of text (tags) with longer strings with names, pronouns, and similar information. These scripts can be written by hand, but learning how to use AutoHotkey is required, and they're hard to make changes to if you ever want to, for example, change your name or your pronouns.

---

### 2.1.1 - Text Shortcut Script Generation module tokens

This section details all the [data tokens](#411---data-tokens-and-regions) that are [module-specific](#423---module-specific-tokens) for the Text Shortcut Script Generation module.

This information likely won't make sense unless you [know how to use it](#4---how-do-i-use-it) first.

---

#### 2.1.1.1 - Module options region (OPTIONAL)

This variant of the [module options region]() includes options supported by the [Text Shortcut Script Generation module](#21---text-shortcut-script-generation-module).

---

##### 2.1.1.1.1 - Custom icon tokens (OPTIONAL)

If desired, you can specify filepaths to custom icons for the shortcut script to use.

Available tokens are:

- `default-icon` : Min 0, max 1. This token allows setting a custom default icon for the script. Set value to the path to the icon file you want to use.
- `suspend-icon` : Min 0, max 1. This token allows setting a custom icon for the script to use when suspended. Set value to the path to the icon file you want to use.

**IMPORTANT NOTE:** If you move an icon file and do not update its path in your input file and regenerate the script, the icon will not be found by AutoHotkey and will not be applied.

**Example:**

```petrichor
module-options:
{
    default-icon: {path to default icon file}.ico
    suspend-icon: {path to suspend icon file}.ico
}
```

For simplicity, if an icon file will be in the same folder as the shortcut script, you can use a [relative path](#2.1.1---relative-file-paths).

**Example:**

```petrichor
module-options:
{
    default-icon: ./{default icon file name}.ico
    suspend-icon: ./{suspend icon file name}.ico
}
```

---

##### 2.1.1.1.2 - Reload / suspend shortcut tokens (OPTIONAL)

If desired, you can include keyboard shortcuts to reload and/or suspend the script.

To include a shortcut to reload the script, add a token to the module options region with the name `reload-shortcut` and set its value to a valid AutoHotkey v2.0 shortcut string; If you do not know how to write one, consult AutoHotkey documentation. To make this easier, some [find and replace strings](#41421---shortcut-find-and-replace-strings) are supported by Petrichor.

To include a shortcut to suspend the script, do the same with a token named `suspend-shortcut`.

**Example:**

```petrichor
module-options:
{
    reload-shortcut: #r // Windows key + R
    suspend-shortcut: #s // Windows key + S
}
```

---

###### 2.1.1.1.2.1 - Shortcut find-and-replace strings

The following strings are supported:

- `[windows]` / `[win]` → Windows key
- `[alt]` → either Alt key
- `[left-alt]` / `[lalt]` → left Alt key
- `[right-alt]` / `[ralt]` → right Alt key
- `[control]` / `[ctrl]` → either Control key
- `[left-control]` / `[lctrl]` → left Control key
- `[right-control]` / `[rctrl]` → right Control key
- `[shift]` → either Shift key
- `[left-shift]` / `[lshift]` → left Shift key
- `[right-shift]` / `[rshift]` → right Shift key
- `[and]` → `&`
- `[alt-graph]` / `[altgr]` → AltGr (AltGraph) key
- `[wildcard]` / `[wild]` → `*`
- `[passthrough]` / `[tilde]` → `~`
- `[send]` → `$`
- `[tab]` → Tab key
- `[caps-lock]` / `[caps]` → CapsLock key
- `[enter]` → Enter key
- `[backspace]` / `[bksp]` → Backspace key
- `[insert]` / `[ins]` → Insert key
- `[delete]` / `[del]` → Delete key
- `[home]` → Home key
- `[end]` → End key
- `[page-up]` / `[pgup]` → PageUp key
- `[page-down]` / `[pgdn]` → PageDown key
- `\[` → `[`
- `\]` → `]`

**Example:**

```petrichor
module-options:
{
    reload-shortcut: [win]r // Windows key + R
    suspend-shortcut: \[win\]s // [win]s
}
```

---

#### 2.1.1.2 - Entry list region (REQUIRED)

This region defines the entries to be converted into macros. It must come before the `template-list` region.

Each entry is an `entry` region which defines a set of values to create shortcut macros from. There is no limit to how many entries the `entry-list` region can contain.

---

##### 2.1.1.2.1 - Entry regions (OPTIONAL)

Entry regions are made up several token types. There are different restrictions and requirements for each type.

- `color` : Min 0, max 1. This token defines a value to associate with all `name` tokens that are present.
- `decoration` : Min 0, max 1. This token defines a value to associate with all `name` tokens that are present.
- `id` : Min 1, max 1. This token defines a value to associated with all `name` tokens that are present.
- `name` : Min 1. This token defines a name/tag pair.
    - This token type's value is structured as `[name] @[tag]`, where the `[name]` portion can be any non-blank string that does not contain an `@` character, and the `[tag]` portion can be any string that does not contain whitespace.
- `last-name` : Min 0, max 1. This token defines a value to associate with all `name` tokens that are present.
    - This token type's value structure is identical to that of `name` tags.
- `pronoun` : Min 0, max 1. This token defines a value to associate with all `name` tokens that are present.

**NOTE:** All token values *should* be unique, even though Petrichor wont take issue with it. If a value is repeated, the AutoHotkey script generated from the input data will misbehave in unpredictable ways.

**Example:**

```petrichor
entry: // all optional tokens present
{
    id: 1234
    name: Sam @sm
    name: Sammy @smy
    last-name: Smith @s
    pronoun: they/them
    color: #89abcd
    decoration: -- a person
}

entry: // only required tokens present
{
    id: 4321
    name: ALEX @AX
}
```

---

#### 2.1.1.3 - Template list region (REQUIRED)

This region defines the structure of generated macros. It must come after the entry list region.

Templates define the structure of AutoHotkey macros to create from entries. There is no limit to how many templates can be used.

---

##### 2.1.1.3.1 - Template tokens (OPTIONAL)

Templates are defined by tokens with the name `template` and a valid AutoHotkey hotstring. Consult AutoHotkey documentation if you do not know how to write one. A basic overview is provided here.

All templates must start with a `find` text string, then `::`, then a `replace` text string.

**NOTE:** You cannot use `::` in a `find` string due to the way AutoHotkey hotstrings work.

These components can have whitespace between them, but note that this whitespace will be trimmed off unless you force it to be kept in by inserting a backtick `` ` `` at the start or end of the `find` and/or `replace` strings.

Use [marker strings](#4.1.6.1.1---template-marker-strings) to define how templates should be applied to entries.

If this is not followed, the generated script wont work correctly, even though Petrichor will run without errors.

**Example:**

```petrichor
template-list:
{
    template: [find string] :: ` [replace string] `
}

// MACROS GENERATED FROM INPUT:

::[find string]::` [replace string] `
```

---

###### 2.1.1.3.1.1 - Template marker strings

Certain symbols will be replaced by fields from entries in the input file by default. This is how templates are able to be used to generate macros.

If no marker strings are present, a template will be inserted into the output file with no changes for every `name` token present in the input file. This technically will not break the script, but it is not recommended.

Available marker strings are:

- `[color]`
- `[decoration]`
- `[id]`
- `[name]`
- `[last-name]`
- `[last-tag]`
- `[pronoun]`
- `[tag]`

**NOTE:** Only these supported marker strings can be used. Unknown marker strings will be rejected.

**NOTE:** By default, you cannot use the `[` or `]` symbols in a template string. Use [escape characters](#41612---escape-characters) to circumvent this.

**Example:**

```petrichor
entry-list:
{
    entry:
    {
        id: 1234
        name: Sam @sm
        name: Sammy @smy
        last-name: Smith @s
        pronoun: they/them
        color: #89abcd
        decoration: -- a person
    }
}

template-list:
{
    template:  [tag][last-tag] :: [id] - [name] [last-name] ([pronoun]) | {[decoration]} | [color]
}

// MACROS GENERATED FROM INPUT:

::sms::1234 - Sam Smith (they/them) | {-- a person} | #89abcd
::smys::1234 - Sammy Smith (they/them) | {-- a person} | #89abcd
```

You can use each marker string in a template as many times as you want

**Example:**

```petrichor
entry-list:
{
    entry:
    {
        id: 1234
        name: Sam @sm
        name: Sammy @smy
        last-name: Smith @s
        pronoun: they/them
        color: #89abcd
        decoration: -- a person
    }
}

template-list:
{
    template: [tag][tag] :: [name] | {[name] [decoration]}
}

// MACROS GENERATED FROM INPUT:

::smsm::Sam (they/them) | [Sam is a person]
::smysmy::Sammy (they/them) | [Sammy is a person]
```

---

# 3 - AutoHotkey? What's that?

The scripts generated by the tool do nothing on their own. They are intended to be run with [AutoHotkey](https://www.autohotkey.com), a Windows scripting tool intended for automation, and without it, they're just a glorified text file.

---

# 4 - How do i use it?

In order to get a useful result from the tool, there are 3 main steps:

1. Write an input file using Petrichor Script.
2. Run the tool using the above.
3. Run the resulting script with AutoHotkey.

---

## 4.1 - Petrichor Script syntax and usage

Petrichor Script is made up of data regions, which are made up of tokens. Some are required and some are optional.

---

### 4.1.1 - Data tokens and regions

All data in Petrichor input files is in the form of data tokens, which can be grouped into data regions.

---

#### 4.1.1.1 - Data tokens

The most basic element in input files is a data token, or simply a token.

Every token consists of a name and a value, separated by a `:`. Whitespace between and around these parts is ignored.

Token names are always in `lower-kebab-case`.

**Example (both tokens are identical to Petrichor):**

```petrichor
token-name:Token value.
 token-name : Token value. 
```

---

#### 4.1.1.2 - Data regions

Related tokens can be grouped into a data region, or simply a region. These consist of a token indicating the start of the region, then the region body, surrounded by brackets `{` / `}`.

**Example:**

```petrichor
region-name:
{
    token-1-in-region-body: Value.
    token-2-in-region-body: Value.
}
```

Regions can be contained within another region.

**Example:**

```petrichor
parent-region:
{
    token-a: Value.

    child-region:
    {
        token-b: Value.
    }
}
```

---

### 4.1.2 - Blank lines and comments

The token `//` starts a comment which continues to the end of the line. The comment token is the only token which can be on the same line as other data.

Blank lines are ignored.

**Example:**

```petrichor
// this is a comment. this line will be ignored. the following line is blank, and will also be ignored.

region:
{
    token: value // this is an inline comment. everything after "//" will be ignored.
}
```

---

### 4.1.3 - Escape characters

Backslash `\` is treated as an "escape character" in some cases. It is used to disable the normal function of special characters. An escape character can be applied to another escape character in order to make the scond one print literally.

**Example:**

```petrichor
do-something: @blah // this hypothetical token treats @ as a special character and changes it.
do-something: \@blah // but in this case, it will be left as-is, since the @ is escaped.
```

---

## 4.2 - Supported tokens

---

### 4.2.1 - Metadata region (REQUIRED)

This region contains required information for Petrichor to run, and it must be the first region in the file regardless of what module is used.

---

#### 4.2.1.1 - Minimum version token (REQUIRED)

This token is required. It specifies the minimum Petrichor version required in order to parse the file.

**Example:**

```petrichor
metadata:
{
    minimum-version: major.minor.patch.preview
}
```

Major and minor version must be specified. If patch or patch and preview versions are blank, they are assumed to be any version.

**Example:**

```petrichor
minimum-version: 1.2.3.pre-4 // Major version 1, minor version 2, patch version 3, preview version pre-4
minimum-version: 1.2.3 // Major version 1, minor version 2, patch version 3, any preview version
minimum-version: 1.2 // Major version 1, minor version 2, any patch or preview version
```

---

#### 4.2.1.2 - Command token (OPTIONAL)

This token is optional. It allows you to specify the [command to run](#42---running-the-tool) in your input file for Petrichor to handle automatically.

Set the token's value to the command to be run.

To use [command options](#422---command-options), add a region body to the token and put options into it as tokens, converting the option names to `kebab-case` and setting the tokens' values to the command option values.

**Example:**

```petrichor
metadata:
{
	minimum-version: {version number}
	command: commandName
	{
		command-option-1: value1
		command-option-2: value2
	}
}
```
```
{install path}\Petrichor\
>Petrichor.exe --inputFile input.txt
```

This is equivalent to the following:
```petrichor
metadata:
{
	minimum-version: {version number}
}
```
```
{install path}\Petrichor\
>Petrichor.exe commandName --inputFile input.txt --commandOption1 value1 --commandOption2 value2
```

---

### 4.2.2 - Module options region (OPTIONAL)

This region is optional. It allows you to configure module-specific options, if supported by a module. Each module that supports this region will have its own version of it. See the relevant module's documentation for more information.

---

### 4.2.3 - Module-specific tokens

Most modules have tokens that are specific to their functions. See the relevant module's documentation for information about its tokens.

---

## 4.3 - Running the tool

Call the executable (`.exe` file) via command line to run it

It may be easier to write a batch script (`.bat` file) to do this for you (see below for how to do this). If you call it with no arguments, it will show helptext explaining how to use it.

**Example:**

```
{install path}\Petrichor\
>Petrichor.exe
```

---

### 4.3.1 - Available commands

Petrichor modules each have a corresponding command to trigger them. These can be given as command line arguments or they can be [put into your input file](#4211---default-command) and Petrichor will attempt to read ane execute them automatically.

---

#### 4.3.1.1 - Default command

If you call Petrichor with no arguments, you will be prompted to use the `--help` option to see available commands.

You can call Petrichor without passing it a command if you [put the command in your input file](#413---metadata-region-required) and use the [--inputFile option](#4221----inputfile-option-optional) when you run Petrichor.

**Example:**

Input file contents:
```petrichor
metadata
{
	minimum-version: {version number}
	command: commandName
	{
		command-option-1: value1
		command-option-2: value2
	}
}
```

Command line usage:
```
{install path}\Petrichor\
>Petrichor.exe --inputFile {path}\input.txt
```

---

#### 4.3.1.2 - `generateTextShortcutScript` command

To generate a text hotstring shortcut script, call Petrichor with the command argument `generateTextShortcutScript`.

This command supports the following options:

- [--inputFile](#4221----inputfile-option-optional)
- [--outputFile](#4222----outputfile-option-optional)
- [--logMode](#4223----logmode-option-optional)
- [--logFile](#4224----logfile-option-optional)


**Example:**

```
{install path}\Petrichor\
>Petrichor.exe generateTextShortcutScript
```

---

### 4.3.2 - Command options

Most commands can have their behavior modified with options. 

---

#### 4.3.2.1 - `--inputFile` option (OPTIONAL)

Add the `--inputFile` option to a command and pass the input file argument after it.

You can specify the input file directory and/or name.
If you only provide the directory, `input.petrichor` will be used as the file name.
If you only provide the file name, `{install path}\Petrichor\` will be used as the directory.

You must include the file extension if you provide a file name. [Relative file paths](#2.1.1---relative-file-paths) can be used.

**Example (full file path):**

```
{install path}\Petrichor\
>Petrichor.exe commandName --inputFile "{path}\inputFile.txt"
```
Petrichor will look for `{path}\inputFile.txt`.

**Example (directory only, default file name):**

```
{install path}\Petrichor\
>Petrichor.exe commandName --inputFile "{path}\"
```
Petrichor will look for `{path}\input.petrichor`.

**Example (directory only, default file name):**

```
{install path}\Petrichor\
>Petrichor.exe commandName --inputFile "inputFile.txt"
```
Petrichor will look for `{install path}\Petrichor\inputFile.txt`.

---

#### 4.3.2.2 - `--outputFile` option (OPTIONAL)

Add the `--outout` option to a command which generates a file and pass the output file argument after it.

You can specify the file directory and/or name.
If you only provide the directory, `output.ahk` will be used as the file name.
If you only provide the file name, `{install path}\Petrichor\_output\` will be used as the directory.

A file extension is not required, and if one is included it will be replaced automatically. [Relative file paths](#2.1.1---relative-file-paths) can be used.

**Example (full file path):**

```
{install path}\Petrichor\
>Petrichor.exe commandName --outputFile "C:\path\to\output\file\outputFile"

RESULT:

C:\path\to\output\file\outputFile.ext ← will be generated by Petrichor
```

**Example (directory only, default file name):**

```
{install path}\Petrichor\
>Petrichor.exe commandName --outputFile "outputFile"

RESULT:

{install path}\Petrichor\_output\outputFile.ext ← will be generated by Petrichor
```

**Example (file name only, default directory):**

```
{install path}\Petrichor\
>Petrichor.exe commandName --outputFile "{path}\output\"

RESULT:

{path}\output\output.ext ← will be generated by Petrichor
```

---

#### 4.3.2.3 - `--logMode` option (OPTIONAL)

This option is used to control where logs are sent.

Values for this option are:

- `all` (DEFAULT) - Send logs to all output locations.
- `fileOnly` - Send logs only to log file.
- `consoleOnly` - Send logs only to console output.
- `none` - Disable logging.

---

#### 4.3.2.4 - `--logFile` option (OPTIONAL)

This option is used to customize the file name and/or location to generate a log file at.

NOTE: log file will only be created if logging to file is enabled (see 4.3.3.3).

**Example (filename only, default directory):**

```
Petrichor.exe comandName --logFile logFile.txt

RESULT:

{path}/Petrichor/_log/logFile.txt ← will be generated by Petrichor
```

**Example (directory path only, default filename):**

```
Petrichor.exe commandName --logMode all --logFile {log path}/

RESULT:

{log path}/{yyyy}-{MM}-{dd}_{HH}-{mm}-{ss}.log ← will be generated by Petrichor
```

**Example: (full filepath, no defaults):**

```
Petrichor.exe commandName --logMode all --logFile {log path}/logFile.txt

RESULT:

{log path}/logFile.txt ← will be generated by Petrichor
```

---

### 4.3.3 - Relative file paths

If you dont like having to get the full path for files, you can use relative paths instead.

`./` gets the folder the .exe file is in, and `../` gets the parent folder of that folder.

**Example:**

FOLDER CONTENTS:

```
- parent/
    - Petrichor/
        - outputFile.ext (will be generated after running)
        - Petrichor.exe
    - inputFile.txt
```

IN COMMAND PROMPT:

```
{install path}\Petrichor\
>Petrichor.exe commandName --inputFile ../inputFile.txt --outputFile ./outputFile
```

---

### 4.3.4 - A note about slashes in file paths on Windows

On Windows, backslashes `\` and forward slashes `/` both work the same way. Use whichever you prefer to. **They are not equivalent to each other in input files, however.**

---

### 4.3.5 - Running Petrichor via batch script (`.bat` file)

If you're going to run the tool with the same arguments every time, it's much simpler to write a simple `.bat` file to run the tool for you.

1. Make a new text file, name it whatever you want, and change its extension to `.bat`.
    -  You can also open it in a text editor such as Notepad and use `save as → Batch file` to do the same thing.
2. Open the file in a text editor program, such as Notepad.
3. Type `start {install path}/Petrichor.exe]`, followed by command usage as shown above.
    - **NOTE:** [Relative paths](#2.1.1---relative-file-paths) are relative to the batch script by default. If relative paths are used in Petrichor commands, they must be relative to Petrichor.exe instead.
4. Save the batch file.

Once you've done these steps, you can run the `.bat` file by double clicking it. Assuming the `.bat` file was made correctly, it will run Petrichor with all the arguments you set.

**Example (command passed with command line arguments):**

FOLDER CONTENTS:

```
- parent\
    - Petrichor\
        - Petrichor.exe
    - example batch file.bat
    - inputFile.txt
    - outputFile.ahk (will be generated after running)
```

IN FILE "example batch file.bat":

```batch
start Petrichor\Petrichor.exe generateTextShortcutScript --inputFile ..\inputFile.txt --outputFile ..\outputFile
```

**Example (command set in input file):**

FOLDER CONTENTS:

```
- parent\
    - Petrichor\
        - Petrichor.exe
    - example batch file.bat
    - inputFile.txt
    - outputFile.ahk (will be generated after running)
```

IN FILE "example batch file.bat":

```batch
start Petrichor\Petrichor.exe --inputFile ..\inputFile.txt
```

Optionally, you can make a batch script wait for Petrichor to finish running and launch the output script automatically.

1. Create a batch script using the steps above.
2. After the `start` keyword, add `/wait`. This will cause the batch script to wait until Petrichor is closed before continuing.
3. Add a new line to the batch script, and enter `start [path/script.ahk]`.
4. Save the batch file.

Once you've done these steps, you can run the `.bat` file by double clicking it. Assuming the `.bat` file was made correctly, it will run Petrichor with all the arguments you set, wait until it closes, then launch the output script.

**NOTE:** If Petrichor fails to generate a new script, any existing version of the output script will be launched instead.

**Example:**

FOLDER CONTENTS:

```
- parent\
    - Petrichor\
        - Petrichor.exe
    - example batch file.bat
    - inputFile.txt
    - outputFile.ahk (will be generated after running)

```

IN FILE "example batch file.bat":

```batch
start /wait Petrichor\Petrichor.exe generateTextShortcutScript --inputFile ..\inputFile.txt --outputFile ..\outputFile
start outputFile.ahk
```

---

# 5 - Using the script generated by Petrichor

---

## 5.1 - Install AutoHotkey

Before you can do anything with your script, you need to install AutoHotkey. Download and install AutoHotkey v2 [here](https://www.autohotkey.com) and install it, then continue.

---

## 5.2 - Running the script

Either double-click the .ahk file or right click on it and click "run script" in the dropdown menu.

---

## 5.3 - Methods to launch the script automatically

If you get sick of launching a script manually, there are a few options.

---

### 5.3.1 - Windows Startup shortcut (RECOMMENDED)

This is the simplest method. It's not totally reliable, but it works the majority of the time. Occasionally a script will launch successfully, but not show up in the taskbar tray. If that bothers you, just relaunch the script manually.

Here's how to do it:

1. Right-click the script in File Explorer.
2. Click `Create shortcut` in the dropdown menu.
3. Press `Win+R` to open the Windows Run dialog.
4. Type `shell:startup` into the dialog, then click OK.
5. The Startup folder will open. Copy the shortcut you created in step 2 into it.

---

### 5.3.2 - Task Scheduler

Wi've found this method to be less reliable than the Windows Startup method, but it does work more often than not. It's also kind of a pain to set up. Wi recommend using the Windows Startup method over this one, unless that method doesnt work for you.

You can follow the directions [here](https://windowsloop.com/run-autohotkey-script-at-windows-startup/) to set it up.

---

### 5.3.3 - Registry (NOT RECOMMENDED)

**DO NOT DO THIS UNLESS YOU KNOW WHAT YOU'RE DOING. Editing the registry can brick your computer if you're not careful.**

Wi strongly recommend using one of the other methods above, unless all of them dont work for you.

Also, wi havent personally tested this method, so wi dont know how reliable it is, but it probably should work about the same as the other two?

1. Open the Registry Editor. There are two days to do this:
	- Press `Win+R` to open the Run dialog, type in `regedit`, then click OK.
	- Open the Start menu and search for either `regedit` or `Registry Editor`.
2. Navigate to `HKEY_CURRENT_USER\SOFTWARE\Microsoft\Windows\CurrentVersion\Run`.
3. Add a new String key. Name it however you prefer.
4. Edit the value of the new string key and put in `"@:\path\to\autohotkey\version\file.exe" "@:\path\to\script\file.ahk"`, using the filepaths of your AutoHotkey installation and your script file.

---

# 6 - I think i found a bug / I have an idea for the project

Report bugs and make suggestions here: [GitHub issues board](https://github.com/SparkliTwizzl/plurality-utilities/issues)

If there's a dead link in this documentation, please report it so it can be fixed.

In order for developers to find bugs easier, please describe what you were doing in as much detail as you're able to (even better, write steps to reproduce the issue), say what you expected to happen, say what actually happened, and if you can, include the log file.
