REM @echo off
REM This script is part of MapWindow, www.mapwindow.org
REM and is covered by the Mozilla Public License.
REM Original author: Chris George. April 2011.

REM Script to run TauDEM function.
REM Parameters should form call of TauDEM function, with parameters, 
REM with quotes around any parameter that may contain spaces.
REM Code assumes at least 5 parameters, of form
REM <logfile> mpiexec -n <number> <taudem_command> <taudem_parameter_1> ...

REM Skip parameter 0 (name of called program)
SHIFT

REM Parameter 1 is file to get standard output and standard error, 
REM for display if error
SET logfile=%0
SHIFT

REM Store TauDEM executable
SET exec=%3
REM Skip "mpiexec -n <number>" if service mpich2_smpd not running
sc query mpich2_smpd | find /i "RUNNING">NUL
IF %errorlevel%==0 GOTO :BUILDCOMMAND
SHIFT
SHIFT
SHIFT

:BUILDCOMMAND
SET taudemcmd=%0
:REPEAT
SHIFT
IF (%0)==() GOTO :RUN
SET taudemcmd=%taudemcmd% %0
GOTO :REPEAT

:RUN
REM Log actual command
echo.TauDEM command: > %logfile%
echo.%taudemcmd% >> %logfile%
echo. >> %logfile%
REM run TauDEM function, logging standard output and standard error
%taudemcmd% 1>> %logfile% 2>>&1
SET taudemresult=%errorlevel%

echo. >> %logfile%
IF %taudemresult% == 0 GOTO :SUCCESS
echo.Error in executing %exec% >> %logfile%
EXIT %taudemresult%

:SUCCESS
echo.Executed %exec% successfully >> %logfile%
EXIT 0

