Out.txt: main.exe
	mono main.exe > Out.txt
	sed -i 's/\\n/\n/g' Out.txt

integrator.dll: integrator.cs
	mcs -target:library integrator.cs -out:integrator.dll

integrator_3.dll: integrator_3.cs
	mcs -target:library integrator_3.cs -out:integrator_3.dll

main.exe: main.cs integrator.dll integrator_3.dll
	mcs -r:integrator.dll -r:integrator_3.dll main.cs -out:main.exe

erf.svg: erf.txt
	echo "set terminal svg; \
	    set output 'erf.svg'; \
		set terminal svg background rgb 'white';\
	    set title 'Comparison of erf(x): computed vs tabulated (new integrator and homework)';\
		set xlabel 'x'; set ylabel 'erf(x)';\
		set key left top;\
		plot 'erf.txt' every ::1 using 1:2 with linespoints lt rgb 'blue' pt 7 ps 1.0 title 'Computed erf with homework',\
			'erf.txt' every ::1 using 1:4 with linespoints lt rgb 'red' pt 7 ps 1.0 title 'Computed erf with integrator_3',\
			'erf.txt' every ::1 using 1:6 with linespoints lt rgb 'green' pt 7 ps 1.0 title 'Exact Erf';\
		" | gnuplot

erf_time.svg: erf.txt
	echo "set terminal svg; \
	    set output 'erf_time.svg'; \
		set terminal svg background rgb 'white';\
	    set title 'Time taken to compute erf(x) as function of x between integrators'; \
	    set xlabel 'x'; \
	    set ylabel 'time (s)'; \
	    plot 'erf.txt' every ::1 using 1:3 with linespoint title 'time in erf(x)',\
			'erf.txt' every ::1 using 1:5 with linespoint title 'time in erf(x) with integrator_3';\
		" | gnuplot

erf1.svg: erf1.txt
	echo "set terminal svg; \
	    set output 'erf1.svg'; \
		set terminal svg background rgb 'white';\
		set logscale xy;\
	    set title 'absolute value of the difference between erf1 and the exact result as function of acc'; \
	    set xlabel 'acc'; \
	    set ylabel 'err'; \
	    plot 'erf1.txt' using 1:2 with linespoint title 'error in erf(1) with integrator_3';\
		" | gnuplot

f_counts_cc.svg: f_counts_cc.txt
	echo "set terminal svg; \
	 	set output 'f_counts_cc.svg'; \
  		set terminal svg background rgb 'white';\
		set title 'Function calls of Integrators and Clenshaw-Curtis method'; \
	 	set xlabel 'function'; \
	 	set ylabel 'function calls'; \
	 	plot 'f_counts_cc.txt' every ::1 using 1:3 with linespoint title 'function calls of CC homework',\
		'f_counts_cc.txt' every ::1 using 1:5 with linespoint title 'function calls of CC new',\
		'f_counts_cc.txt' every ::1 using 1:6 with linespoint title 'function calls of adaptive3 new';\
  		" | gnuplot

integrator_error.svg: integrator_error.txt
	echo "set terminal svg; \
   		set output 'integrator_error.svg'; \
		set terminal svg background rgb 'white';\
  		set title 'Error of the integrator estimate'; \
   		set xlabel 'function'; \
   		set ylabel 'error'; \
   		plot 'integrator_error.txt' every ::1 using 1:2 with linespoint title 'error in integrator homework',\
			'integrator_error.txt' every ::1 using 1:3 with linespoint title 'actual error in integrator_3',\
  			'integrator_error.txt' every ::1 using 1:4 with linespoint title 'error in integrator_3 new',\
			'integrator_error.txt' every ::1 using 1:5 with linespoint title 'actual error in integrator_3 new';\
	" | gnuplot

clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
