import math
from scipy import integrate

ncalls = 0
def f1(x):
    global ncalls
    ncalls += 1
    return 1.0/math.sqrt(x)

def f2(x):
    global ncalls
    ncalls += 1
    return math.log(x)/math.sqrt(x)

tests = [
    ("1/sqrt(x)",      f1, 0.0, 1.0),
    ("ln(x)/sqrt(x)",  f2, 0.0, 1.0),
]

for name, f, a, b in tests:
    ncalls = 0
    val, err = integrate.quad(f, a, b, epsabs=0.001, epsrel=0.001)
    print(f"{name:20s} calls={ncalls:6d}  result={val:.8e}  err_est={err:.2e}")
