Out.txt: main.exe
	mono main.exe n=$(n) > Out.txt

main.exe: main.cs vector.dll matrix.dll jacobi.dll
	mcs -target:exe -out:main.exe -reference:vector.dll -reference:matrix.dll -reference:jacobi.dll main.cs

jacobi.dll: jacobi.cs matrix.dll vector.dll
	mcs -target:library -out:jacobi.dll -reference:matrix.dll -reference:vector.dll jacobi.cs

vector.dll: vector.cs
	mcs -target:library -out:vector.dll vector.cs

matrix.dll: matrix.cs vector.dll
	mcs -target:library -out:matrix.dll -reference:vector.dll matrix.cs


clean:
	rm -f *.dll *.exe Out.txt
