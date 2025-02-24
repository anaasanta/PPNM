Out.txt : main.exe
	mono main.exe > Out.txt

main.exe: main.cs functions.dll
	mcs -target:exe -out:main.exe -reference:functions.dll -reference:System.Numerics.dll main.cs

functions.dll: functions.cs
	mcs -target:library -out:functions.dll -reference:System.Numerics.dll functions.cs

clean:
	rm -f main.exe functions.dll Out.txt
