#include <iostream>
using namespace std;

int main()
{

    int *p;
    p = new int;
    int *q;
    q = new int;
    q = p;
    *p = 20;
    *q = 30;
    cout << *p << " " << *q;

    return 0;
}
