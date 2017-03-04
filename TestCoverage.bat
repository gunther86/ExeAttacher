@echo off
cls

:: VSCommandPrompt
if not defined DevEnvDir (
    call "%VS140COMNTOOLS%\VsDevCmd.bat"
)

:: Local variables
set CURRENT_DIR=.\
set SOLUTION_NAME=ExeAttacher.sln
set COMPILATION_DROP_DIR=bin\Debug
set COVERAGE_DIR=%CURRENT_DIR%CoverageAnalysis\
set REPORT_DIR=%COVERAGE_DIR%Report\
set OPEN_COVER_PATH=%CURRENT_DIR%packages\OpenCover.4.6.519\tools\OpenCover.Console.exe
set REPORT_GENERATOR_PATH=%CURRENT_DIR%packages\ReportGenerator.2.5.5\tools\ReportGenerator.exe
set TEST_RUNNER_PATH=%CURRENT_DIR%\packages\xunit.runner.console.2.2.0\tools\xunit.console.exe
set RESULTS_FILE_NAME=TestResults.xml
set FILTER=" +[ExeAttacher*]* -[*Tests*]* -[*UI.Resources*]* "

:: Print local variables
echo.
echo CURRENT_DIR                    = %CURRENT_DIR%
echo SOLUTION_NAME                  = %SOLUTION_NAME%
echo COMPILATION_DROP_DIR           = %COMPILATION_DROP_DIR%
echo COVERAGE_DIR                   = %COVERAGE_DIR%
echo REPORT_DIR 					= %REPORT_DIR%
echo OPEN_COVER_PATH                = %OPEN_COVER_PATH%
echo REPORT_GENERATOR_PATH          = %REPORT_GENERATOR_PATH%
echo TEST_RUNNER_PATH				= %TEST_RUNNER_PATH%
echo RESULTS_FILE_NAME 				= %RESULTS_FILE_NAME%
echo FILTER							= %FILTER%
echo.

:: Restore Nuget packages
msbuild %CURRENT_DIR%%NUGET_DIR%\NuGet.targets /target:RestorePackages

:: Compile solution
msbuild %CURRENT_DIR%%SOLUTION_NAME%^
 /target:Rebuild^
 /property:Configuration=Debug;OutDir=%CURRENT_DIR%\%COMPILATION_DROP_DIR%\

:: Clean folders
if exist  %COVERAGE_DIR% (
    rmdir %COVERAGE_DIR% /s /q 2>NUL
	)
if exist  %CURRENT_DIR%TestResults (
    rmdir %CURRENT_DIR%TestResults /s /q 2>NUL
	)

:: Create folders
mkdir %COVERAGE_DIR% 2>NUL
mkdir %REPORT_DIR% 2>NUL

setlocal enableextensions enabledelayedexpansion
for /f %%F in ('dir /b *.Tests') do (
	:: Clean loop variables
	set PROJECT_DIR=
	set PROJECT_FILE=
	:: Set loop variables
	set PROJECT_DIR=!PROJECT_DIR!%%F
	set PROJECT_FILE=!PROJECT_DIR!\%COMPILATION_DROP_DIR%\!PROJECT_DIR!.dll
  
	:: Create the coverageResults foler
	mkdir %COVERAGE_DIR%!PROJECT_DIR! 2>NUL
	set REPORTS=!REPORTS!%COVERAGE_DIR%!PROJECT_DIR!\%RESULTS_FILE_NAME%;

	:: Run Unit test through OpenCover
    :: This allows OpenCover to gather code coverage results
	call %OPEN_COVER_PATH%^
		 -register:user^
		 -target:%TEST_RUNNER_PATH%^
		 -targetargs:"%CURRENT_DIR%!PROJECT_FILE! -noshadow"^
		 -coverbytest:*^
		 -filter:%FILTER%^
		 -output:%COVERAGE_DIR%!PROJECT_DIR!\%RESULTS_FILE_NAME%
)

:: Generate the report
call %REPORT_GENERATOR_PATH%^
	-reports:!REPORTS!^
	-targetdir:%REPORT_DIR%!>NUL
    
:: Open the coverage reports
start %REPORT_DIR%\index.htm
endlocal