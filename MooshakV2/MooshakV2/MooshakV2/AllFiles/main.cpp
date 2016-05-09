#include <iostream>

using namespace std;
const int LINA = 3;
const int DALKUR = 3;

void makeMylla(char mylla[LINA][DALKUR]);
void printMylla(char mylla[LINA][DALKUR]);
char getInput(char currentPlayer, char mylla[LINA][DALKUR]);
void updateMylla(int& position, char mylla[LINA][DALKUR], char currentPlayer);
bool oWin(char mylla[LINA][DALKUR]);
bool xWin(char mylla[LINA][DALKUR]);
void printWinner(char mylla[LINA][DALKUR]);
void players(char &mylla);
bool illegalMove(char mylla[LINA][DALKUR],int position);

int main()
{
    char mylla[LINA][DALKUR];
    makeMylla(mylla);
    printMylla(mylla);
    char currentPlayer = 'X';
    int position;
    illegalMove(mylla, position) ;
    for (int i = 0; i < 9; i++)
    {
        position = getInput(currentPlayer, mylla);
        updateMylla(position, mylla, currentPlayer);
        printMylla(mylla);
        players(currentPlayer);
        printWinner(mylla);
        if (oWin(mylla) || xWin(mylla))
        {
            return 0;
        }
    }
    cout << "Draw!";
    return 0;

}
void makeMylla(char mylla[LINA][DALKUR])
{

    int f = '1';
    for (int i = 0; i < LINA; i++)
    {
        for (int j = 0; j < DALKUR; j++)
        {
            mylla[i][j] = f;
            f++;
        }
    }


}


void printMylla(char mylla[LINA][DALKUR])
{

    for (int i = 0; i < LINA; i++)
    {
        for (int j = 0; j < DALKUR; j++)
        {
            cout << mylla[i][j] << " ";
        }
        cout << endl;
    }
}

char getInput(char currentplayer, char mylla[LINA][DALKUR])
{

    char position;
    int pos;
    do
    {
        cout << currentplayer << " position: ";
        cin >> position;

        pos = position - '0';
        if (pos < 1 || pos > 9 || illegalMove(mylla, pos))
        {
            cout << "Illegal move!" << endl;
        }

    }
    while(pos < 1 || pos > 9 || illegalMove(mylla, pos));




    return pos;

}

void updateMylla(int& position, char mylla[LINA][DALKUR], char currentPlayer)
{


    if(position == 1)
    {
        mylla[0][0] = currentPlayer;
    }
    if(position == 2)
    {
        mylla[0][1] = currentPlayer;
    }
    if(position == 3)
    {
        mylla[0][2] = currentPlayer;
    }
    if(position == 4)
    {
        mylla[1][0] = currentPlayer;
    }
    if(position == 5)
    {
        mylla[1][1] = currentPlayer;
    }
    if(position == 6)
    {
        mylla[1][2] = currentPlayer;
    }
    if(position == 7)
    {
        mylla[2][0] = currentPlayer;
    }
    if(position == 8)
    {
        mylla[2][1] = currentPlayer;
    }
    if(position == 9)
    {
        mylla[2][2] = currentPlayer;
    }

}
bool xWin(char mylla[LINA][DALKUR])
{
    if(mylla[0][0] == 'X' && mylla[0][1] == 'X' && mylla[0][2] == 'X')
    {
        return true;
    }
    else if(mylla[1][0] == 'X' && mylla[1][1] == 'X' && mylla[1][2] == 'X')
    {
        return true;
    }
    else if(mylla[2][0] == 'X' && mylla[2][1] == 'X' && mylla[2][2] == 'X')
    {
        return true;
    }
    else if(mylla[0][0] == 'X' && mylla[1][0] == 'X' && mylla[2][0] == 'X')
    {
        return true;
    }
    else if(mylla[0][1] == 'X' && mylla[1][1] == 'X' && mylla[2][1] == 'X')
    {
        return true;
    }
    else if(mylla[0][2] == 'X' && mylla[1][2] == 'X' && mylla[2][2] == 'X')
    {
        return true;
    }
    else if(mylla[0][0] == 'X' && mylla[1][1] == 'X' && mylla[2][2] == 'X')
    {
        return true;
    }
    else if(mylla[0][2] == 'X' && mylla[1][1] == 'X' && mylla[2][0] == 'X')
    {
        return true;
    }
    return false;
}
bool oWin(char mylla[LINA][DALKUR])
{
    if(mylla[0][0] == 'O' && mylla[0][1] == 'O' && mylla[0][2] == 'O')
    {
        return true;
    }
    else if(mylla[1][0] == 'O' && mylla[1][1] == 'O' && mylla[1][2] == 'O')
    {
        return true;
    }
    else if(mylla[2][0] == 'O' && mylla[2][1] == 'O' && mylla[2][2] == 'O')
    {
        return true;
    }
    else if(mylla[0][0] == 'O' && mylla[1][0] == 'O' && mylla[2][0] == 'O')
    {
        return true;
    }
    else if(mylla[0][1] == 'O' && mylla[1][1] == 'O' && mylla[2][1] == 'O')
    {
        return true;
    }
    else if(mylla[0][2] == 'O' && mylla[1][2] == 'O' && mylla[2][2] == 'O')
    {
        return true;
    }
    else if(mylla[0][0] == 'O' && mylla[1][1] == 'O' && mylla[2][2] == 'O')
    {
        return true;
    }
    else if(mylla[0][2] == 'O' && mylla[1][1] == 'O' && mylla[2][0] == 'O')
    {
        return true;
    }
    return false;
}
void printWinner(char mylla[LINA][DALKUR])
{

    if(oWin(mylla))
    {
        cout << "Winner is: ";
        cout << "O" << endl;
    }
    if (xWin(mylla))
    {
        cout << "Winner is: ";
        cout << "X" << endl;
    }
}
void players(char &mylla)
{
    if(mylla == 'X')
    {
        mylla = 'O';
    }
    else
        mylla = 'X';
}
bool illegalMove(char mylla[LINA][DALKUR],int position)
{

    if(position == 1)
    {
        if (mylla[0][0] == 'O' || mylla[0][0] == 'X')
        {
            return true;
        }
    }
    if(position == 2)
    {
        if (mylla[0][1] == 'O' || mylla[0][1] == 'X')
        {
            return true;
        }
    }
    if(position == 3)
    {
        if (mylla[0][2] == 'O' || mylla[0][2] == 'X')
        {
            return true;
        }
    }
    if(position == 4)
    {
        if (mylla[1][0] == 'O' || mylla[1][0] == 'X')
        {
            return true;
        }
    }
    if(position == 5)
    {
        if (mylla[1][1] == 'O' || mylla[1][1] == 'X')
        {
            return true;
        }
    }
    if(position == 6)
    {
        if (mylla[1][2] == 'O' || mylla[1][2] == 'X')
        {
            return true;
        }
    }
    if(position == 7)
    {
        if (mylla[2][0] == 'O' || mylla[2][0] == 'X')
        {
            return true;
        }
    }
    if(position == 8)
    {
        if (mylla[2][1] == 'O' || mylla[2][1] == 'X')
        {
            return true;
        }
    }
    if(position == 9)
    {
        if (mylla[2][2] == 'O' || mylla[2][2] == 'X')
        {
            return true;
        }

    }
    return false;
}
