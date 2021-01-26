#pragma once

class Date {
	int y;
	int m;
	int d;
public:
	Date() : y(0), m(0), d(0) {}
	Date(int y, int m = 1, int d = 1) : y(y), m(m), d(d) {}

	int year() { return y; };
	int month() { return m; };
	int day() { return d; };

	void disp();
};