CXXFLAGS = -Wall -std=c++23 -O3 -ffast-math
CODE = $(filter %.cs,$^)
LIBS = $(addprefix -reference:,$(filter %.dll,$^))
MKEXE = mcs -target:exe -out:$@ $(LIBS) $(CODE)
MKLIB = mcs -target:library -out:$@ $(LIBS) $(CODE)
TIME = time --output=$@ --append --format "$$nthreads %e %U"

test: out1 out2 out3 out4

out1: main.exe ; $(TIME) mono $< -nthreads 2 -terms 5e8 >$@ 2>log1
out2: main.exe ; $(TIME) mono $< -nthreads 4 -terms 5e8 >$@ 2>log2
out3: main.exe ; $(TIME) mono $< -nthreads 6 -terms 5e8 >$@ 2>log3
out4: main.exe ; $(TIME) mono $< -nthreads 8 -terms 5e8 >$@ 2>log4
