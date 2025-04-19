using OPS.Domain.Enums;

namespace OPS.Infrastructure.OneCompiler;

internal static class LanguageHelper
{
    public static string GetDefaultFileName(LanguageId language)
    {
        return s_fileNames.GetValueOrDefault(language, "main.txt");
    }

    private static readonly Dictionary<LanguageId, string> s_fileNames = new()
    {
        { LanguageId.java, "Main.java" },
        { LanguageId.python, "index.py" },
        { LanguageId.python2, "index.py" },
        { LanguageId.c, "main.c" },
        { LanguageId.cpp, "main.cpp" },
        { LanguageId.nodejs, "index.js" },
        { LanguageId.javascript, "index.js" },
        { LanguageId.groovy, "main.groovy" },
        { LanguageId.jshell, "Main.jsh" },
        { LanguageId.haskell, "main.hs" },
        { LanguageId.tcl, "main.tcl" },
        { LanguageId.lua, "main.lua" },
        { LanguageId.ada, "main.adb" },
        { LanguageId.commonlisp, "main.lisp" },
        { LanguageId.d, "main.d" },
        { LanguageId.elixir, "main.ex" },
        { LanguageId.erlang, "main.erl" },
        { LanguageId.fsharp, "Program.fs" },
        { LanguageId.fortran, "main.f90" },
        { LanguageId.assembly, "main.asm" },
        { LanguageId.scala, "Main.scala" },
        { LanguageId.php, "index.php" },
        { LanguageId.csharp, "Program.cs" },
        { LanguageId.perl, "main.pl" },
        { LanguageId.ruby, "main.rb" },
        { LanguageId.go, "main.go" },
        { LanguageId.r, "main.r" },
        { LanguageId.racket, "main.rkt" },
        { LanguageId.ocaml, "main.ml" },
        { LanguageId.vb, "Program.vb" },
        { LanguageId.basic, "main.bas" },
        { LanguageId.bash, "script.sh" },
        { LanguageId.clojure, "main.clj" },
        { LanguageId.typescript, "index.ts" },
        { LanguageId.cobol, "main.cob" },
        { LanguageId.kotlin, "Main.kt" },
        { LanguageId.pascal, "main.pas" },
        { LanguageId.prolog, "main.pl" },
        { LanguageId.rust, "main.rs" },
        { LanguageId.swift, "main.swift" },
        { LanguageId.objectivec, "main.m" },
        { LanguageId.octave, "main.m" },
        { LanguageId.text, "text.txt" },
        { LanguageId.brainfk, "main.bf" },
        { LanguageId.coffeescript, "main.coffee" },
        { LanguageId.ejs, "index.ejs" },
        { LanguageId.dart, "main.dart" },
        { LanguageId.deno, "index.ts" },
        { LanguageId.bun, "index.ts" }
    };
}