cd ..\..\..\..\WindowsGame1\bin\x86\Release\
IF EXISTS GravityShift.exe
( 
	GravityShift.exe %1
)
IF NOT EXISTS GravityShift.exe
(
	cd ..\Debug\
	GravityShift.exe %1
)

