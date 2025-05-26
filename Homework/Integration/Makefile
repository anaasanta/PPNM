exA.txt: Makefile exA.exe
	mono exA.exe > exA.txt
	sed -i 's/,/./g' exA.txt
erf.txt: Makefile exA.exe
	mono exA.exe erf > erf.txt
	sed -i 's/,/./g' erf.txt

erf1.txt: Makefile exA.exe
	mono exA.exe erf1 > erf1.txt
	sed -i 's/,/./g' erf1.txt

exB.txt: Makefile exB.exe
	mono exB.exe > exB.txt
	sed -i 's/,/./g' exB.txt

exB_inf.txt: Makefile exB.exe
	mono exB.exe inf > exB_inf.txt
	sed -i 's/,/./g' exB_inf.txt

exC.txt: Makefile exC.exe
	mono exC.exe > exC.txt
	sed -i 's/,/./g' exC.txt

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

compare_calls.txt: exB.txt python_calls.txt
	@echo "C# integrator calls:" >> compare_calls.txt
	@cat exB.txt >> compare_calls.txt
	@echo "" >> compare_calls.txt
	@echo "Python quad calls:" >> compare_calls.txt
	@cat python_calls.txt >> compare_calls.txt

compare_calls_inf.txt: exB_inf.txt python_calls_inf.txt
	@echo "C# integrator calls (inf):" >> compare_calls_inf.txt
	@cat exB_inf.txt >> compare_calls_inf.txt
	@echo "" >> compare_calls_inf.txt
	@echo "Python quad calls (inf):" >> compare_calls_inf.txt
	@cat python_calls_inf.txt >> compare_calls_inf.txt


erf.svg: erf.txt
	echo "set terminal svg; \
	    set output 'erf.svg'; \
		set terminal svg background rgb 'white';\
	    set title 'Comparison of erf(x): computed vs tabulated';\
		set xlabel 'x'; set ylabel 'erf(x)';\
		set key left top;\
		plot 'erf.txt' using 1:2 with linespoints lt rgb 'blue' pt 7 ps 1.0 title 'Computed erf',\
  			 'erf.txt' using 1:3 with linespoints lt rgb 'red'  pt 7 ps 1.0 title 'MathNet Erf'\
		" | gnuplot

erf1.svg: erf1.txt
	echo "set terminal svg; \
	    set output 'erf1.svg'; \
		set terminal svg background rgb 'white';\
		set logscale xy;\
	    set title 'absolute value of the difference between erf1 and the exact result as function of acc'; \
	    set xlabel 'acc'; \
	    set ylabel 'err'; \
	    plot 'erf1.txt' using 1:2 with linespoint title 'error in erf(1)" | gnuplot
	
clean:
	rm -f *.svg *.dll *.exe *.txt *.svg
