Out.txt: main.exe
	mono main.exe n=$(n) > Out.txt

Hydrogen.txt : exerciseB.exe
	mono exerciseB.exe rmax=$(rmax) dr=$(dr) > Hydrogen.txt

exerciseB.exe: exerciseB.cs matrix.dll vector.dll jacobi.dll
	mcs -target:exe -out:exerciseB.exe -reference:matrix.dll -reference:vector.dll -reference:jacobi.dll exerciseB.cs

main.exe: main.cs vector.dll matrix.dll jacobi.dll
	mcs -target:exe -out:main.exe -reference:vector.dll -reference:matrix.dll -reference:jacobi.dll main.cs

jacobi.dll: jacobi.cs matrix.dll vector.dll
	mcs -target:library -out:jacobi.dll -reference:matrix.dll -reference:vector.dll jacobi.cs

vector.dll: vector.cs
	mcs -target:library -out:vector.dll vector.cs

matrix.dll: matrix.cs vector.dll
	mcs -target:library -out:matrix.dll -reference:vector.dll matrix.cs

plot.convergence_dr.svg: convergence_dr.txt Makefile
	echo "set terminal svg; \
		set output 'plot.convergence_dr.svg'; \
		set terminal svg background rgb 'white';\
		set title 'Convergence in Δr (with a fixed rmax)'; \
		set xlabel 'Δr'; \
	    set ylabel 'ε₀'; \
	    plot 'convergence_dr.txt' using 1:2 with linespoints title 'ε₀ vs Δr'" | gnuplot

convergence_dr.txt: exerciseB.exe
	for dr in $$(seq 0.1 0.1 0.8); do \
		mono exerciseB.exe rmax=10 dr=$$dr mode=dr; \
	done
	sed -i 's/,/./g' convergence_dr.txt

plot.convergence_rmax.svg: convergence_rmax.txt Makefile
	echo "set terminal svg; \
	    set output 'plot.convergence_rmax.svg'; \
		set terminal svg background rgb 'white';\
	    set title 'Convergence in rmax (dr fixed)'; \
	    set xlabel 'rmax'; \
	    set ylabel 'ε₀'; \
	    plot 'convergence_rmax.txt' using 1:2 with linespoints title 'ε₀ vs rmax'" | gnuplot

convergence_rmax.txt: exerciseB.exe
	for rmax in $$(seq 5 1 17); do \
	    mono exerciseB.exe rmax=$$rmax dr=0.3 mode=rmax; \
	done

wavefunction.svg: wavefunction.txt Makefile
	echo "set terminal svg; \
		set output 'wavefunction.svg'; \
		set terminal svg background rgb 'white';\
		set key bottom right; \
		set title 'Wavefunctions'; \
		set xlabel 'r'; \
	    plot 'wavefunction.txt' using 1:2 with linespoints title 'wavefunction 1' lc 'red', \
	    	 'wavefunction.txt' using 1:3 with linespoints title 'wavefunction 2' lc 'blue', \
	    	 'wavefunction.txt' using 1:4 with linespoints title 'wavefunction 3' lc 'green', \
	    	 'wavefunction.txt' using 1:5 with linespoints title 'wavefunction 4' lc 'yellow', \
	    	 'wavefunction.txt' using 1:6 with linespoints title 'analytical function' lc 'pink'" | gnuplot


wavefunction.txt: exerciseB.exe
	mono exerciseB.exe rmax=10 dr=0.3 mode=wavefunction

clean:
	rm -f *.dll *.exe Out.txt wavefunction.txt convergence_dr.txt convergence_rmax.txt plot.convergence_dr.svg plot.convergence_rmax.svg 
