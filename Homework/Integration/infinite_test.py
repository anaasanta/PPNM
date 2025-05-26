import math
from scipy import integrate

ncalls = 0
def f1(x):
    global ncalls
    ncalls += 1
    return math.exp(x)

def f2(x):
    global ncalls
    ncalls += 1
    return x*math.exp(-x)

def f3(x):
    global ncalls
    ncalls += 1
    return math.exp(-x**2)

tests = [
    
    ("exp(x)", f1, -float('inf'), 1.0),# Integral from -inf to 1
    ("x*exp(-x)", f2, 0.0, float('inf')), # Integral from 0 to inf
    ("exp(-x^2)", f3, -float('inf'), float('inf')), #Integral from -inf to inf

]

for name, f, a, b in tests:
    ncalls = 0
    val, err = integrate.quad(f, a, b, epsabs=0.001, epsrel=0.001)
    print(f"{name:20s} calls={ncalls:6d}  result={val:.8e}  err_est={err:.2e}")
