out_exA.txt out_exA_oscillator.txt: Makefile exA.exe
	mono exA.exe > out_exA.txt
	mono exA.exe -oscillator > out_exA_oscillator.txt
	sed -i 's/,/./g' out_exA.txt out_exA_oscillator.txt

out_exB_circular.txt out_exB_ellipse.txt out_exB_precession.txt: Makefile exB.exe
	mono exB.exe > out_exB_circular.txt out_exB_ellipse.txt out_exB_precession.txt
	sed -i 's/,/./g' out_exB_circular.txt out_exB_ellipse.txt out_exB_precession.txt

out_exC_1.txt out_exC_2.txt out_exC_4.txt: Makefile exC.exe
	mono exC.exe > out_exC_1.txt out_exC_2.txt out_exC_4.txt
	sed -i 's/,/./g' out_exC_1.txt out_exC_2.txt out_exC_4.txt

exA.exe: exA.cs vector.dll solveODE.dll
	mcs -target:exe -out:exA.exe -reference:vector.dll,solveODE.dll exA.cs

exB.exe: exB.cs vector.dll solveODE.dll
	mcs -target:exe -out:exB.exe -reference:vector.dll,solveODE.dll exB.cs

exC.exe: exC.cs vector.dll solveODE.dll
	mcs -target:exe -out:exC.exe -reference:vector.dll,solveODE.dll exC.cs

vector.dll: vector.cs
	mcs -target:library -out:vector.dll vector.cs

solveODE.dll: solveODE.cs vector.dll
	mcs -target:library -out:solveODE.dll -reference:vector.dll solveODE.cs

exA.svg: out_exA.txt
	echo " \
		set terminal svg; \
		set output 'exA.svg'; \
		set key left; \
		set title ' Solution to u''=u with RK ODE integrator'; \
  		set xlabel 't'; \
  		set ylabel 'u(t)'; \
		set terminal svg background rgb 'white';\
		plot '$<' using 2:3 with lines title 'XY' lc 'red', \
			 '$<' using 1:2 with lines title 'x(t)' lc 'green'  , \
			 '$<' using 1:3 with lines title 'y(t)' lc 'blue'\
	" | gnuplot

exA_oscillator.svg : out_exA_oscillator.txt
	echo " \
		set terminal svg; \
		set output 'exA_oscillator.svg'; \
		set key left; \
		set title 'Reproduction example oscillator with friction'; \
		set xlabel 'x'; \
		set ylabel 'y'; \
		set terminal svg background rgb 'white';\
		plot '$<' using 1:2 with lines title 'theta(t)' lc 'green', \
			 '$<' using 1:3 with lines title 'omega(t)' lc 'blue'\
	" | gnuplot

exB.svg: out_exB_circular.txt out_exB_ellipse.txt out_exB_precession.txt
	echo " \
		set terminal svg; \
		set output 'exB.svg'; \
		set key left; \
		set terminal svg background rgb 'white';\
		set title 'Relativistic precession of planetary orbits'; \
		set xlabel 'x'; \
		set ylabel 'y'; \
		plot 'out_exB_circular.txt' using (cos(\$$1)/\$$2):(sin(\$$1)/\$$2) with lines title 'Newtonian circular motion', \
			 'out_exB_ellipse.txt' using (cos(\$$1)/\$$2):(sin(\$$1)/\$$2) with lines title 'Newtonian elliptical motion', \
			 'out_exB_precession.txt' using (cos(\$$1)/\$$2):(sin(\$$1)/\$$2) with lines title 'Relativistic precession of planetary orbit' \
	" | gnuplot


exC_1.svg : out_exC_1.txt
	echo " \
		set terminal svg; \
		set output 'exC_1.svg'; \
		set key left; \
		set terminal svg background rgb 'white';\
		set title 'test of stepper of order 23 on ddy=2x';\
		set xlabel 'x'; \
		set ylabel 'y'; \
		f(x) = x**3/3; \
		plot '$<' every::1 using 1:2 with lines title 'x(t)' lc 'blue'  , \
			  f(x) with lines title 'x^3/3' \
	" | gnuplot

exC_2.svg :  out_exC_2.txt
	echo " \
		set terminal svg; \
		set output 'exC_2.svg'; \
		set key left; \
		set terminal svg background rgb 'white';\
		set title 'test of driver that returns the quadratic spline of the table {xi,yi}';\
		set xlabel 'x'; \
		set ylabel 'y'; \
		f(x) = x**3/3; \
		plot '$<' using 1:2 with lines title 'x(t)' lc 'green'  , \
			  f(x) with lines title 'x^3/3' \
	" | gnuplot

exC_4.svg : out_exC_4.txt
	echo " \
		set terminal svg; \
		set output 'exC_4.svg'; \
		set key left; \
		set terminal svg background rgb 'white';\
		set title 'Stable planar periodic solution of the Newtonian gravitational three-body problem';\
		set xlabel 'x'; \
		set ylabel 'y'; \
		plot '$<' using 1:2 with lines title 'mass 1', \
			 '$<' using 3:4 with lines title 'mass 2', \
			 '$<' using 5:6 with lines title 'mass 3' \
	" | gnuplot
clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
