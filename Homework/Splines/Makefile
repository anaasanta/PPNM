exA.txt: Makefile exA.exe
	mono exA.exe > exA.txt
	sed -i 's/,/./g' exA.txt

exB.txt: Makefile exB.exe
	mono exB.exe > exB.txt
	sed -i 's/,/./g' exB.txt

exC.txt: Makefile exC.exe
	mono exC.exe > exC.txt
	sed -i 's/,/./g' exC.txt

exA.exe: exA.cs vector.dll
	mcs -target:exe -out:exA.exe -reference:vector.dll exA.cs

exB.exe: exB.cs vector.dll
	mcs -target:exe -out:exB.exe -reference:vector.dll exB.cs

exC.exe: exC.cs vector.dll
	mcs -target:exe -out:exC.exe -reference:vector.dll exC.cs

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

exB.svg: exB.txt
	echo " \
		set terminal svg; \
		set output 'exB.svg'; \
		set key left; \
		set title 'Quadratic Interpolation'; \
		set xlabel 'x'; \
		set ylabel 'y'; \
		set terminal svg background rgb 'white';\
		plot '$<' using 1:2 with lines title 'Interpolation' lc 'green'  , \
			 '$<' using 1:3 with lines title 'Derivative' lc 'blue' , \
			 '$<' using 1:4 with lines title 'Anti-Derivative' lc 'red' \
	" | gnuplot

exC.svg: exC.txt
	echo " \
		set terminal svg; \
		set output 'exC.svg'; \
		set key left; \
		set title 'Cubic Interpolation'; \
		set xlabel 'x'; \
		set ylabel 'y'; \
		set terminal svg background rgb 'white';\
		plot '$<' using 1:2 with lines title 'Interpolation' lc 'green'  , \
			 '$<' using 1:3 with lines title 'Derivative' lc 'blue' , \
			 '$<' using 1:4 with lines title 'Anti-Derivative' lc 'red' \
	" | gnuplot
	
clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
