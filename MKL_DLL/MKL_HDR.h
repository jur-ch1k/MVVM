#pragma once
#include "mkl.h"

extern "C"  _declspec(dllexport)
void MKL_func(
	MKL_INT nx,
	MKL_INT ny,
	double* x,
	double* y,
	double* scoeff,
	MKL_INT nsite,
	double* site,
	MKL_INT ndorder,
	MKL_INT * dorder,
	double* interpolate_result,
	MKL_INT nlim,
	double* llim,
	double* rlim,
	double* integral_resault,
	int& ret
);