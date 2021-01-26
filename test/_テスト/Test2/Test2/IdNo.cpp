#include "IdNo.h"

int IdNo::counter = 0;

int IdNo::get_max_id()
{
	return counter;
}