#include <sstream>
#include <iostream>
#include "Date.h"

using namespace std;

void Date::disp()
{
	ostringstream s;
	s << y << "�N" << m << "��" << d << "��\n";
	cout << s.str();
}
