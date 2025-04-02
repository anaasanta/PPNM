exA.txt: Makefile exA.exe
	mono exA.exe > exA.txt
	sed -i 's/,/./g' exA.txt

exA.exe: exA.cs vector.dll
	mcs -target:exe -out:exA.exe -reference:vector.dll exA.cs

vector.dll: vector.cs
	mcs -target:library -out:vector.dll vector.cs

exA.svg: exA.txt
	echo " \
		set terminal svg; \
		set output 'exA.svg'; \
		set key left; \
		set title 'Spline Interpolation'; \
		set xlabel 'x'; \
		set ylabel 'y'; \
		set terminal svg background rgb 'white';\
		plot cos(x) with lines title 'cos(x)' lc 'red', \
			 '$<' using 1:2 with lines title 'Linear Interpolant' lc 'green'  , \
			 '$<' using 1:3 with lines title 'Anti-derivative' lc 'blue'\
	" | gnuplot
	
clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
