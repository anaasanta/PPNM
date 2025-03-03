Out.txt: main.exe
	mono main.exe n=$(n) m=$(m) > Out.txt

main.exe: main.cs vector.dll matrix.dll QRGS.dll
	mcs -target:exe -out:main.exe -reference:vector.dll -reference:matrix.dll -reference:QRGS.dll main.cs

mainC.exe: mainC.cs vector.dll matrix.dll QRGS.dll
	mcs -target:exe -out:mainC.exe -reference:vector.dll -reference:matrix.dll -reference:QRGS.dll mainC.cs

QRGS.dll: QRGS.cs matrix.dll vector.dll
	mcs -target:library -out:QRGS.dll -reference:matrix.dll -reference:vector.dll QRGS.cs

vector.dll: vector.cs
	mcs -target:library -out:vector.dll vector.cs

matrix.dll: matrix.cs vector.dll
	mcs -target:library -out:matrix.dll -reference:vector.dll matrix.cs

plot.QR.svg: out.times.data Makefile
	echo " \
		set terminal svg; \
		set output 'plot.QR.svg'; \
		set key left; \
		set title 'Execution time of QR vs. N'; \
		set xlabel 'N'; \
		set ylabel 'Time (s)'; \
		set terminal svg background rgb 'white';\
		a=1; b=3; \
		f(x) = a*x**b; \
		fit f(x) '$<' via a,b; \
		plot '$<' with points title 'Data', \
		f(x) with lines title 'N^3' \
	" | gnuplot



out.times.data: mainC.exe
	> $@
	> log.err
	for N in $$(seq 400 20 600); do \
		time --format "$$N %e" --output $@ --append \
		mono $< -size:$$N 1>> out 2>> log.err ; \
	done

clean:
	rm -f *.dll *.exe Out.txt out.times.data out err
