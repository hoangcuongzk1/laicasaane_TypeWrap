# TypeWrap

This library utilizes Roslyn Source Generator to generate type wrappers for any C# type.

## Features

- Any C# type can be wrapped, with an exception of `dynamic`.
- Wrappers can be a `struct`, a `class`, or a `record`.
    - A `record` wrapper can also be generic if needed.
- Wrappers have `implicit` and `explicit` operators to convert between itself and the original type.
- All public members of the original type are exposed in the wrapper. Supported members are fields, properties,
  events, indexers, methods, and operators.
- `IEquatable<T>` and `IComparable<T>` are implemented for the wrapper if the original type allows them.
- Equality operators (`==`, `!=`) are overloaded for the wrapper if the original type allows them.

## Installation

### Requirements

- Unity 2022.3 or later

### Unity Package Manager

1. Open menu `Window` -> `Package Manager`.
2. Click the `+` button at the top-left corner, then choose `Add package from git URL...`.

    ![add package by git url](imgs/add-package-by-git-url-1.png)

3. Enter the package URL 
    ```
    https://github.com/laicasaane/TypeWrap.git?path=/Packages/com.laicasaane.typewrap#1.2.4
    ```

    ![enter git url then press add button](imgs/add-package-by-git-url-2.png)

### OpenUPM

1. Install [OpenUPM CLI](https://openupm.com/docs/getting-started.html#installing-openupm-cli).
2. Run the following command in your Unity project root directory:

```sh
openupm add com.laicasaane.typewrap
```

## Usage

### [WrapType] Attribute

- Use this attribute if the wrapper itself is either a `struct` or a `class`.
    - In case of a `class`, it must not inherit from any other class.
    - The wrapper must be `partial`.
- By default, the underlying type name is `value`. You can change it by specifying the `memberName` argument.
- By default, a type converter is generated for the wrapper. You can exclude it by specifying the `ExcludeConverter` property.

```cs
[WrapType(typeof(int))]
public partial struct IntWrapper { }

[WrapType(typeof(List<int>), memberName: "wrappedList")]
public partial class ListInt { }

[WrapType(typeof(IDisposable), ExcludeConverter = true)]
public readonly partial struct DisposableObject { }
```

### [WrapRecord] Attribute

- Use this attribute if the wrapper itself is either a `record struct` or a `record class`.
    - In case of a `record class`, it must not inherit from any other class.
    - The wrapper must be `partial`.
- The primary constructor of the record must have exactly 1 parameter.
- By default, a type converter is generated for the wrapper.
  You can exclude it by specifying the `ExcludeConverter` property.

```cs
[WrapRecord(ExcludeConverter = true)]
public partial record struct IntWrapper(int Value);

[WrapRecord]
public partial record class ListT<T>(List<T> _);

[WrapRecord]
public readonly partial record struct Coord2D(Vector2Int _);
```

### A sample for generated code

- For this user-written code:

  ```cs
  public enum FruitKind { Apple, Banana, Orange, }

  [WrapRecord]
  public readonly partial record struct FruitKindValue(FruitKind _);
  ```

- Source generator will emit something like this:

  [FruitKindValue.g.cs](docs/FruitKindValue.g.cs)

> [!NOTE]
> The file above has been sanitized to a degree to improve readability within this tutorial.
> The actual generated code includes additional attributes and fully qualified type names
> to ensure correctness and avoid ambiguities.

## Notes

- To have `record` in Unity, you'll need
  - Unity 2022.3 or later
  - Enable `C# 10` feature by placing this
  [`csc.rsp`](Packages/com.laicasaane.typewrap/Samples~/TypeWrap.Samples/csc.rsp) file inside your `Assets` folder.
    - Or better: place it inside the folder that contains an Assembly Definition (`.asmdef` file).

- To have `readonly record` in any assembly (or `.asmdef`):
  - Copy this [`IsExternalInit.cs`](Packages/com.laicasaane.typewrap/Samples~/TypeWrap.Samples/IsExternalInit.cs) file
  into that assembly.
    - Or better: copy to the core assembly which is referenced by other assemblies.

- You might also want to place this [`Directory.Build.props`](Directory.Build.props) file in your `Assets`
  **and** the root of your project to enable the `C# 10` feature for the code editor.

> [!IMPORTANT]
> If you're using Visual Studio or VSCode and have installed packagg [`com.unity.ide.visualstudio`][vspackage2_0_24]
> version `2.0.24` or later, you don't need `Directory.Build.props` file. Because the compiler option `-langversion`
> specified in `csc.rsp` file will be respected and applied to the generated `.csproj` files.
>

[vspackage2_0_24]: https://docs.unity3d.com/Packages/com.unity.ide.visualstudio@2.0/changelog/CHANGELOG.html#2024---2025-09-04
