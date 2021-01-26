#pragma once

class IntArray
{
	int nelem;
	int* vec;
public :
	IntArray(int size) : nelem(size){
		vec = new int[nelem];
	}
	~IntArray(){
		delete[] vec;
	}

	int size() const {
		return nelem;
	}

	int& operator[](int i){
		return vec[i];
	}
};