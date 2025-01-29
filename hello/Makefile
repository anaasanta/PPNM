Out.txt : main.exe
	mono main.exe > Out.txt

main.exe : main.cs hello.dll
	mcs -target:exe -reference:hello.dll -out:main.exe main.cs

hello.dll : hello.cs
	mcs -target:library -out:hello.dll hello.cs
