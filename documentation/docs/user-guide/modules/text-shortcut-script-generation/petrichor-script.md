---
short_title: Petrichor Script
title: Petrichor Script - Text Shortcut Script Generation - Modules
---

<h1 align="center">Petrichor Script</h1>
<h2 align="center"><a href="./index.html">Text Shortcut Script Generation</a></h2>


---
## Module options token

This module's variant of the [module options token](../../getting-started/petrichor-script.html#module-options-token) supports the following tokens.

- [Default icon](#default-icon-and-suspend-icon-tokens)
- [Suspend icon](#default-icon-and-suspend-icon-tokens)
- [Reload shortcut](#reload-shortcut-and-suspend-shortcut-tokens)
- [Suspend shortcut](#reload-shortcut-and-suspend-shortcut-tokens)

???+ important "Restrictions"

    OPTIONAL

    Maximum allowed: 1

    Must come after [`metadata`](../../getting-started/petrichor-script.html#metadata-token) token.

???+ example

    ```petrichor
    metadata:
    {
        // Metadata goes here.
    }

    module-options:
    {
        // Module-specific options go here.
    }
    ```

---
### Default icon and suspend icon tokens

These tokens allow you to specify file paths to custom icons for a text shortcut script.

The `default-icon` token sets the file path of the icon shown on a script by default.

The `suspend-icon` token sets the file path of the icon shown when a script is suspended.

[Relative file paths](../../getting-started/command-usage.html#relative-file-paths) can be used.

!!! note

    File paths can be surrounded with quotes, but they do not need to be.

!!! warning

    If you move an icon file and do not update its path in your input file, the icon will not be found and will not be applied.

???+ important "Restrictions"

    OPTIONAL

    Maximum allowed: 1

    Must be in [`module-options`](#module-options-token) token body.

???+ example

    ```petrichor
    module-options:
    {
        default-icon: <path>/default.ico
        suspend-icon: "./suspend_icon.png"
    }
    ```


---
### Reload shortcut and suspend shortcut tokens

These tokens allow you to set keyboard shortcuts to control the operation of a script.

The `reload-shortcut` token sets a keyboard shortcut to reload a script.

The `suspend-shortcut` token sets a keyboard shortcut to suspend and resume a script.

These keyboard shortcuts can be written in AutoHotkey v2 syntax, but for simplicity, Petrichor supports a [find-and-replace table](#control-shortcut-find-and-replace-table) of common modifier keys.

???+ important "Restrictions"

    OPTIONAL

    Maximum allowed: 1

    Must be in [`module-options`](#module-options-token) token body.

???+ example

    ```petrichor
    module-options:
    {
        reload-shortcut: <shortcut>
        suspend-shortcut: <shortcut>
    }
    ```


---
#### Control shortcut find-and-replace table

Petrichor supports the following tags in [script control shortcuts](#reload-shortcut-and-suspend-shortcut-tokens).

| Tag                      | Alias               | Encodes for       | Key / symbol name                     |
| ------------------------ | ------------------- | ----------------- | ------------------------------------- |
| `#!ptcr [windows]`       | `#!ptcr [win]`      | ++win++           | Windows key                           |
| `#!ptcr [alt]`           |                     | ++alt++           | Alt key                               |
| `#!ptcr [left-alt]`      | `#!ptcr [lalt]`     | ++lalt++          | Left Alt key                          |
| `#!ptcr [right-alt]`     | `#!ptcr [ralt]`     | ++ralt++          | Right Alt key                         |
| `#!ptcr [control]`       | `#!ptcr [ctrl]`     | ++ctrl++          | Control key                           |
| `#!ptcr [left-control]`  | `#!ptcr [lctrl]`    | ++lctrl++         | Left Control key                      |
| `#!ptcr [right-control]` | `#!ptcr [rctrl]`    | ++rctrl++         | Right Control key                     |
| `#!ptcr [shift]`         |                     | ++shift++         | Shift key                             |
| `#!ptcr [left-shift]`    | `#!ptcr [lshift]`   | ++lshift++        | Left Shift key                        |
| `#!ptcr [right-shift]`   | `#!ptcr [rshift]`   | ++rshift++        | Right Shift key                       |
| `#!ptcr [combine]`       | `#!ptcr [and]`      | ++"&"++           | AutoHotkey combine (Ampersand symbol) |
| `#!ptcr [alt-graph]`     | `#!ptcr [altgr]`    | ++altgr++         | AltGraph key                          |
| `#!ptcr [wildcard]`      | `#!ptcr [asterisk]` | ++"*"++           | AutoHotkey wildcard (Asterisk symbol) |
| `#!ptcr [passthrough]`   | `#!ptcr [tilde]`    | ++tilde++         | AutoHotkey passthrough (Tilde symbol) |
| `#!ptcr [send]`          | `#!ptcr [dollar]`   | ++"$"++           | AutoHotkey send (Dollar sign)         |
| `#!ptcr [tab]`           |                     | ++tab++           | Tab key                               |
| `#!ptcr [caps-lock]`     | `#!ptcr [caps]`     | ++caps-lock++     | CapsLock key                          |
| `#!ptcr [enter]`         |                     | ++enter++         | Enter key                             |
| `#!ptcr [backspace]`     | `#!ptcr [bksp]`     | ++backspace++     | Backspace key                         |
| `#!ptcr [insert]`        | `#!ptcr [ins]`      | ++ins++           | Insert key                            |
| `#!ptcr [delete]`        | `#!ptcr [del]`      | ++del++           | Delete key                            |
| `#!ptcr [end]`           |                     | ++end++           | End key                               |
| `#!ptcr [home]`          |                     | ++home++          | Home key                              |
| `#!ptcr [page-up]`       | `#!ptcr [pgup]`     | ++page-up++       | PageUp key                            |
| `#!ptcr [page-down]`     | `#!ptcr [pgdn]`     | ++page-dn++       | PageDown key                          |
| `#!ptcr [open-bracket]`  | `#!ptcr \[`         | ++bracket-left++  | Left square bracket                   |
| `#!ptcr [close-bracket]` | `#!ptcr \]`         | ++bracket-right++ | Right square bracket                  |

!!! note

    `#!ptcr \[` and `#!ptcr \]` make use of [escape characters](../../getting-started/petrichor-script.html#escape-characters).

???+ example

    ```petrichor
    module-options:
    {
        reload-shortcut: [win]r // Windows key + R
        suspend-shortcut: \[win\]s // [win]s
    }
    ```


---
## Shortcut list token

This token contains definitions for the text shortcuts to be generated.

???+ important "Restrictions"

    REQUIRED

    Mininum required: 1

    Maximum allowed: 1

    Must come after [`metadata`](../../getting-started/petrichor-script.html#metadata-token) token.

???+ example

    ```petrichor
    metadata:
    {
        // Metadata goes here.
    }

    shortcut-list:
    {
        // Shortcuts go here.
    }
    ```


---
### Shortcut token

The `shortcut` token defines a plaintext shortcut.

This token's values can use AutoHotkey special behavior if written correctly. Consult AutoHotkey documentation to learn more about this.

Shortcuts consist of 3 parts: A hotstring, a divider consisting of 2 right-angle brackets ( `>>` ), and a replacement string.

!!! note

    These components can have whitespace between them.

    This whitespace will be trimmed off unless you force it to be kept in by surrounding it with backticks ( `` ` `` ).

!!! warning

    You cannot use `::` in a hotstring due to the way AutoHotkey hotstrings work.

    Petrichor will allow you to do it, but the generated shortcuts will not work.

    [Escaping](../../getting-started/petrichor-script.html#escape-characters) the characters will not fix this.

???+ important "Restrictions"

    OPTIONAL

    No restrictions on number of instances that can be present.

    Must be in [`shortcut-list`](#shortcut-list-token) token body.

???+ example

    ```petrichor title="Input"
    shortcut-list:
    {
        shortcut: <hotstring> >> ` <replacement string> `
    }
    ```

    ```autohotkey title="Shortcuts generated from input"
    ; This is a standard shortcut. The <hotstring> and <replacement string> will be inserted into the output file unaltered.
    ::<hotstring>::` <replacement string> `
    ```


---
### Shortcut template token

The `shortcut-template` token defines a templated shortcut.

It behaves the same way as the [shortcut token](#shortcut-token), but with additional features.

Shortcuts will be generated from the template, filling in `#!ptcr [field]` tags with user-provided data.

Supported fields:

- `#!ptcr [color]`
- `#!ptcr [decoration]`
- `#!ptcr [id]`
- `#!ptcr [name]`
- `#!ptcr [last-name]`
- `#!ptcr [last-tag]`
- `#!ptcr [pronoun]`
- `#!ptcr [tag]`

Additional features are supported via subtokens.

If no subtokens are used, this token does not need a body.

!!! note

    By default, you cannot use the `[` or `]` symbols in a template. Use [escape characters](../../getting-started/command-usage.html#escape-characters) to circumvent this.

!!! warning

    You cannot use `::` in a hotstring due to the way AutoHotkey hotstrings work.

    Petrichor will allow you to do it, but the generated shortcuts will not work.

    [Escaping](../../getting-started/petrichor-script.html#escape-characters) the characters will not fix this.

???+ important "Restrictions"

    OPTIONAL

    No restrictions on number of instances that can be present.

    Must be in [`shortcut-list`](#shortcut-list-token) token body.

???+ example

    ```petrichor title="Input"
    shortcut-list:
    {
        shortcut-template: [tag] [last-tag] >> [id] - [name] [last-name] ([pronoun]) | {[decoration]} | [color]
    }
    ```

    ```autohotkey title="Shortcuts generated from input"
    ::sm smt::1234 - Sam Smith (they/them) | {a person} | #123456
    ::jo brn::5678 - Joe Brown (they/them) | {another person person} | #789abc
    ```


---
#### Find and replace tokens

The `find` and `replace` token pair defines a custom find-and-replace dictionary for a `shortcut-template` token.

The find-and-replace dictionary is only applied to the template's replacement string.

It is applied after `#!ptcr [field]` tags are populated with data, and therefore can modify that data.

`Find keys` and `replace values` are defined in comma-separated lists surrounded by curly brackets ( `{` `}` ).

The `find` and `replace` lists cannot contain blank items.

The `find` and `replace` lists must contain the same number of items as each other.

!!! note

    `Find keys` are case-sensitive.

!!! note

    If a `find` token does not have a matching `replace` token, all the `find` keys will just be removed from the template.

???+ important "Find token restrictions"

    OPTIONAL

    Maximum allowed: 1

    Must be in [`shortcut-template`](#shortcut-template-token) token body.


???+ important "Replace token restrictions"

    OPTIONAL

    Maximum allowed: 1

    Must be in [`shortcut-template`](#shortcut-template-token) token body.

    Must come after `find` token.


???+ example

    ```petrichor title="Input"
    shortcut-list:
    {
        shortcut-template: <hotstring> >> <replacement string> custom find 1, custom find 2, Custom find 2
        {
            find: { custom find 1, custom find 2 } // These are the `find keys`.
            replace: { replace 1, replace 2 } // These are the corresponding `replace values`.
        }
        shortcut-template: <hotstring> >> <replacement string> custom remove 1, custom remove 2, Custom remove 2
        {
            find: { custom remove 1, custom remove 2 } // These `find keys` will be removed, since there are no `replace values` for them.
        }
    }
    ```

    ```autohotkey title="Shortcuts generated from input"
    ::<hotstring>::<replacement string> replace 1, replace 2, Custom find 2
    ::<hotstring>::<replacement string> , , Custom remove 2
    ```

    If the `find keys` are present in `#!ptcr [field]` values within the `<replacement string>`, they will be replaced there as well.


---
## Text case token

The `text-case` token is used to change the text case of a shortcut after it is generated from a template.

Case conversion is applied after `#!ptcr [field]` tags are populated and [find-and-replace dictionaries](#find-and-replace-tokens) are applied.

Allowed values:

- unchanged (as-written; default)
- upper (UPPER CASE)
- lower (lower case)
- firstCaps (First Capitals Case)

???+ important "Restrictions"

    OPTIONAL

    Maximum allowed: 1

    Must be in [`shortcut-template`](#shortcut-template-token) token body.

???+ example

    === "unchanged"

        ```petrichor title="Input"
        shortcut-list:
        {
            shortcut-template: <hotstring> >> <replacement STRING>
            {
                text-case: unchanged
            }
        }
        ```

        ```autohotkey title="Shortcuts generated from input"
        ::<hotstring>::<replacement STRING>
        ```

    === "upper"

        ```petrichor title="Input"
        shortcut-list:
        {
            shortcut-template: <hotstring> >> <replacement STRING>
            {
                text-case: upper
            }
        }
        ```

        ```autohotkey title="Shortcuts generated from input"
        ::<hotstring>::<REPLACEMENT STRING>
        ```

    === "lower"

        ```petrichor title="Input"
        shortcut-list:
        {
            shortcut-template: <hotstring> >> <replacement STRING>
            {
                text-case: lower
            }
        }
        ```

        ```autohotkey title="Shortcuts generated from input"
        ::<hotstring>::<replacement string>
        ```

    === "firstCaps"

        ```petrichor title="Input"
        shortcut-list:
        {
            shortcut-template: <hotstring> >> <replacement STRING>
            {
                text-case: firstCaps
            }
        }
        ```

        ```autohotkey title="Shortcuts generated from input"
        ::<hotstring>::<Replacement String>
        ```


---
## Entry list token

The `entry-list` token contains entries to apply to templated shortcuts.

???+ important "Restrictions"

    REQUIRED

    Minimum required: 1

    Maximum allowed: 1

    Must come after [`metadata`](../../getting-started/petrichor-script.html#metadata-token) token.

???+ example

    ```petrichor
    metadata:
    {
        // Metadata goes here.
    }

    entry-list:
    {
        // Entries go here.
    }
    ```


---
### Entry token

The `entry` token defines a set of data to populate a templated shortcut with.

This token's value is ignored, but will show up in logs. You can put notes into it if desired.

This token's body contains subtokens defining its data.

These subtokens correspond to the `#!ptcr [field]` tags in [templated shortcuts](#shortcut-template-token).

!!! warning

    All token values should be unique, even though Petrichor wont take issue with it.

    If a value is repeated, the script generated from the input data can misbehave in unpredictable ways.

???+ important "Restrictions"

    OPTIONAL

    No restrictions on number of instances that can be present.

    Must be in [`entry-list`](#entry-list-token) token body.

???+ example

    ```petrichor
    entry-list:
    {
        entry: Notes about entry.
        {
            // Entry data goes here.
        }
    }
    ```


---
#### Color token

The `color` token defines a value for the `#!ptcr [color]` field tag in [templated shortcuts](#shortcut-template-token).

???+ important "Restrictions"

    OPTIONAL

    Maximum allowed: 1

    Must be in [`entry`](#entry-token) token body.

???+ example

    ```petrichor
    entry:
    {
        color: <value>
    }
    ```

---
#### Decoration token

The `decoration` token defines a value for the `#!ptcr [decoration]` field tag in [templated shortcuts](#shortcut-template-token).

???+ important "Restrictions"

    OPTIONAL

    Maximum allowed: 1

    Must be in [`entry`](#entry-token) token body.

???+ example

    ```petrichor
    entry:
    {
        decoration: <value>
    }
    ```


---
#### ID token

The `id` token defines a value for the `#!ptcr [id]` field tag in [templated shortcuts](#shortcut-template-token).

???+ important "Restrictions"

    REQUIRED

    Minimum required: 1

    Maximum allowed: 1

    Must be in [`entry`](#entry-token) token body.

???+ example

    ```petrichor
    entry:
    {
        id: <value>
    }
    ```


---
#### Name token

The `name` token defines values for the `#!ptcr [name]` and `#!ptcr [tag]` field tags in [templated shortcuts](#shortcut-template-token).

A shortcut will be generated from each template for each `name` token in an entry.

`Name` token values must consist of a `#!ptcr [name]` and `#!ptcr [tag]` field, separated by an at-sign ( `@` ).

The `#!ptcr [name]` field value can be any non-blank string that does not contain an at-sign ( `@` ).

The `#!ptcr [tag]` field value can be any string that does not contain whitespace.

???+ important "Restrictions"

    REQUIRED

    Minimum required: 1

    Must be in [`entry`](#entry-token) token body.

???+ example

    ```petrichor
    entry:
    {
        name: name string @tagstring
    }
    ```


---
#### Last name token

The `last-name` token defines values for the `#!ptcr [last-name]` and `#!ptcr [last-tag]` field tags in [templated shortcuts](#shortcut-template-token).

The `last-name` token has the same structure as the [`name`](#name-token) token.

???+ important "Restrictions"

    OPTIONAL

    Maximum allowed: 1

    Must be in [`entry`](#entry-token) token body.

???+ example

    ```petrichor
    entry:
    {
        last-name: name string @tagstring
    }
    ```


---
#### Pronoun token

The `pronoun` token defines a value for the `#!ptcr [pronoun]` field tag in [templated shortcuts](#shortcut-template-token).

???+ important "Restrictions"

    OPTIONAL

    Maximum allowed: 1

    Must be in [`entry`](#entry-token) token body.

???+ example

    ```petrichor
    entry:
    {
        pronoun: <value>
    }
    ```


---
### Full usage example

???+ example

    ```petrichor title="Input"
    metadata:
    {
        // Metadata goes here.
    }


    module-options:
    {
        default-icon: <file path>
        suspend-icon: <file path>
        reload-shortcut: <shortcut>
        suspend-shortcut: <shortcut>
    }


    shortcut-list:
    {
        shortcut: replaceme >> withme
        shortcut-template: [tag] >> [name] [last-name] ([pronoun]) [decoration]
        {
            find: { this, that }
            replace: { these, those }
            text-case: firstCaps
        }
    }


    entry-list:
    {
        entry: A
        {
            id: idValueA
            last-name: last name value A @lastTagValueA
            name: name value A 1 @tagValueA1
            name: name value A 2 @tagValueA2
            pronoun: pronounValueA this
            decoration: decorationValueA that
            color: colorValueA
        }

        entry: B
        {
            id: idValueB
            last-name: last name value B @lastTagValueB
            name: name value B 1 @tagValueB1
            name: name value B 2 @tagValueB2
            pronoun: pronounValueB THIS
            decoration: decorationValueB THAT
            color: colorValueB
        }
    }
    ```

    ```autohotkey title="Shortcuts generated from input"
    ::replaceme::withme
    ::tagValueA1::Name Value A 1 Last Name Value A (Pronounvaluea These) Decorationvaluea Those
    ::tagValueA2::Name Value A 2 Last Name Value A (Pronounvaluea These) Decorationvaluea Those
    ::tagValueB1::Name Value B 1 Last Name Value B (Pronounvalueb This) Decorationvalueb That
    ::tagValueB2::Name Value B 2 Last Name Value B (Pronounvalueb This) Decorationvalueb That
    ```
