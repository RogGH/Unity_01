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

// 静的データメンバ
//int main()
//{
//	IdNo a;
//	IdNo b;
//
//	cout << "識別番号a:" << a.id() << "\n";
//	cout << "識別番号b:" << b.id() << "\n";
//	cout << "識別番号最大値:" <<IdNo::get_max_id() << "\n";
//}


// 演算子関数
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

// 日付クラス
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

// クラス基礎
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



// 関数テンプレート
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
	cout << "整数の最大値" << maxof(ix, isize) << "\n";

	const int dsize = 5;
	double dx[dsize] = {539.2, 2.456, 95.5, 1239.5, 3.14};
	cout << "実数の最大値" << maxof(dx, dsize) << "\n";
}

// ポインタ2
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


// ポインタ1
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
//	cout << a << "\n";	// ①
//	cout << *ptr << "\n";	// ②
//}


// 関数
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
//	cout << "x,yの最大値:" << max(x, y) << "\n";
//	cout << "x,y,zの最大値:" << max(x, y, z) << "\n";
//}
