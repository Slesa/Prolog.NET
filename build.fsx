#I @"Tools\Fake"
#r "FakeLib.dll"

open Fake

// properties
let projectName = "Prolog.NET"
let projectSummary = "A .NET-base implementation of Prolog based on the Warren Abstract Machine (WAM) architecture."
let authors = ["R. Todd"; "J. Preiss"]
let homepage = "https://github.com/Slesa/Prolog.NET"
let mail = "joerg.preiss@slesa.de"

let currentVersion =
  if not isLocalBuild then buildVersion else
  "1.0.1"

TraceEnvironmentVariables()


// Directories
let srcDir = @".\src\"
let binDir = @".\bin\"
let buildDir = binDir @@ @"build\"
let testDir = binDir @@ @"test\"
let nugetDir = binDir @@ @"nuget\"
let reportDir = binDir @@ @"report\"
let packagesDir = srcDir @@ @"packages\"
let samplesDir = @".\Samples\"
let testsDir = @".\Tests\"

// Tools
let MSpecVersion = GetPackageVersion packagesDir "Machine.Specifications"
let mspecTool = sprintf @"%sMachine.Specifications.%s\tools\mspec-clr4.exe" packagesDir MSpecVersion 


// Files
let appReferences  = 
  !+ @"**\Prolog\Prolog.csproj" 
    ++ @"src\PrologTest\PrologTest.csproj"
    ++ @"src\PrologScheduler\PrologScheduler.csproj"
    ++ @"src\PrologWorkbench\PrologWorkbench.csproj"
        |> Scan

let testReferences = 
  !+ @"**\Prolog.Specs\Prolog.Specs.csproj" 
    |> Scan


// Targets
Target "Clean" (fun _ ->
  CleanDirs [buildDir; testDir; nugetDir; reportDir ]
)

Target "SetAssemblyInfo" (fun _ ->
  AssemblyInfo
    (fun p ->
    {p with
      CodeLanguage = CSharp;
      Guid = "";
      ComVisible = None;
      CLSCompliant = None;
      AssemblyCompany = "Richard G. Todd";
      AssemblyProduct = "Prolog.NET";
      AssemblyCopyright = "Copyright Â©  2010";
      AssemblyTrademark = "MS Public License";
      AssemblyVersion = currentVersion;
      OutputFileName = srcDir @@ @"\VersionInfo.cs"})
)

Target "BuildApp" (fun _ ->
  MSBuildRelease buildDir "Build" appReferences
    |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
  MSBuildDebug testDir "Build" testReferences
    |> Log "TestBuild-Output: "
)

Target "RunTest" (fun _ ->
  !! (testDir @@ "*.Specs.dll")
    |> MSpec (fun p ->
      {p with
        ToolPath = mspecTool
        HtmlOutputDir = reportDir})
)

Target "Deploy" (fun _ ->
  
  let libDir = nugetDir @@ @"lib\net40"
  CreateDir libDir
  let toolsDir = nugetDir @@ @"tools\"
  CreateDir toolsDir
  let contentDir = nugetDir @@ @"content\"
  CreateDir contentDir
  let nugetExe = @"tools\NuGet\NuGet.exe"

  !+ (buildDir @@ "Prolog.dll")
    ++ (buildDir @@ "Lingua.dll")
      |>Scan
        |> CopyTo libDir
  !+ (buildDir @@ "Prolog.dll")
    ++ (buildDir @@ "Microsoft.WindowsAPICodePack.dll")
    ++ (buildDir @@ "Microsoft.WindowsAPICodePack.Shell.dll")
    ++ (buildDir @@ "Lingua.dll")
    ++ (buildDir @@ "PrologTest.exe")
    ++ (buildDir @@ "PrologTest.exe.config")
    ++ (buildDir @@ "PrologScheduler.exe")
    ++ (buildDir @@ "PrologScheduler.exe.config")
    ++ (buildDir @@ "PrologWorkbench.exe")
    ++ (buildDir @@ "PrologWorkbench.exe.config")
      |> Scan
        |> CopyTo toolsDir
  !+ "Copyright.txt"
    ++ "License.txt"
    ++ "License-Lingua.txt"
    ++ "Readme.txt"
    |> Scan
      |> CopyTo contentDir
  
  let nugetSamplesDir = toolsDir @@ @"Samples\"
  CreateDir nugetSamplesDir
  let nugetTestsDir = toolsDir @@ @"Tests\"
  CreateDir nugetTestsDir

  XCopy samplesDir nugetSamplesDir
  XCopy testsDir nugetTestsDir

  NuGet (fun p -> 
    {p with               
      Authors = authors
      Project = projectName
      Description = projectSummary                               
      OutputPath = nugetDir
      AccessKey = getBuildParamOrDefault "nugetkey" ""
      Publish = hasBuildParam "nugetkey" }) "Prolog.nuspec"
)

Target "Default" DoNothing


// Dependencies
"Clean"
  ==> "SetAssemblyInfo"
  ==> "BuildApp" <=> "BuildTest"
  ==> "RunTest"
  ==> "Deploy"
  ==> "Default"


// start build
RunParameterTargetOrDefault "target" "Default"

