Out.txt: main.exe
	mono main.exe n=$(n) > Out.txt

out_convergence_dr.txt: exerciseB.exe
	for dr in $$(seq 0.1 0.1 0.8); do \
		mono exerciseB.exe rmax=10 dr=$$dr mode=dr; \
	done
	sed -i 's/,/./g' out_convergence_dr.txt

out_convergence_rmax.txt: exerciseB.exe
	for rmax in $$(seq 5 1 17); do \
	    mono exerciseB.exe rmax=$$rmax dr=0.3 mode=rmax; \
	done

out_wavefunction.txt: exerciseB.exe
	mono exerciseB.exe rmax=10 dr=0.3 mode=wavefunction

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

convergence_dr.svg: out_convergence_dr.txt Makefile
	echo "set terminal svg; \
		set output 'convergence_dr.svg'; \
		set terminal svg background rgb 'white';\
		set title 'Convergence in Δr (with a fixed rmax)'; \
		set xlabel 'Δr'; \
	    set ylabel 'ε₀'; \
	    plot 'out_convergence_dr.txt' using 1:2 with linespoints title 'ε₀ vs Δr'" | gnuplot


convergence_rmax.svg: out_convergence_rmax.txt Makefile
	echo "set terminal svg; \
	    set output 'convergence_rmax.svg'; \
		set terminal svg background rgb 'white';\
	    set title 'Convergence in rmax (dr fixed)'; \
	    set xlabel 'rmax'; \
	    set ylabel 'ε₀'; \
	    plot 'out_convergence_rmax.txt' using 1:2 with linespoints title 'ε₀ vs rmax'" | gnuplot


wavefunction.svg: out_wavefunction.txt Makefile
	echo "set terminal svg; \
		set output 'wavefunction.svg'; \
		set terminal svg background rgb 'white';\
		set key bottom right; \
		set title 'Wavefunctions'; \
		set xlabel 'r'; \
	    plot 'out_wavefunction.txt' using 1:2 with linespoints title 'wavefunction 1' lc 'red', \
	    	 'out_wavefunction.txt' using 1:3 with linespoints title 'wavefunction 2' lc 'blue', \
	    	 'out_wavefunction.txt' using 1:4 with linespoints title 'wavefunction 3' lc 'green', \
	    	 'out_wavefunction.txt' using 1:5 with linespoints title 'wavefunction 4' lc 'yellow', \
	    	 'out_wavefunction.txt' using 1:6 with linespoints title 'analytical function' lc 'pink'" | gnuplot



exerciseC.exe: exerciseC.cs matrix.dll vector.dll jacobi.dll
	mcs -target:exe -out:exerciseC.exe -reference:matrix.dll -reference:vector.dll -reference:jacobi.dll exerciseC.cs

out_diagonalization.txt: exerciseC.exe
	for n in $$(seq 10 10 100); do \
		mono exerciseC.exe n=$$n repetitions=10>> out_diagonalization.txt & \
		done; \
		wait 

diagonalization.svg: out_diagonalization.txt Makefile
	echo "set terminal svg; \
		set output 'diagonalization.svg'; \
		set terminal svg background rgb 'white';\
		set key bottom right; \
		set title 'Diagonalization time'; \
		set xlabel 'n'; \
	    set ylabel 'time'; \
		a=1; b=3; \
		f(x) = a*x**b; \
		fit f(x) '$<' via a,b; \
	    plot 'out_diagonalization.txt' using 1:2 with linespoints title 'time vs n', \
			  f(x) with lines title 'N^3" | gnuplot

clean:
	rm -f *.dll *.exe *.txt *.svg 
