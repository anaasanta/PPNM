CODE = $(filter %.cs,$^) 
LIBS = $(addprefix -reference:,$(filter %.dll,$^))
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)

all: plot.test.svg 

plot.gamma.svg : data.txt Makefile  
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key bottom ;\
		set xlabel "x" ;\
		set ylabel "Î“(x)" ;\
		plot [-4:][-20:20] \
			"$<" index 1 with lines title "gamma(x)" \
			,"$<" index 3 with points title "factorials" \
	' | tee log.erf.gpi | gnuplot

plot.erf.svg : data.txt erf.data Makefile
	echo ' \
		set terminal svg; \
		set output "$@"; \
		set key left ;\
		plot  \
			"$<" index 0 with lines title "erf(x)" \
			,"erf.data" with points pointtype 4 title "tabulated erf(x)" \
	' | tee log.erf.gpi | gnuplot

plot.test.svg: test.gpi
	gnuplot $<

data.txt: main.exe
	mono $< > $@

sfuns.dll : sfuns.cs ; $(MKLIB)
main.exe : main.cs sfuns.dll; $(MKEXE)

clean:	
	$(RM) *.exe *.dll [Oo]ut* [Ll]og* *.svg data.txt