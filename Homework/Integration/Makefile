out_exA.txt out_erf.txt out_erf1.txt: Makefile exA.exe
	mono exA.exe > out_exA.txt
	mono exA.exe erf > out_erf.txt
	mono exA.exe erf1 > out_erf1.txt
	sed -i 's/,/./g' out_exA.txt out_erf.txt out_erf1.txt

out_exB.txt out_exB_inf.txt: Makefile exB.exe
	mono exB.exe > out_exB.txt
	mono exB.exe inf > out_exB_inf.txt
	sed -i 's/,/./g' out_exB.txt out_exB_inf.txt

out_exC.txt: Makefile exC.exe
	mono exC.exe > out_exC.txt
	sed -i 's/,/./g' out_exC.txt

exA.exe: exA.cs integrator.dll
	mcs -r:integrator.dll exA.cs -out:exA.exe

exB.exe: exB.cs integrator.dll
	mcs -r:integrator.dll exB.cs -out:exB.exe

exC.exe: exC.cs integrator.dll
	mcs -r:integrator.dll exC.cs -out:exC.exe
	
integrator.dll: integrator.cs
	mcs -target:library integrator.cs -out:integrator.dll

python_calls.txt: finite_test.py
	python3 finite_test.py > python_calls.txt

python_calls_inf.txt: infinite_test.py
	python3 infinite_test.py inf > python_calls_inf.txt

out_compare_calls.txt: out_exB.txt python_calls.txt
	@echo "C# integrator calls:" >> out_compare_calls.txt
	@cat out_exB.txt >> out_compare_calls.txt
	@echo "" >> out_compare_calls.txt
	@echo "Python quad calls:" >> out_compare_calls.txt
	@cat python_calls.txt >> out_compare_calls.txt

out_compare_calls_inf.txt: out_exB_inf.txt python_calls_inf.txt
	@echo "C# integrator calls (inf):" >> out_compare_calls_inf.txt
	@cat out_exB_inf.txt >> out_compare_calls_inf.txt
	@echo "" >> out_compare_calls_inf.txt
	@echo "Python quad calls (inf):" >> out_compare_calls_inf.txt
	@cat python_calls_inf.txt >> out_compare_calls_inf.txt


erf.svg: out_erf.txt
	echo "set terminal svg; \
	    set output 'erf.svg'; \
		set terminal svg background rgb 'white';\
	    set title 'Comparison of erf(x): computed vs tabulated';\
		set xlabel 'x'; set ylabel 'erf(x)';\
		set key left top;\
		plot 'out_erf.txt' using 1:2 with linespoints lt rgb 'blue' pt 7 ps 1.0 title 'Computed erf',\
  			 'out_erf.txt' using 1:3 with linespoints lt rgb 'red'  pt 7 ps 1.0 title 'Exact Erf'\
		" | gnuplot

erf1.svg: out_erf1.txt
	echo "set terminal svg; \
	    set output 'erf1.svg'; \
		set terminal svg background rgb 'white';\
		set logscale xy;\
	    set title 'absolute value of the difference between erf1 and the exact result as function of acc'; \
	    set xlabel 'acc'; \
	    set ylabel 'err'; \
	    plot 'out_erf1.txt' using 1:2 with linespoint title 'error in erf(1)" | gnuplot

exC.svg: out_exC.txt
	echo "set terminal svg; \
	    set output 'exC.svg'; \
		set terminal svg background rgb 'white';\
	    set title 'Error estimation of different functions';\
		set xlabel 'function index'; set ylabel 'error';\
		set key left top;\
		plot 'out_exC.txt' every ::1 using 1:4 with linespoint lt rgb 'blue' title 'Estimated error',\
  			 'out_exC.txt' every :: 1using 1:5 with linespoint lt rgb 'red'  title 'Actual error'\
		" | gnuplot
	
clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
