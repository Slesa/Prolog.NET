#I @"Tools\Fake"
#r "FakeLib.dll"

open Fake

// properties
let projectName = "Prolog.NET"
//let projectSummary = ""
let authors = ["R. Todd", "J. Preiss"]

TraceEnvironmentVariables()

// Directories
let binDir = @".\deploy\"
let buildDir = binDir @@ @"build\"
let testDir = binDir @@ @"test\"
let reportDir = binDir @@ @"report\"
let packagesDir = binDir @@ @"packages"
let mspecDir = packagesDir @@ "MSpec"

// Files
let appReferences  = 
  !+ @"**\Lingua\Lingua.csproj" 
    ++ @"**\LinguaDemo\LinguaDemo.csproj"
        |> Scan

let testReferences = 
  !+ @"**\Lingua.Specs\Lingua.Specs.csproj" 
    |> Scan

// Targets
Target "Clean" (fun _ ->
  CleanDirs [buildDir; testDir; reportDir; packagesDir]

  CreateDir mspecDir
  !! (@"packages\Machine.Specifications.*\**\*.*")
    |> CopyTo mspecDir
)

Target "BuildApp" (fun _ ->
  MSBuildRelease buildDir "Build" appReferences
    |> Log "AppBuild-Output: "
)

Target "BuildTest" (fun _ ->
  MSBuildDebug testDir "Build" testReferences
    |> Log "TestBuildOutput: "
)

Target "RunTest" (fun _ ->
  let mspecTool = mspecDir @@ "mspec-clr4.exe"
  trace mspecTool

  !! (testDir @@ "*.Specs.dll")
    |> MSpec (fun p ->
      {p with
        ToolPath = mspecTool
        HtmlOutputDir = reportDir})
)

Target "Default" DoNothing

// Dependencies
"Clean"
  ==> "BuildApp" <=> "BuildTest"
  ==> "RunTest"
  ==> "Default"

// start build
RunParameterTargetOrDefault "target" "Default"

