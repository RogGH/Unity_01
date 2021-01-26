#include <sstream>
#include <iostream>
#include "Date.h"

using namespace std;

void Date::disp()
{
	ostringstream s;
	s << y << "”N" << m << "ŒŽ" << d << "“ú\n";
	cout << s.str();
}
