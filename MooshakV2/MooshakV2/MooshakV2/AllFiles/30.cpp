#include <iostream>
using namespace std;

class Time
{
private:
    int hours;
    int minutes;
    int seconds;

public:
    Time();
    Time(int h, int m, int s);
    friend ostream &operator << (ostream& output, Time t);
};



int main()
{
    Time time;
    Time time2(10, 20, 30);
    cout << time;
    cout << time2;


    return 0;
}

ostream &operator << (ostream& output, Time t)
    {
        output << t.hours << ":" << t.minutes << ":" << t.seconds << endl;
        return output;
    }

Time::Time()
{
    hours = 0;
    minutes = 0;
    seconds = 0;
}

Time::Time(int h, int m, int s)
{
    hours = h;
    minutes = m;
    seconds = s;
}

