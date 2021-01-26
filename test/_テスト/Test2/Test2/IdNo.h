#pragma once

class IdNo {
	static int counter;
	int id_no;
public:
	IdNo(){ 
		++counter;
		id_no = counter;
	};
	int id() const{
		return id_no;
	};

	static int get_max_id();
};