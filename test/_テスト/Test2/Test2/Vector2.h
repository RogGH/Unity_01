#pragma once

class Vector2 {
	float x;
	float y;
public:
	Vector2(float x = 0, float y = 0) : x(x), y(y) {}
	void Disp() {
		std::cout << x << "," << y << "\n";
	}

	Vector2& operator+=(const Vector2& vec) {
		x += vec.x;
		y += vec.y;
		return *this;
	}
	friend Vector2 operator+(const Vector2& vec1, const Vector2& vec2) {
		return Vector2(vec1.x + vec2.x, vec1.y + vec2.y);
	}	

	friend Vector2& operator - (const Vector2& vec1, const Vector2& vec2){
	}
};

