# PySharp Interpreter
PySharp is a programming language I've created and implemented using this interpreter in C#. You can find the language's specifications and how to run it in this file.

## Installation and usage
### Installation
1. [.NET 9.0](https://dotnet.microsoft.com/en-us/download/dotnet/9.0) must be installed to run the program and install it as a package. You can run the program as any other .NET project with `dotnet run`
2. To prepare the package for installation run `dotnet pack` in the main directory of the C# project
3. You can install the interpreter globally with `dotnet tool install --global --add-source ./nupkg PySharpCompiler`
4. Now you can call the interpreter in any directory with `pycs`

### Usage
**Running code in a .pycs file**
`$ pycs --f program.pycs`

**Running code in terminal**
`pycs "print('Hello world');"`

## Language specifications
- The language is static and weakly typed.
- Data types:
  - `bool`
  - `int`
  - `float`
  - `string`
  - `function`
  - `list`
  - | left side \  right side | `bool` | `int` | `float` | `string` | `function` | `list` |
    | -------------------------- | --------------------------------- | -------------------------------------------------- | ------------------------- | --------------------------------------- | ------------------ | --------------------- |
    | `bool` | **error** | Non zero to `int` `true`, zero to `false` | As in `int` | Empty to `false`, otherwise `true` | **error** | **error** |
    | `int` | `true` -> `1`, `false` -> `0` | **always** | Round down | Attempt converting to `int`, otherwise error | **błąd** | **error** |
    | `float` | `true` -> `1.0`, `false` -> `0.0` | `1` -> `1.0` | **always** | Conversion attempt, otherwise error | **error** | **error** |
    | `string` | `true, false` -> `'true, false'` | `1` -> `'1'` | `1.0` -> `'1.0'` | **always** | `(params) -> type` | `[1,2]` -> `'[1, 2]'` |
    | `function` | **error** | **error** | **error** | **error** | **always** | **error** |
    | `list` | **error** | **error** | **error** | **error** | **error** | **always** |
