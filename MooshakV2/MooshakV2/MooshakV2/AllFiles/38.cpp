#include <iostream>
#include <vector>
#include <string>

using namespace std;

struct chessPlayer
{

    string name;
    int year;
    int rating;
};

int main()
{
    string name;
    int year;
    int rating;
    int n;
    cout << "Number of players: ";
    cin >> n;
    cout << "--- Reading players ---" << endl;

    vector<chessPlayer> player;

    for(int i = 0; i < n; i++)
    {
        chessPlayer temp;
        cout << "Name: ";
        cin >> temp.name;
        cout << "Year: ";
        cin >> temp.year;
        cout << "rating: ";
        cin >> temp.rating;


        player.push_back(temp);
    }



    return 0;
}
