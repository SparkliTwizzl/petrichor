# PluralityUtilities

## version 0.5 - 2023-01-06


---
## 1 - what is it?

This tool is a command-line app with utilities intended to be useful to plural systems.


---
## 2 - what does it do?

Currently the only utility it supports is generating AutoHotkey scripts to replace short sequences of text (tags) with longer strings with names, pronouns, and similar information. These scripts can be written by hand, but learning how to use AutoHotkey is required, and they're hard to make changes to if you ever want to, for example, change your name or your pronouns.


---
## 3 - AutoHotkey? what's that?

The scripts generated by the tool do nothing on their own. They are intended to be run with [AutoHotkey](https://www.autohotkey.com), a Windows scripting tool intended for automation, and without it, they're just a glorified text file.


---
## 4 - how do i use it?

In order to get a useful result from the tool, there are four main steps:

1. write an input file
2. write a templates file
3. run the tool using the above
4. run the resulting script with AutoHotkey


---
### 4.1 - input files


#### 4.1.1 - Input files are made up of entries, which contain fields.

Each entry represents a person and must contain at least one identity (a name paired with a tag).

To write an entry, start with an open curly brace `{` on one line and a close curly brace `}` on another, with nothing else on those lines.

example:
```
{
}
```


#### 4.1.2 - Between the braces, write the fields for the entry on separate lines.

Whitespace at the start of lines for fields is ignored, so feel free to indent or not as you prefer to.

Fields are identified by marker symbols:

- `#name#`
  - Name fields are a sub-field of identity fields (see below).
  - Name fields must be surrounded on both sides by hash symbols `#`.
  - Note that this means that name fields cannot contain hash symbols.
- `@tag`
  - Tag fields are a sub-field of identity fields (see below).
  - Tag fields cannot contain spaces.
- `$pronoun`
  - Pronoun fields are optional.
  - Entries cannot have more than one pronoun field.
- `&decoration`
  - Decoration fields are optional.
  - Entries cannot have more than one decoration field.

Identity fields are a special case, as they consist of pairs of name and tag fields:

- `% #name# @tag`
  - Every entry must contain at least one identity field.
  - There is no upper limit to how many identity fields an entry can have.

example:
```
{
  % #Sam# @sm
  $they/them
  &-- a person
}
```


#### 4.1.3 - There's no limit on how many entries an input file can have, and entries and fields dont have to be unique.

If you want to, for example, have the same set of names paired with a different pronoun and/or decoration, you can include multiple entries that are the same aside from small changes (see below).

IMPORTANT NOTE: All tag fields *should* be unique in order for the generated script to work correctly, even though PluralityUtilities wont take issue with it. If a tag field is repeated, only the first one in the script will work.

example:
```
{
  % #Sam# @sm
  % #Sammy# @smy
}
{
  % #Sam# @sm-t
  % #Sammy# @smy-t
  $they/them
  &-- a person
}
{
  % #Sam# @sm-h
  % #Sammy# @smy-h
  $he/him
  &-- a person
}
{
  % #Sam# @sm-s
  % #Sammy# @smy-s
  $she/her
  &-- a person
}
```


---
### 4.2 - template files

In order for PluralityUtilities to know what format(s) you want the macros in your script to have, you need to provide templates for them.


#### 4.2.1 - Templates must use the same basic structure in order for the generated script to work.

All templates have to start with two colons `::`, a string of text including an at sign `@` representing the tag, then two more colons `::`.

The tag string can be anything you want, as long as it contains at least one at sign `@` and no spaces. Additional text is optional.

If this is not followed, the generated script wont work, even though PluralityUtilities will run without errors.

example:
```
::@:: ← the rest of the template must come after this
```


#### 4.2.2 - Templates must contain marker symbols for the tool to replace in order for them to do anything.

Certain symbols will be replaced by fields from entries in the input file by default. This is how templates are able to be used to generate macros.

Below is a list of the marker symbols and the fields they will be replaced by when the tool runs:

- `# → name`
- `@ → tag`
- `$ → pronoun`
- `& → decoration`

example:
```
input:

{
  % #Sam# @sm
  % #Sammy# @smy
  $they/them
  &-- a person
}


templates:

::@::# ($) | [&]


result:

::sm::Sam (they/them) | [-- a person]
::smy::Sammy (they/them) | [-- a person]
```


#### 4.2.3 - You can use each marker symbol in a template as many times as you want.

example:
```
input:

{
  % #Sam# @sm
  % #Sammy# @smy
  $they/them
  & is a person
}


templates:

::@@::# ($) | [#&]


result:

::smsm::Sam (they/them) | [Sam is a person]
::smysmy::Sammy (they/them) | [Sammy is a person]
```


#### 4.2.4 - You can use a backslash `\`, aka an "escape character", to use marker symbols without them being replaced.

Note that you can apply an escape character to a backslash in order to make it print literally.

example:
```
input:

{
  % #Sam# @sm
  % #Sammy# @smy
  $they/them
  &a person
}


templates:

::\@@::# ($) \\ [\#&]


result:

::@sm::Sam (they/them) \ [#a person]
::@smy::Sammy (they/them) \ [#a person]
```


#### 4.2.5 - Templates files can have as many templates as you want.

Although templates dont have to be unique, repeating a template will generate duplicate macros, which could break the generated script.

example:
```
input:

{
  % #Sam# @sm
  % #Sammy# @smy
  $they/them
  &-- a person
}
{
  % #Alex# @ax
  $it/its
  &[a person too]
}
{
  % #Raven# @rvn
  $thon/thons>they/them
}
{
  % #Beck# @bk
}


templates:

::\@@::#
::\@@-::# ($)
::<\@@>::# ($) | [&]


result:

::@sm::Sam
::@sm-::Sam (they/them)
::<@sm>::Sam (they/them) | [-- a person]

::@smy::Sammy
::@smy-::Sammy (they/them)
::@smy::Sammy (they/them) | [-- a person]

::@ax::Alex
::@ax-::Alex (it/its)
::@ax::Alex (it/its) | [[a person too]]

::@rvn::Raven
::@rvn-::Raven (thon/thons>they/them)
::@rvn::Raven (thon/thons>they/them) || []

::@bk::Beck
::@bk-::Beck ()
::@bk::Beck () || []
```


---
### 4.3 - running the tool


#### 4.3.1 - Call the executable (.exe file) via command line to run it.

It's easier to write a batch script (.bat file) to do this for you (see below for how to do this). If you call it with no arguments, it will show helptext explaining how to use it.

example:
```
C:\path\to\tool\folder\PluralityUtilities\
>PluralityUtilities.exe
```


#### 4.3.2 - In order to generate an AutoHotkey script with the tool, you need an input file and a templates file (see above for how to write them).

Pass the path to the input file as the first argument (arg0), the path to the templates file as the second argument (arg1), and the path to where you want the output file to be generated as the third argument (arg2).

You must include the file extensions for the input and templates files, but the extension on the output file will be ignored and is optional.

IMPORTANT NOTE: If you pass the path to an existing output file, it will be overwritten.

example:
```
C:\path\to\tool\folder\PluralityUtilities\
>PluralityUtilities.exe C:\path\to\input\file\input.txt C:\path\to\templates\file\templates.txt C:\path\to\output\file\output
```


#### 4.3.3 - If you dont like having to get the full path for files, you can use relative paths instead.

`./` gets the folder the .exe file is in, and `../` gets the parent folder of that folder.

example:
```
folder contents:

- parent/
  - PluralityUtilities/
    - output.ahk (will be generated after running)
    - PluralityUtilities.exe
  - input.txt
  - templates.txt


in command prompt:

C:\path\to\parent\PluralityUtilities\
>PluralityUtilities.exe ../input.txt ../templates.txt ./output
```


#### 4.3.4 - A note about slashes in paths:

On Windows, backslashes `\` and forward slashes `/` both work the same way. Use whichever you prefer to. These are not equivalent in the input and template files, however.


#### 4.3.5 - You can pass just a filename for the output file and it will be generated in a default location.

If you pass a filename with no path, the file will be generated in a folder called `_output` inside the tool's folder.

example:
```
folder contents:

- parent/
  - PluralityUtilities/
    - _output\
      - output.ahk (will be generated after running)
    - PluralityUtilities.exe
  - input.txt
  - templates.txt


in command prompt:

C:\path\to\parent\PluralityUtilities\
>PluralityUtilities.exe ../input.txt ../templates.txt output
```


#### 4.3.6 - Optionally, you can pass a fourth argument to enable logging.

Pass `-l` as the fourth argument (arg3) to enable logging in basic mode (writes log info to log file only), or `-v` to enable logging in verbose mode (writes log info to log file and to the console window).

Log files are found in a folder called `_log` inside the tool's folder. Log file names are automatically generated using the date and time when the tool is run.

example:
```
folder contents:

- parent\
  - PluralityUtilities\
    - _log
      - yyyy-MM-dd_HH-mm-ss.log (will be generated after running)
    - PluralityUtilities.exe
  - input.txt
  - output.txt
  - templates.txt


in command prompt:

C:\path\to\parent\folder\PluralityUtilities\
>PluralityUtilities.exe ../input.txt ../templates.txt ../output -l
```


#### 4.3.7 - You can make a batch script (.bat file) to run the tool for you.

If you're going to run the tool with the same arguments every time, it's much simpler to write a simple .bat file to run the tool for you.


##### 4.3.7.1 - Make a new text file, name it whatever you want, and change its extension to .bat.

You can also open it in a text editor such as Notepad and use `save as → Batch file` to do the same thing.


##### 4.3.7.2 - Open the file in a text editor program, such as Notepad.


##### 4.3.7.3 - In the file, put in the command usage as shown above, then save it.

example:
```
folder contents:

- parent\
  - PluralityUtilities\
    - _log\
      - (log files will be generated here after running the tool if logging is enabled)
    - PluralityUtilities.exe
  - example batch file.bat
  - input.txt
  - templates.txt
  - output.ahk (will be generated after running the tool)


in file "example batch file.bat":

PluralityUtilities/PluralityUtilities.exe ./input.txt ./templates.txt ./output -v

```

##### 4.3.7.4 - Once you've done all that, run the .bat file by double clicking it.

Assuming the .bat file was made correctly, it will run PluralityUtilities with all the arguments you set.


---
### 4.4 - using the script generated by PluralityUtilities


#### 4.4.1 - Before you can do anything with your script, you need to install AutoHotkey.

Download it [here](https://www.autohotkey.com) and install it, then continue.


#### 4.4.2 - Now that AutoHotkey is installed, you need to run the script.

Either double-click the .ahk file or right click on it and click "run script" in the dropdown menu.

#### 4.4.3 - Having to launch the script every time you boot your computer can get annoying.

If you get sick of it, you can follow the directions [here](https://windowsloop.com/run-autohotkey-script-at-windows-startup/) to make it run automatically. It might still sometimes fail to launch, but it works the majority of the time.


---
## 5 - i think i found a bug / i have an idea for the project

Report bugs and make suggestions here: [GitHub issues board](https://github.com/SparkliTwizzl/plurality-utilities/issues)

If there's a dead link in this documentation, please report it so it can be fixed.

In order for developers to find bugs easier, please describe what you were doing in as much detail as you're able to (even better, write steps to reproduce the issue), say what you expected to happen, say what actually happened, and if you can, include the log file.