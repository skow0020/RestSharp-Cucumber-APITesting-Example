@echo off
REM Usage: Run.bat <ENVIRONMENT> <SCENARIO_CATEGORY>
IF "%1"=="help" (
	ECHO	"Usage: Run.bat <ENVIRONMENT> <SCENARIO_CATEGORY>"
	EXIT /B 1
)

IF "%1"=="" (
	ECHO	"Usage: Run.bat <ENVIRONMENT> <SCENARIO_CATEGORY>"
	EXIT /B 1
)

IF "%2"=="" (
	ECHO	"Usage: Run.bat <ENVIRONMENT> <SCENARIO_CATEGORY>"
	EXIT /B 1
)

SET environment=%1
SET category=%2


For /f "tokens=2-4 delims=/ " %%a in ('date /t') do (set mydate=%%a%%b)
For /f "tokens=1-2 delims=/:,/ " %%a in ('time /t') do (set mytime=%%a%%b)

SET resultsFile=TestResults_%mydate%_%mytime%.xml

REM Clean and Build the testing solution
"D:/Program Files (x86)/Microsoft Visual Studio 14.0/Common7/IDE/devenv" APIAutomationTestSuite.sln /Clean
"D:/Program Files (x86)/Microsoft Visual Studio 14.0/Common7/IDE/devenv" APIAutomationTestSuite.sln /Build "Debug|AnyCPU"

REM Kick off tests
"packages\NUnit.ConsoleRunner.3.10.0\tools\nunit3-console.exe" APIAutomationTestSuite\bin\Debug\APIAutomationTestSuite.dll --where "cat == %category%" --result=%resultsFile%