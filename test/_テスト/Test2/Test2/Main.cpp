#include <iostream>

using namespace std;

// new delete
//int main() {
//	const int size = 8;
//	IntArray a(size);
//
//	for( int i = 0; i < a.size(); ++i ){
//		a[i] = i; 
//	}
//	for (int i = 0; i < a.size(); ++i) {
//		cout << "a[" << i << "] = " << a[i] << "\n";
//	}
//}

// �ÓI�f�[�^�����o
//int main()
//{
//	IdNo a;
//	IdNo b;
//
//	cout << "���ʔԍ�a:" << a.id() << "\n";
//	cout << "���ʔԍ�b:" << b.id() << "\n";
//	cout << "���ʔԍ��ő�l:" <<IdNo::get_max_id() << "\n";
//}


// ���Z�q�֐�
//int main() {
//	Vector2 vec1(1,1);
//	Vector2 vec2(2,1);
//	Vector2 vec3;
//
//	vec1 += vec2;
//	vec3 = vec1 + vec2;
//
//	vec1.Disp();
//	vec2.Disp();
//	vec3.Disp();
//}

// ���t�N���X
//using namespace std;
//
//int main()
//{
//	Date a(2025, 11, 18);
//	Date b = a;
//	Date c;
//
//	c = Date(2023);
//
//	a.disp();
//	b.disp();
//	c.disp();
//}

// �N���X��b
//class Account {
//private:
//	string name;
//	int number;
//public:
//	Account(string name, int number) : name(name), number(number){}
//
//	void disp(){
//		cout << name << "\t" <<  number << "\n";
//	}
//};
//
//int main(){
//	Account suzuki("suzuki", 100);
//	suzuki.disp();
//}



// �֐��e���v���[�g
template <class Type>
Type maxof(const Type* x, int n)
{
	Type max = x[0];
	for( int i = 1; i < n; ++i ){
		if( x[i] > max ){
			max = x[i];
		}
	}
	return max;
}

int main(){
	const int isize = 5;
	int ix[isize] = {12, 35, 125, 2, 532};
	cout << "�����̍ő�l" << maxof(ix, isize) << "\n";

	const int dsize = 5;
	double dx[dsize] = {539.2, 2.456, 95.5, 1239.5, 3.14};
	cout << "�����̍ő�l" << maxof(dx, dsize) << "\n";
}

// �|�C���^2
//int main()
//{
//	int a[5] = {1,2,3,4,5};
//	int* ptr = a;
//
//	for( int i = 0; i < 5; ++i ){
//		cout << ptr[i] << "\n";
//		cout << &ptr[i] << "\n";
//	}
//}


// �|�C���^1
//int main()
//{
//	int a = 100, b = 150;
//	int* ptr;
//
//	ptr = &a;
//	*ptr = 300;
//
//	ptr = &b;
//
//	cout << a << "\n";	// �@
//	cout << *ptr << "\n";	// �A
//}


// �֐�
//inline int max(int a, int b) {
//	return a > b ? a : b;
//}
//inline int max(int a, int b, int c) {
//	int max = a;
//	if (b > max) max = b;
//	if (c > max) max = c;
//	return max;
//}
//int main() {
//	int x = 15, y = 31, z = 42;
//	cout << "x,y�̍ő�l:" << max(x, y) << "\n";
//	cout << "x,y,z�̍ő�l:" << max(x, y, z) << "\n";
//}
