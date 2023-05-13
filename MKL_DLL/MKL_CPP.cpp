#include "pch.h"
#include "mkl.h"

extern "C"  _declspec(dllexport)
void MKL_func(
	MKL_INT nx,        // кол-во точек в сетке
	MKL_INT ny,        // 1
	double* x,         // массив координат
	double* y,         // массив значий поля 1
	double* bc,        // граничные условия произв
	double* scoeff,    // double[ny * 4 * (nx - 1)]
	MKL_INT nsite,     // кол-во точек в сетке
	double* site,      // массив New_coord
	MKL_INT ndorder,   // 3
	MKL_INT * dorder,  // int[3] = [1, 1, 1]
	double* interpolate_result,    // float[ny * 3 * nsites]
	MKL_INT nlim,      // 1
	double* llim,      // int[1] = [integral_lim_left]
	double* rlim,      // int[1] = [integral_lim_right]
	double* integral_resault,      // float[ny * nlim]
	bool IsUnuform,
	int* ret)          // -1
{
	try
	{
		int info;
		DFTaskPtr task;

		int grid_type;
		if (IsUnuform)
			grid_type = DF_UNIFORM_PARTITION;
		else
			grid_type = DF_NON_UNIFORM_PARTITION;


		//4й параметр надо менять между DF_NON_UNIFORM_PARTITION и DF_UNIFORM_PARTITION, в зависимости от типа разбиения
		info = dfdNewTask1D(&task, nx, x, grid_type, ny, y, DF_MATRIX_STORAGE_ROWS);
		if (info != DF_STATUS_OK) {
			ret[0] = -1;
			throw info;
		}
		info = dfdEditPPSpline1D(task, DF_PP_CUBIC, DF_PP_NATURAL, DF_BC_2ND_LEFT_DER | DF_BC_2ND_RIGHT_DER, bc, DF_NO_IC, NULL, scoeff, DF_NO_HINT);
		if (info != DF_STATUS_OK) {
			ret[0] = -2;
			throw info;
		}
		info = dfdConstruct1D(task, DF_PP_SPLINE, DF_METHOD_STD);
		if (info != DF_STATUS_OK) {
			ret[0] = -3;
			throw info;
		}
		info = dfdInterpolate1D(task, DF_INTERP, DF_METHOD_PP, nsite, site, DF_NON_UNIFORM_PARTITION, ndorder, dorder, NULL, interpolate_result, DF_MATRIX_STORAGE_ROWS, NULL);
		if (info != DF_STATUS_OK) {
			ret[0] = -4;
			throw info;
		}
		info = dfdIntegrate1D(task, DF_METHOD_PP, nlim, llim, DF_UNIFORM_PARTITION, rlim, DF_UNIFORM_PARTITION, NULL, NULL, integral_resault, DF_MATRIX_STORAGE_ROWS);
		if (info != DF_STATUS_OK) {
			ret[0] = -5;
			throw info;
		}
		info = dfDeleteTask(&task);
		if (info != DF_STATUS_OK) {
			ret[0] = -6;
			throw info;
		}

		ret[0] = 0;
	}
	catch (int ex)
	{
		ret[1] = ex;
	}
	catch (...) {
		ret[0] = -1;
		ret[1] = -1;
	}
}