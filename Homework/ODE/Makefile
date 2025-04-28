exA.txt: Makefile exA.exe
	mono exA.exe > exA.txt
	sed -i 's/,/./g' exA.txt

exA_oscillator.txt: Makefile exA.exe
	mono exA.exe -oscillator > exA_oscillator.txt
	sed -i 's/,/./g' exA_oscillator.txt

exB_circular.txt: Makefile exB.exe
	mono exB.exe > exB_circular.txt
	sed -i 's/,/./g' exB_circular.txt

exB_ellipse.txt: Makefile exB.exe
	mono exB.exe > exB_ellipse.txt
	sed -i 's/,/./g' exB_ellipse.txt

exB_precession.txt: Makefile exB.exe
	mono exB.exe > exB_precession.txt
	sed -i 's/,/./g' exB_precession.txt

exA.exe: exA.cs vector.dll solveODE.dll
	mcs -target:exe -out:exA.exe -reference:vector.dll,solveODE.dll exA.cs

exB.exe: exB.cs vector.dll solveODE.dll
	mcs -target:exe -out:exB.exe -reference:vector.dll,solveODE.dll exB.cs

vector.dll: vector.cs
	mcs -target:library -out:vector.dll vector.cs

solveODE.dll: solveODE.cs vector.dll
	mcs -target:library -out:solveODE.dll -reference:vector.dll solveODE.cs

exA.svg: exA.txt
	echo " \
		set terminal svg; \
		set output 'exA.svg'; \
		set key left; \
		set terminal svg background rgb 'white';\
		plot '$<' using 2:3 with lines title 'XY' lc 'red', \
			 '$<' using 1:2 with lines title 'x(t)' lc 'green'  , \
			 '$<' using 1:3 with lines title 'y(t)' lc 'blue'\
	" | gnuplot

exA_oscillator.svg : exA_oscillator.txt
	echo " \
		set terminal svg; \
		set output 'exA_oscillator.svg'; \
		set key left; \
		set terminal svg background rgb 'white';\
		plot '$<' using 1:2 with lines title 'theta(t)' lc 'green'  , \
			 '$<' using 1:3 with lines title 'omega(t)' lc 'blue'\
	" | gnuplot

exB_circular.svg : exB_circular.txt
	echo " \
		set terminal svg; \
		set output 'exB_circular.svg'; \
		set key left; \
		set terminal svg background rgb 'white';\
		plot '$<' using (cos(\$$1)/\$$2):(sin(\$$1)/\$$2) with lines notitle \
	" | gnuplot

exB_ellipse.svg : exB_ellipse.txt
	echo " \
		set terminal svg; \
		set output 'exB_ellipse.svg'; \
		set key left; \
		set terminal svg background rgb 'white';\
		plot '$<' using (cos(\$$1)/\$$2):(sin(\$$1)/\$$2) with lines notitle \
	" | gnuplot

exB_precession.svg : exB_precession.txt
	echo " \
		set terminal svg; \
		set output 'exB_precession.svg'; \
		set key left; \
		set terminal svg background rgb 'white';\
		plot '$<' using (cos(\$$1)/\$$2):(sin(\$$1)/\$$2) with lines notitle \
	" | gnuplot


clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
